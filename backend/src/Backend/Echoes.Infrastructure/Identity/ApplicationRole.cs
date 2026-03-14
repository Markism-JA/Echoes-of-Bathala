using Microsoft.AspNetCore.Identity;

namespace Echoes.Infrastructure.Identity
{
    /// <summary>
    /// Represents the custom role structure for the application, using a <see cref="Guid"/>
    /// as the primary key for consistency across the distributed system.
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid> { }
}
