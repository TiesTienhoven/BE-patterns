namespace Patterns.StateMachine
{
    partial class ConnectionManager : StateMachineManager<ConnectionStatus, ConnectionTriggers>
    {
        public ConnectionManager() : base(ConnectionStatus.Disconnected)
        {
        }

        protected override List<ConnectionTriggers> GetApiAvailableTriggers() => [ConnectionTriggers.Connect, ConnectionTriggers.Disconnect];

        protected override void ConfigureStateMachine()
        {
            // Configure the state machine here
            var errorTrigger = stateMachine.SetTriggerParameters<ConnectionError>(ConnectionTriggers.Error);

            stateMachine.Configure(ConnectionStatus.Disconnected)
                .OnEntryFrom(ConnectionTriggers.Disconnect, OnDisconnect)
                .OnEntryFrom(errorTrigger, OnError)
                .PermitIf(ConnectionTriggers.Connect, ConnectionStatus.Connecting, () => this.port != null)
                .Ignore(ConnectionTriggers.VerifyConnection)
                .Ignore(ConnectionTriggers.SuccesfullyConnected)
                .Ignore(ConnectionTriggers.Disconnect)
                .PermitReentry(ConnectionTriggers.Error);

            stateMachine.Configure(ConnectionStatus.Connecting)
                .OnEntryFrom(ConnectionTriggers.Connect, OnConnect)
                .Ignore(ConnectionTriggers.Connect)
                .Permit(ConnectionTriggers.VerifyConnection, ConnectionStatus.Veryfying)
                .Ignore(ConnectionTriggers.SuccesfullyConnected)
                .Permit(ConnectionTriggers.Disconnect, ConnectionStatus.Disconnected)
                .Permit(ConnectionTriggers.Error, ConnectionStatus.Disconnected);

            stateMachine.Configure(ConnectionStatus.Veryfying)
                .OnEntryFrom(ConnectionTriggers.VerifyConnection, OnVerifyConnection)
                .Ignore(ConnectionTriggers.Connect)
                .Ignore(ConnectionTriggers.VerifyConnection)
                .Permit(ConnectionTriggers.SuccesfullyConnected, ConnectionStatus.Connected)
                .Permit(ConnectionTriggers.Disconnect, ConnectionStatus.Disconnected)
                .Permit(ConnectionTriggers.Error, ConnectionStatus.Disconnected);

            stateMachine.Configure(ConnectionStatus.Connected)
                .OnEntryFrom(ConnectionTriggers.SuccesfullyConnected, OnConnected)
                .Ignore(ConnectionTriggers.Connect)
                .Ignore(ConnectionTriggers.VerifyConnection)
                .Ignore(ConnectionTriggers.SuccesfullyConnected)
                .Permit(ConnectionTriggers.Disconnect, ConnectionStatus.Disconnected)
                .Permit(ConnectionTriggers.Error, ConnectionStatus.Disconnected);
        }

        private static void OnError(ConnectionError error)
        {
            Console.WriteLine(error.ToString());
        }

        private void OnConnect()
        {
            // Simulate connection logic
            Console.WriteLine($"Connecting to {port}");
            // Simulate successful connection
            stateMachine.Fire(ConnectionTriggers.VerifyConnection);

        }

        private void OnVerifyConnection()
        {
            Console.WriteLine($"Verifying made connection is wanted machine...");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine($"...");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Console.WriteLine($"Connection verified");
            stateMachine.Fire(ConnectionTriggers.SuccesfullyConnected);
        }

        private void OnConnected()
        {
            Console.WriteLine($"Connected to {port}");
        }

        private void OnDisconnect()
        {
            Console.WriteLine($"Disconnecting from {port}...");
            port = null;
            Console.WriteLine($"Disconnected from {port}");
        }
    }
}
