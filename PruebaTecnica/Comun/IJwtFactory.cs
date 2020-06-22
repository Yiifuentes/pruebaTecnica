using System.Security.Claims;
using System.Threading.Tasks;

namespace PruebaTecnica.Web.Comun
{
    public interface IJwtFactory
    {
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, string rolTexto);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
    }
}