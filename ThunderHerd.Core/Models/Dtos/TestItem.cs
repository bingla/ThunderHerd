using ThunderHerd.Core.Enums;
using ThunderHerd.Core.Models.Requests;

namespace ThunderHerd.Core.Models.Dtos
{
    public class TestItem
    {
        public Guid Id { get; set; }
        public HttpMethods Method { get; set; } = HttpMethods.GET;
        public string? Url { get; set; }

        public static TestItem Map(TestCreateRequest.TestItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));

            return new TestItem
            {
                Method = item.Method,
                Url = item.Url,
            };
        }

        public static TestItem Map(Entities.TestItem entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            return new TestItem
            {
                Id = entity.Id,
                Method = entity.Method,
                Url = entity.Url,
            };
        }
    }
}
