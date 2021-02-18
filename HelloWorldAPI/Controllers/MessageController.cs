using HelloWorldBus;
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
        private IBusService _busService;

        public MessageController(ILogger<MessageController> logger, IBusService busService)
        {
            _logger = logger;
            _busService = busService;
        }

        public IActionResult Send(Message message)
        {
            try
            {
                // Fila
                _busService.Initialize("localhost", "HelloWorldAPI");
                _busService.Send(message);

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
