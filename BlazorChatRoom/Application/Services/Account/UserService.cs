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
using BlazorChatRoom.Shared.DTOs.Auth;
using BlazorChatRoom.Shared.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Account;

public class UserService : IUserService
{
    private readonly IDataBaseContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public UserService(IDataBaseContext context, IConfiguration configuration, UserManager<User> userManager)
    {
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<ServiceResponse<string>> Register(RegisterDto request)
    {
        if (await UserExists(request.Email))
        {
            return new ServiceResponse<string>() { Success = false, Message = "User already exists." };
        }

        var user = new User()
        {
            Name = request.Name,
            Picture = "/images/no-photo.jpg",

        };

        await _userManager.CreateAsync(user);
   
        

        return new ServiceResponse<string>() { Data = user.Id, Success = true ,Message = "Registration Successfull !"};
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
        else if (await _userManager.CheckPasswordAsync(user, request.Password) == false)
        {
            response.Success = false;
            response.Message = "Wrong Password";
        }
        else
        {
            response.Data = await CreateToken(user);
        }

        return response;
    }

    public async Task<List<UserDto>> GetUsers(string userId)
    {
       
        return await _context.Users.Where(x =>
            x.Id != userId).Select(s => new UserDto()
        {
                Name = s.Email,
                Date = s.CreationDate.ToString("d"),
                Id = s.Id,
                Picture = string.IsNullOrWhiteSpace(s.Picture) ? "/No-photo.png" : s.Picture
        }).ToListAsync();
    }

    public async Task<UserDto> GetUserDetail(string userId)
    {
        return await _context.Users.Where(user => user.Id == userId).Select(s => new UserDto()
        {
            Id = s.Id,
            Name = s.Name,
            Picture = s.Picture,
            Date = s.CreationDate.ToString("d")
        }).FirstOrDefaultAsync() ?? new UserDto();
    }

    public async Task<string> GetUserIdByEmail(string email)
    {
        return (await _context.Users.FirstOrDefaultAsync(x => x.Email == email))!.Id;
    }

    private async Task<string> CreateToken(User user)
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