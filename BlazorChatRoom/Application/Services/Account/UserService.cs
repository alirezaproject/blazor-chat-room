using Application.Interfaces.Account;
using Application.Interfaces.Context;
using BlazorChatRoom.Shared;
using BlazorChatRoom.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;

namespace Application.Services.Account;

public class UserService : IUserService
{
    private readonly IDataBaseContext _context;

    public UserService(IDataBaseContext context)
    {
        _context = context;
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

    public async Task<bool> UserExists(string email) =>
        await _context.Users.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}