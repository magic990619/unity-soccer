using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.SubStates
{
    public class ManualControl : BState
    {
        Pass _pass;
        Shot _shot;
        Transform _refObject;                 // A reference to the main camera in the scenes transform

        public override void Enter()
        {
            base.Enter();

            // enable the user controlled icon
            Owner.PlayerControlInfoWidget.Root.SetActive(true); 

            // set the animator
            Owner.Animator.SetTrigger("Move");
        }

        public override void Execute()
        {
            base.Execute();

            //capture input
            float horizontalRot = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
            float verticalRot = -ControlFreak2.CF2Input.GetAxisRaw("Vertical");

            //calculate the direction to rotate to
            Vector3 input = new Vector3(verticalRot, 0f, horizontalRot);

            // calculate camera relative direction to move:
            Vector3 Movement = input;

            if (ControlFreak2.CF2Input.GetButtonDown("LongPass"))
            {
                // set the direction of movement
                Vector3 direction = Movement == Vector3.zero ? Owner.transform.forward : Movement;

                // find pass in direction
                bool canPass = Owner.CanLongPassInDirection(direction, out _pass);

                // go to kick ball if can pass
                if (canPass)
                {
                    //go to kick-ball state
                    Owner.KickDecision = KickDecisions.Pass;
                    Owner.Pass = _pass;

                    SuperMachine.ChangeState<KickBallMainState>();
                }
            }
            else if (ControlFreak2.CF2Input.GetButtonDown("ShortPass/Press"))
            {
                // set the direction of movement
                Vector3 direction = Movement == Vector3.zero ? Owner.transform.forward : Movement;

                // find pass in direction
                bool canPass = Owner.CanShortPassInDirection(direction, out _pass);

                // go to kick ball if can pass
                if (canPass)
                {
                    //go to kick-ball state
                    Owner.KickDecision = KickDecisions.Pass;
                    Owner.Pass = _pass;

                    SuperMachine.ChangeState<KickBallMainState>();
                }
            }
            else if (ControlFreak2.CF2Input.GetButtonDown("Shoot"))
            {
                //fix shoot logic
                // check if I can score
                bool canScore = Owner.CanScore(out _shot, false, true);

                // shoot if I can score
                if (canScore)
                {
                    //go to kick-ball state
                    Owner.KickDecision = KickDecisions.Shot;
                    Owner.Shot = _shot;

                    SuperMachine.ChangeState<KickBallMainState>();
                }
                else
                {
                    // reconsider shot without considering the shot
                    // safety
                    canScore = Owner.CanScore(out _shot, false, false);

                    // shoot if I can score
                    if (canScore)
                    {
                        //go to kick-ball state
                        Owner.KickDecision = KickDecisions.Shot;
                        Owner.Shot = _shot;

                        SuperMachine.ChangeState<KickBallMainState>();
                    }
                }
            }
            else
            {
                //process if any key down
                if (input == Vector3.zero)
                {
                    // enable direction object
                    if(Owner.PlayerDirectionInfoWidget.Root.activeInHierarchy == true)
                        Owner.PlayerDirectionInfoWidget.Root.SetActive(false);

                    if (Owner.RPGMovement.Steer == true)
                        Owner.RPGMovement.SetSteeringOff();

                    if (Owner.RPGMovement.Track == true)
                        Owner.RPGMovement.SetTrackingOff();
                }
                else
                {
                    if (Owner.PlayerDirectionInfoWidget.Root.activeInHierarchy == false)
                        Owner.PlayerDirectionInfoWidget.Root.SetActive(true);

                    // set the movement
                    Vector3 moveDirection = Movement == Vector3.zero ? Vector3.zero : Owner.transform.forward;
                    Owner.RPGMovement.SetMoveDirection(moveDirection);
                    Owner.RPGMovement.SetRotateFaceDirection(Movement);

                    // rotate arrow
                    Owner.PlayerDirectionInfoWidget.Root.transform.rotation = Quaternion.Slerp(Owner.PlayerDirectionInfoWidget.Root.transform.rotation, Quaternion.LookRotation(new Vector3(Movement.x, 0f, Movement.z), Vector3.up), Time.deltaTime);

                    // check sprinting
                    bool isSprinting = ControlFreak2.CF2Input.GetButton("Sprint");
                    Owner.RPGMovement.Speed = isSprinting == true ? Owner.ActualSprintSpeed : Owner.ActualJogSpeed;

                    // set the steering to on
                    if (Owner.RPGMovement.Steer == false)
                        Owner.RPGMovement.SetSteeringOn();

                    if (Owner.RPGMovement.Track == false)
                        Owner.RPGMovement.SetTrackingOn();

                    // update animator
                    if (isSprinting == true)
                        Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f * Time.deltaTime);
                    else
                        Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f * Time.deltaTime);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // disable the user controlled icon
            if (Owner.PlayerDirectionInfoWidget.Root.activeInHierarchy == true)
                Owner.PlayerDirectionInfoWidget.Root.SetActive(false);
            Owner.PlayerControlInfoWidget.Root.SetActive(false);

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
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
