using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.SubStates
{
    public class CatchBall : BState
    {
        public override void Enter()
        {
            base.Enter();

            // stop the ball
            Ball.Instance.DisablePhysics();
            Ball.Instance.Trap();

            //ToDo::attach ball to player hand
            //Ball.Instance.transform.SetParent(Owner.transform);

            //set the animator to exit the dive state
            //Owner.Animator.SetTrigger("Exit");

            // raise that I now have control of the ball
            ActionUtility.Invoke_Action(Owner.OnGoalKeeperGainedControlOfBall);
        }

        public override void Execute()
        {
            base.Execute();

            // get bone positions
            //Vector3 leftHandBonePosition = Owner.Animator.GetBoneTransform(HumanBodyBones.LeftHand).transform.position;
            //Vector3 rightHandBonePosition = Owner.Animator.GetBoneTransform(HumanBodyBones.RightHand).transform.position;

            //// the ball is always between the player hands
            //Ball.Instance.Position = Vector3.Lerp(leftHandBonePosition, rightHandBonePosition, 0.5f);
        }

        Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
