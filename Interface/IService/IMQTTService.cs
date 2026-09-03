using Hauto.Models.DTOs;

namespace Hauto.Interface.IService;
public interface IMqttService
{
    Task ConnectAsync(CancellationToken cancellationToken);
    Task PublishCommand(GetDeviceDto obj);
    Task PublishSync(List<GetDeviceDto> obj);
}