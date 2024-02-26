using ThunderHerd.Core.Models.Dtos;

namespace ThunderHerd.Domain.Interfaces
{
    public interface IScheduleService
    {
        Task<Schedule?> CreateScheduleAsync(Schedule schedule, CancellationToken cancellationToken = default);
        Task<Schedule?> FindScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default);
        Task ScheduleTestAsync(Guid scheduleId, CancellationToken cancellationToken = default);
        Task ScheduleTestAsync(Schedule? schedule, CancellationToken cancellationToken = default);
        Task ScheduleTestImmediatelyAsync(Guid testId, CancellationToken cancellationToken = default);
    }
}
