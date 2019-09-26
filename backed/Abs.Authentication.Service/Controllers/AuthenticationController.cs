using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            this.logger = logger;
        }

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
            [FromQuery] string name
            //[FromQuery] string ticketAut,
            //[FromServices] TicketAutenticacionClient ticketClient,
            //[FromServices] UsuarioClient usuarioClient,
            //[FromServices] AccesoClient accesoClient
        )
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

            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { name });
            }

            var claims = GetTestUser(name);

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

        [HttpGet("has-access")]
        public async Task<IActionResult> HasAccess([FromQuery] string[] permissions)
        {
            logger.LogDebug("checking permissions for {user} and {@permissions}", User.Identity.Name, permissions);
            await Task.Delay(2000);
            return Ok(true);
        }

        private static IEnumerable<Claim> GetTestUser(string token)
        {
            var givennames = new[] { "Antonio", "Mario", "Juan" };
            var surnames = new[] { "Estrada", "Gutierrez", "Garcia" };

            var random = new Random();
            var i = random.Next(0, givennames.Length - 1);
            var j = random.Next(0, surnames.Length - 1);
            var name = givennames[i] + " " + surnames[j];
            var email = name.ToLower().Replace(" ", ".") + "@email.com";
            return new [] {
                new Claim(ClaimTypes.Name, token),
                new Claim(ClaimTypes.GivenName, name),
                new Claim(ClaimTypes.Email, email)
            };
        }
    }
}