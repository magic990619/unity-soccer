using Assets.FootballGameEngine_Indie.Scripts.Controllers;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.SubStates
{
    public class PrepareForCornerKick : BState
    {
        bool _hasPlacedPlayers;
        bool isBallOnLeftSide;

        float _waitTime;

        Vector3 _rotationVector;
        TeamPlayer _teamPlayer = null;


        // get mesh vertices
        Vector3[] cornerKickDefendMeshVertices;
        Vector3[] cornerKickSupportMeshVertices;

        public override void Initialize()
        {
            base.Initialize();

            // get mesh vertices
            cornerKickDefendMeshVertices = Owner.PitchRegions.CornerKickDefendRegion.GetComponent<MeshFilter>().mesh.vertices;
            cornerKickSupportMeshVertices = Owner.PitchRegions.CornerKickSupportRegion.GetComponent<MeshFilter>().mesh.vertices;
        }

        public override void Enter()
        {
            base.Enter();

            // reset stuff
            _hasPlacedPlayers = false;

            if(Owner.HasCornerKick == true)
            {
                _waitTime = 0.2f;

                // place entities
                PlaceBallAtCornerKickPosition();
                PlaceControllingPlayerAtPosition();
                PlaceEveryOtherSupportingPlayerAtPosition();
                MoveCameraToPosition();
            }
            else
                _waitTime = 0.1f;
        }

        public override void Execute()
        {
            base.Execute();

            if(_hasPlacedPlayers == false)
            {
                _waitTime -= Time.deltaTime;
                if(_waitTime <= 0f)
                {
                    if (Owner.HasCornerKick == true)
                    {
                        // trigger state change
                        Machine.ChangeState<TakeCornerKick>();
                    }
                    else
                    {
                        // trigger state change
                        Machine.ChangeState<DefendCornerKick>();
                    }
                }
            }
        }

        void MoveCameraToPosition()
        {
            // place the camera at position
            CameraController.Instance.Target = Owner.PitchRegions.CornerKickSupportRegion.transform;
            Vector3 cameraPosition = CameraController.Instance.CalculateNextPosition();
            CameraController.Instance.Position = cameraPosition;

            // if the ball is behind the camera then track the ball
            Vector3 dirFromCamToBall = Ball.Instance.Position - CameraController.Instance.Position;
            dirFromCamToBall.y = 0f;
            Vector3 camForward = CameraController.Instance.transform.forward;
            camForward.y = 0f;

            float dot = Vector3.Dot(camForward, dirFromCamToBall.normalized);
            if(dot < 0)
            {
                CameraController.Instance.Target = Ball.Instance.transform;
                cameraPosition = CameraController.Instance.CalculateNextPosition();
                CameraController.Instance.Position = cameraPosition;
            }
        }

        void PlaceBallAtCornerKickPosition()
        {
            // stop ball
            Ball.Instance.Trap();

            // check side
            Vector3 localBallPosition = Owner.Goal.transform.InverseTransformPoint(Owner.CachedBallPosition);
            isBallOnLeftSide = localBallPosition.x < 0;

            // place ball
            if(isBallOnLeftSide)
                Ball.Instance.Position = Owner.LeftSideCornerKickSpot.position;
            else
                Ball.Instance.Position = Owner.RightSideCornerKickSpot.position;

        }

        void PlaceControllingPlayerAtPosition()
        {

            if(isBallOnLeftSide == true)
            {
                _rotationVector = Vector3.RotateTowards(Owner.transform.forward, Owner.transform.right, 90f * Mathf.Deg2Rad, 0f);
                _teamPlayer = GetPlayer(Owner.TeamData.LeftSideCornerKickTaker);
            }
            else
            {
                _rotationVector = Vector3.RotateTowards(Owner.transform.forward, Owner.transform.right, -90f * Mathf.Deg2Rad, 0f);
                _teamPlayer = GetPlayer(Owner.TeamData.RightSideCornerKickTaker);
            }

            // set the position
            Vector3 position = Ball.Instance.Position + _rotationVector * -1f;
            _teamPlayer.CurrentHomePosition.position = position;
            _teamPlayer.Player.transform.position = position;
            _teamPlayer.Player.transform.rotation = Quaternion.LookRotation(_rotationVector);

            //get the take kick of state and set the controlling player
            Owner.ControllingPlayer = _teamPlayer.Player;
        }

        void PlaceEveryOtherSupportingPlayerAtPosition()
        {
            // control loop
            int loopControl = 11;

            // place defending players at pos
            Owner.Players.Where(tM => tM != _teamPlayer
                && tM.Player.PlayerType != PlayerTypes.Goalkeeper
                && tM.Player.CanJoinCornerKick == false)
            .ToList()
            .ForEach(tM =>
            {
                // find a random position on the corner-kick support spot
                Vector3 position;
                do
                {
                    // decrement
                    --loopControl;

                    // get the pos
                    int randomIndex = Random.Range(0, cornerKickDefendMeshVertices.Length);
                    position = Owner.PitchRegions.CornerKickDefendRegion.transform.TransformPoint(cornerKickDefendMeshVertices[randomIndex]);

                } while (tM.Player.IsTeamMemberWithinDistance(5f, position) == true && loopControl <= 0);

                // place the players at pos
                tM.CurrentHomePosition.position = position;
                tM.Player.Position = tM.CurrentHomePosition.position;
                tM.Player.transform.LookAt(Ball.Instance.Position);
            });

            // reset loop control
            loopControl = 11;

            // place supporting players
            Owner.Players.Where(tM => tM != _teamPlayer 
                && tM.Player.PlayerType != PlayerTypes.Goalkeeper 
                && tM.Player.CanJoinCornerKick == true)
            .ToList()
            .ForEach(tM =>
            {
                // find a random position on the corner-kick support spot
                Vector3 position;
                do
                {
                    // decrement
                    --loopControl;

                    // get the pos
                    int randomIndex = Random.Range(0, cornerKickSupportMeshVertices.Length);
                    position = Owner.PitchRegions.CornerKickSupportRegion.transform.TransformPoint(cornerKickSupportMeshVertices[randomIndex]);

                } while (tM.Player.IsTeamMemberWithinDistance(5f, position) == true && loopControl <= 0);

                // place the players at pos
                tM.CurrentHomePosition.position = position;
                tM.Player.Position = tM.CurrentHomePosition.position;
                tM.Player.transform.LookAt(Ball.Instance.Position);
            });
        }

        TeamPlayer GetPlayer(int id)
        {
            return Owner.Players[id];
        }

        public FootballGameEngine_Indie.Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
