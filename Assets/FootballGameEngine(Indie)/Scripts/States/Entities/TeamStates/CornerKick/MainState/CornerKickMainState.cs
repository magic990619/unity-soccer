using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.MainState
{
    public class CornerKickMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<DefendCornerKick>();
            AddState<PrepareForCornerKick>();
            AddState<TakeCornerKick>();

            // set initial state
            SetInitialState<PrepareForCornerKick>();
        }
    }
}
