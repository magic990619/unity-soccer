using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates
{
    public class SteerToDefendCornerKickPoint : BState
    {
        Vector3 _target;

        public override void Enter()
        {
            base.Enter();

            // set the animator
            Owner.Animator.SetTrigger("Move");

            // set steering
            Owner.RPGMovement.SetRotateFacePosition(_target);
            Owner.RPGMovement.SetMoveTarget(_target);
            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }


        public override void Execute()
        {
            base.Execute();

            // stop if at target
            if (Owner.IsAtTarget(_target))
                Machine.ChangeState<WaitAtDefendCornerKickPoint>();

            //update the animator
            Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
            Owner.Animator.SetFloat("Forward", 0f);
            Owner.Animator.ResetTrigger("Move");
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }

        public Vector3 Target { get => _target; set => _target = value; }
    }
}
