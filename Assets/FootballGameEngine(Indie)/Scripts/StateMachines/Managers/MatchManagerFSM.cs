using Assets.RobustFSM.Mono;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.Init.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOver.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.WaitState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchPaused.MainState;

namespace Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers
{
    public class MatchManagerFSM : MonoFSM<MatchManager>
    {
        public override void AddStates()
        {
            //add the states
            AddState<InitMainState>();
            AddState<MatchOnMainState>();
            AddState<MatchOverMainState>();
            AddState<WaitMainState>();

            //set the initial state
            SetInitialState<WaitMainState>();
        }
    }
}
