using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using RobustFSM.Base;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.Init.MainState;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.WaitState.MainState
{
    public class WaitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // register to the on initialize event
            Owner.OnInitialize += GoToInitMainState;
        }

        public override void Exit()
        {
            base.Exit();

            // deregister to the event
            Owner.OnInitialize -= GoToInitMainState;
        }

        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }

        void GoToInitMainState()
        {
            Machine.ChangeState<InitMainState>();
        }
    }
}
