using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using BlazorSignalRApp.Hubs; // Assure-toi que ce namespace est bon
using NLog;
using BlazorSignalRApp.data; //for Nlog

namespace BlazorSignalRApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatApiController : ControllerBase
    {
        
        private static Logger _logger = LogManager.GetCurrentClassLogger();// added line for Nlog

        private readonly IHubContext<ChatHub> _hubContext;

        private readonly AppDbContext _context;
        public ChatApiController(IHubContext<ChatHub> hubContext, AppDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            _logger.Info($"API received message from {message.User}: {message.Text}"); // added line for Log the message
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.User, message.Text);
            return Ok(new { status = "Message sent from API" });
        }
    }

    public class ChatMessage
    {
        public string User { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
