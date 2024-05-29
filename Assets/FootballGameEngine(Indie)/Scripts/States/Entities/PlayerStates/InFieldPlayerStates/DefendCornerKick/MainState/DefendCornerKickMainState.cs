using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.MainState
{
    public class DefendCornerKickMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<FindCornerKickThreat>();
            AddState<FindRandomPointToDefendCornerKick>();
            AddState<PickOutCornerKickThreat>();
            AddState<SteerToDefendCornerKickPoint>();
            AddState<WaitAtCornerKickThreatTarget>();
            AddState<WaitAtDefendCornerKickPoint>();

            // set initial state
            SetInitialState<FindCornerKickThreat>();
        }

        public override void Enter()
        {
            base.Enter();

            //listen to variaus events
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToGoToHome += Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to variaus events
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToGoToHome -= Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToWait -= Instance_OnWait;
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        private void Instance_OnInstructedToGoToHome()
        {
            Machine.ChangeState<GoToHomeMainState>();
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
