using EmocionesApi.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace EmocionesApi.Models
{
    public class Jwt
    {


        public string Key { get; set; }
        public string  Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }


       

        
    }
}
