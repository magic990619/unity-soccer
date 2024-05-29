using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState
{
    /// <summary>
    /// The player finds the support spot for the controlling player
    /// </summary>
    public class SupportAttackerMainState : BHState
    {
        float _ballPower;
        float _ballTimeToTarget;

        Player _controllingPlayer;
        SupportSpot _supportSpot;

        public float BallPower { get => _ballPower; set => _ballPower = value; }
        public float BallTimeToTarget { get => _ballTimeToTarget; set => _ballTimeToTarget = value; }

        public Vector3 PositionInConsideration { get; set; }
        public Player ControllingPlayer { get => _controllingPlayer; set => _controllingPlayer = value; }
        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }
        public SupportSpot SupportSpot { get => _supportSpot; set => _supportSpot = value; }

        public override void AddStates()
        {
            base.AddStates();

            //add the states
            AddState<SteerToSupportSpot>();
            AddState<WaitAtSupportSpot>();

            //set the initial state
            SetInitialState<SteerToSupportSpot>();
        }

        public override void Enter()
        {
            base.Enter();

            // set some stuff
            _supportSpot.SetIsPickedOut(Owner);

            //listen to variaus events
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnGoToSupportSpot += Instance_OnGoToSupportSpot;
            Owner.OnInstructedToReceiveBall += Instance_OnInstructedToReceiveBall;
            Owner.OnNoSupportSpotFound += Instance_OnNoSupportSpotFound;
            Owner.OnTeamLostControl += Instance_OnTeamLostControl;
            Owner.OnInstructedToWait += Instance_OnWait;

            //find support spot
            CalculateSteeringTargetAndSpeed();
        }

        public override void Execute()
        {
            base.Execute();

#if UNITY_EDITOR

            // draw line to home region
            Debug.DrawLine(Owner.Position + Vector3.up, Owner.HomePosition.position + Vector3.up, Color.green);

            // draw line to support spot
            if (SupportSpot != null)
            {
                Debug.DrawLine(Owner.HomePosition.position + Vector3.up, SupportSpot.transform.position + Vector3.up, Color.blue);
                Debug.DrawLine(Owner.Position + Vector3.up, SupportSpot.transform.position + Vector3.up, Color.red);
            }
#endif
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if I'm too far from my home position, I should go back
            bool isTooFarFromHomePosition = Owner.IsPositionWithinWanderRadius(Owner.Position) == false;
            if (isTooFarFromHomePosition == true)
                SuperMachine.ChangeState<GoToHomeMainState>();
            else
                CalculateSteeringTargetAndSpeed();
        }

        public override void Exit()
        {
            base.Exit();

            // reset support spot
            _supportSpot.SetIsNotPickedOut();
            _supportSpot = null;

            //stop listening to variaus events
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnGoToSupportSpot -= Instance_OnGoToSupportSpot;
            Owner.OnInstructedToReceiveBall -= Instance_OnInstructedToReceiveBall;
            Owner.OnNoSupportSpotFound -= Instance_OnNoSupportSpotFound;
            Owner.OnTeamLostControl -= Instance_OnTeamLostControl;
            Owner.OnInstructedToWait -= Instance_OnWait;
        }

        private void CalculateSteeringTargetAndSpeed()
        {
            // calculate the steer target
            Vector3 steeringTarget = CalculateSteeringTarget();
            float steeringSpeed = CalculateSteeringSpeed(steeringTarget);
            bool isSprinting = steeringSpeed == Owner.ActualSprintSpeed;

            // set the child states data
            GetState<SteerToSupportSpot>().IsSprinting = isSprinting;
            GetState<SteerToSupportSpot>().SteeringSpeed = steeringSpeed;
            GetState<SteerToSupportSpot>().SteeringTarget = steeringTarget;

            GetState<WaitAtSupportSpot>().SteeringTarget = steeringTarget;
        }

        private Vector3 CalculateSteeringTarget()
        {
            return _supportSpot.Position;
        }

        private float CalculateSteeringSpeed(Vector3 _steeringTarget)
        {
            // find time of ball to threat
            float powerToReachTarget = Ball.Instance.FindPower(Ball.Instance.Position, _supportSpot.Position, Owner.BallShortPassArriveVelocity);
            float timeOfBallToTarget = Ball.Instance.TimeToCoverDistance(Ball.Instance.Position,
                _supportSpot.Position,
                powerToReachTarget);

            // find my time to steering target
            float timeOfMeToTarget = Ball.Instance.TimeToCoverDistance(Owner.Position,
                _steeringTarget,
                Owner.ActualJogSpeed,
                false);

            // decide to sprint
            if (timeOfBallToTarget < timeOfMeToTarget)
            {
                // return sprint speed
                return Owner.ActualSprintSpeed;
            }
            else
            {
                // return jog speed
                return Owner.ActualJogSpeed;
            }
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        private void Instance_OnGoToSupportSpot(Player controllingPlayer, SupportSpot supportSpot)
        {
            _supportSpot.SetIsNotPickedOut();

            _controllingPlayer = controllingPlayer;
            _supportSpot = supportSpot;

            _supportSpot.SetIsPickedOut(Owner);
        }

        public void Instance_OnInstructedToReceiveBall(float time, Vector3 position)
        {
            //get the receive ball state and init the steering target
            Machine.GetState<ReceiveBallMainState>().SetSteeringTarget(time, position);
            Machine.ChangeState<ReceiveBallMainState>();
        }

        private void Instance_OnNoSupportSpotFound()
        {
            Machine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnTeamLostControl()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }
    }
}
