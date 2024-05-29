using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.KickBall.SubStates
{
    public class PassBall : BState
    {
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();
  
            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;

            //// set the animator
            bool isLongPass = Owner.Pass.PassType == PassTypesEnum.Long;
            bool isShortPass = Owner.Pass.PassType == PassTypesEnum.Short;
            Owner.Animator.SetBool("IsLongPass", isLongPass);
            Owner.Animator.SetBool("IsShortPass", isShortPass);
            Owner.Animator.SetTrigger("PassBall");
        }

        public override void Exit()
        {
            base.Exit();

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset animator
            Owner.Animator.SetBool("IsLongPass", false);
            Owner.Animator.SetBool("IsShortPass", false);
            Owner.Animator.ResetTrigger("PassBall");
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            //make a normal pass to the player
            Owner.MakePass(Owner.Pass);

            //go to recover state
            Machine.ChangeState<RecoverFromKick>();
        }
    }
}
