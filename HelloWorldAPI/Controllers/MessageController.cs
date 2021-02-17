using HelloWorldDomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace HelloWorldAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
        }

        public IActionResult Send(Message message)
        {
            try
            {
                // Fila
                var bus = new HelloWorldBus.BusService("localhost", "HelloWorldAPI");
                bus.Send(message);

                // Devolve objeto processado.
                return Accepted(message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao enviar mensagem.", ex);

                return new StatusCodeResult(500);
            }

        }
    }
}
