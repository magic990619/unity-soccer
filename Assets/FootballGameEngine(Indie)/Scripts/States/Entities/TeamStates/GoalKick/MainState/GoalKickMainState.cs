using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.MainState
{
    public class GoalKickMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states 
            AddState<DefendGoalKick>();
            AddState<PrepareForGoalKick>();
            AddState<TakeGoalKick>();

            // set initial state
            SetInitialState<PrepareForGoalKick>();
        }
    }
}
