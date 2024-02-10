namespace ThunderHerd.Core.Entities
{
    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public bool Recurring { get; set; } = false;
        public string? CronSchedule { get; set; }

        public Test Test { get; set; }

        public static Schedule Map(Models.Dtos.Schedule entity)
        {
            return new Schedule
            {
                Id = entity.Id,
            };
        }
    }
}
