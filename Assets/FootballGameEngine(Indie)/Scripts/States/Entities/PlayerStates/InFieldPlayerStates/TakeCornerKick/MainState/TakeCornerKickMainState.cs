using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.MainState
{
    public class TakeCornerKickMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<AutomaticTakeCornerKick>();
            AddState<ManualTakeCornerKick>();
            AddState<PrepareToTakeCornerKick>();
            AddState<RecoverFromKick>();

            // set initial state
            SetInitialState<PrepareToTakeCornerKick>();
        }

        public override void Enter()
        {
            base.Enter();

            // listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listen to some events
            Owner.OnInstructedToWait -= Instance_OnWait;
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
