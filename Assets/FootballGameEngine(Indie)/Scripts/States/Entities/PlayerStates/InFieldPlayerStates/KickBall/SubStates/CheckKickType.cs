using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates
{
    public class CheckKickType : BState
    {
        public override void Enter()
        {
            base.Enter();

            //trigger thr right state transition
            if (Owner.KickDecision == KickDecisions.Pass)
                Machine.ChangeState<PassBall>();
            else if (Owner.KickDecision == KickDecisions.Shot)
                Machine.ChangeState<ShootBall>();
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
