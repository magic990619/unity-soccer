using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using RobustFSM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    [RequireComponent(typeof(TeamFSM))]
    public class Team : MonoBehaviour
    {

        [SerializeField]
        bool _isUserControlled;

        [Header("Player-AI Variables")]

        [Range(0.1f, 1f)]
        [SerializeField]
        float _aiUpdateFrequency = 0.5f;

        [Range(0.1f, 1f)]
        [SerializeField]
        float _tightPressFrequency = 0.5f;

        [SerializeField]
        float _distancePassMax = 15f;

        [SerializeField]
        float _distancePassMin = 5f;

        [SerializeField]
        float _distanceShotValidMax = 30f;

        [SerializeField]
        float _distanceTendGoal = 3f;

        [SerializeField]
        float _distanceThreatMax = 1f;

        [SerializeField]
        float _distanceThreatMin = 5f;

        [SerializeField]
        float _distanceThreatTrack = 1f;

        [SerializeField]
        float _distanceWonderMax = 15f;

        [SerializeField]
        float _velocityLongPassArrive = 15f;

        [SerializeField]
        float _velocityShortPassArrive = 15f;

        [SerializeField]
        float _velocityShotArrive = 30f;

        [Header("Player Attribute Variables")]

        [SerializeField]
        float _diveSpeed = 4f;

        [SerializeField]
        float _jogSpeed = 4f;

        [SerializeField]
        float _jumpDistance = 1f;

        [SerializeField]
        float _jumpHeight = 0.5f;

        [SerializeField]
        float _power = 30f;

        [SerializeField]
        float _reach = 0.5f;

        [SerializeField]
        float _speed = 3.5f;

        [Header("Entities")]

        [SerializeField]
        CornerGoalKickTriggers _cornerGoalKickTriggers;

        [SerializeField]
        Goal _goal;

        [SerializeField]
        PitchRegions _pitchRegions;

        [SerializeField]
        Team _opponent;

        [Header("Transforms")]


        [SerializeField]
        Transform _leftSideCornerKickSpot;

        [SerializeField]
        Transform _leftSideGoalKickSpot;

        [SerializeField]
        Transform _rightSideCornerKickSpot;

        [SerializeField]
        Transform _rightSideGoalKickSpot;

        [SerializeField]
        Transform _pitchCenterSpot;

        [SerializeField]
        Transform _kickOffRefDirection;

        [SerializeField]
        Transform _playerSupportSpots;

        [SerializeField]
        Transform _rootFormations;

        [SerializeField]
        Transform _rootPlayers;

        [Header("Data")]
        [SerializeField]
        InGameTeamDto _teamData;

        Formation _formation;

        public bool IsUserControlled { get => _isUserControlled; }

        public bool HasCornerKick { get; set; }
        public bool HasInitialKickOff { get; set; }
        public bool HasKickOff { get; set; }
        public bool HasGoalKick { get; set; }
        public bool HasThrowIn { get; set; }
        public int Goals { get; set; }
        public IFSM FSM { get; set; }
        public Vector3 CachedBallPosition { get; set; }

        public Player ControllingPlayer { get; set; }
        public Player ReceivingPlayer { get; set; }

        //todo::optimize, store goalkeeper in variable
        public Player GoalKeeper { get => Players
                .Where(p => p.Player.PlayerType == PlayerTypes.Goalkeeper)
                .Select(p => p.Player).FirstOrDefault(); 
        }
        public List<InGamePlayerDto> Substitues = new List<InGamePlayerDto>();
        public List<TeamPlayer> Players = new List<TeamPlayer>();

        public Action OnAttackCornerKick;
        public Action OnDefendCornerKick;
        public Action OnGainPossession;
        public Action OnGoalKeeperGainedBallControl;
        public Action OnLostPossession;
        public Action OnInit;
        public Action OnOppFinishedInit;

        public Action OnMessagedToStop;
        public Action OnMessagedToTakeCornerKick;
        public Action OnMessagedToTakeGoalKick;
        public Action OnMessagedToTakeThrowIn;
        public Action OnMessagedToTakeKickOff;
        public Action OnInstructPlayersToWait;

        public Action OnTakeGoalKick;
        public Action OnTakeCornerKick;
        public Action OnTakeKickOff;
        public Action OnTakeThrowIn;

        public delegate void BallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target);

        public BallLaunched OnBallLaunched;

        private void Awake()
        {
            //initialize some variables
            FSM = GetComponent<TeamFSM>();

            //set-up some variables
            HasKickOff = HasInitialKickOff;
        }

        public void Init(float aiUpdateFrequency,
            float tightPressFrequency,
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
            float diveSpeed,
            float jogSpeed,
            float jumpDistance,
            float jumpHeight,
            float power,
            float reach,
            float speed,
            InGameTeamDto teamData)
        {
            _aiUpdateFrequency = aiUpdateFrequency;
            _distancePassMax = distancePassMax;
            _distancePassMin = distancePassMin;
            _distanceShotValidMax = distanceShotValidMax;
            _distanceTendGoal = distanceTendGoal;
            _distanceThreatTrack = distanceThreatTrack;
            _distanceWonderMax = distanceWonderMax;
            _velocityLongPassArrive = velocityLongPassArrive;
            _velocityShortPassArrive = velocityShortPassArrive;
            _velocityShotArrive = velocityShotArrive;
            _distanceThreatMax = distanceThreatMax;
            _distanceThreatMin = distanceThreatMin;
            _tightPressFrequency = tightPressFrequency;

            // set player control variables
            _diveSpeed = diveSpeed;
            _jogSpeed = jogSpeed;
            _jumpDistance = jumpDistance;
            _jumpHeight = jumpHeight;
            _speed = speed;
            _reach = reach;
            _power = power;

            // set the team data
            _teamData = teamData;

            // update some data
            _tightPressFrequency *= _teamData.DefendTactic.TightPressFrequency;
        }

        public void Invoke_OnBallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target)
        {
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(flightTime,
                    velocity,
                    initial,
                    target);
        }

        public void Invoke_OnGoalKeeperGainedBallControl()
        {
            ActionUtility.Invoke_Action(OnGoalKeeperGainedBallControl);
        }

        public void Invoke_OnOppFinishedInit()
        {
            ActionUtility.Invoke_Action(OnOppFinishedInit);
        }

        public void Invoke_OnMessagedToTakeGoalKick()
        {
            ActionUtility.Invoke_Action(OnMessagedToTakeGoalKick);
        }

        public void Invoke_OnMessagedToTakeCornerKick()
        {
            ActionUtility.Invoke_Action(OnMessagedToTakeCornerKick);
        }

        /// <summary>
        /// Invokes the OnStop action of this instance. Register this method to any event
        /// that the team needs to be aware of for it to go to prepare-for-kick-off state
        /// </summary>
        public void Invoke_OnMessagedToTakeKickOff()
        {
            ActionUtility.Invoke_Action(OnMessagedToTakeKickOff);
        }

        public void Invoke_OnMessagedToTakeKThrowIn(Vector3 position)
        {
            // cache the ball position
            CachedBallPosition = position;

            // invoke that it's a throw-in
            ActionUtility.Invoke_Action(OnMessagedToTakeThrowIn);
        }

        /// <summary>
        /// Invokes the OnStop action of this instance. Register this method to any event
        /// that the team needs to be aware of for it to go to wait state
        /// </summary>
        public void Invoke_OnMessagedToStop()
        {
            ActionUtility.Invoke_Action(OnMessagedToStop);
        }

        public void Invoke_OnLostPossession()
        {
            ActionUtility.Invoke_Action(OnLostPossession);
        }

        public void Invoke_OnGainPossession(Player player)
        {
            // set the controlling player
            ControllingPlayer = player;

            // raise the event that I have gained possession
            ActionUtility.Invoke_Action(OnGainPossession);
        }

        public void Invoke_OnPlayerChaseBall(Player chasingPlayer)
        {
            // get the current closest player to ball
            TeamPlayer currClosestPlayerToPoint = GetClosestPlayerToPoint(Ball.Instance.NormalizedPosition);

            if (chasingPlayer != currClosestPlayerToPoint.Player)
            {
                // message the closest player to go out of chaseball
                if (currClosestPlayerToPoint != null)
                    chasingPlayer.Invoke_OnIsNoLongerTheClosestPlayerToBall();

                // make the current closet player chase the ball
                currClosestPlayerToPoint.Player.Invoke_OnBecameTheClosestPlayerToBall();

            }
        }

        public void MovePlayersDownField()
        {
            //loop through each player and update it's position
            foreach (TeamPlayer teamPlayer in Players)
            {
                //find the percentage to move the player upfield
                Vector3 ballGoalLocalPosition = Opponent.Goal.transform.InverseTransformPoint(Ball.Instance.transform.position);
                float playerMovePercentage = Mathf.Clamp01((ballGoalLocalPosition.z / Pitch.Instance.Length) * TeamData.DefendTactic.PushBackRatio);

                //move the home position a similar percentage up the field
                Vector3 currentPlayerHomePosition = Vector3.Lerp(teamPlayer.AttackingHomePosition.transform.position,
                    teamPlayer.DefendingHomePosition.position,
                    playerMovePercentage);

                // only move upfield. 
                teamPlayer.CurrentHomePosition.position = currentPlayerHomePosition;
            }
        }

        public void MovePlayersUpField()
        {
            // check if I should move upfield
            bool canPushUpField = UnityEngine.Random.value <= TeamData.AttackTactic.PushForwardFrequency;
            if (canPushUpField == true)
            {
                //loop through each player and update it's position
                foreach (TeamPlayer teamPlayer in Players)
                {
                    //find the percentage to move the player upfield
                    Vector3 ballGoalLocalPosition = Goal.transform.InverseTransformPoint(Ball.Instance.transform.position);
                    float playerMovePercentage = Mathf.Clamp01((ballGoalLocalPosition.z / Pitch.Instance.Length) * TeamData.AttackTactic.PushAheadRatio);

                    //move the home position a similar percentage up the field
                    Vector3 currentPlayerHomePosition = Vector3.Lerp(teamPlayer.DefendingHomePosition.transform.position,
                        teamPlayer.AttackingHomePosition.position,
                        playerMovePercentage);

                    // only move upfield.
                    teamPlayer.CurrentHomePosition.position = currentPlayerHomePosition;
                }
            }
        }

        public void MovePlayersUpField(float lengthPitch, float pushUpFieldFactor, Vector3 refPosition)
        {
            //loop through each player and update it's position
            foreach (TeamPlayer teamPlayer in Players)
            {
                //find the percentage to move the player upfield
                Vector3 ballGoalLocalPosition = _goal.transform.InverseTransformPoint(refPosition);
                float playerMovePercentage = Mathf.Clamp01((ballGoalLocalPosition.z / lengthPitch) * pushUpFieldFactor);

                //move the home position a similar percentage up the field
                Vector3 currentPlayerHomePosition = Vector3.Lerp(teamPlayer.DefendingHomePosition.transform.position,
                    teamPlayer.AttackingHomePosition.position,
                    playerMovePercentage);

                //update the current player home position position
                teamPlayer.CurrentHomePosition.position = currentPlayerHomePosition;
            }
        }

        public void PlaceEveryPlayerAtHomePosition(float ratio, Vector3? lookAtPosition)
        {
            Vector3 normalizedLookAtPosition = (Vector3)lookAtPosition;

            Players.ForEach(tM =>
            {
                // calculate player position
                Vector3 currentPlayerHomePosition = Vector3.Lerp(tM.DefendingHomePosition.transform.position,
                         tM.AttackingHomePosition.position,
                         ratio);

                tM.Player.Position = currentPlayerHomePosition;
                tM.Player.transform.LookAt(normalizedLookAtPosition);
            });
        }

        public void OnOppScoredAGoal()
        {
            // set has kick-off
            HasKickOff = true;
        }

        public void OnTeamScoredAGoal()
        {
            // unset has kick-off
            HasKickOff = false;

            // increment number of goals scored
            ++Goals;
        }

        public void TriggerPlayersToPickOutThreats()
        {
            // get threatening players
            List<TeamPlayer> threats = GetThreateningPlayers()
                .OrderBy(tM => Vector3.Distance(tM.Player.Position, Goal.Position))
                .ToList();

            // now find the best player to track the threat
            threats.ForEach(t =>
            {
                // find closest player to this threat
                TeamPlayer closestPlayerToPoint = GetClosestPlayerToThreat(t.Player.Position);

                // tell player to pick-out threat
                closestPlayerToPoint?.Player.OnTrackThreat?.Invoke(t.Player);
            });
        }

        public void TriggerPlayersToSupportAttacker()
        {
            // loop through each team players and try to find a support spot for this player to go
            Players.Where(tP => tP.Player.PlayerType == PlayerTypes.InFieldPlayer
                    && ControllingPlayer != tP.Player)
                .ToList()
                .ForEach(tP =>
                {
                    if (ControllingPlayer != null
                    && tP.Player.IsPositionWithinDistance(Ball.Instance.Position, tP.CurrentHomePosition.position, tP.Player.DistancePassMax))
                    {
                        // find the support spot for this player
                        SupportSpot supportSpot = PlayerSupportSpots
                        .GetComponentsInChildren<SupportSpot>()
                        .ToList()
                        .Where(sS => sS.IsPickedOut(tP.Player) == false
                            && tP.Player.IsPositionWithinWanderRadius(sS.transform.position)
                            && tP.Player.IsTeamMemberWithinMinPassDistance(sS.transform.position) == false
                            && tP.Player.CanMakeShortPass(ControllingPlayer.Position, sS.Position) == true)// todo::consider long passes...decide between numerous support spots
                        .FirstOrDefault();

                        // if we have a support spot for this player then tell him to go to this spot
                        if (supportSpot != null) tP.Player.OnGoToSupportSpot?.Invoke(ControllingPlayer, supportSpot);
                    }
                    else
                    {
                        // tell the other player that no support spot for him wa found
                        tP.Player.OnNoSupportSpotFound?.Invoke();
                    }
                });
        }


        public bool IsControllingPlayer(Player player)
        {
            return ControllingPlayer == player;
        }

        public bool IsGoalKeeper(Player player)
        {
            return GoalKeeper == player;
        }

        public Player FindClosestPlayerToPosition(Vector3 position, bool ignoreControllingPlayer = true, bool ignoreGoalKeeper = true)
        {
            // get player
            Player closestPlayerToPosition = Players
                .Where(tP => (ignoreControllingPlayer == false && IsGoalKeeper(tP.Player) == true)
                    || (ignoreGoalKeeper == false && IsGoalKeeper(tP.Player) == true))
                .OrderBy(p => Vector3.Distance(p.Player.Position, position))
                .Select(p => p.Player)
                .FirstOrDefault();

            // return result
            return closestPlayerToPosition;
        }

        public TeamPlayer GetClosestPlayerToPoint(Vector3 position)
        {
            // get the closest player to point
            TeamPlayer player = Players
                .Where(tm => tm.Player.PlayerType == PlayerTypes.InFieldPlayer
                && tm.Player.InFieldPlayerFSM.IsCurrentState<TackledMainState>() == false)
                .OrderBy(tm => Vector3.Distance(tm.Player.Position,
                position))
                .First();

            // return player
            return player;
        }

        public TeamPlayer GetClosestPlayerToThreat(Vector3 position)
        {
            // get the closest player to point
            TeamPlayer player = Players
                .Where(tP => tP.Player.PlayerType == PlayerTypes.InFieldPlayer
                    && tP.Player.InFieldPlayerFSM.IsCurrentState<TackledMainState>() == false
                    && tP.Player.InFieldPlayerFSM.IsCurrentState<PickOutThreatMainState>() == false
                    && tP.Player.IsPositionWithinDistance(tP.CurrentHomePosition.position, position, tP.Player.DistanceMaxWonder))
                .OrderBy(tm => Vector3.Distance(tm.Player.Position, position))
                .FirstOrDefault();

            // return player
            return player;
        }

        public List<TeamPlayer> GetThreateningPlayers()
        {
            // get threatening players
            // todo::prioritise players to track depending on possibility to receive a pass and to score
            List<TeamPlayer> threataningPlayers = _opponent.Players
                .Where(tM => tM.Player.IsPickedOut(null) == false
                    && tM.Player.PlayerType == PlayerTypes.InFieldPlayer
                    && tM.Player != _opponent.ControllingPlayer)
                .ToList();

            // return result
            return threataningPlayers;
        }

        public Goal Goal
        {
            get
            {
                return _goal;
            }
        }

        public Team Opponent
        {
            get
            {
                return _opponent;
            }
        }

        public Transform RootPlayers
        {
            get
            {
                return _rootPlayers;
            }
        }

        public Transform KickOffRefDirection { get => _kickOffRefDirection; set => _kickOffRefDirection = value; }
        public Transform PlayerSupportSpots { get => _playerSupportSpots; set => _playerSupportSpots = value; }

        public float DistancePassMax { get => _distancePassMax; set => _distancePassMax = value; }
        public float DistancePassMin { get => _distancePassMin; set => _distancePassMin = value; }
        public float Power { get => _power; set => _power = value; }
        public float Speed { get => _speed; set => _speed = value; }

        public float DistanceThreatMin
        {
            get => _distanceThreatMin;
            set => _distanceThreatMin = value;
        }

        public float DistanceThreatMax
        {
            get => _distanceThreatMax;
            set => _distanceThreatMax = value;
        }
        public float DistanceShotValidMax { get => _distanceShotValidMax; set => _distanceShotValidMax = value; }
        public float DistanceTendGoal { get => _distanceTendGoal; set => _distanceTendGoal = value; }
        public float DistanceThreatTrack { get => _distanceThreatTrack; set => _distanceThreatTrack = value; }
        public float DistanceWonderMax { get => _distanceWonderMax; set => _distanceWonderMax = value; }
        public float VelocityShortPassArrive { get => _velocityShortPassArrive; set => _velocityShortPassArrive = value; }
        public float VelocityShotArrive { get => _velocityShotArrive; set => _velocityShotArrive = value; }
        public InGameTeamDto TeamData { get => _teamData; set => _teamData = value; }
        public Transform RootFormations { get => _rootFormations; set => _rootFormations = value; }
        public Formation Formation { get => _formation; set => _formation = value; }
        public float Reach { get => _reach; set => _reach = value; }
        public float DiveSpeed { get => _diveSpeed; set => _diveSpeed = value; }
        public float DiveDistance { get => _jumpDistance; set => _jumpDistance = value; }
        public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
        public float JogSpeed { get => _jogSpeed; set => _jogSpeed = value; }
        public float VelocityLongPassArrive { get => _velocityLongPassArrive; set => _velocityLongPassArrive = value; }
        public Transform PitchCenterSpot { get => _pitchCenterSpot; set => _pitchCenterSpot = value; }
        public CornerGoalKickTriggers CornerGoalKickTriggers { get => _cornerGoalKickTriggers; set => _cornerGoalKickTriggers = value; }
        public Transform LeftSideGoalKickSpot { get => _leftSideGoalKickSpot; set => _leftSideGoalKickSpot = value; }
        public Transform RightSideGoalKickSpot { get => _rightSideGoalKickSpot; set => _rightSideGoalKickSpot = value; }
        public Transform LeftSideCornerKickSpot { get => _leftSideCornerKickSpot; set => _leftSideCornerKickSpot = value; }
        public Transform RightSideCornerKickSpot { get => _rightSideCornerKickSpot; set => _rightSideCornerKickSpot = value; }
        public PitchRegions PitchRegions { get => _pitchRegions; set => _pitchRegions = value; }
        public float AiUpdateFrequency { get => _aiUpdateFrequency; set => _aiUpdateFrequency = value; }
    }

    [Serializable]
    public class PitchRegions
    {
        [SerializeField]
        GameObject _cornerKickDefendRegion;

        [SerializeField]
        GameObject _cornerKickSupportRegion;

        public GameObject CornerKickSupportRegion { get => _cornerKickSupportRegion; set => _cornerKickSupportRegion = value; }
        public GameObject CornerKickDefendRegion { get => _cornerKickDefendRegion; set => _cornerKickDefendRegion = value; }
    }

    [Serializable]
    public class TeamPlayer
    {
        public Player Player;

        public Transform AttackingHomePosition;

        public Transform DefendingHomePosition;

        public Transform CurrentHomePosition;

        public Transform KickOffHomePosition;

        public TeamPlayer(Player player, 
            Team team, 
            Transform attackingHomePosition, 
            Transform defendingHomePosition, 
            Transform currentPlayerHomePosition, 
            Transform kickOffHomePosition,
            float aiUpdateFrequency,
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
            float power,
            float reach,
            float speed,
            int playerIndex,
            PitchRegions pitchRegions,
            InGameTeamDto teamDto)
        {
            //init player details
            Player = player;

            // get player data
            InGamePlayerDto playerDto = teamDto.Players[playerIndex];
            InGameKitDto kit = teamDto.Kit;

            // calculate long ball frequency for team
            float forwardRunFrequency = teamDto.AttackTactic.ForwardRunProbability;
            float longBallFrequency = teamDto.AttackTactic.LongBallProbability;
            float tightPressFrequency = teamDto.DefendTactic.TightPressFrequency;

            // initialize player
            Player.Init(aiUpdateFrequency,
                distancePassMax,
                distancePassMin,
                distanceShotValidMax,
                distanceTendGoal,
                distanceThreatMax,
                distanceThreatMin,
                distanceThreatTrack,
                distanceWonderMax,
                velocityLongPassArrive,
                velocityShortPassArrive,
                velocityShotArrive,
                diveDistance,
                diveSpeed,
                jogSpeed,
                jumpHeight,
                forwardRunFrequency,
                longBallFrequency,
                tightPressFrequency,
                power, 
                reach,
                speed,
                pitchRegions,
                kit,
                playerDto,
                team);

            // initialize the formation positions
            AttackingHomePosition = attackingHomePosition;
            DefendingHomePosition = defendingHomePosition;
            CurrentHomePosition = currentPlayerHomePosition;
            KickOffHomePosition = kickOffHomePosition;

            //set the initial home position of the player
            Player.HomePosition = CurrentHomePosition;
        }
    }
}
