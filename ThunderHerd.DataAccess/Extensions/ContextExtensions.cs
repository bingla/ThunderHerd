using Microsoft.EntityFrameworkCore;

namespace ThunderHerd.DataAccess.Extensions
{
    public static class ContextExtensions
    {
        public static void UpdateProperties(this DbContext context, object target, object source)
        {
            foreach (var propertyEntry in context.Entry(target).Properties)
            {
                var property = propertyEntry.Metadata;

                // Skip shadow and key properties
                if (property.IsShadowProperty() || (propertyEntry.EntityEntry.IsKeySet && property.IsKey()))
                    continue;

                // Skip property update if source value is null
                var value = property.GetGetter().GetClrValue(source);
                if (value == default)
                    continue;

                propertyEntry.CurrentValue = value;
            }
        }
    }
}
