using Hikvision.Api.Models;
using Hikvision.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hikvision.Api.Controllers;
// Controller-də belə istifadə edə bilərsiniz:
[ApiController]
[Route("api/[controller]")]
public class HikvisionController : ControllerBase
{
    private readonly IHikvisionService _hikvisionService;

    public HikvisionController(IHikvisionService hikvisionService)
    {
        _hikvisionService = hikvisionService;
    }

    [HttpPost("connect")]
    public ActionResult<ApiResponse<object>> Connect(ConnectRequest request)
    {
        var result = _hikvisionService.HikConnection(request);
        return Ok(result);
    }

    [HttpPost("disconnect")]
    public ActionResult<ApiResponse<object>> Disconnect()
    {
        var result = _hikvisionService.Disconnect();
        return Ok(result);
    }
}