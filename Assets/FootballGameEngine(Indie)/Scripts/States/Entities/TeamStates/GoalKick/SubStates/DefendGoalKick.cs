using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.SubStates
{
    public class DefendGoalKick : BState
    {
        public override void Enter()
        {
            base.Enter();

            // listen to opp ontakegoalkick event
            Owner.Opponent.OnTakeGoalKick += Instance_OnOpponentTakeGoalKick;
        }

        public override void Exit()
        {
            base.Exit();

            // delisten to opp ontakegoalkick event
            Owner.Opponent.OnTakeGoalKick -= Instance_OnOpponentTakeGoalKick;
        }

        private void Instance_OnOpponentTakeGoalKick()
        {
            SuperMachine.ChangeState<DefendMainState>();
        }

        public FootballGameEngine_Indie.Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
