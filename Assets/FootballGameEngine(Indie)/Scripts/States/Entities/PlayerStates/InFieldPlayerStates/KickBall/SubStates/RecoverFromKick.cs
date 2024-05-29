using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Shared;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class RecoverFromKick : BState
    {
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            //set the wait time 
            waitTime = Globals.RecoverFromKickWaitTime;
        }

        public override void Execute()
        {
            base.Execute();

            //decrement time
            waitTime -= Time.deltaTime;

            //go to home after state
            if (waitTime <= 0f && Owner.Animator.GetCurrentAnimatorStateInfo(1).IsName("New State")) 
                SuperMachine.ChangeState<GoToHomeMainState>();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
