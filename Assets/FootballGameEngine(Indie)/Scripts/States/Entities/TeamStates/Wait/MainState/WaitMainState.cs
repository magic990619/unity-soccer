using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.MainState;
using RobustFSM.Base;
using System.Linq;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Wait.MainState
{
    public class WaitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // reset some stuff
            Owner.HasCornerKick = false;
            Owner.HasGoalKick = false;

            //listen to some team events
            Owner.OnMessagedToTakeCornerKick += Instance_OnMessagedToTakeCornerKick;
            Owner.OnMessagedToTakeGoalKick += Instance_OnMessagedToTakeGoalKick;
            Owner.OnMessagedToTakeKickOff += Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnMessagedToTakeThrowIn += Instance_OnMessagedToTakeThrowIn;

            // raise the team wait event
            ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);

        }

        public override void Execute()
        {
            base.Execute();

            // get players not in wait and keep trying to invoke them to go to wait
            bool isAnyInFieldPlayerNotInWaitState = Owner.Players
                .Any(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer && p.Player.InFieldPlayerFSM.IsCurrentState<PlayerStates.InFieldPlayerStates.Wait.MainState.WaitMainState>() == false);
            if (isAnyInFieldPlayerNotInWaitState)
                ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);

            bool isAnyGoalKeeeperPlayerNotInWaitState = Owner.Players
               .Any(p => p.Player.PlayerType == PlayerTypes.Goalkeeper && p.Player.GoalKeeperFSM.IsCurrentState<PlayerStates.GoalKeeperStates.Wait.MainState.WaitMainState>() == false);
            if (isAnyGoalKeeeperPlayerNotInWaitState)
                ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to some team events
            Owner.OnMessagedToTakeCornerKick -= Instance_OnMessagedToTakeCornerKick;
            Owner.OnMessagedToTakeGoalKick -= Instance_OnMessagedToTakeGoalKick;
            Owner.OnMessagedToTakeKickOff -= Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnMessagedToTakeThrowIn -= Instance_OnMessagedToTakeThrowIn;
        }

        private void Instance_OnMessagedToTakeCornerKick()
        {
            // check if I have the throw in
            Owner.HasCornerKick = Owner.Players.Any(tM => tM.Player == Ball.Instance.OwnerWithLastTouch) == false;

            // change state
            Machine.ChangeState<CornerKickMainState>();
        }

        private void Instance_OnMessagedToTakeGoalKick()
        {
            // check if I have the throw in
            Owner.HasGoalKick = Owner.Players.Any(tM => tM.Player == Ball.Instance.OwnerWithLastTouch) == false;

            // change state
            Machine.ChangeState<GoalKickMainState>();
        }

        private void Instance_OnMessagedSwitchToTakeKickOff()
        {
            Machine.ChangeState<KickOffMainState>();
        }

        private void Instance_OnMessagedToTakeThrowIn()
        {
            // check if I have the throw in
            Owner.HasThrowIn = Owner.Players.Any(tM => tM.Player == Ball.Instance.OwnerWithLastTouch) == false;

            // change state
            Machine.ChangeState<ThrowInMainState>();
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
