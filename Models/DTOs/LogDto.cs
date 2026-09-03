namespace Hauto.Models.DTOs
{
    public class AddLogDto
    {
        public int DeviceId { get; set; }
        public string LogStatement { get; set; }
        public DateTime Date { get; set; }
    }
    public class GetLogDto
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public GetDeviceDto Device { get; set; }
        public string LogStatement { get; set; }
        public DateTime Date { get; set; }
    }
    public class LogResponse : BaseResponse
    {
        public List<GetLogDto> Data { get; set; } = new List<GetLogDto>();
    }
    public class SingleLogResponse : BaseResponse
    {
        public GetLogDto Data { get; set; } = new GetLogDto();
    }
}
