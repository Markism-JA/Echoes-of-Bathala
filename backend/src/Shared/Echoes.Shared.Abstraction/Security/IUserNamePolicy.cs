using System.Threading;
using System.Threading.Tasks;

namespace Echoes.Shared.Abstraction.Security
{
    public interface IUserNamePolicy
    {
        public Task<ValidationResult> IsAllowedAsync(
            string username,
            CancellationToken ct = default
        );

        public string Normalize(string username);
    }
}
