using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.Init.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.Init.MainState
{
    public class InitMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<Initialize>();
            AddState<WaitForMatchOnInstruction>();

            //set initial state
            SetInitialState<Initialize>();
        }

        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
