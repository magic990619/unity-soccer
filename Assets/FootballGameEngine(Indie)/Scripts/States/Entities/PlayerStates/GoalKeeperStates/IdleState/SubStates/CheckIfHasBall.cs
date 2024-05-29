using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.SubStates
{
    public class CheckIfHasBall : BState
    {
        public override void Enter()
        {
            base.Enter();

            //if the player has ball then hold ball
            //else check if player has to tend goal
            if (Owner.HasBall)
                Machine.ChangeState<HoldBall>();
            else
                Machine.ChangeState<CheckIfIHaveToTendGoal>();
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
