using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates
{
    public class PickOutCornerKickThreat : BState
    {
        float _threatTrackDistance;
        Vector3 _target;
        Player _threat;

        public Player Threat { get => _threat; set => _threat = value; }

        public override void Enter()
        {
            base.Enter();

            // set some value
            _threatTrackDistance = 0.25f * Random.value;

            // set threat is picked out
            _threat.SupportSpot.SetIsPickedOut(Owner);

            //// set the animator
            Owner.Animator.SetTrigger("Move");

            // set steering point
            _target = GetTargetPosition();
            Owner.RPGMovement.SetRotateFacePosition(_target);
            Owner.RPGMovement.SetMoveTarget(_target);

            // enable steering
            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            //// go back to find threat if threat is not mine
            if (_threat.SupportSpot.Owner != Owner)
                Machine.ChangeState<FindCornerKickThreat>();

            // go to wait at threat target position
            if (Owner.IsAtTarget(_target))
            {
                Machine.GetState<WaitAtCornerKickThreatTarget>().Threat = _threat;
                Machine.ChangeState<WaitAtCornerKickThreatTarget>();
            }

        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // find a target
            _target = GetTargetPosition();

            // set steering point
            Owner.RPGMovement.SetRotateFacePosition(_target);
            Owner.RPGMovement.SetMoveTarget(_target);
        }

        public Vector3 GetTargetPosition()
        {
            return _threat.Position + (Owner.TeamGoal.Position - _threat.Position).normalized
            * (_threat.Radius + Owner.Radius + Random.value * _threatTrackDistance);
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
