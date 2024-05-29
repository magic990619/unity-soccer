using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Init.MainState
{
    public class InitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // disable the widgets
            Owner.PlayerControlInfoWidget.Root.SetActive(false);
            Owner.PlayerDirectionInfoWidget.Root.SetActive(false);
            Owner.PlayerHealthInfoWidget.Root.SetActive(false);
            Owner.PlayerNameInfoWidget.Root.SetActive(false);

            //init
            Owner.Init();

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to some events
            Owner.OnInstructedToWait -= Instance_OnWait;

            // set the animator
            Owner.Animator.ResetTrigger("Idle");
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
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
