using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.MainState
{
    public class TakeGoalKickMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // set states
            AddState<AutomaticTakeGoalKick>();
            AddState<ManualTakeGoalKick>();
            AddState<PrepareToTakeGoalKick>();
            AddState<RecoverFromKick>();

            // set initial state
            SetInitialState<PrepareToTakeGoalKick>();
        }

        public override void Enter()
        {
            base.Enter();

            // set the ball
            Ball.Instance.OwnerWithLastTouch = Owner;

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;

            // enable the player widget
            Owner.PlayerNameInfoWidget.Root.SetActive(true);
        }


        public override void Exit()
        {
            base.Exit();

            //listen to some events
            Owner.OnInstructedToWait -= Instance_OnWait;

            // disable the player widget
            Owner.PlayerNameInfoWidget.Root.SetActive(false);
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }

        Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
