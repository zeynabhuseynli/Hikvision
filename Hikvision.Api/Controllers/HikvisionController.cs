using Hikvision.Api.Models;
using Hikvision.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hikvision.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HikvisionController : ControllerBase
{
    private readonly IHikvisionService _hikvisionService;
    public HikvisionController(IHikvisionService hikvisionService)
    {
        _hikvisionService = hikvisionService;
    }

    // Cihaza bağlan
    [HttpPost("connect")]
    public ActionResult<ApiResponse<object>> Connect(ConnectRequest request)
    {
        return _hikvisionService.HikConnection(request);
    }
}
