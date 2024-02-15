using Hangfire;
using ThunderHerd.Core.Models.Dtos;
using ThunderHerd.DataAccess.Interfaces;
using ThunderHerd.Domain.Interfaces;

namespace ThunderHerd.Domain.Services
{
    /// <summary>
    /// Run scheduler
    /// </summary>
    public class ScheduleService : IScheduleService
    {
        private readonly ITestService _testService;
        private readonly IRunService _runService;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly ITestRepository _testRepository;

        public ScheduleService(ITestService testService,
            IRunService runService,
            IScheduleRepository scheduleRepository,
            ITestRepository testRepository)
        {
            _testService = testService;
            _runService = runService;
            _scheduleRepository = scheduleRepository;
            _testRepository = testRepository;
        }

        public async Task<Schedule?> CreateScheduleAsync(Schedule? schedule, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(schedule, nameof(schedule));

            var entity = await _scheduleRepository.CreateAsync(Core.Entities.Schedule.Map(schedule), cancellationToken);
            return Schedule.Map(entity);
        }

        public async Task<Schedule?> FindScheduleAsync(Guid scheduleId, CancellationToken cancellationToken = default)
        {
            var entity = await _scheduleRepository.FindAsync(scheduleId, cancellationToken);
            return Schedule.Map(entity);
        }

        public async Task ScheduleTestAsync(Guid scheduleId, CancellationToken cancellationToken = default)
        {
            var schedule = await FindScheduleAsync(scheduleId, cancellationToken);
            await ScheduleTestAsync(schedule, cancellationToken);
        }

        public Task ScheduleTestAsync(Schedule? schedule, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(schedule, nameof(schedule));

            // Schedule with HangFire
            if (!schedule.Recurring)
            {
                // Schedule to run immediately
                BackgroundJob.Enqueue(() => _runService.RunTestAsync(schedule.TestId, cancellationToken));
            }
            else
            {
                // TODO: Schedule to run on cron job
            }

            return Task.CompletedTask;
        }

        public async Task ScheduleTestImmediatelyAsync(Guid testId, CancellationToken cancellationToken = default)
        {
            var schedule = new Schedule { TestId = testId, Recurring = false };
            await ScheduleTestAsync(schedule, cancellationToken);
        }
    }
}
