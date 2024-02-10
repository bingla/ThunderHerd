using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Entities
{
    public class TestItem
    {
        public Guid Id { get; set; }
        public Guid TestId { get; set; }
        public HttpMethods Method { get; set; } = HttpMethods.GET;
        public string? Url { get; set; }

        public virtual Test? Test { get; set; }

        public record TestItemId(Guid Id);

        public static TestItem Map(Models.Dtos.TestItem entity)
        {
            return new TestItem
            {
                Id = entity.Id,
                Method = entity.Method,
                Url = entity.Url,
            };
        }
    }
}
