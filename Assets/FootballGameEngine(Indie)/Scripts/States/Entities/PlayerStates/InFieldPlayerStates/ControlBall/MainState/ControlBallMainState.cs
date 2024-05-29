using System;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState
{
    public class ControlBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<AutomaticControl>();
            AddState<ChooseControlType>();
            AddState<ManualControl>();

            //set inital state
            SetInitialState<ChooseControlType>();
        }

        public override void Enter()
        {
            base.Enter();

            // set new speed
            Owner.RPGMovement.RotationSpeed *= Owner.Speed;
            Owner.RPGMovement.Speed *= Owner.Speed;

            //listen to game events
            Owner.OnTackled += Instance_OnTackled;
            Owner.OnInstructedToWait += Instance_OnWait;

            //set the ball to is kinematic
            Ball.Instance.CurrentOwner = Owner;
            Ball.Instance.OwnerWithLastTouch = Owner;
            Ball.Instance.IsAutoRotatingBall = false;
            Ball.Instance.Rigidbody.isKinematic = true;
            Owner.Team.ControllingPlayer = Owner;

            // raise event that I'm controlling the ball
            Owner.Invoke_OnControlBall();

            // enable the player widget
            Owner.PlayerControlInfoWidget.Root.SetActive(true);
            Owner.PlayerNameInfoWidget.Root.SetActive(true);
        }

        public override void Execute()
        {
            base.Execute();

            //place ball infront of me
            Owner.PlaceBallInfronOfMe();

            // rotate the ball
            Vector3 playerVelocity = Owner.RPGMovement.CharacterController.velocity;
            Ball.Instance.RotateBallModel(Ball.Instance.BallModel.transform.right, playerVelocity.magnitude * 200f);
        }

        public override void Exit()
        {
            base.Exit();

            // restore player speed
            Owner.RPGMovement.RotationSpeed = Owner.ActualRotationSpeed;
            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;

            //listen to game events
            Owner.OnTackled -= Instance_OnTackled;
            Owner.OnInstructedToWait -= Instance_OnWait;

            //unset the ball to is kinematic
            Ball.Instance.CurrentOwner = null;
            Ball.Instance.IsAutoRotatingBall = true;
            Ball.Instance.Rigidbody.isKinematic = false;
            Owner.Team.ControllingPlayer = null;

            // disable the player widget
            Owner.PlayerControlInfoWidget.Root.SetActive(false);
            Owner.PlayerNameInfoWidget.Root.SetActive(false);
        }

        public void Instance_OnTackled()
        {
            SuperMachine.ChangeState<TackledMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
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
