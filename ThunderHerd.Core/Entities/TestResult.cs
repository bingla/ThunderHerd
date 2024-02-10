﻿namespace ThunderHerd.Core.Entities
{
    public class TestResult
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public DateTime RunStarted { get; set; }
        public DateTime RunCompleted { get; set; }
        public TimeSpan RunDuration { get; set; }
        public TimeSpan WarmupDuration { get; set; }

        public static TestResult Map(Models.Dtos.TestResult entity)
        {
            if (entity == default)
                return default;

            return new TestResult
            {
                Id = entity.Id,
                RunStarted = entity.RunStarted,
                RunCompleted = entity.RunCompleted,
                RunDuration = entity.RunDuration,
                WarmupDuration = entity.WarmupDuration,
            };
        }
    }
}