using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.MainState;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.MainState
{
    public class InteractWithBallMainState : BHState
    {
        bool _isBallCatchable;
        bool _isBallInteractable;
        bool _isTwoHandsUsable;

        float _playerJumpSpeed;
        float _requiredJumpHeight;
        float _turn;
        float _weightMultiplier;

        Transform _leftHandTargetPosition;
        Transform _rightHandTargetPosition;

        public bool IsBallCatchable { get => _isBallCatchable; set => _isBallCatchable = value; }
        public bool IsBallInteractable { get => _isBallInteractable; set => _isBallInteractable = value; }
        public bool IsTwoHandsUsable { get => _isTwoHandsUsable; set => _isTwoHandsUsable = value; }
        
        public float PlayerJumpSpeed { get => _playerJumpSpeed; set => _playerJumpSpeed = value; }
        public float RequiredJumpHeight { get => _requiredJumpHeight; set => _requiredJumpHeight = value; }

        public Transform LeftHandTargetPosition { get => _leftHandTargetPosition; set => _leftHandTargetPosition = value; }
        public Transform RightHandTargetPosition { get => _rightHandTargetPosition; set => _rightHandTargetPosition = value; }

        Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        InterceptShotMainState _interceptShotMainState;

        public override void AddStates()
        {
            base.AddStates();

            // add states 
            AddState<CatchBall>();
            AddState<CheckRequiredBallInteraction>();
            AddState<PunchBall>();

            // set initial state
            SetInitialState<CheckRequiredBallInteraction>();
        }

        public override void Initialize()
        {
            base.Initialize();

            // get the intercept shot main state
            _interceptShotMainState = Machine.GetState<InterceptShotMainState>();
        }

        public override void Enter()
        {
            // set the ball
            Ball.Instance.OwnerWithLastTouch = Owner;

            GetState<CheckRequiredBallInteraction>().IsBallCatchable = _isBallCatchable;
            GetState<CheckRequiredBallInteraction>().IsBallInteractable = _isBallInteractable;

            // run substates
            base.Enter();
        }

        public override void Execute()
        {
            base.Execute();

            //go to idle state the moment the player gets into idle state
            //if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            //    Machine.ChangeState<IdleMainState>();
        }

        public override void Exit()
        {
            // set has ball
            Owner.HasBall = IsCurrentState<CatchBall>();

            base.Exit();

            // reset stuff
            ResetBallReference();
            ReSetTheAnimatorParameters();
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            //// declare the weight variables
            //float leftHandWeight;
            //float rightHandWeight;
            //float lookAtWeight;

            //// calculate the player IK
            //CalculateHandAndLookWeights(_turn,
            //                           out leftHandWeight,
            //                           out rightHandWeight,
            //                           out lookAtWeight);

            //// move the hands to ik target
            //_interceptShotMainState.MoveHandsToIkTarget(leftHandWeight,
            //                                            rightHandWeight,
            //                                            lookAtWeight,
            //                                            _leftHandTargetPosition.position,
            //                                            _rightHandTargetPosition.position);
        }

        public override void OnAnimatorExecuteMove()
        {
            base.OnAnimatorExecuteMove();

            // move the model root
            //_interceptShotMainState.MoveModelRoot(Owner.ModelRoot.localPosition.y,
            //                                      0f,
            //                                      _playerJumpSpeed);
        }

        public void CalculateHandAndLookWeights(float turn, out float leftHandWeight, out float rightHandWeight, out float lookAtWeight)
        {
            //declare the weights
            leftHandWeight = 0f;
            rightHandWeight = 0f;
            lookAtWeight = 0f;

            // adjust the weight multiplier depending on how the player has fallen back to the ground
            _weightMultiplier = Owner.ModelRoot.localPosition.y / _requiredJumpHeight;

            //choose which hands to effect
            if (_turn == 0f)
            {
                //set the weights
                leftHandWeight = _weightMultiplier;
                rightHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;
            }
            else if (_turn == -1)
            {
                //set the weights
                leftHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;

                // update right hand multiplier
                rightHandWeight = _isTwoHandsUsable ? _weightMultiplier : 0f;

            }
            else if (_turn == 1)
            {
                //set the weights
                rightHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;

                // update left hand multiplier
                leftHandWeight = _isTwoHandsUsable ? _weightMultiplier : 0f;
            }
        }

        public void ResetBallReference()
        {
            // reset the ball reference
            //_interceptShotMainState.ResetBallReference();
        }

        public void ReSetTheAnimatorParameters()
        {
            //set the dive animation to play
            Owner.Animator.SetBool("IsBallCatchable", false);
            Owner.Animator.SetBool("IsTwoHandsUsable", false);
            Owner.Animator.SetFloat("Height", 0f);
            Owner.Animator.SetFloat("Turn", 0f);
        }
    }
}
