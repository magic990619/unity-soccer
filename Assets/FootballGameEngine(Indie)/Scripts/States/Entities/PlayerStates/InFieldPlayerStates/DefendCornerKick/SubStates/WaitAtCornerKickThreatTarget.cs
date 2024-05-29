using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates
{
    public class WaitAtCornerKickThreatTarget : BState
    {
        Player _threat;

        public Player Threat { get => _threat; set => _threat = value; }

        public override void Enter()
        {
            base.Enter();

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // set the steering
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.NormalizedPosition);
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // find a target
            Vector3 target = _threat.Position + (Owner.TeamGoal.Position - _threat.Position).normalized
                * (_threat.Radius + Owner.Radius + 0.25f);

            if (Owner.IsAtTarget(target) == false)
                Machine.ChangeState<PickOutCornerKickThreat>();
        }

        public override void Exit()
        {
            base.Exit();

            // reset the animator
            Owner.Animator.ResetTrigger("Idle");
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
