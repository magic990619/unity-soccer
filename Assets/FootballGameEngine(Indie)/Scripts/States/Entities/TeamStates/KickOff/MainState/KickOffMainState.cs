using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.MainState
{
    public class KickOffMainState : BHState
    {
        public override void AddStates()
        {
            //add the states
            AddState<TakeKickOff>();
            AddState<PrepareForKickOff>();
            AddState<WaitForKickOff>();

            //set the initial state
            SetInitialState<PrepareForKickOff>();
        }
    }
}
