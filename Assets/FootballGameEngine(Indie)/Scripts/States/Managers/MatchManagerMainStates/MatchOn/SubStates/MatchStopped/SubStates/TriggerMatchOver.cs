using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOver.MainState;
using RobustFSM.Base;
using RobustFSM.Interfaces;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.SubStates
{
    public class TriggerMatchOver : BState
    {
        public override void Enter()
        {
            base.Enter();

            //got to Match Over
            SuperFSM.ChangeState<MatchOverMainState>();
        }

        /// <summary>
        /// Access the super state machine
        /// </summary>
        public IFSM SuperFSM
        {
            get
            {
                return (MatchManagerFSM)SuperMachine;
            }
        }

    }
}
