using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using static Assets.FootballGameEngine_Indie.Scripts.Entities.Player;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState
{
    public class ChaseBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<AutomaticChase>();
            AddState<ChooseChaseType>();
            AddState<ManualChase>();

            // set initial state
            SetInitialState<ChooseChaseType>();
        }

        public override void Enter()
        {
            base.Enter();

            // listen to player events
            Owner.OnInstructedToWait += Instance_OnWait;
            Owner.OnIsNoLongerClosestPlayerToBall += Instance_OnIsNoLongerClosestPlayerToBall;

            // set the animator
            Owner.Animator.SetTrigger("Move");
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //if team is incontrol, raise the event that I'm chasing ball
            if (Owner.IsTeamInControl)
            {
                ChaseBallDel temp = Owner.OnChaseBall;
                if (temp != null)
                    temp.Invoke(Owner);
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister from listening to some events
            Owner.OnInstructedToWait -= Instance_OnWait;
            Owner.OnIsNoLongerClosestPlayerToBall -= Instance_OnIsNoLongerClosestPlayerToBall;

            // set the animator
            Owner.Animator.ResetTrigger("Move");
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }

        private void Instance_OnIsNoLongerClosestPlayerToBall()
        {
            Machine.ChangeState<GoToHomeMainState>();
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
