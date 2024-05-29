using System;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.SubStates
{
    public class WaitForKickOff : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to opponent ontake-kick-off event
            Owner.Opponent.OnTakeKickOff += Instance_OnOpponentTakeKickOff;
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to opponent ontake-kick-off event
            Owner.Opponent.OnTakeKickOff -= Instance_OnOpponentTakeKickOff;
        }

        private void Instance_OnOpponentTakeKickOff()
        {
            SuperMachine.ChangeState<DefendMainState>();
        }

        public Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
