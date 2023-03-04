using Application.Interfaces.Account;
using Application.Interfaces.Context;
using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BlazorChatRoom.Shared.DTOs.ChatDto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Account;

public class UserService : IUserService
{
    private readonly IDataBaseContext _context;
    private readonly IConfiguration _configuration;
   
    public UserService(IDataBaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
     
    }

    public async Task<ServiceResponse<long>> Register(RegisterDto request)
    {
        if (await UserExists(request.Email))
        {
            return new ServiceResponse<long>() { Success = false, Message = "User already exists." };
        }

        CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = new User()
        {
            Email = request.Email,
            Name = request.Name,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Picture = "/Img/No-photo.png",
            
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new ServiceResponse<long>() { Data = user.Id, Success = true ,Message = "Registration Successfull !"};
    }

    public async Task<ServiceResponse<string>> Login(LoginDto request)
    {
        var response = new ServiceResponse<string>();

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(request.Email.ToLower()));

        if (user == null)
        {
            response.Success = false;
            response.Message = "User Not found";
        }
        else if (!VerifyPassowrdHash(request.Password,user.PasswordHash,user.PasswordSalt))
        {
            response.Success = false;
            response.Message = "Wrong Password";
        }
        else
        {
            response.Data =await CreateToken(user);
        }

        return response;
    }

    public async Task<List<UserDto>> GetUsers(long userId)
    {
       
        return await _context.Users.Where(x =>
            x.Id != userId).Select(s => new UserDto()
        {
                Name = s.Name,
                Date = s.CreationDate.ToString("d"),
                Id = s.Id,
                PictureName = s.Picture
        }).ToListAsync();
    }

    public async Task<long> GetUserIdByEmail(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(x => x.Email == email))!.Id;
    }

    private async Task<string?> CreateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims =await GetClaims(user);
        var (jwtSecurityToken, dateTime) = GenerateTokenOptions(signingCredentials, claims);
     
        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return jwt;
    }


    private Tuple<JwtSecurityToken, DateTime> GenerateTokenOptions(SigningCredentials signingCredentials,
        IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JWtConfig");
        var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(
            jwtSettings.GetSection("expires").Value));

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetSection("issuer").Value,
            claims: claims,
            audience: jwtSettings.GetSection("audience").Value,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        return Tuple.Create(token, expiration);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = _configuration["JWtConfig:Key"];
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private  Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
        };

        return Task.FromResult(claims);
    }

    private static bool VerifyPassowrdHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    public async Task<bool> UserExists(string email) =>
        await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}