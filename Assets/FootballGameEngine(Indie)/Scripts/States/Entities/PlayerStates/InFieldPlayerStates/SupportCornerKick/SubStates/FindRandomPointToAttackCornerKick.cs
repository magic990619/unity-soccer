using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportCornerKick.SubStates
{
    /// <summary>
    /// Finds a random point wwithin the corner kick attack region
    /// </summary>
    public class FindRandomPointToAttackCornerKick : BState
    {
        Vector3 _target;
        Vector3[] _cornerKickSupportMeshVertices;

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Enter()
        {
            base.Enter();

            if(_cornerKickSupportMeshVertices == null)
            {
                // init mesh vertices
                _cornerKickSupportMeshVertices = Owner.PitchRegions
                    .CornerKickSupportRegion
                    .GetComponent<MeshFilter>()
                    .mesh
                    .vertices
                    .Select(v => Owner.PitchRegions.CornerKickSupportRegion.transform.TransformPoint(v))
                    .ToArray();
            }

            // if I can't join corner kick simply wait at point
            // todo::pick out possible threat
            if (Owner.CanJoinCornerKick == true)
            {
                // check the state to start with
                bool isStartingWithWaitAtPointState = Random.value <= 0.3f;

                // trigger state change
                if (isStartingWithWaitAtPointState == true)
                    Machine.ChangeState<WaitAtAttackCornerKickPoint>();
                else
                {
                    // find possible points
                    Vector3[] possibleSpots = _cornerKickSupportMeshVertices
                    .Where(v => (Owner.Position - v).magnitude >= 1f
                    && Owner.IsPositionWithinDistance(Owner.HomePosition.position, v, 5f)
                    && Owner.IsTeamMemberWithinDistance(5f, v) == false)
                    .ToArray();
                    
                    if (possibleSpots.Length > 0)
                    {
                        // choose point
                        _target = possibleSpots[Random.Range(0, possibleSpots.Length)];

                        // set the target
                        Machine.GetState<SteerToAttackCornerKickPoint>().Target = _target;
                        Machine.ChangeState<SteerToAttackCornerKickPoint>();
                    }
                    else
                    {
                        Machine.ChangeState<WaitAtAttackCornerKickPoint>();
                    }
                }

            }
            else
                Machine.ChangeState<WaitAtAttackCornerKickPoint>();
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
