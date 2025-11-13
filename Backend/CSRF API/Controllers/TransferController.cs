using CSRF_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSRF_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class TransferController : ControllerBase
    {
        private readonly LedgerService _ledger;

        public TransferController(LedgerService ledger)
        {
            _ledger = ledger;
        }

        // VULNERABLE endpoint: usa GET para cambiar estado (intencional).
        //transferir
        [HttpGet("transfer")]
        public IActionResult Transfer([FromQuery] string amount, [FromQuery] string recipient)
        {
            var user = Request.Cookies["session_user"];
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized("No autenticado. Llama a /api/auth/login?user=tuNombre primero.");
            }

            if (string.IsNullOrEmpty(amount))
            {
                return BadRequest("Especifica ?amount=NNN");
            }

            if (string.IsNullOrEmpty(recipient))
            {
                return BadRequest("Especifica ?recipient=DESTINO");
            }

            var entry = $"User {user} transferred {amount} to {recipient} at {DateTime.UtcNow:u}";
            _ledger.Add(entry);

            var html = $@"<html><body>
                <p>Transferencia realizada: {System.Net.WebUtility.HtmlEncode(entry)}</p>
                <p><a href=""/api/ledger"">Ver ledger</a></p>
                </body></html>";

            return Content(html, "text/html");
        }

        // Mostrar ledger
        [HttpGet("ledger")]
        public IActionResult Ledger()
        {
            var items = _ledger.GetAll()
                .Select(i => System.Net.WebUtility.HtmlEncode(i));

            var html = "<html><body><h2>Ledger</h2><ul>";
            foreach (var it in items) html += $"<li>{it}</li>";
            html += "</ul></body></html>";

            return Content(html, "text/html");
        }

        // Reset ledger
        [HttpGet("reset")]
        public IActionResult Reset()
        {
            _ledger.Reset();
            return Content("<p>Ledger reseteado</p>", "text/html");
        }

        // Endpoint JSON para obtener transacciones (para el frontend)
        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            var user = Request.Cookies["session_user"];
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { error = "No autenticado" });
            }

            var items = _ledger.GetAll()
                .Where(i => i.Contains($"User {user}"))
                .Select(i => new
                {
                    entry = i,
                    date = ExtractDate(i),
                    amount = ExtractAmount(i),
                    recipient = ExtractRecipient(i)
                })
                .OrderByDescending(t => t.date)
                .Take(10)
                .ToList();

            return Ok(items);
        }

        // Endpoint JSON para obtener saldo (calculado desde transacciones)
        [HttpGet("balance")]
        public IActionResult GetBalance()
        {
            var user = Request.Cookies["session_user"];
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { error = "No autenticado" });
            }

            // Saldo inicial: 5000
            var initialBalance = 5000.0m;
            
            // Calcular saldo basado en transacciones del usuario
            var userTransactions = _ledger.GetAll()
                .Where(i => i.Contains($"User {user}"))
                .ToList();
            
            var totalTransferred = userTransactions
                .Select(t => ExtractAmount(t))
                .Sum();
            
            // Saldo actual = saldo inicial - total transferido
            var currentBalance = initialBalance + totalTransferred; // totalTransferred es negativo
            
            return Ok(new { balance = currentBalance, currency = "MXN" });
        }

        // Endpoint JSON para transferencia (alternativa al GET vulnerable)
        [HttpPost("transfer")]
        public IActionResult TransferPost([FromBody] TransferRequest request)
        {
            var user = Request.Cookies["session_user"];
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { error = "No autenticado" });
            }

            if (request == null || request.Amount <= 0)
            {
                return BadRequest(new { error = "Amount inválido" });
            }

            if (string.IsNullOrEmpty(request.Recipient))
            {
                return BadRequest(new { error = "Recipient requerido" });
            }

            var entry = $"User {user} transferred {request.Amount} to {request.Recipient} at {DateTime.UtcNow:u}";
            _ledger.Add(entry);

            return Ok(new { 
                success = true, 
                message = "Transferencia realizada",
                entry = entry
            });
        }

        private string ExtractDate(string entry)
        {
            // Extraer fecha del formato: "User X transferred Y to Z at 2025-01-15T10:30:00Z"
            var atIndex = entry.LastIndexOf(" at ");
            if (atIndex > 0)
            {
                return entry.Substring(atIndex + 4).Trim();
            }
            return DateTime.UtcNow.ToString("u");
        }

        private decimal ExtractAmount(string entry)
        {
            // Extraer monto del formato: "User X transferred Y to Z"
            var transferredIndex = entry.IndexOf(" transferred ");
            var toIndex = entry.IndexOf(" to ");
            if (transferredIndex > 0 && toIndex > transferredIndex)
            {
                var amountStr = entry.Substring(transferredIndex + 12, toIndex - transferredIndex - 12);
                if (decimal.TryParse(amountStr, out var amount))
                {
                    return -amount; // Negativo porque es una salida
                }
            }
            return 0;
        }

        private string ExtractRecipient(string entry)
        {
            // Extraer destinatario del formato: "User X transferred Y to Z at ..."
            var toIndex = entry.IndexOf(" to ");
            var atIndex = entry.LastIndexOf(" at ");
            if (toIndex > 0 && atIndex > toIndex)
            {
                return entry.Substring(toIndex + 4, atIndex - toIndex - 4).Trim();
            }
            return "Unknown";
        }
    }

    // Clase para el request de transferencia POST
    public class TransferRequest
    {
        public decimal Amount { get; set; }
        public string Recipient { get; set; } = string.Empty;
    }
}
