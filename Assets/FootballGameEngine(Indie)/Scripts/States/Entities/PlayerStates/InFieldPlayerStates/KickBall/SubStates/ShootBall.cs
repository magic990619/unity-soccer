using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class ShootBall : BState
    {
        public override void Enter()
        {
            base.Enter();

            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;

            // set the animator
            Owner.Animator.SetBool("IsShortPass", true);
            Owner.Animator.SetTrigger("ShootBall");
        }

        public override void Exit()
        {
            base.Exit();

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset animator
            Owner.Animator.ResetTrigger("ShootBall");
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            //make a shot
            Owner.MakeShot(Owner.Shot);

            //got to recover state
            Machine.ChangeState<RecoverFromKick>();
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
