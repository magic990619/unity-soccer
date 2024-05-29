using Assets.SimpleSteering.Scripts.Movement;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.References;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TacklePlayer.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System.Linq;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.InfoWidgets.PlayerInfoWidget;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Shared;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    [RequireComponent(typeof(RPGMovement))]
    [RequireComponent(typeof(SupportSpot))]
    public class Player : MonoBehaviour
    {
        [SerializeField]
        bool _isUserControlled;

        [Header("AI Variables")]

        [Range(0.1f, 1f)]
        [SerializeField]
        float _tightPressFrequency = 0.5f;

        [SerializeField]
        float _ballContrallableDistance = 1f;

        [SerializeField]
        float _ballLongPassArriveVelocity = 8f;

        [SerializeField]
        float _ballShortPassArriveVelocity = 5f;

        [SerializeField]
        float _ballShotArriveVelocity = 10f;

        [SerializeField]
        float _ballTacklableDistance = 3f;

        [SerializeField]
        float _distanceMaxWonder = 10f;

        [SerializeField]
        float _distancePassMax = 15f;

        [SerializeField]
        float _distancePassMin = 5f;

        [SerializeField]
        float _distanceShotMaxValid = 20f;

        [SerializeField]
        float _distanceThreatMax = 0.5f;

        [SerializeField]
        float _distanceThreatMin = 1f;

        [SerializeField]
        float _rotationSpeed = 3.5f;

        [SerializeField]
        float _threatTrackDistance = 1f;

        [SerializeField]
        float _tendGoalHorizontalMovemetInfluence = 3f;

        [SerializeField]
        float _tendGoalDistance = 1f;

        [SerializeField]
        [Range(0.1f, 5f)]
        float _tendGoalSpeed = 4f;

        [Header("Basic Player Attributes")]

        [SerializeField]
        [Range(0.1f, 1f)]
        float _accuracy;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _goalKeeping = 0.8f;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _diveDistance;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _jumpHeight;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _power;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _speed;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _tackling;

        [SerializeField]
        PlayerTypes _playerType;

        [Header("Goal Keeper Attributes")]

        [SerializeField]
        [Range(0.1f, 1f)]
        float _diveSpeed;

        [SerializeField]
        [Range(0.1f, 1f)]
        float _reach;


        [Header("Tactic Variable")]
        [SerializeField]
        bool _canJoinCornerKick;

        [Header("Player Components")]

        /// <summary>
        /// A reference to this instance's animator
        /// </summary>
        [SerializeField]
        Animator _animator;

        [Header("Player Meshes")]

        [SerializeField]
        SkinnedMeshRenderer _glovesMesh;

        [SerializeField]
        SkinnedMeshRenderer _kitMesh;

        [SerializeField]
        Transform _modelRoot;

        [Header("Ball Positions")]
        [SerializeField]
        Transform _ballDropKickPosition;

        [SerializeField]
        Transform _ballFrontPosition;

        [SerializeField]
        Transform _ballTopPosition;

        [Header("Entities")]

        [SerializeField]
        BallReference _ballReference;

        [SerializeField]
        GameObject _radarItem;

        [SerializeField]
        Goal _oppGoal;

        [SerializeField]
        Goal _teamGoal;

        [SerializeField]
        Transform _homeRegion;

        [SerializeField]
        List<Player> _oppositionMembers;

        [SerializeField]
        List<Player> _teamMembers;

        [SerializeField]
        List<SupportSpot> _pitchPoints;

        [SerializeField]
        PitchRegions _pitchRegion;

        [Header("Biped References")]

        [SerializeField]
        Transform _ballLeftHand;

        [SerializeField]
        Transform _ballRightHand;

        [Header("Widgets")]

        [SerializeField]
        PlayerControlInfoWidget _playerControlInfoWidget;

        [SerializeField]
        PlayerDirectionInfoWidget _playerDirectionInfoWidget;

        [SerializeField]
        PlayerHealthInfoWidget _playerHealthInfoWidget;

        [SerializeField]
        PlayerNameInfoWidget _playerNameInfoWidget;

        int _eighteenAreaLayerMask;
        float _radius;

        Player _prevPassReceiver;
        RPGMovement _rpgMovement;
        Team _team;

        public Action OnBecameTheClosestPlayerToBall;
        public Action OnGoalKeeperGainedControlOfBall;
        public Action OnInstructedToDefendCornerKick;
        public Action OnInstructedToGoToHome;
        public Action OnInstructedToInteractWithBall;
        public Action OnInstructedToPutBallBackIntoPlay;
        public Action OnInstructedToSupportCornerKick;
        public Action OnInstructedToTakeCornerKick;
        public Action OnInstructedToTakeGoalKick;
        public Action OnInstructedToTakeKickOff;
        public Action OnInstructedToTakeThrowIn;
        public Action OnInstructedToWait;
        public Action OnIsNoLongerClosestPlayerToBall;
        public Action OnNoSupportSpotFound;
        public Action OnPunchBall;
        public Action OnPutBallBackIntoPlay;
        public Action OnTackled;
        public Action OnTakeGoalKick;
        public Action OnTakeKickOff;
        public Action OnTakeThrowIn;
        public Action OnTeamGainedPossession;
        public Action OnTeamLostControl;

        public Action<Player> OnTrackThreat;
        public Action<Player, SupportSpot> OnGoToSupportSpot;
        public Action<Shot> OnShotTaken;

        public delegate void ChaseBallDel(Player player);
        public delegate void ControlBallDel(Player player);
        public delegate void InstructedToReceiveBall(float time, Vector3 position);
        public delegate void TakeCornerKick(float ballTime, Vector3? position, Player receiver);

        public ChaseBallDel OnChaseBall;
        public ControlBallDel OnControlBall;
        public TakeCornerKick OnTakeCornerKick;

        public InstructedToReceiveBall OnInstructedToReceiveBall;

        public bool HasBall { get; set; }

        [SerializeField]
        public bool IsTeamInControl;// { get; set; }

        public bool IsUserControlled { get => _isUserControlled; set => _isUserControlled = value; }

        public float ActualAccuracy { get; set; }
        public float ActualDiveSpeed { get; set; }
        public float ActuaDiveDistance { get; set; }
        public float ActualJogSpeed { get; set; }
        public float ActuaJumpHeight { get; set; }
        public float ActualPower { get; set; }
        public float ActualReach { get; set; }
        public float ActualSprintSpeed { get; set; }
        public float BallTime { get; set; }
        public float ForwardRunFrequency { get; set; }
        public float Height { get; set; }
        public float KickPower { get; set; }
        public float KickTime { get; set; }
        public float LongBallFrequency { get; set; }
        public float SprintAnimatorValue { get; set; }
        public float TightPressFrequency { get => _tightPressFrequency; set => _tightPressFrequency = value; }

        public Vector3? KickTarget { get; set; }

        public IFSM GoalKeeperFSM { get; set; }
        public IFSM InFieldPlayerFSM { get; set; }
        public KickDecisions KickDecision { get; set; }
        public PassTypesEnum PassType { get; set; }
        public MatchStatuses MatchStatus { get; set; }

        public Pass Pass { get; set; }
        public Player PassReceiver { get; set; }
        public Shot Shot { get; set; }
        public SupportSpot SupportSpot { get; set; }

        public Team Team { get => _team; }
        public Transform HomePosition { get => _homeRegion; set => _homeRegion = value; }

        public List<Player> OppositionMembers { get => _oppositionMembers; set => _oppositionMembers = value; }
        public List<Player> TeamMembers { get => _teamMembers; set => _teamMembers = value; }

        #region MonoBehaviour Methods
        private void Awake()
        {
            //get some components
            GoalKeeperFSM = GetComponent<GoalKeeperFSM>();
            InFieldPlayerFSM = GetComponent<InFieldPlayerFSM>();
            RPGMovement = GetComponent<RPGMovement>();
            SupportSpot = GetComponent<SupportSpot>();

            // cache some component data
            _radius = GetComponent<CharacterController>().radius;
            Height = GetComponent<CharacterController>().height;

            // set-up our masks
            _eighteenAreaLayerMask = 1 << LayerMask.NameToLayer(Globals.EighteenAreaLayerMask);

            if (_playerType == PlayerTypes.InFieldPlayer)
            {
                if (_glovesMesh != null)
                {
                    _glovesMesh.gameObject.SetActive(false);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ball")
                Ball.Instance.OwnerWithLastTouch = this;
        }

        #endregion

        public bool IsPlayerReadyToPlay()
        {
            if (PlayerType == PlayerTypes.Goalkeeper)
                return true;
            else
                return !InFieldPlayerFSM.IsCurrentState<TackleMainState>() && !InFieldPlayerFSM.IsCurrentState<TackledMainState>();
        }

        public bool CanBallReachPoint(Vector3 fromPosition, Vector3 toPosition, float power, out float time, bool useFriction = true)
        {
            // determine the friction to use
            if (useFriction == true)
            {
                //calculate the time
                time = TimeToTarget(fromPosition,
                           toPosition,
                           power,
                           Ball.Instance.Friction);
            }
            else
            {
                //calculate the time
                time = TimeToTarget(fromPosition,
                           toPosition,
                           power);
            }

            //return result
            return time > 0;
        }

        public bool CanBallReachPoint(Vector3 toPosition, float power, out float time)
        {
            //calculate the time
            CanBallReachPoint(Ball.Instance.NormalizedPosition,
                       toPosition,
                       power,
                       out time);

            //return result
            return time > 0;
        }

        public bool CanMakeThrowIn(bool limitFiledOfView, Vector3 direction, out Pass result)
        {
            // prepare some variables
            bool isThrowPossible = false;
            result = null;
            Dictionary<Player, List<Tuple<float, float, Vector3>>> playerAndPassOptions = new Dictionary<Player, List<Tuple<float, float, Vector3>>>();

            // loop though and get a player I can pass to
            foreach (Player player in TeamMembers)
            {
                // check some conditions
                bool isPlayerNotMe = player != this;
                bool isPlayerInFieldPlayer = player.PlayerType == PlayerTypes.InFieldPlayer;
                bool isPlayerInCorrectDirection = limitFiledOfView ? Vector3.Angle(direction, player.Position - Position) <= 30f : true;
                bool isPlayerReadyToPlay = player.IsPlayerReadyToPlay();
                bool isPlayerWithinThrowRange = Vector3.Distance(Position, player.Position) <= 30f;

                // continue if all the conditions are true
                if (isPlayerInCorrectDirection
                    && isPlayerInFieldPlayer
                    && isPlayerNotMe
                    && isPlayerReadyToPlay
                    && isPlayerWithinThrowRange)
                {
                    /* check if a pass is safe to that point */

                    //get the possible pass options
                    List<Tuple<float, float, Vector3>> possiblePlayerPasses = new List<Tuple<float, float, Vector3>>();
                    List<Vector3> passOptions = GetPassPositionOptions(player.Position);

                    //loop through each option and search if it is possible to 
                    //pass to it. Consider positions higher up the pitch
                    foreach (Vector3 passOption in passOptions)
                    {
                        //find power to kick ball
                        float power = FindPower(Ball.Instance.NormalizedPosition,
                        passOption,
                        _ballLongPassArriveVelocity,
                        0f);

                        //clamp the power to the player's max power
                        power = Mathf.Clamp(power, 0f, this.ActualPower);

                        // get time to target
                        float ballTimeToTarget = TimeToTarget(Ball.Instance.NormalizedPosition,
                           passOption,
                           power);

                        // get time of player to point
                        float timeOfReceiverToTarget = TimeToTarget(passOption,
                            passOption,
                            ActualSprintSpeed);

                        // check if receiver can reach point before the ball
                        bool canReceiverReacheBallBeforeTarget = timeOfReceiverToTarget < ballTimeToTarget;

                        // continue if receiver can reach point before ball
                        if (canReceiverReacheBallBeforeTarget == true)
                        {
                            // check if pass is safe
                            bool isThrowSafe = IsLongPassSafeFromAllOpponents(Position,
                                player.Position,
                                passOption,
                                power,
                                ballTimeToTarget);

                            // if throw is safe continue
                            if (isThrowSafe)
                            {
                                // add the pass option to the possible player pass
                                possiblePlayerPasses.Add(new Tuple<float, float, Vector3>(ballTimeToTarget, power, passOption));
                            }
                        }
                    }

                    // check if we have something in the player passes
                    bool isPlayerPassesAvailable = possiblePlayerPasses.Count > 0;

                    // if we have possible passes then add the player and his passes to the dictionary
                    if (isPlayerPassesAvailable)
                        playerAndPassOptions.Add(player, possiblePlayerPasses);
                }
            }

            // check if we have players to pass to
            bool isReceiversAvailable = playerAndPassOptions.Count > 0;

            // procedd if we have receivers 
            if (isReceiversAvailable)
            {
                // set throw is possible
                isThrowPossible = true;

                // randomly choose a player and a pass position
                int randomIndex = Random.Range(0, playerAndPassOptions.Count);
                KeyValuePair<Player, List<Tuple<float, float, Vector3>>> chosenPlayerAndPassOptions = playerAndPassOptions.ElementAt(randomIndex);

                // randomly choose a pass option
                randomIndex = Random.Range(0, chosenPlayerAndPassOptions.Value.Count);
                Tuple<float, float, Vector3> randomlyChoosenPassOption = chosenPlayerAndPassOptions.Value[randomIndex];

                // set the data
                Pass pass = new Pass()
                {
                    BallTimeToTarget = randomlyChoosenPassOption.Item1,
                    KickPower = randomlyChoosenPassOption.Item2,

                    PassType = PassTypesEnum.Long,

                    FromPosition = Position,
                    ToPosition = randomlyChoosenPassOption.Item3,

                    Receiver = chosenPlayerAndPassOptions.Key
                };

                // set the result
                result = pass;
            }

            // return result
            return isThrowPossible;
        }

        /// <summary>
        /// Checks whether a player can pass
        /// </summary>
        /// <returns></returns>
        /// ToDo::Implement logic to cache players to message so that they can intercept the pass
        public bool CanPass(out Pass result, bool considerPassSafety = true)
        {
            // cached passes
            List<Pass> optionalPasses = new List<Pass>();

            //loop through each team player and find a pass for each
            foreach (Player receiver in TeamMembers)
            {
                // can't pass to myself
                bool isPlayerMe = receiver == this;
                if (isPlayerMe)
                    continue;

                // we don't want to pass to the last receiver
                bool isPlayePrevPassReceiver = receiver == _prevPassReceiver;
                if (isPlayePrevPassReceiver)
                    continue;

                // can't pass to the goalie
                bool isPlayerGoalKeeper = receiver.PlayerType == PlayerTypes.Goalkeeper;
                if (isPlayerGoalKeeper)
                    continue;

                // we can't pass to a tackling or tackled player
                if (receiver.InFieldPlayerFSM.IsCurrentState<TackleMainState>() && receiver.InFieldPlayerFSM.IsCurrentState<TackledMainState>())
                    continue;

                // find the best pass for this player
                Pass newPass = FindBestPass(receiver, considerPassSafety);
                if(newPass != null)optionalPasses.Add(newPass);
            }

            // get the furthest pass up the field
            Pass bestPass = optionalPasses.
                OrderBy(oP => Vector3.Distance(oP.ToPosition, OppGoal.Position))
                .FirstOrDefault();

            // set the out pass
            result = bestPass;
           
            // return result
            return bestPass != null;
        }

        public Pass FindBestPass(Player receiver, bool considerPassSafety = true, bool considerPlayerClosestToMe = false)
        {
            // return result
            return FindBestPass(receiver.Position, receiver, considerPassSafety, considerPlayerClosestToMe);
        }

        public Pass FindBestPass(Vector3 toPosition, Player receiver, bool considerPassSafety = true, bool considerPlayerClosestToMe = false)
        {
            //get the possible pass options
            List<Vector3> passOptions = GetPassPositionOptions(toPosition);
            passOptions = passOptions // remove some points that are behind me
                .Where(pO =>
                {
                    Vector3 dirOfReciverToPassOrigin = Ball.Instance.NormalizedPosition - receiver.Position;
                    Vector3 dirOfReciverToPassOption = pO - receiver.Position;
                    float dot = Vector3.Dot(dirOfReciverToPassOrigin.normalized, dirOfReciverToPassOption.normalized);

                    //return result
                    return dot > 0;
                })
                .ToList();
            passOptions.Add(toPosition);

            // check if the player should make a long ball
            bool isLongBall = Random.value <= LongBallFrequency;

            //make the appropriate pass
            if (isLongBall)
            {
                // find the best short pass
                Pass bestLongPass = FindBestLongPass(Ball.Instance.Position, passOptions, receiver);
                
                // return result
                return bestLongPass;
            }
            else
            {
                // find the best short pass
                Pass bestShortPass = FindBestShortPass(Ball.Instance.Position, passOptions, receiver);

                // return result
                return bestShortPass;
            }
        }

        public Pass FindBestLongPass(Vector3 fromPosition, List<Vector3> toPassOptions, Player receiver, bool considerPassSafety = true)
        {
            // set some default data
            bool isBestPassOptionFound = false;
            float distanceToGoal = 100f;
            Pass bestPassOption = new Pass();

            foreach (Vector3 toPassOption in toPassOptions)
            {
                // declare some variables
                float ballTimeToTarget = 0;
                float kickPower = 0;

                // check if canMakeShortPass
                bool canMakeLongPass = CanMakeLongPass(fromPosition, toPassOption, receiver, out ballTimeToTarget, out kickPower, considerPassSafety);
                if (canMakeLongPass == true)
                {
                    // choose a pass option higher up the pitch
                    float currDistanceToGoal = Vector3.Distance(fromPosition, toPassOption);
                    if (currDistanceToGoal < distanceToGoal)
                    {
                        // set is best pass option found to true
                        isBestPassOptionFound = true;

                        // cache the clossest distannce to goal so far
                        distanceToGoal = currDistanceToGoal;

                        // create the curr pass
                        bestPassOption.Init(ballTimeToTarget, kickPower, PassTypesEnum.Long, fromPosition, toPassOption, receiver);
                    }
                }
            }

            // if we have the best pass option return that one
            if (isBestPassOptionFound == true)
                return bestPassOption;
            else
                return null;
        }

        public Pass FindBestShortPass(Vector3 fromPosition, List<Vector3> toPassOptions, Player receiver, bool considerPassDist = false, bool considerPassSafety = true)
        {
            // set some default data
            bool isBestPassOptionFound = false;
            float distanceToGoal = 100f;
            Pass bestPassOption = new Pass();

            foreach(Vector3 toPassOption in toPassOptions)
            {
                // declare some variables
                float ballTimeToTarget = 0;
                float kickPower = 0;

                // check if canMakeShortPass
                bool canMakeShortPass = CanMakeShortPass(fromPosition, 
                    toPassOption, 
                    receiver, 
                    out ballTimeToTarget, 
                    out kickPower, 
                    considerPassDist,
                    considerPassSafety);

                if(canMakeShortPass == true)
                {
                    // choose a pass option higher up the pitch
                    float currDistanceToGoal = Vector3.Distance(fromPosition, toPassOption);
                    if(currDistanceToGoal < distanceToGoal)
                    {
                        // set is best pass option found to true
                        isBestPassOptionFound = true;

                        // cache the clossest distannce to goal so far
                        distanceToGoal = currDistanceToGoal;

                        // create the curr pass
                        bestPassOption.Init(ballTimeToTarget, kickPower, PassTypesEnum.Short, fromPosition, toPassOption, receiver);
                    }
                }
            }

            // if we have the best pass option return that one
            if (isBestPassOptionFound == true)
                return bestPassOption;
            else
                return null;
        }

        public bool CanMakeLongPass(Vector3 fromPosition, Vector3 toPosition, Player receiver, out float ballTimeToTarget, out float kickPower, bool considerPassSafety = true)
        {
            // assign out parameters
            ballTimeToTarget = 0;
            kickPower = 0;

            // check if position is within pass range
            bool isPositionWithinLongPassRange = IsPositionWithinLongPassRange(toPosition);

            // we consider a target which is out of our min pass distance
            if (isPositionWithinLongPassRange == true)
            {
                // long passes should be made only to move the ball upfield
                Vector3 dirOfPassToPoint = toPosition - fromPosition;
                Vector3 dirOfTeamGoalToOppGoal = OppGoal.Position - TeamGoal.Position;
                float dot = Vector3.Dot(dirOfPassToPoint.normalized, dirOfTeamGoalToOppGoal.normalized);
                if (dot < 0.3f)
                    return false;

                //find power to kick ball
                kickPower = FindPower(fromPosition,
                    toPosition,
                    _ballLongPassArriveVelocity,
                    0f);

                //clamp the power to the player's max power
                kickPower = Mathf.Clamp(kickPower, 0f, ActualPower);

                //find if ball can reach point
                bool canBallReachTarget = CanBallReachPoint(fromPosition,
                    toPosition,
                    kickPower,
                    out ballTimeToTarget, 
                    false);

                //return false if the time is less than zero
                //that means the ball can't reach it's target
                if (canBallReachTarget == false)
                    return false;

                // if we have a receiver, check if receiver can reach target before the ball
                if (receiver != null)
                {
                    // get time of receiver to point
                    float timeOfReceiverToTarget = TimeToTarget(receiver.Position,
                        toPosition,
                        ActualSprintSpeed);// use my speed, since I don't know the high speed of the receiver

                    // pass is not safe if receiver can't reach target before the ball
                    if (timeOfReceiverToTarget > ballTimeToTarget)
                        return false;
                }

                // check if pass is safe from all opponents
                bool isLongPassSafeFromAllOpponents = true;
                if (considerPassSafety)
                {
                    // check pass safety
                    isLongPassSafeFromAllOpponents = IsLongPassSafeFromAllOpponents(fromPosition,
                        receiver?.Position,
                        toPosition,
                        kickPower,
                        ballTimeToTarget);
                }

                // return result
                return isLongPassSafeFromAllOpponents;
            }

            // return false
            return false;
        }

        public bool CanMakeShortPass(Vector3 fromPosition, Vector3 toPosition, bool considerPassSafety = true, Player receiver = null)
        {
            // declare some variables
            float ballTimeToTarget;
            float kickPower;

            // return result
            return CanMakeShortPass(fromPosition, toPosition, receiver, out ballTimeToTarget, out kickPower, considerPassSafety);
        }

        public bool CanMakeShortPass(Vector3 fromPosition, Vector3 toPosition, Player receiver, out float ballTimeToTarget, out float kickPower, bool considerPassDist = false, bool considerPassSafety = true)
        {
            // assign out parameters
            ballTimeToTarget = 0;
            kickPower = 0;

            // check if position is within pass range
            bool isPositionWithinShortPassRange = considerPassDist == false 
                || considerPassDist == true && IsWithinDistanceRange(fromPosition,
                toPosition,
                _distancePassMin,
                _distancePassMax);

            // we consider a target which is out of our min pass distance
            if (isPositionWithinShortPassRange == true)
            {
                //find power to kick ball
                kickPower = FindPower(fromPosition,
                    toPosition,
                    _ballShortPassArriveVelocity,
                    Ball.Instance.Friction);

                //clamp the power to the player's max power
                kickPower = Mathf.Clamp(kickPower, 0f, ActualPower);

                //find if ball can reach point
                bool canBallReachTarget = CanBallReachPoint(fromPosition,
                    toPosition,
                    kickPower,
                    out ballTimeToTarget);

                //return false if the time is less than zero
                //that means the ball can't reach it's target
                if (canBallReachTarget == false)
                    return false;

                // if we have a receiver, check if receiver can reach target before the ball
                if (receiver != null)
                {
                    // get time of receiver to point
                    float timeOfReceiverToTarget = TimeToTarget(receiver.Position,
                        toPosition,
                        ActualSprintSpeed);// use my speed, since I don't know the high speed of the receiver

                    // pass is not safe if receiver can't reach target before the ball
                    if (timeOfReceiverToTarget > ballTimeToTarget)
                        return false;
                }

                // check if pass is safe from all opponents
                bool isPassSafeFromAllOpponents = true;
                if (considerPassSafety)
                {
                    // check pass safety
                    isPassSafeFromAllOpponents = IsShortBallKickSafeFromAllOpponents(fromPosition,
                        receiver?.Position,
                        toPosition,
                        kickPower);
                }

                // return result
                return isPassSafeFromAllOpponents;
            }

            // return false
            return false;
        }

        public bool CanMakeCornerKick(out Pass result)
        {
            // reset result
            result = null;

            // pick a random player inside the attack corner-kick region
            Player[] players = TeamMembers.Where(tM => 
                    tM != this 
                    && IsPositionWithinDistance(Position, tM.Position, 43f))
                .ToArray();

            // get a random player
            int randomIndex = Random.Range(0, players.Length - 1);
            Player player = players[randomIndex];

            //find power to kick ball
            float power = FindPower(Ball.Instance.NormalizedPosition,
                player.Position,
                _ballLongPassArriveVelocity,
                0f);

            // get time to target
            float ballTimeToTarget = TimeToTarget(Ball.Instance.NormalizedPosition,
               player.Position,
               power);

            // set other variables
            result = new Pass()
            {
                BallTimeToTarget = ballTimeToTarget,
                KickPower = power,

                PassType = PassTypesEnum.Long,

                FromPosition = Position,
                ToPosition = player.Position,

                Receiver = player
            };

            // return result
            return player != null;
        }

        public bool CanScore(out Shot shot, bool considerGoalDistance = true, bool considerShotSafety = true)
        {
            // set default value for shot
            shot = null;

            // shoot if distance to goal is valid
            if (considerGoalDistance)
            {
                bool isDistanceValid = IsPositionWithinDistance(OppGoal.Position, Position, _distanceShotMaxValid);
                if (isDistanceValid == false)
                    return false;
            }

            //define some positions to be local to the goal
            //get the shot reference point. It should be a point some distance behinde the 
            //goal-line/goal
            Vector3 refShotTarget = _oppGoal.ShotTargetReferencePoint;

            //number of tries to find a shot
            float numOfTries = Random.Range(1, 6);

            //loop through and find a valid shot
            for (int i = 0; i < numOfTries; i++)
            {
                //find a random target
                Vector3 randomGoalTarget = FindRandomShot();

                float power = FindPower(Ball.Instance.NormalizedPosition,
                    randomGoalTarget,
                    _ballShotArriveVelocity);

                //clamp the power
                power = Mathf.Clamp(power, 0f, ActualPower);
              
                //check if ball can reach the target
                float time = 0f;
                bool canBallReachPoint = CanBallReachPoint(randomGoalTarget,
                    power,
                    out time);

                // if ball can't reach target then return false
                if (canBallReachPoint == false)
                    return false;

                //check if shot to target is possible
                bool isShotPossible = true;
                //if (considerShotSafety)
                //{
                //    isShotPossible = IsShortBallKickSafeFromAllOpponents(Ball.Instance.NormalizedPosition,
                //        null,
                //        randomGoalTarget,
                //        power);
                //}

                //if shot is possible set the data
                if (isShotPossible == false && considerShotSafety == false
                    || isShotPossible == true && considerShotSafety == true)
                {
                    // recalculate the time
                    time = TimeToTarget(Ball.Instance.NormalizedPosition,
                        randomGoalTarget,
                        _ballShotArriveVelocity);

                    // calculate the y-target
                    float yPos = (power / _ballShotArriveVelocity) * (TeamGoal.GoalHeight) * _accuracy * 0.1f; // Random.value;
                    randomGoalTarget.y = yPos;

                    //set the data
                    shot = new Shot()
                    {
                        BallTimeToTarget = time,
                        KickPower = power,

                        ShotTypeEnum = ShotTypesEnum.Default,

                        ToPosition = randomGoalTarget
                    };

                    //return result
                    return true;
                }
            }

            return false;
        }

        public bool CanLongPassInDirection(Vector3 direction, out Pass result)
        {
            //set the pass target
            result = null;

            // variable for possible pass receivers
            List<Player> possiblePassReceivers = TeamMembers
                .Where(receiver => receiver != this
                    && receiver.PlayerType == PlayerTypes.InFieldPlayer
                    && IsPositionInDirection(direction, receiver.Position, 30f))
                .ToList();

            // if we don't have possible pass receiver, just create a pass in direction
            // else find the best player to get the pass
            if(possiblePassReceivers.Count == 0)
            {
                // create some data
                float passPower = 0.5f * BallLongPassArriveVelocity;
                Vector3 passInitPos = Ball.Instance.Position;
                Vector3 passTarget = passInitPos + (direction.normalized * 30);
                float passTimeToTarget = Ball.Instance.TimeToCoverDistance(
                    passInitPos,
                    passTarget,
                    passPower,
                    false);

                // just create a pass in this direction
                Pass newPass = new Pass()
                {
                    BallTimeToTarget = passTimeToTarget,
                    KickPower = passPower,
                    FromPosition = passInitPos,
                    PassType = PassTypesEnum.Long,
                    ToPosition = passTarget,
                };

                // add to list
                result = newPass;

            }
            else
            {
                // cached passes
                List<Pass> optionalPasses = new List<Pass>();

                //loop through each team player and find a pass for each
                foreach (Player receiver in possiblePassReceivers)
                {
                    //get the possible pass options
                    List<Vector3> passOptions = GetPassPositionOptions(receiver.Position);
                    passOptions.Add(receiver.Position);

                    // find the best short pass
                    Pass bestLongPass = FindBestLongPass(Ball.Instance.Position, passOptions, receiver);
                    if (bestLongPass != null)
                    {
                        optionalPasses.Add(bestLongPass);
                    }
                    else
                    {
                        // create some data
                        float passPower = BallLongPassArriveVelocity;
                        Vector3 passInitPos = Ball.Instance.Position;
                        Vector3 passTarget = receiver.Position;
                        float passTimeToTarget = Ball.Instance.TimeToCoverDistance(
                            passInitPos,
                            passTarget,
                            passPower,
                            false);

                        // just create a pass in this direction
                        Pass newPass = new Pass()
                        {
                            BallTimeToTarget = passTimeToTarget,
                            KickPower = passPower,
                            FromPosition = passInitPos,
                            PassType = PassTypesEnum.Long,
                            ToPosition = passTarget,
                            Receiver = receiver,
                        };

                        // add pass
                        optionalPasses.Add(newPass);
                    }
                }

                // get the furthest pass up the field
                Pass bestPass = optionalPasses
                    .OrderBy(oP => Vector3.Distance(oP.ToPosition, OppGoal.Position))
                    .FirstOrDefault();

                // set the out pass
                result = bestPass;
            }
            
            //return result
            return result != null;
        }

        public bool CanShortPassInDirection(Vector3 direction, out Pass result)
        {
            //set the pass target
            result = null;

            // variable for possible pass receivers
            List<Player> possiblePassReceivers = TeamMembers
                .Where(receiver => receiver != this 
                    && receiver.PlayerType == PlayerTypes.InFieldPlayer
                    && IsPositionInDirection(direction, receiver.Position, 30f))
                .ToList();
            
            // if we don't have possible pass receiver, just create a pass in direction
            // else find the best player to get the pass
            if(possiblePassReceivers.Count == 0)
            {
                // create some data
                float passPower = 0.85f * ActualPower;
                Vector3 passInitPos = Ball.Instance.Position;
                Vector3 passTarget = passInitPos + (direction.normalized * passPower);
                float passTimeToTarget = Ball.Instance.TimeToCoverDistance(
                    passInitPos,
                    passTarget,
                    passPower);

                // just create a pass in this direction
                Pass newPass = new Pass()
                {
                    BallTimeToTarget = passTimeToTarget,
                    KickPower = passPower,
                    FromPosition = passInitPos,
                    PassType = PassTypesEnum.Short,
                    ToPosition = passTarget,
                };

                // add to list
                result = newPass;
            } 
            else
            {
                //set the pass target
                List<Pass> optionalPasses = new List<Pass>();

                //loop through each team player and find a pass for each
                foreach (Player possiblePassReceiver in possiblePassReceivers)
                {
                    //get the possible pass options
                    List<Vector3> passOptions = GetPassPositionOptions(possiblePassReceiver.Position);
                    passOptions.Add(possiblePassReceiver.Position);

                    // find the best pass for this player
                    Pass newPass = FindBestShortPass(Ball.Instance.Position, passOptions, possiblePassReceiver);
                    if (newPass != null)
                    {
                        optionalPasses.Add(newPass);
                    }
                    else
                    {
                        // create some data
                        Vector3 passInitPos = Ball.Instance.Position;
                        Vector3 passTarget = possiblePassReceiver.Position;
                        float passPower = Mathf.Clamp(FindPower(
                            passInitPos,
                            passTarget,
                            _ballShortPassArriveVelocity,
                            Ball.Instance.Friction),
                            0,
                            ActualPower);
                        float passTimeToTarget = Ball.Instance.TimeToCoverDistance(
                            passInitPos,
                            passTarget,
                            passPower);

                        // just create a pass in this direction
                        newPass = new Pass()
                        {
                            BallTimeToTarget = passTimeToTarget,
                            KickPower = passPower,
                            FromPosition = passInitPos,
                            PassType = PassTypesEnum.Short,
                            ToPosition = passTarget,
                            Receiver = possiblePassReceiver,
                        };

                        // add to list
                        optionalPasses.Add(newPass);
                    }
                }

                // get the furthest pass up the field
                Pass bestPass = optionalPasses
                    .OrderBy(oP => Vector3.Distance(oP.ToPosition, this.Position))
                    .FirstOrDefault();

                // set the out pass
                result = bestPass;
            }

            //return result
            return result != null;
        }

        public bool IsBallPositionThreateningGoal()
        {
            return IsPositionThreateningGoal(Ball.Instance.Position);
        }

        public bool IsPositionThreateningGoal(Vector3 position)
        {
            return IsPositionWithinDistance(_teamGoal.Position, position, 50f);
        }

        public bool IsLongPassSafeFromAllOpponents(Vector3 initialPosition, Vector3? receiverPosition, Vector3 target, float initialBallVelocity, float time)
        {
            //look for a player threatening the pass
            foreach (Player player in OppositionMembers)
            {
                bool isPassSafeFromOpponent = IsLongPassSafeFromOpponent(initialPosition,
                    target,
                    player.Position,
                    receiverPosition,
                    initialBallVelocity,
                    time);

                //return false if the pass is not safe
                if (isPassSafeFromOpponent == false)
                    return false;
            }

            //return result
            return true;
        }

        public bool IsLongPassSafeFromOpponent(Vector3 initialPosition, Vector3 target, Vector3 oppPosition, Vector3? receiverPosition, float initialBallVelocity, float timeOfBall)
        {
            #region Consider some logic that might threaten the pass

            //we might not want to pass to a player who is highly threatened(marked)
            if (IsPositionHighlyThreatened(target, oppPosition))
                return false;

            //return false if opposition is closer to target than reciever
            if (receiverPosition != null)
            {
                if (IsPositionCloserThanPosition(target, oppPosition, (Vector3)receiverPosition))
                    return false;
            }

            // return result
            return true;

            #endregion
        }

        public bool IsShortBallKickSafeFromAllOpponents(Vector3 fromPosition, Vector3? receiverPosition, Vector3 toPosition, float initialBallVelocity)
        {
            //if there's an opposition player threatening the pass, then return false
            foreach (Player opp in OppositionMembers)
            {
                bool isPassSafeFromOpponent = IsShortBallKickSafeFromOpponent(fromPosition,
                    toPosition,
                    opp.Position,
                    receiverPosition,
                    initialBallVelocity);

                //return false if the pass is not safe
                if (isPassSafeFromOpponent == false)
                    return false;
            }

            //return result
            return true;
        }

        public bool IsShortBallKickSafeFromOpponent(Vector3 fromPosition, Vector3 toPosition, Vector3 oppPosition, Vector3? receiverPosition, float initialBallVelocity)
        {
            #region Consider some logic that might threaten the pass

            //we might not want to pass to a player who is highly threatened(marked)
            if (IsPositionHighlyThreatened(toPosition, oppPosition))
                return false;

            if (receiverPosition != null)
            {
                //return false if opposition is closer to target than reciever
                if (IsPositionCloserThanPosition(toPosition, oppPosition, (Vector3)receiverPosition))
                    return false;

                //If oppossition is not between the passing lane then he is behind the passer
                //receiver and he can't intercept the ball
                if (IsPositionBetweenTwoPoints(fromPosition, (Vector3)receiverPosition, oppPosition) == false)
                    return true;
            }

            #endregion

            #region find if opponent can intercept ball

            //check if pass to position can be intercepted
            Vector3 orthogonalPoint = GetPointOrthogonalToLine(fromPosition,
                toPosition,
                oppPosition);

            //get time of ball to point
            float timeOfBallToOrthogonalPoint = 0f;
            CanBallReachPoint(orthogonalPoint, initialBallVelocity, out timeOfBallToOrthogonalPoint);

            // find the point the player can intercept ball
            float ballControllableDistance = _ballContrallableDistance + Radius;

            Vector3 oppDirToOrthogonalPoint = orthogonalPoint - oppPosition;
            if (oppDirToOrthogonalPoint.magnitude > ballControllableDistance)
                orthogonalPoint = oppPosition + oppDirToOrthogonalPoint.normalized * (oppDirToOrthogonalPoint.magnitude - ballControllableDistance);

            //get time of opponent to target
            float timeOfOpponentToTarget = TimeToTarget(oppPosition,
            orthogonalPoint,
            ActualSprintSpeed);

            //ball is safe if it can reach that point before the opponent
            bool canBallReachOrthogonalPointBeforeOpp = timeOfBallToOrthogonalPoint < timeOfOpponentToTarget;

            if (canBallReachOrthogonalPointBeforeOpp == true)
                return true;
            else
                return false;
            // return true;
            #endregion
        }

        /// <summary>
        /// Checks whether this instance is picked out or not
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPickedOut(Player player)
        {
            return SupportSpot.IsPickedOut(player);
        }

        public bool IsPositionBetweenTwoPoints(Vector3 A, Vector3 B, Vector3 point)
        {
            //find some direction vectors
            Vector3 fromAToPoint = point - A;
            Vector3 fromBToPoint = point - B;
            Vector3 fromBToA = A - B;
            Vector3 fromAToB = -fromBToA;

            //check if point is inbetween and return result
            return Vector3.Dot(fromAToB.normalized, fromAToPoint.normalized) > 0
                && Vector3.Dot(fromBToA.normalized, fromBToPoint.normalized) > 0;
        }

        /// <summary>
        /// Checks whether the first position is closer to target than the second position
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position001"></param>
        /// <param name="position002"></param>
        /// <returns></returns>
        public bool IsPositionCloserThanPosition(Vector3 target, Vector3 position001, Vector3 position002)
        {
            return Vector3.Distance(position001, target) < Vector3.Distance(position002, target);
        }

        public bool IsPositionInDirection(Vector3 forward, Vector3 position, float angle)
        {
            // find direction to target
            Vector3 directionToTarget = position - Position;

            // find angle between forward and direction to target
            float angleBetweenDirections = Vector3.Angle(forward.normalized, directionToTarget.normalized);

            // return result
            return Math.Abs(angleBetweenDirections) <= angle;
        }

        public bool IsPositionThreatened(Vector3 position)
        {
            return OppositionMembers.Any(oTM => IsPositionThreatened(position, oTM.Position));
        }

        public bool IsPositionWithinMinPassDistance(Vector3 position)
        {
            return IsPositionWithinDistance(Position,
                position,
                _distancePassMin);
        }

        public bool IsPositionWithinMinPassDistance(Vector3 center, Vector3 position)
        {
            return IsPositionWithinDistance(center,
                position,
                _distancePassMin);
        }

        public bool IsPositionWithinWanderRadius(Vector3 position)
        {
            return IsPositionWithinDistance(_homeRegion.position,
                position,
                _distanceMaxWonder);
        }

        /// <summary>
        /// Finds the power
        /// </summary>
        /// <param name="from">initial position</param>
        /// <param name="to">target</param>
        /// <param name="arriveVelocity">required velocity on arrival to target</param>
        /// <param name="friction">force acting against motion</param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float arriveVelocity, float friction)
        {
            // v^2 = u^2 + 2as => u^2 = v^2 - 2as => u = root(v^2 - 2as)

            //calculate some values
            float vSquared = Mathf.Pow(arriveVelocity, 2f);
            float twoAS = 2 * friction * Vector3.Distance(from, to);
            float uSquared = vSquared - twoAS;

            //find result
            float result = Mathf.Sqrt(uSquared);

            //return result
            return result;
        }

        public float TimeToTarget(Vector3 fromPosition, Vector3 toPosition, float velocityInitial)
        {
            //use S = D/T => T = D/S
            return Vector3.Distance(fromPosition, toPosition) / velocityInitial;
        }

        /// <summary>
        /// Calculates the time it will take to reach the target
        /// </summary>
        /// <param name="inital">start position</param>
        /// <param name="target">final position</param>
        /// <param name="initialVelocity">initial velocity</param>
        /// <param name="acceleration">force acting aginst motion</param>
        /// <returns></returns>
        public float TimeToTarget(Vector3 initial, Vector3 target, float velocityInitial, float acceleration)
        {
            //using  v^2 = u^2 + 2as 
            float distance = Vector3.Distance(initial, target);
            float uSquared = Mathf.Pow(velocityInitial, 2f);
            float v_squared = uSquared + (2 * acceleration * distance);

            //if v_squared is less thaSn or equal to zero it means we can't reach the target
            if (v_squared <= 0)
                return -1.0f;

            //find the final velocity
            float v = Mathf.Sqrt(v_squared);

            //find time to travel 
            return TimeToTravel(velocityInitial, v, acceleration);
        }

        public float TimeToTravel(float initialVelocity, float finalVelocity, float acceleration)
        {
            // t = v-u
            //     ---
            //      a
            float time = (finalVelocity - initialVelocity) / acceleration;

            //return result
            return time;
        }

        /// <summary>
        /// Finds a random target on the goal
        /// </summary>
        /// <returns></returns>
        public Vector3 FindRandomShot()
        {
            //define some positions to be local to the goal
            //get the shot reference point. It should be a point some distance behinde the 
            //goal-line/goal
            Vector3 refShotTarget = _oppGoal.transform.InverseTransformPoint(_oppGoal.ShotTargetReferencePoint);

            //find an x-position within the goal mouth
            float randomXPosition = Random.Range(_oppGoal.BottomLeftRelativePosition.x,
                _oppGoal.BottomRightRelativePosition.x);

            //set result
            Vector3 goalLocalTarget = new Vector3(randomXPosition, refShotTarget.y, refShotTarget.z);
            Vector3 goalGlobalTarget = _oppGoal.transform.TransformPoint(goalLocalTarget);

            //return result
            return goalGlobalTarget;
        }

        public Player GetClosestTeamPlayerToPosition(Vector3 position)
        {
            // get the player
            Player player = _teamMembers.Where(tM => tM != this && tM.IsPlayerReadyToPlay())
                .OrderBy(tM => Vector3.Distance(tM.Position, position))
                .First();

            // return result
            return player;
        }

        /// <summary>
        /// Calculates a point on line a-b that is at right angle to a point
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 GetPointOrthogonalToLine(Vector3 from, Vector3 to, Vector3 point)
        {
            //this is the normal
            Vector3 fromTo = to - from;

            //this is the vector/direction
            Vector3 fromPoint = point - from;

            //find projection
            Vector3 projection = Vector3.Project(fromPoint, fromTo);

            //find point on normal
            return projection + from;
        }

        /// <summary>
        /// Gets the options to pass the ball
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<Vector3> GetPassPositionOptions(Vector3 position)
        {
            //create a list to hold the results
            List<Vector3> result = new List<Vector3>();

            //the first position is the current position
            result.Add(position);

            //set some data
            float incrementAngle = 45;
            float iterations = 360 / incrementAngle;

            //find some positions around the player
            for (int i = 0; i < iterations; i++)
            {
                //get the direction
                float angle = incrementAngle * i;

                //rotate the direction
                Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

                //get point
                Vector3 point = position + direction * _distancePassMin * Random.value;

                //add to list
                result.Add(point);
            }

            //return results
            return result;
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        public void Init()
        {
            ActuaDiveDistance *= _diveDistance;
            ActualDiveSpeed *= _diveSpeed;
            ActuaJumpHeight *= _jumpHeight;
            ActualPower *= _power;
            ActualReach *= _reach;
            ActualRotationSpeed =  _rotationSpeed * _speed * 1.5f;
            ActualSprintSpeed += ActualJogSpeed;

            //Init the RPGMovement
            RPGMovement.Init(_speed, 
                ActualJogSpeed,
                ActualRotationSpeed, 
                ActualSprintSpeed);
        }

        /// <summary>
        /// Initializes this instance
        /// </summary>
        /// <param name="power"></param>
        /// <param name="speed"></param>
        public void Init(float aiUpdateFrequency, 
           float distancePassMax,
           float distancePassMin,
           float distanceShotValidMax,
           float distanceTendGoal,
           float distanceThreatMax,
           float distanceThreatMin,
           float distanceThreatTrack,
           float distanceWonderMax,
           float velocityLongPassArrive,
           float velocityShortPassArrive,
           float velocityShotArrive,
           float diveDistance,
           float diveSpeed,
           float jogSpeed,
           float jumpHeight,
           float forwardRunFrequency,
           float longBallFrequency,
           float tightPressFrequency,
           float power,
           float reach,
           float speed,
           PitchRegions pitchRegion,
           InGameKitDto kit,
           InGamePlayerDto playerDto,
           Team team)
        {
            // set up the fsm
            if (GoalKeeperFSM != null)
            {
                GoalKeeperFSM.ManualUpdateFrequency = aiUpdateFrequency;
                GoalKeeperFSM.StartManualExecute();
            }

            if (InFieldPlayerFSM != null)
            {
                InFieldPlayerFSM.ManualUpdateFrequency = aiUpdateFrequency;
                InFieldPlayerFSM.StartManualExecute();
            }

            // ai values
            _distancePassMax = distancePassMax;
            _distancePassMin = distancePassMin;
            _distanceShotMaxValid = distanceShotValidMax;
            _tendGoalDistance = distanceTendGoal;
            _threatTrackDistance = distanceThreatTrack;
            _distanceMaxWonder = distanceWonderMax;
            _ballLongPassArriveVelocity = velocityLongPassArrive;
            _ballShortPassArriveVelocity = velocityShortPassArrive;
            _ballShotArriveVelocity = velocityShotArrive;
            _distanceThreatMax = distanceThreatMax;
            _distanceThreatMin = distanceThreatMin;

            // set the actual values
            ActuaDiveDistance = diveDistance;
            ActualDiveSpeed = diveSpeed;
            ActualJogSpeed = jogSpeed;
            ActuaJumpHeight = jumpHeight;
            ActualPower = power;
            ActualReach = reach;
            ActualSprintSpeed = (speed - jogSpeed) * playerDto.Speed;

            ForwardRunFrequency = forwardRunFrequency;
            LongBallFrequency = longBallFrequency;
            TightPressFrequency = tightPressFrequency;

            // setup the player attributes
            _accuracy = playerDto.Accuracy;
            _canJoinCornerKick = playerDto.CanJoinCornerKick;
            _diveDistance = playerDto.DiveDistance;
            _diveSpeed = playerDto.DiveSpeed;
            _goalKeeping = playerDto.GoalKeeping;
            _jumpHeight = playerDto.JumpHeight;
            _power = playerDto.Power;
            _reach = playerDto.Reach;
            _speed = playerDto.Speed;
            _tackling = playerDto.Tackling;

            // set the mesh texture
            _kitMesh.material.mainTexture = _playerType == PlayerTypes.Goalkeeper ? kit.GoalKeeperKit : kit.InFieldPlayerKit;
            _radarItem.GetComponent<MeshRenderer>().material.color = kit.Color;

            // init pitc values
            _pitchRegion = pitchRegion;
            _team = team;

            // calculate the sprint animator value
            SprintAnimatorValue = 0.5f + (0.5f * ((speed - jogSpeed) / ActualSprintSpeed));

            // init the info component
            string displayName = string.Format("{0}. {1}", playerDto.KitNumber, playerDto.KitName);
            _playerNameInfoWidget.Init(displayName);
            _playerNameInfoWidget.Root.SetActive(false);
        }

        public bool IsAheadOfMe(Vector3 position)
        {
            // find the relative positons
            Vector3 playerRelativePositionToGoal = _teamGoal.transform.InverseTransformPoint(Position);
            Vector3 positionRelativePositionToGoal = _teamGoal.transform.InverseTransformPoint(position);

            // return result
            return playerRelativePositionToGoal.z <= positionRelativePositionToGoal.z;
        }

        /// <summary>
        /// Checks whether the player is at target
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsAtTarget(Vector3 position)
        {
            return IsPositionWithinDistance(Position, position, 0.1f);
        }

        public bool IsBallContrallable()
        {
            return IsBallWithinControllableDistance() && IsBallWithinControllableHeight();
        }

        public bool IsBallWithinControllableDistance()
        {
            return IsPositionWithinDistance(Position, Ball.Instance.NormalizedPosition, _ballContrallableDistance + Radius);
        }

        public bool IsBallWithinControllableDistance(Vector3 fromPositiom, Vector3 toPosition)
        {
            return IsPositionWithinDistance(fromPositiom, toPosition, _ballContrallableDistance + Radius);
        }

        public bool IsBallWithinControllableHeight()
        {
            return Ball.Instance.Position.y <= Height;
        }

        public bool IsBallTacklable()
        {
            return IsBallWithinTacklableDistance() && IsBallWithinControllableHeight();
        }

        public bool IsBallWithinTacklableDistance()
        {
            return IsPositionWithinDistance(Position, Ball.Instance.NormalizedPosition, _ballTacklableDistance + _radius);
        }

        public bool IsInfrontOfPlayer(Vector3 position, float angle = 15f)
        {
            // find the direction to target
            Vector3 direction = position - Position;

            // find the dot product
            float angleBetweenDirectionAndForward = Vector3.Angle(direction.normalized, transform.forward);

            return angleBetweenDirectionAndForward <= angle;
        }

        public bool IsInsideMyTeamEighteenArea()
        {

            // check if is inside eighteen area
            RaycastHit raycastHit;
            bool isInsideEighteenArea = Physics.Raycast(Position + Vector3.up,
                Vector3.down, 
                out raycastHit, 
                Mathf.Infinity, 
                _eighteenAreaLayerMask);

            // check if the goal is my team's goal
            if (isInsideEighteenArea == true)
            {
                // check if it's mine
                Goal goal = raycastHit.transform.GetComponent<EighteenArea>().Goal;
                return goal == TeamGoal;
            }

            // return default result
            return false;
        }

        /// <summary>
        /// Checks whether a player is a threat
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerAThreat(Player player)
        {
            return IsPositionWithinDistance(Position, player.Position, _distanceThreatMax);
        }

        public bool IsPositionThreatened(Vector3 center, Vector3 position)
        {
            return IsPositionWithinDistance(center, position, _distanceThreatMin);
        }

        public bool IsPositionWithinLongPassRange(Vector3 position)
        {
            return IsPositionWithinDistance(Position,
                position,
                _distancePassMax) == false;
        }

        public bool IsPositionWithinShortPassRange(Vector3 position)
        {
            return IsWithinDistanceRange(Position,
                position,
                _distancePassMin,
                _distancePassMax);
        }

        public bool IsPositionWithinShortPassRange(Vector3 center, Vector3 position)
        {
            return IsWithinDistanceRange(center,
                position,
                _distancePassMin,
                _distancePassMax);
        }

        /// <summary>
        /// Check whether a position is a threat or not
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsPositionAThreat(Vector3 position)
        {
            // position is a threat if its within saftey distance
            return IsPositionWithinDistance(Position, position, DistanceThreatMax);
        }

        public bool IsPositionAThreat(Vector3 center, Vector3 position)
        {
            return IsPositionWithinDistance(center, position, DistanceThreatMax);
        }

        public bool IsPositionHighlyThreatened(Vector3 center, Vector3 position)
        {
            return IsPositionWithinDistance(center, position, _distanceThreatMax);
        }

        public bool IsTeamMemberWithinMinPassDistance(Vector3 position)
        {
            return _teamMembers.ToList()
                .Any(tM => tM != this && IsPositionWithinDistance(position, tM.Position, _distancePassMin));
        }

        public bool IsTeamMemberWithinDistance(float distance, Vector3 position)
        {
            return _teamMembers.ToList()
                .Any(tM => tM != this && IsPositionWithinDistance(tM.Position, position, distance));
        }

        public bool IsThreatened()
        {
            // return true if there is any threatening player
            return OppositionMembers
                .Any(oP => IsPlayerAThreat(oP) == true);
        }

        public bool IsTooFarFromGoalMouth()
        {
            return Vector3.Distance(Position, TeamGoal.Position) > Globals.MaxWanderDistanceFromGoal;
        }

        public bool IsPositionWithinDistance(Vector3 center, Vector3 position, float distance)
        {
            return Vector3.Distance(center, position) <= distance;
        }

        public bool IsWithinDistanceRange(Vector3 center, Vector3 position, float minDistance, float maxDistance)
        {
            return !IsPositionWithinDistance(center, position, minDistance) && IsPositionWithinDistance(center, position, maxDistance);
        }

        /// <summary>
        /// Finds the power needed to kick the ball and make it reach
        /// a particular target with a particular velocity
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="arrivalVelocity"></param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float arrivalVelocity)
        {
            //find the power to target
            float power = Ball.Instance.FindPower(from,
                to,
                arrivalVelocity);

            //return result
            return power;
        }

        #region invoke actions region

        public void Invoke_OnShotTaken(Shot shot)
        {
            ActionUtility.Invoke_Action(shot, OnShotTaken);
        }

        public void Invoke_OnControlBall()
        {
            // raise event that I'm controlling the ball
            ControlBallDel temp = OnControlBall;
            if (temp != null)
                temp.Invoke(this);
        }

        public void Invoke_OnBecameTheClosestPlayerToBall()
        {
            ActionUtility.Invoke_Action(OnBecameTheClosestPlayerToBall);
        }

        public void Invoke_OnInstructedToDefendCornerKick()
        {
            ActionUtility.Invoke_Action(OnInstructedToDefendCornerKick);
        }

        public void Invoke_OnInstructedGoToHome()
        {
            ActionUtility.Invoke_Action(OnInstructedToGoToHome);
        }

        public void Invoke_OnInstructedToInteractWithBall()
        {
            ActionUtility.Invoke_Action(OnInstructedToInteractWithBall);
        }

        public void Invoke_OnInstructedToPutBallBackIntoPlay()
        {
            ActionUtility.Invoke_Action(OnInstructedToPutBallBackIntoPlay);
        }

        public void Invoke_OnInstructedToSupportCornerKick()
        {
            ActionUtility.Invoke_Action(OnInstructedToSupportCornerKick);
        }

        public void Invoke_OnInstructedToTakeKickOff()
        {
            ActionUtility.Invoke_Action(OnInstructedToTakeKickOff);
        }

        public void Invoke_OnInstructedToTakeCornerKick()
        {
            ActionUtility.Invoke_Action(OnInstructedToTakeCornerKick);
        }

        public void Invoke_OnInstructedToTakeGoalKick()
        {
            ActionUtility.Invoke_Action(OnInstructedToTakeGoalKick);
        }

        public void Invoke_OnInstructedToTakeThrowIn()
        {
            ActionUtility.Invoke_Action(OnInstructedToTakeThrowIn);
        }

        public void Invoke_OnInstructedToWait()
        {
            ActionUtility.Invoke_Action(OnInstructedToWait);
        }

        public void Invoke_OnIsNoLongerTheClosestPlayerToBall()
        {
            ActionUtility.Invoke_Action(OnIsNoLongerClosestPlayerToBall);
        }

        public void Invoke_OnPunchBall()
        {
            ActionUtility.Invoke_Action(OnPunchBall);
        }

        public void Invoke_OnTackled()
        {
            ActionUtility.Invoke_Action(OnTackled);
        }

        public void Invoke_OnTeamGainedPossession()
        {
            // set that my team is in control
            IsTeamInControl = true;

            // raise event that team is now in control
            ActionUtility.Invoke_Action(OnTeamGainedPossession);
        }

        public void Invoke_OnTeamLostControl()
        {
            // set team no longer in control
            IsTeamInControl = false;

            // invoke team has lost control
            ActionUtility.Invoke_Action(OnTeamLostControl);
        }
        #endregion

        /// <summary>
        /// Player kicks the ball from his position to the target
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void MakePass(Vector3 from, Vector3 to, Player receiver, float power, float time)
        {
            // kinck
            if (Pass.PassType == PassTypesEnum.Long)
                Ball.Instance.LaunchToPoint(to, power);
            else if (Pass.PassType == PassTypesEnum.Short)
                Ball.Instance.KickToPoint(to, power);

            //message the receiver to receive the ball
            InstructedToReceiveBall temp = receiver.OnInstructedToReceiveBall;
            if (temp != null)
                temp.Invoke(time, to);
        }

        public void MakePass(Pass pass)
        {
            // kick
            if (pass.PassType == PassTypesEnum.Long)
                Ball.Instance.LaunchToPoint(pass.ToPosition, pass.KickPower);
            else if (pass.PassType == PassTypesEnum.Short)
                Ball.Instance.KickToPoint(pass.ToPosition, pass.KickPower);

            //message the receiver to receive the ball
            if (pass.Receiver != null)
            {
                InstructedToReceiveBall temp = pass.Receiver.OnInstructedToReceiveBall;
                if (temp != null)
                    temp.Invoke(pass.BallTimeToTarget, pass.ToPosition);
            }
        }

        public void MakeShot(Shot shot)
        {
            //launch the ball
            Ball.Instance.LaunchToPoint(shot.ToPosition, shot.KickPower);

            // raise the ball shot event
            ActionUtility.Invoke_Action(shot, Ball.Instance.OnBallShot);
        }

        //public void MakeShot(Vector3 from, Vector3 to, float power, float time)
        //{
        //    //launch the ball
        //    Ball.Instance.LaunchToPoint(to, power);

        //    // raise the ball shot event
        //    Ball.BallLaunched temp = Ball.Instance.OnBallShot;
        //    if (temp != null)
        //        temp.Invoke(time, power, from, to);
        //}

        /// <summary>
        /// Puts the ball infront of this player
        /// </summary>
        public void PlaceBallInfronOfMe()
        {
            Ball.Instance.NormalizedPosition = Position + transform.forward * (_radius + _ballContrallableDistance);
            Ball.Instance.transform.rotation = transform.rotation;
        }

        public List<Player> GetTeamMembersInRadius(float radius)
        {
            //get the players
            List<Player> result = _teamMembers.FindAll(tM => Vector3.Distance(this.Position, tM.Position) <= radius 
            && this != tM);

            //retur result
            return result;
        }

        public Player GetRandomTeamMemberInRadius(float radius)
        {
            //get the list
            List<Player> players = GetTeamMembersInRadius(radius);

            //return random player
            if (players == null)
                return null;
            else
                return players[Random.Range(0, players.Count)];
        }

        /// <summary>
        /// Gets the infield player closest to the position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Player GetTeamMemberClosestToPosition(Vector3 position)
        {
            // get the closest player to point
            Player player = _teamMembers
                .Where(tM => tM != this && tM.PlayerType == PlayerTypes.InFieldPlayer)
                .OrderBy(tM => Vector3.Distance(tM.Position, position))
                .FirstOrDefault();

            // return result
            return player;
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }

            set
            {
                transform.rotation = value;
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(transform.position.x, 0f, transform.position.z);
            }

            set
            {
                transform.position = new Vector3(value.x, 0f, value.z);
            }
        }

        public Goal OppGoal { get => _oppGoal; set => _oppGoal = value; }
        public float BallShortPassArriveVelocity { get => _ballShortPassArriveVelocity; set => _ballShortPassArriveVelocity = value; }
        public List<SupportSpot> PlayerSupportSpots { get => _pitchPoints; set => _pitchPoints = value; }

        public float DistancePassMin
        {
            get => _distancePassMin;
            set => _distancePassMin = value;
        }

        public float DistanceThreatMin
        {
            get => _distanceThreatMin + _radius;
            set => _distanceThreatMin = value;
        }

        public float DistanceThreatMax
        {
            get => _distanceThreatMax + _radius;
            set => _distanceThreatMax = value;
        }

        public RPGMovement RPGMovement
        {
            get
            {
                // set the rpg movement
                if (_rpgMovement == null)
                {
                    gameObject.AddComponent<RPGMovement>();
                    _rpgMovement = GetComponent<RPGMovement>();
                }

                // return result
                return _rpgMovement;
            }

            set
            {
                _rpgMovement = value;
            }
        }

        public float Radius { get => _radius; set => _radius = value; }
        public Goal TeamGoal { get => _teamGoal; set => _teamGoal = value; }
        public PlayerTypes PlayerType { get => _playerType; set => _playerType = value; }
        public float ThreatTrackDistance { get => _threatTrackDistance; set => _threatTrackDistance = value; }
        public float TendGoalSpeed { get => _tendGoalSpeed; set => _tendGoalSpeed = value; }
        public float TendGoalDistance { get => _tendGoalDistance; set => _tendGoalDistance = value; }
        public float GoalKeeping { get => _goalKeeping; set => _goalKeeping = value; }
        public float DistancePassMax { get => _distancePassMax; set => _distancePassMax = value; }
        public Player PrevPassReceiver { get => _prevPassReceiver; set => _prevPassReceiver = value; }
        public PlayerControlInfoWidget PlayerControlInfoWidget { get => _playerControlInfoWidget; set => _playerControlInfoWidget = value; }
        public SkinnedMeshRenderer KitMesh { get => _kitMesh; set => _kitMesh = value; }
        public float Tackling { get => _tackling; set => _tackling = value; }
        public Animator Animator { get => _animator; set => _animator = value; }
        public float Reach { get => _reach; set => _reach = value; }
        public float JumpDistance { get => _diveDistance; set => _diveDistance = value; }
        public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
        public Transform ModelRoot { get => _modelRoot; set => _modelRoot = value; }
        public float DiveSpeed { get => _diveSpeed; set => _diveSpeed = value; }
        public BallReference BallReference { get => _ballReference; set => _ballReference = value; }
        public Transform BallFrontPosition { get => _ballFrontPosition; set => _ballFrontPosition = value; }
        public Transform BallDropKickPosition { get => _ballDropKickPosition; set => _ballDropKickPosition = value; }
        public float TendGoalHorizontalMovemetInfluence { get => _tendGoalHorizontalMovemetInfluence; set => _tendGoalHorizontalMovemetInfluence = value; }
        public float BallContrallableDistance { get => _ballContrallableDistance; set => _ballContrallableDistance = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float ActualRotationSpeed { get; set; }
        public float BallLongPassArriveVelocity { get => _ballLongPassArriveVelocity; set => _ballLongPassArriveVelocity = value; }
        public Transform BallTopPosition { get => _ballTopPosition; set => _ballTopPosition = value; }
        public bool CanJoinCornerKick { get => _canJoinCornerKick; set => _canJoinCornerKick = value; }
        public float DistanceShotMaxValid { get => _distanceShotMaxValid; set => _distanceShotMaxValid = value; }
        public float DistanceMaxWonder { get => _distanceMaxWonder; set => _distanceMaxWonder = value; }
        public float DistanceMaxWonder1 { get => _distanceMaxWonder; set => _distanceMaxWonder = value; }
        public SkinnedMeshRenderer GlovesMesh { get => _glovesMesh; set => _glovesMesh = value; }
        public PitchRegions PitchRegions { get => _pitchRegion; set => _pitchRegion = value; }
        public float RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        public float Accuracy { get => _accuracy; set => _accuracy = value; }
        public float DiveDistance { get => _diveDistance; set => _diveDistance = value; }
        public float Power { get => _power; set => _power = value; }
        public PlayerNameInfoWidget PlayerNameInfoWidget { get => _playerNameInfoWidget; set => _playerNameInfoWidget = value; }
        public PlayerDirectionInfoWidget PlayerDirectionInfoWidget { get => _playerDirectionInfoWidget; set => _playerDirectionInfoWidget = value; }
        public PlayerHealthInfoWidget PlayerHealthInfoWidget { get => _playerHealthInfoWidget; set => _playerHealthInfoWidget = value; }
        public Transform BallLeftHand { get => _ballLeftHand; set => _ballLeftHand = value; }
        public Transform BallRightHand { get => _ballRightHand; set => _ballRightHand = value; }
        public GameObject RadarItem { get => _radarItem; set => _radarItem = value; }
    }
}
