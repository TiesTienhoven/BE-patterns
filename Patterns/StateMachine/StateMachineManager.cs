using Stateless;
using Stateless.Reflection;

namespace Patterns.StateMachine
{
    public abstract class StateMachineManager<TState, TTrigger>
    {
        protected StateMachine<TState, TTrigger> stateMachine;

        protected StateMachineManager(TState initialState)
        {
            stateMachine = new StateMachine<TState, TTrigger>(initialState);
            ConfigureStateMachine();
        }
        protected abstract List<TTrigger> GetApiAvailableTriggers();

        protected abstract void ConfigureStateMachine();

        public IReadOnlyList<TTrigger> GetDirectTriggers(TState startState, TState targetState)
        {
            var sourceState = stateMachine.GetInfo().States.FirstOrDefault(state => System.Convert.ToString(state.UnderlyingState) == System.Convert.ToString(startState));

            if (sourceState is null)
            {
                return Array.Empty<TTrigger>();
            }

            var directTriggers = sourceState.FixedTransitions
                .OfType<FixedTransitionInfo>()
                .Where(transition => System.Convert.ToString(transition.DestinationState) == System.Convert.ToString(targetState))
                .Select(transition => (TTrigger)transition.Trigger.UnderlyingTrigger);

            return directTriggers
                .Intersect(GetApiAvailableTriggers())
                .ToArray();
        }

        public IReadOnlyList<TTrigger> GetDirectTriggers(TState targetState)
            => GetDirectTriggers(stateMachine.State, targetState);

    }
}
