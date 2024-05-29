using RobustFSM.Base;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.MainState;
using Assets.RobustFSM.Mono;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.ExitState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.SettingsState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.TrophyState.MainState;

namespace Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers
{
    public class GameManagerFSM : MonoFSM<GameManager>
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<ExitMainState>();
            AddState<GameOnMainState>();
            AddState<HomeMainState>();
            AddState<TrophyMainState>();
            AddState<InitMainState>();
            AddState<PrepareForMatchMainState>();
            AddState<SettingsMainState>();

            // set initial state
            SetInitialState<InitMainState>();
        }
    }
}
