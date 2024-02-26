namespace ThunderHerd.Core.Models.Dtos
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; }

        public static Schedule? Map(Guid runId, Requests.ScheduleRequest? request)
        {
            if (request == default)
                return default;

            return new Schedule
            {
                TestId = runId,
                Recurring = request.Recurring,
                CronSchedule = request.CronSchedule,
            };
        }

        public static Schedule? Map(Entities.Schedule? entity)
        {
            if (entity == default)
                return default;

            return new Schedule
            {
                Id = entity.Id,
                TestId = entity.Test.Id,
                Recurring = entity.Recurring,
                CronSchedule = entity.CronSchedule,
            };
        }
    }
}
