using System.Diagnostics.CodeAnalysis;

namespace Patterns.StateMachine
{
    public record Connection(string? Port, ConnectionStatus Status);

    public enum ConnectionStatus
    {
        Disconnected = 0,
        Connecting = 1,
        Veryfying = 2,
        Connected = 3,
    }

    public enum ConnectionTriggers
    {
        Connect = 0,
        VerifyConnection = 1,
        SuccesfullyConnected = 2,
        Disconnect = 3,
        Error = 4
    }

    public enum ConnectionError
    {
        PortNotFound = 0,
        ConnectionFailed = 1,
    }
}
