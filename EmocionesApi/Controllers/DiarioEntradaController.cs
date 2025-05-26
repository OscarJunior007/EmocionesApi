using EmocionesApi.Data;
using EmocionesApi.Models;
using EmocionesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmocionesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiarioEntradaController : Controller
    {

        private readonly AppDbcontext _context;
        private IConfiguration _configuration;
        private AuthService _jwt;
        public DiarioEntradaController(AppDbcontext context, IConfiguration configuration, AuthService jwt)
        {
            _context = context;
            _configuration = configuration;
            _jwt = jwt;
        }

        [HttpPost("entrada")]
        [Authorize]

        public IActionResult saveDiario([FromBody] DiarioDTO model)
        {


            var identity = HttpContext.User.Identity as ClaimsIdentity;

            var rToken = _jwt.ValidarToken(identity);

            if (!rToken.succes) return rToken;

            User usuario = rToken.result;


            var nuevaEntrada = new DIarioEntrada
            {

                EstadoAnimo = model.EstadoAnimo,
                Descripcion = model.Descripcion,
                Etiquetas = model.Etiquetas,
                UsuarioId = usuario.Id
            };

            _context.DiarioUser.Add(nuevaEntrada);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Diario almacenado exitosamente",
                Usuario = new
                {
                    nuevaEntrada.Id,
                    nuevaEntrada.EstadoAnimo,
                    nuevaEntrada.Etiquetas
                }
            });
        }

        [HttpPut("actualizar/{id}")]
        [Authorize]
        public IActionResult UpdateDiarioDescription(Guid id, [FromBody] editEntradaDTO model)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = _jwt.ValidarToken(identity);

            if (!rToken.succes) return rToken;

            User usuario = rToken.result;

            var entradaExistente = _context.DiarioUser
                .FirstOrDefault(e => e.Id == id && e.UsuarioId == usuario.Id);

            if (entradaExistente == null)
            {
                return NotFound(new { Message = "Entrada de diario no encontrada" });
            }

            entradaExistente.Descripcion = model.Descripcion;

            _context.DiarioUser.Update(entradaExistente);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Descripción actualizada exitosamente",
                Entrada = new
                {
                    entradaExistente.Id,
                    entradaExistente.Descripcion,
                    entradaExistente.EstadoAnimo,
                    entradaExistente.Etiquetas
                }
            });
        }

        [HttpDelete("eliminar/{id}")]
        [Authorize]
        public IActionResult EliminarEntrada(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = _jwt.ValidarToken(identity);

            if (!rToken.succes) return rToken;

            User usuario = rToken.result;

            var entrada = _context.DiarioUser
                .FirstOrDefault(e => e.Id == id && e.UsuarioId == usuario.Id);

            if (entrada == null)
            {
                return NotFound(new { Message = "Entrada no encontrada" });
            }

            _context.DiarioUser.Remove(entrada);
            _context.SaveChanges();

            return Ok(new { Message = "Entrada eliminada exitosamente" });
        }


        [HttpGet("diarios")]
        [Authorize]
        public IActionResult obtenerDiarios()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity.FindFirst("id").Value);

            var result = _jwt.ObtenerEntradasPorUsuario(userId);

            if (result.Result is NotFoundObjectResult)
            {
                return NotFound(result.Value);
            }

            return Ok(result.Value);

        }

        [HttpGet("diarios/recientes")]
        [Authorize]
        public IActionResult obtenerEntradasRecientes()
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity.FindFirst("id").Value);

            var result = _jwt.ObtenerEntradasRecientes(userId);

            if (result.Result is NotFoundObjectResult)
            {
                return NotFound(result.Value);
            }

            return Ok(result.Value);

        }
    }
}
