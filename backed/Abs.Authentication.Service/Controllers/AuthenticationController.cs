using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Minfin.SSO.Api.Models.TicketAutenticacion;
using Minfin.SSO.WebApi.Client.Clients;

namespace Abs.Authentication.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private const string AuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        private const string ClaimsNamespace = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";

        [HttpGet("")]
        [HttpGet("/")]
        public IActionResult Get()
        {
            var identity = User.Identity;
            var claims = User.Claims
                .ToDictionary(
                    claim => claim.Type.Replace(ClaimsNamespace, ""), 
                    claim => claim.Value
                );
            return Ok(new { identity.IsAuthenticated, identity.Name, claims });
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateAsync(
            [FromQuery] string ticketAut,
            [FromServices] TicketAutenticacionClient ticketClient,
            [FromServices] UsuarioClient usuarioClient,
            [FromServices] AccesoClient accesoClient)
        {
            //var result = ticketClient.Usar(ticketAut);

            //if (!result.Estado.Equals(EstadoRespuestaUsar.Ok))
            //{
            //    return BadRequest(result);
            //}

            //var acceso = accesoClient.Obtener(result.Nit);

            //if (!acceso)
            //{
            //    return BadRequest(new { result.Nit, acceso });
            //}

            //var usuario = usuarioClient.Obtener(result.Nit);

            //if (usuario == null || !usuario.Activo)
            //{
            //    return BadRequest(new { result.Nit, usuario });
            //}

            //var claims = new List<Claim>
            //{
            //  new Claim(ClaimTypes.Name, usuario.Nit),
            //  new Claim(ClaimTypes.GivenName, usuario.Nombre),
            //  new Claim(ClaimTypes.Email, usuario.Correo)
            //};

            var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name, "2252438K"),
              new Claim(ClaimTypes.GivenName, "Fernando Ruano"),
              new Claim(ClaimTypes.Email, "ioch@minfin.gob.gt")
            };

            var identity = new ClaimsIdentity(claims, AuthScheme);
            var properties = new AuthenticationProperties();
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(AuthScheme, principal, properties);

            return Redirect("/");
        }



        [HttpGet("signout")]
        public async Task<IActionResult> SignoutAsync()
        {
            await HttpContext.SignOutAsync(AuthScheme);
            return Redirect("/");
        }
    }
}