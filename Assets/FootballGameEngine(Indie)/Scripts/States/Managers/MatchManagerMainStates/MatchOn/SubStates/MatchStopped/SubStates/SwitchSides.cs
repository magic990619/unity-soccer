using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates
{
    public class SwitchSides : BState
    {
        public override void Enter()
        {
            base.Enter();

            //set-up the game scene
            if(Owner.CurrGameTime == GameTimeEnum.NormalTime)
            {
                Owner.CurrentHalf = 2;
                Owner.NextStopTime += Owner.NormalTimeHalfLength;
            }
            else if(Owner.CurrGameTime == GameTimeEnum.ExtraTime)
            {
                Owner.CurrentHalf = 2;
                Owner.NextStopTime += Owner.ExtraTimeHalfLength;
            }

            // rotate teams on pitch
            Owner.RootTeam.transform.Rotate(Owner.RootTeam.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

            // set the team's kickoff
            Owner.TeamAway.HasKickOff = !Owner.TeamAway.HasInitialKickOff;
            Owner.TeamHome.HasKickOff = !Owner.TeamHome.HasInitialKickOff;

            //got back to wait for kick-off state
            ((IState)Machine).Machine.ChangeState<BroadcastHalfStatus>();
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

        /// <summary>
        /// Access the owner of the state machine
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
