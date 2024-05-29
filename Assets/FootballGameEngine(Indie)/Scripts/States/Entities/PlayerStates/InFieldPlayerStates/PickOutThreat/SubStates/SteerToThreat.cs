using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class SteerToThreat : BState
    {
        bool _isSprinting;

        float _steeringSpeed;

        Vector3 _steeringTarget;

        Player _threat;

        public bool IsSprinting { get => _isSprinting; set => _isSprinting = value; }
        public float SteeringSpeed { get => _steeringSpeed; set => _steeringSpeed = value; }
        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }
        public Vector3 SteeringTarget { get => _steeringTarget; set => _steeringTarget = value; }

        public override void Enter()
        {
            base.Enter();

            //set steering on
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            Owner.RPGMovement.Speed = _steeringSpeed;
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();

            // set the animator
            Owner.Animator.SetTrigger("Move");
        }

        public override void Execute()
        {
            base.Execute();

            //check if now at target and switch to wait for ball
            if (Owner.IsAtTarget(_steeringTarget) == true)
            {
                Machine.ChangeState<WaitAtThreatPosition>();
            }

            // update the animator
            if (_isSprinting == true)
                Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f);
            else
                Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //update the steering to target
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            Owner.RPGMovement.Speed = _steeringSpeed;
        }

        public override void Exit()
        {
            base.Exit();

            //set steering off
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // reset the animator
            Owner.Animator.SetFloat("Forward", 0f);
            Owner.Animator.ResetTrigger("Move");
        }
    }
}
