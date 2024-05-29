using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.SubStates
{
    /// <summary>
    /// Checks whether the player is at home position
    /// If not the SteerToHome state is enabled
    /// </summary>
    public class WaitAtHome : BState
    {
        bool _isAtTarget;
        float _randomWaitTime;

        public override void Enter()
        {
            base.Enter();

            // time to delay state change
            _randomWaitTime = Random.value * Owner.Speed;

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            // check if at target
            _isAtTarget = Owner.IsAtTarget(Owner.HomePosition.position);

            // decrement wait time
            if (_isAtTarget == false)
            {
                // decrement time
                if (_randomWaitTime > 0)
                    _randomWaitTime -= Time.deltaTime;
            }

            //update the track position
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.NormalizedPosition);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //steer if not at target
            if (_isAtTarget == false)
            {
                // trigger state change
                if(_randomWaitTime <= 0)
                    Machine.ChangeState<SteerToHome>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
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
