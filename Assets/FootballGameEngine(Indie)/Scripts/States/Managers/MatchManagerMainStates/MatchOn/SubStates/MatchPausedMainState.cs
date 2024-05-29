using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchPaused.MainState
{
    public class MatchPausedMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            Owner.OnExitMatchPaused += Instance_ExitMatchPaused;

            Time.timeScale = 0f;
        }

        private void Instance_ExitMatchPaused()
        {
            Machine.ChangeState<ExhaustHalf>();
        }

        public override void Exit()
        {
            base.Exit();

            Time.timeScale = 1f;

            Owner.OnExitHalfTimeState -= Instance_ExitMatchPaused;

            ActionUtility.Invoke_Action(Owner.OnExitMatchPausedState);
        }

        /// <summary>
        /// Returns the owner of this instance
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
