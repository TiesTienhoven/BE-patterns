using Stateless;

namespace Patterns.StateMachine
{
    public partial class ConnectionManager
    {
        private string? port;

        public Connection GetConnection() => new(port, stateMachine.State);

        internal Connection UpdateConnection(Connection connectionUpdate)
        {
            // Unguarded because example
            port = connectionUpdate.Port;

            var directTriggers = GetDirectTriggers(connectionUpdate.Status);
            if (directTriggers.Count == 0)
            {
                throw new InvalidOperationException($"Transition from {stateMachine.State} to {connectionUpdate.Status} is not allowed.");
            }
            else if (directTriggers.Count == 1)
            {
                stateMachine.Fire(directTriggers.Single());
            }
            else
            {
                 
                // Determine from other properties which trigger to fire.
                // For example, if we support COM and network at the same time
                // We could get port = COM1 or port = 127.0.0.1 
                // Where one could trigger ConnectToCom and the other ConnectToNetwork
            }
            return GetConnection();
        }

    }
}
