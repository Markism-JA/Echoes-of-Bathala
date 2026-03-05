using System.Threading.Tasks;

namespace Echoes.Shared.Abstraction.Security
{
    public interface IPasswordPolicy
    {
        public Task<ValidationResult> IsAllowedAsync(
            string password,
            string? username = null,
            string? email = null
        );
    }
}
