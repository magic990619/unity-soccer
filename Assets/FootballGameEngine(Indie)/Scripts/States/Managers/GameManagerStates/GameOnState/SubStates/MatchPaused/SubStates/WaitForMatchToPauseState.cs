using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using System;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchPaused.SubStates
{
    public class WaitForMatchToPauseState : BState
    {
        Action OnPauseMatch;

        public override void Enter()
        {
            base.Enter();

            // set the Match Manager to listen to some GameManager events
            OnPauseMatch += MatchManager.Instance.Instance_OnMatchPlayPaused;

            // register to match manager events
            MatchManager.Instance.OnEnterMatchPausedState += Instance_OnEnterMatchPausedState;

            // invoke the match paused
            ActionUtility.Invoke_Action(OnPauseMatch);
        }

        public override void Exit()
        {
            base.Exit();

            // deregister the Match Manager to listen to some GameManager events
            OnPauseMatch -= MatchManager.Instance.Instance_OnMatchPlayPaused;

            // deregister to events
            MatchManager.Instance.OnEnterMatchPausedState -= Instance_OnEnterMatchPausedState;

        }

        private void Instance_OnEnterMatchPausedState(string message)
        {
            // init match-info-state
            Machine.GetState<MatchPausedInfoState>().Message = message;
            Machine.ChangeState<MatchPausedInfoState>();
        }
    }
}
