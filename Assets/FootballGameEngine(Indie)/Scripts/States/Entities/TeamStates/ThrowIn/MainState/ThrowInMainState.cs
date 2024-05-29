using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.MainState
{
    /// <summary>
    /// Handles team logic during a throw-in
    /// </summary>
    public class ThrowInMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<DefendThrowIn>();
            AddState<PrepareForThrowIn>();
            AddState<TakeThrowIn>();

            // set initial state
            SetInitialState<PrepareForThrowIn>();
        }
    }
}
