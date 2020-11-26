namespace Common
{
    public class GameStateMachine
    {
        private IState _current;
        
        public void Enter<Tstate>(Tstate state) where Tstate : IState
        {
            _current.Exit();
            state.Enter();
            _current = state;
        }
    }
}