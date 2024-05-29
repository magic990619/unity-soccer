using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates
{
    public class WaitForCornerKickToComplete : BState
    {
        bool hasInvokedTakeCornerKickEvent;
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set hasn't invoked kick off event
            hasInvokedTakeCornerKickEvent = false;
            waitTime = 1f;

            //register the teams to listen to the take-off events
            Owner.OnBroadcastTakeCornerKick += Owner.TeamAway.Invoke_OnMessagedToTakeCornerKick;
            Owner.OnBroadcastTakeCornerKick += Owner.TeamHome.Invoke_OnMessagedToTakeCornerKick;

            //listen to team OnTakeKickOff events
            Owner.TeamAway.OnTakeCornerKick += Instance_OnTeamTakeCornerKick;
            Owner.TeamHome.OnTakeCornerKick += Instance_OnTeamTakeCornerKick;
        }

        public override void Execute()
        {
            base.Execute();

            if (hasInvokedTakeCornerKickEvent == false)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    hasInvokedTakeCornerKickEvent = true;
                    ActionUtility.Invoke_Action(Owner.OnBroadcastTakeCornerKick);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            //deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeCornerKick -= Owner.TeamAway.Invoke_OnMessagedToTakeCornerKick;
            Owner.OnBroadcastTakeCornerKick -= Owner.TeamHome.Invoke_OnMessagedToTakeCornerKick;

            //delisten to team OnTakeKickOff events
            Owner.TeamAway.OnTakeCornerKick -= Instance_OnTeamTakeCornerKick;
            Owner.TeamHome.OnTakeCornerKick -= Instance_OnTeamTakeCornerKick;
        }

        private void Instance_OnTeamTakeCornerKick()
        {
            Machine.ChangeState<ExhaustHalf>();
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
