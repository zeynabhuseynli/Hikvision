using Hikvision.Api.Models;

namespace Hikvision.Api.Services;
public interface IHikvisionService
{
    ApiResponse<object> HikConnection(ConnectRequest request);
    ApiResponse<object> Disconnect();
}