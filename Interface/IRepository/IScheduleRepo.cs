using Hauto.Entities;

namespace Hauto.Interface.IRepository
{
    public interface IScheduleRepo : IGenericRepo<Schedule>
    {
        Task<Schedule> GetSchedule(int Id);
        Task<IList<Schedule>> GetAllSchedules();
    }
}
