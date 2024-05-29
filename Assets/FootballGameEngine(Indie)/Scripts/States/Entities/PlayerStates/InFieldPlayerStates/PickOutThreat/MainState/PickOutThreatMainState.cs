using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState
{
    public class PickOutThreatMainState : BHState
    {
        bool _isSetPiece;

        float _setPieceClearDistance = 9.15f;

        Player _threat;

        public bool IsSetPiece { get => _isSetPiece; set => _isSetPiece = value; }
        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }
        public Player Threat { get => _threat; set => _threat = value; }

        public override void AddStates()
        {
            base.AddStates();

            AddState<SteerToThreat>();
            AddState<WaitAtThreatPosition>();

            SetInitialState<SteerToThreat>();
        }

        public override void Enter()
        {
            base.Enter();

            // register that I have picked out threat
            _threat.SupportSpot.SetIsPickedOut(Owner);

            // register to on became closest player to ball
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnTeamGainedPossession += Instance_OnTeamGainedPossession;
            Owner.OnInstructedToWait += Instance_OnWait;

            // calculate steering target and speed
            CalculateSteeringTargetAndSpeed();
        }

        public override void Execute()
        {
            base.Execute();

#if UNITY_EDITOR

            // draw line to home region
            Debug.DrawLine(Owner.Position + Vector3.up, Owner.HomePosition.position + Vector3.up, Color.green);

            // draw line to support spot
            if (_threat != null)
                Debug.DrawLine(Owner.HomePosition.position + Vector3.up, _threat.Position + Vector3.up, Color.blue);
#endif
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if I'm too far from my home position, I should go back
            bool isTooFarFromHomePosition = Owner.IsPositionWithinWanderRadius(Owner.Position) == false;
            if (isTooFarFromHomePosition == true || _threat == null) 
                SuperMachine.ChangeState<GoToHomeMainState>();
            else
                CalculateSteeringTargetAndSpeed();
        }

        public override void Exit()
        {
            base.Exit();

            // reset the threat
            _threat.SupportSpot.SetIsNotPickedOut();
            _threat = null;

            // register to on became closest player to ball
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnTeamGainedPossession -= Instance_OnTeamGainedPossession;
            Owner.OnInstructedToWait -= Instance_OnWait;
        }

        public void CalculateSteeringTargetAndSpeed()
        {
            // calculate the steer target
            Vector3 steeringTarget = CalculateSteeringTarget();
            float steeringSpeed = CalculateSteeringSpeed(steeringTarget);
            bool isSprinting = steeringSpeed == Owner.ActualSprintSpeed;

            // set the child states data
            GetState<SteerToThreat>().IsSprinting = isSprinting;
            GetState<SteerToThreat>().SteeringSpeed = steeringSpeed;
            GetState<SteerToThreat>().SteeringTarget = steeringTarget;

            GetState<WaitAtThreatPosition>().SteeringTarget = steeringTarget;
        }

        public float CalculatePressHardness()
        {
            // press harder if the ball is closer to my goal than when it is further up the patch
            float pressHardness = Owner.ThreatTrackDistance * (1 - (Owner.TeamGoal.transform.InverseTransformPoint(Ball.Instance.Position).z / 90f));

            // return result
            return pressHardness;
        }

        public float CalculateSteeringSpeed(Vector3 _steeringTarget)
        {
            // find time of ball to threat
            float powerToReachTarget = Ball.Instance.FindPower(Ball.Instance.Position, _threat.Position, Owner.BallShortPassArriveVelocity);
            float timeOfBallToTarget = Ball.Instance.TimeToCoverDistance(Ball.Instance.Position,
                _threat.Position,
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

        public Vector3 CalculateSteeringTarget()
        {
            if (_isSetPiece == true)
            {
                //find direction to goal
                Vector3 directionOfThreatToGoal = Owner.TeamGoal.Position - _threat.Position;

                //the spot is somewhere between the threat and my goal
                Vector3 steeringTarget = _threat.Position
                    + directionOfThreatToGoal.normalized
                    * (Owner.ThreatTrackDistance + Owner.Radius + _threat.Radius);

                // check if threat is too close to ball
                bool isThreatTooCloseToBall = Vector3.Distance(steeringTarget, Ball.Instance.NormalizedPosition) <= _setPieceClearDistance;

                // if we are too close to the ball then move away from it
                if (isThreatTooCloseToBall)
                {
                    // find the direction from ball to threat
                    Vector3 directionOfBallToThreat = _threat.Position - Ball.Instance.NormalizedPosition;

                    // update the steering target
                    steeringTarget = Ball.Instance.NormalizedPosition + directionOfBallToThreat.normalized * (_setPieceClearDistance + Random.value * 1f);
                }

                //return result
                return steeringTarget;
            }
            else
            {
                // find direction of ball to threat and ball to goal
                Vector3 directionOfBallToGoal = Owner.TeamGoal.Position - Ball.Instance.Position;
                Vector3 directionOfBallToThreat = _threat.Position - Ball.Instance.Position;

                // check if threat and goal are in the same direction from ball
                float dotDirOfBallToGoalAndDirOfBallToThreat = Vector3.Dot(directionOfBallToGoal.normalized, directionOfBallToThreat.normalized);
                bool isBallAndThreatInSameDirection = dotDirOfBallToGoalAndDirOfBallToThreat > 0;

                // if goal and threat are in the same direction then decide to block pass to threat or block path to goal
                // else block pass path to threat
                if (isBallAndThreatInSameDirection == true)
                {
                    // check if goal and threat are ahead of me
                    Vector3 directionOfMeToGoal = Owner.TeamGoal.Position - Owner.Position;
                    Vector3 directionOfMeToThreat = _threat.Position - Owner.Position;

                    float dotDirOfMeToGoalAndDirOfMeToThreat = Vector3.Dot(directionOfMeToGoal.normalized, directionOfMeToThreat.normalized);
                    bool isGoalAndThreatAheadOfMe = dotDirOfMeToGoalAndDirOfMeToThreat > 1;

                    // if goal and threat are ahead of me then try to block path to ball
                    // else block path to goal
                    if (isGoalAndThreatAheadOfMe == true)
                    {
                        return CalculateSteeringTargetToBlockPassToThreat();
                    }
                    else
                    {
                        return CalculateSteeringTargetToBlockPathToGoal();
                    }
                }
                else
                {
                    return CalculateSteeringTargetToBlockPassToThreat();
                }
            }
        }

        public Vector3 CalculateSteeringTargetToBlockPathToGoal()
        {
            // calculate press hardness
            float pressHardness = CalculatePressHardness();

            // declare steering target
            Vector3 steeringTarget = Vector3.zero;

            //find direction to goal
            Vector3 directionOfThreatToGoal = Owner.TeamGoal.Position - _threat.Position;

            //the spot is somewhere between the threat and my goal
            steeringTarget = _threat.Position
                + directionOfThreatToGoal.normalized
                * (pressHardness + Owner.Radius + _threat.Radius);

            // return result
            return steeringTarget;
        }

        public Vector3 CalculateSteeringTargetToBlockPassToThreat()
        {
            // calculate press hardness
            float pressHardness = CalculatePressHardness();

            // declare steering target
            Vector3 steeringTarget = Vector3.zero;

            //find direction to goal
            Vector3 directionOfThreatToBall = Ball.Instance.Position - _threat.Position;

            //the spot is somewhere between the threat and my goal
            steeringTarget = _threat.Position
                + directionOfThreatToBall.normalized
                * (pressHardness + Owner.Radius + _threat.Radius);

            // return result
            return steeringTarget;
        }

        public void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        public void Instance_OnTeamGainedPossession()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }
    }
}
