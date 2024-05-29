using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ChaseBall.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.PutBallBackIntoPlay.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.PutBallBackIntoPlay.MainState
{
    public class PutBallBackIntoPlayMainState : BHState
    {
        public Player Owner { get => ((GoalKeeperFSM) SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<RecoverFromKick>();
            AddState<TakeCounterKick>();

            // set initial state
            SetInitialState<TakeCounterKick>();
        }

        public override void Enter()
        {
            base.Enter();

            //listen to variaus events
            Owner.OnInstructedToGoToHome += Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToWait += Instance_OnWait;

            // enable the player widget
            Owner.PlayerNameInfoWidget.Root.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to variaus events
            Owner.OnInstructedToGoToHome -= Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToWait -= Instance_OnWait;

            // disable the player widget
            Owner.PlayerNameInfoWidget.Root.SetActive(false);
        }

        public void Instance_OnInstructedToGoToHome()
        {
            // go to home
            Machine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }
    }
}
