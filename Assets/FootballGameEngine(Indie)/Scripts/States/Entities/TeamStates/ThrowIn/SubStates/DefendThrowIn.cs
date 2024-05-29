using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates
{
    /// <summary>
    /// Handles team behaviour if when it's the one defending the throw-in
    /// </summary>
    public class DefendThrowIn : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to opponent ontake-kick-off event
            Owner.Opponent.OnTakeThrowIn += Instance_OnOpponentTakeThrowIn;
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // trigger players to pickout threats
            Owner.TriggerPlayersToPickOutThreats();
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to opponent ontake-kick-off event
            Owner.Opponent.OnTakeThrowIn -= Instance_OnOpponentTakeThrowIn;
        }

        private void Instance_OnOpponentTakeThrowIn()
        {
            // tell every player that it's no longer a setpiece
            Owner.Players
                   .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer)
                   .ToList()
                   .ForEach(p =>
                   {
                        // set some stuff
                        p.Player.InFieldPlayerFSM.ChangeState<GoToHomeMainState>();
                   });

            // defend
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
