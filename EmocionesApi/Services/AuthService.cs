using EmocionesApi.Data;
using EmocionesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmocionesApi.Services
{
    public class AuthService
    {
        private readonly AppDbcontext _context;

        public AuthService(AppDbcontext context)
        {
            _context = context;

        }
        public dynamic ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        succes = false,
                        message = "Verifica si estas enviando un token valido",
                        result = ""
                    };
                }

                var idClaim = identity.Claims.FirstOrDefault(x => x.Type == "id");
                var id = Guid.Parse(idClaim.Value);

                var usuarioFind = _context.Usuarios.FirstOrDefault(x => x.Id == id);

                if (usuarioFind == null)
                {
                    return new
                    {
                        succes = false,
                        message = "Usuario no encontrado",
                        result = ""
                    };
                }

                return new
                {
                    succes = true,
                    message = "Token válido",
                    result = usuarioFind
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    succes = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }

        public ActionResult<List<DIarioEntrada>> ObtenerEntradasPorUsuario(Guid usuarioId)
        {
            try
            {
                var entradas = _context.DiarioUser
                                .Where(x => x.UsuarioId == usuarioId)
                                .OrderByDescending(x => x.FechaHora)
                                .ToList();

                if (!entradas.Any())
                {
                    return new NotFoundObjectResult(new
                    {
                        Success = false,
                        Message = "No se encontraron entradas para este usuario"
                    });
                }

                return entradas;
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }

        public ActionResult<List<DIarioEntrada>> ObtenerEntradasRecientes(Guid usuarioId)
        {
            try
            {
                var entradas = _context.DiarioUser
                                .Where(x => x.UsuarioId == usuarioId)
                                .OrderByDescending(x => x.FechaHora)
                                .Take(5)
                                .ToList();

                if (!entradas.Any())
                {
                    return new NotFoundObjectResult(new
                    {
                        Success = false,
                        Message = "No se encontraron entradas para este usuario"
                    });
                }

                return entradas;
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
