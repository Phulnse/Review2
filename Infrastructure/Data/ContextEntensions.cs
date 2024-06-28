using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Data
{
    internal static class ContextEntensions
    {
        public static List<Action<IMutableEntityType>> Conventions = new List<Action<IMutableEntityType>>();

        public static void AddRemovePluralizeBehavior(this ModelBuilder builder)
        {
            Conventions.Add(et => et.SetTableName(et.DisplayName()));
        }

        public static void AddRemoveOneToManyCascade(this ModelBuilder builder)
        {
            Conventions.Add(et => et.GetForeignKeys()
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict));
        }

        public static void ApplyConventions(this ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                foreach (Action<IMutableEntityType> action in Conventions)
                {
                    action(entity);
                }
            }

            Conventions.Clear();
        }
    }
}
