using Microsoft.AspNetCore.Mvc;
using Patterns.StateMachine;

namespace Patterns.Controllers
{
    [ApiController]
    [Route("connection")]
    [Consumes("application/json")]
    public class ConnectionController(ConnectionManager connectionManager) : ControllerBase
    {

        [HttpGet]
        public Connection Get() => connectionManager.GetConnection();

        [HttpPatch]
        public Connection Update(Connection connectionUpdate) => connectionManager.UpdateConnection(connectionUpdate);
    }
}
