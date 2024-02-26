namespace ThunderHerd.Core.Entities
{
    public class Schedule
    {
        public Guid Id { get; init; } = Guid.Empty;
        public Guid TestId { get; set; }
        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; }

        public virtual Test? Test { get; set; }

        public static Schedule Map(Models.Dtos.Schedule entity)
        {
            return new Schedule
            {
                Id = entity.Id,
                TestId = entity.TestId,
                Recurring = entity.Recurring,
                CronSchedule = entity.CronSchedule,
            };
        }
    }
}
