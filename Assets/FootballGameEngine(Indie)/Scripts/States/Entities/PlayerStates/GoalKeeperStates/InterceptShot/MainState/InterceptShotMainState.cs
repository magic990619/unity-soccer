using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.MainState
{
    public class InterceptShotMainState : BHState
    {
        Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<CheckIfIShouldDiveOrMoveToInterceptPoint>();
            AddState<DiveToInterceptPoint>();
            AddState<MoveToInterceptPoint>();

            // set initial state
            SetInitialState<CheckIfIShouldDiveOrMoveToInterceptPoint>();
        }

        public void MoveHandsToIkTarget(float leftHandWeight, float rightHandWeight, float lookAtWeight, Vector3 leftHandPosition, Vector3 rightHandPosition)
        {
            //set the animations weights
            Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            Owner.Animator.SetLookAtWeight(lookAtWeight);

            //set the animations positions
            Owner.Animator.SetLookAtPosition(Ball.Instance.Position);
            Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosition);
            Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPosition);
        }

        public void MoveModelRoot(float initial, float final, float speed)
        {
            // move towards the jump height
            float positionY = Mathf.MoveTowards(initial, final, speed * Time.deltaTime);

            // set the movement position for the model root
            Vector3 localPosition = new Vector3(Owner.ModelRoot.transform.localPosition.x,
                positionY,
                Owner.ModelRoot.transform.localPosition.z);

            // move the model root
            Owner.ModelRoot.transform.localPosition = localPosition;
        }
    }
}
