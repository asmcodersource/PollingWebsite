using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PollingServer.Controllers.Authorization
{
    public class JsonWebTokenSettings
    {   
        public string Issuer { get; set; } = String.Empty;
        public string Audience { get; set; } = String.Empty;
        public string Key { get; set; } = String.Empty;
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    };
}
