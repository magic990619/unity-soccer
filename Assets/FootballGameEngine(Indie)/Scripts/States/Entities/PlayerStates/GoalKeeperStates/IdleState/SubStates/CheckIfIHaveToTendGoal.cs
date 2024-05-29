using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ProtectGoal.MainState;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.SubStates
{
    public class CheckIfIHaveToTendGoal : BState
    {
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Execute()
        {
            base.Execute();

            //if the ball is within threatening distance then tend goal
            if (Owner.IsBallPositionThreateningGoal())
                SuperMachine.ChangeState<ProtectGoalMainState>();
        }
    }
}
