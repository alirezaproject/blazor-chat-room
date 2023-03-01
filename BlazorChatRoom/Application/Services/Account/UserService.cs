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
            response.Data = CreateToken(user);
        }

        return response;
    }

    private string? CreateToken(User user)
    {
        var claim = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
            
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("Appsettings:Token").Value));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(claims: claim, expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
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