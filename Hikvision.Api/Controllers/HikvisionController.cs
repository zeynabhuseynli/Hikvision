using Hikvision.Api.Models;
using Hikvision.Api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hikvision.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HikvisionController : ControllerBase
    {
        private readonly IHikvisionAccessControlClient _client;

        public HikvisionController(IHikvisionAccessControlClient client)
        {
            _client = client;
        }

        [HttpPost("connect")]
        public IActionResult Connect([FromBody] ConnectRequest request)
        {
            try
            {
                _client.Initialize();
                _client.Connect(request.Ip, request.Port, request.Username, request.Password);
                return Ok(new { message = "Connected" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Error = ex.Message, Code = _client.GetLastError() });
            }
        }

        [HttpPost("disconnect")]
        public IActionResult Disconnect()
        {
            _client.Disconnect();
            return Ok(new { message = "Disconnected" });
        }

        [HttpGet("error")]
        public IActionResult LastError()
        {
            var code = _client.GetLastError();
            return Ok(new { errorCode = code });
        }
    }
}
