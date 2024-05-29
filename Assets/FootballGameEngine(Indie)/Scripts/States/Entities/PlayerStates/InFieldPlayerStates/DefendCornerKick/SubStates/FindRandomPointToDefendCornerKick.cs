using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates
{
    public class FindRandomPointToDefendCornerKick : BState
    {
        Vector3 _target;
        Vector3[] _cornerKickDefendMeshVertices;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Enter()
        {
            base.Enter();

            if(_cornerKickDefendMeshVertices == null)
            {
                // init mesh vertices
                _cornerKickDefendMeshVertices = Owner.OppositionMembers[0].PitchRegions
                    .CornerKickSupportRegion
                    .GetComponent<MeshFilter>()
                    .mesh
                    .vertices
                    .Select(v => Owner.OppositionMembers[0].PitchRegions.CornerKickSupportRegion.transform.TransformPoint(v))
                    .ToArray();
            }

            // check the state to start with
            bool isStartingWithWaitAtPointState = Random.value <= 0.3f;

            // trigger state change
            if (isStartingWithWaitAtPointState == true)
                Machine.ChangeState<WaitAtDefendCornerKickPoint>();
            else
            {
                // find possible points
                Vector3[] possibleSpots = _cornerKickDefendMeshVertices
                    .Where(v => (Owner.Position - v).magnitude >= 1f
                    && Owner.IsPositionWithinDistance(Owner.HomePosition.position, v, 5f)
                    && Owner.IsTeamMemberWithinDistance(5f, v) == false)
                    .ToArray();
                
                if (possibleSpots.Length > 0)
                {
                    // choose point
                    _target = possibleSpots[Random.Range(0, possibleSpots.Length)];

                    // set the target
                    Machine.GetState<SteerToDefendCornerKickPoint>().Target = _target;
                    Machine.ChangeState<SteerToDefendCornerKickPoint>();
                }
                else
                {
                    Machine.ChangeState<WaitAtDefendCornerKickPoint>();
                }
            }
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
