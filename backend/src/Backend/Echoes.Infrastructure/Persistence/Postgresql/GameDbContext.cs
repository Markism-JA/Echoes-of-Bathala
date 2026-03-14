using System.Linq.Expressions;
using Echoes.Domain.Common;
using Echoes.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Infrastructure.Persistence.Postgresql
{
    /// <summary>
    /// The main database context for the Echoes PostgreSql Database.
    /// </summary>
    public class GameDbContext(DbContextOptions<GameDbContext> options)
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
    {
        /// <summary>
        /// Configures the model mapping and applies global security/logic filters.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDbContext).Assembly);

            ApplyGlobalFilters(modelBuilder);
        }

        /// <summary>
        /// Dynamically applies a 'IsDeleted == false' filter to all entities implementing <see cref="ISoftDelete"/>.
        /// </summary>
        private void ApplyGlobalFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
                }
            }
        }

        private static LambdaExpression ConvertFilterExpression(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var comparison = Expression.Equal(property, Expression.Constant(false));
            return Expression.Lambda(comparison, parameter);
        }
    }
}
