using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderHerd.Core.Enums;

namespace ThunderHerd.Core.Entities
{
    public class TestItem
    {
        public Guid Id { get; set; }
        public HttpMethods Method { get; set; } = HttpMethods.GET;
        public string? Url { get; set; }

        public virtual Run Run { get; set; }

        public record TestItemId(Guid Id);
    }
}
