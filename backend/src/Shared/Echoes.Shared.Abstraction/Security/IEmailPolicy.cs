using System.Threading;
using System.Threading.Tasks;

namespace Echoes.Shared.Abstraction.Security
{
    public interface IEmailPolicy
    {
        public Task<ValidationResult> IsAllowedAsync(string email, CancellationToken ct = default);

        public string Normalize(string email);
    }
}
