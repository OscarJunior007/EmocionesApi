using EmocionesApi.Data;
using EmocionesApi.Models;
using EmocionesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmocionesApi.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private AuthService _jwt;

        private readonly AppDbcontext _context;
        private IConfiguration _configuration;

        public UserController(AppDbcontext context, IConfiguration configuration, AuthService jwt)
        {
            _context = context;
            _configuration = configuration;
            _jwt = jwt;

        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult getMe()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken =  _jwt.ValidarToken(identity);

            if (!rToken.succes) return Unauthorized(rToken);

            User usuario = rToken.result;
            
            var UseroOutDTO = new UseroOutDTO
            {

                Id = usuario.Id.ToString(),
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Rol = usuario.Rol
            };
            return Ok(new
            {
                Message = "usuario logeado",
                Usuario = new
                {
                    UseroOutDTO.Id,
                    UseroOutDTO.Nombre,
                    UseroOutDTO.Email,
                    UseroOutDTO.Rol
                }
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistroDTO model)
        {
            if (_context.Usuarios.Any(u => u.Email == model.Email))
            {
                return BadRequest("El correo ya está registrado.");
            }

            var nuevoUsuario = new User
            {
                Id = Guid.NewGuid(),
                Rol = "default",
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                Password = model.Password
            };

            _context.Usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Usuario registrado exitosamente",
                Usuario = new
                {
                    nuevoUsuario.Id,
                    nuevoUsuario.Nombre,
                    nuevoUsuario.Apellido,
                    nuevoUsuario.Email,
                    nuevoUsuario.Rol
                }
            });
        }

     

        [HttpPost("Login")]

        public IActionResult IniciarSesion([FromBody] LoginDTO optData)
        {
          

            string email = optData.Email;
            string password = optData.Password;
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == email && x.Password == password);

            if (usuario == null)
            {
                return BadRequest(new
                {
                    succes = false,
                    message = "Credenciales incorrectas",
                    result = ""
                });

               
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim("id",usuario.Id.ToString()),
                new Claim("usuario",usuario.Nombre),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials:singIn

                );
            return Ok(new
            {
                succes = true,
                message = "Exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }


    }
}
