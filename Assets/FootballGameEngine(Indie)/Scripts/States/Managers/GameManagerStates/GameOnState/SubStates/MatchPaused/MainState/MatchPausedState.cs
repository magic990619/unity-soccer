using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchPaused.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates
{
    public class MatchPausedState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<WaitForMatchToPauseState>();
            AddState<MatchPausedInfoState>();
            AddState<MatchTeamManagement>();

            // set initial state
            SetInitialState<WaitForMatchToPauseState>();
        }

        public override void Enter()
        {
            // disable children
            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .DisableChildren();

            // enable game paused menu
            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .Root
                .SetActive(true);


            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            // disable game paused menu
            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .Root
                .SetActive(false);
        }
    }
}
