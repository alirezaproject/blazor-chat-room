using System.Security.Claims;
using Application.Interfaces.Account;
using Application.Interfaces.Message;
using BlazorChatRoom.Shared.DTOs.Chat;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatRoom.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;

        public ChatController(IUserService userService, IMessageService messageService)
        {
            _userService = userService;
            _messageService = messageService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            var allUsers = await _userService.GetUsers(userId!);
            return Ok(allUsers);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user = await _userService.GetUserDetail(userId);
            return Ok(user);
        }

        [HttpPost("SaveMessage")]
        public async Task<IActionResult> SaveMessageAsync(MessageDto message)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();

            await _messageService.SaveChatMessage(message);

            return Ok();
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetConversationAsync(string contactId)
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault()!;

            var messages = await _messageService.GetConversationAsync(contactId, userId); 
            return Ok(messages);
        }
    }
}