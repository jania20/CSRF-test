using Microsoft.AspNetCore.Mvc;

namespace CSRF_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        /// <summary>
        /// Inicia sesión estableciendo una cookie de sesión
        /// </summary>
        /// <param name="user">Nombre de usuario (opcional, por defecto "guest")</param>
        /// <returns>Página HTML confirmando el login</returns>
        [HttpGet("login")]
        [ProducesResponseType(typeof(string), 200, "text/html")]
        public IActionResult Login([FromQuery] string? user)
        {
            if (string.IsNullOrWhiteSpace(user)) user = "guest";

            // Establecemos cookie de sesión (HttpOnly).
            // Intencionalmente NO establecemos SameSite ni Secure (solo PARA DEMO local).
            // SameSite=None permitiría cookies cross-site, pero para esta demo local usamos Lax
            Response.Cookies.Append("session_user", user, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax, // Permite cookies en requests del mismo sitio y navegación
                // Secure = false (for local http)
                Path = "/", // Asegurar que la cookie esté disponible en toda la aplicación
            });

            var html = $@"<html><body>
                <h3>Logged in as <b>{System.Net.WebUtility.HtmlEncode(user)}</b></h3>
                <p>Cookie <code>session_user</code> establecida (HttpOnly).</p>
                <p>Usa /api/transfer?amount=NNN para transferir (demo vulnerable).</p>
                <p><a href=""/api/ledger"">Ver ledger</a></p>
                </body></html>";

            return Content(html, "text/html");
        }

        /// <summary>
        /// Obtiene el usuario actualmente autenticado (basado en la cookie)
        /// </summary>
        /// <returns>Información del usuario autenticado en formato JSON</returns>
        [HttpGet("me")]
        [ProducesResponseType(typeof(object), 200, "application/json")]
        public IActionResult GetCurrentUser()
        {
            var user = Request.Cookies["session_user"];
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { error = "No autenticado", user = (string?)null });
            }

            return Ok(new { user = user, authenticated = true });
        }

        //logout (borra cookie)
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("session_user");
            return Content("<p>Logged out</p>", "text/html");
        }
    }
}
