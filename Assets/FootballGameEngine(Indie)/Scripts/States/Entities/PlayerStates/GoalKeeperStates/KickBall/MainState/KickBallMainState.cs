using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.KickBall.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.KickBall.MainState
{
    public class KickBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<PassBall>();
            AddState<RecoverFromKick>();

            //set initial state
            SetInitialState<PassBall>();
        }
    }
}
