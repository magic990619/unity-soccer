using Patterns.Singleton;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Controllers
{
    public class CameraController : Singleton<CameraController>
    {
        [SerializeField]
        bool _canAutoFollowTarget = true;

        [SerializeField]
        float _distanceFollow = 15f;

        [SerializeField]
        float _distanceMaxXDisplacement = 20f;

        [SerializeField]
        float _distanceMaxZDisplacement = 30f;

        [SerializeField]
        float _distanceMinXDisplacement = 20f;

        [SerializeField]
        float _height = 15f;

        [SerializeField]
        float _speedFollow = 3f;

        [SerializeField]
        Transform _target;

        float _distanceMaxZ;
        float _distanceMinZ;
        float _cameraHeight;

        public override void Awake()
        {
            base.Awake();

            // set stuff
            _distanceMaxZ = transform.position.z + _distanceMaxZDisplacement;
            _distanceMinZ = transform.position.z - _distanceMaxZDisplacement;
            _cameraHeight = _target.position.y + _height;
        }

        private void LateUpdate()
        {
            // only follow if we are allowed to follow
            if (_canAutoFollowTarget)
            {
                // find the next position to move
                Vector3 nextPosition = CalculateNextPosition(_target.position);

                // set the next position
                transform.position = Vector3.MoveTowards(transform.position,
                    nextPosition,
                    _speedFollow * Time.deltaTime);
            }
        }

        public Vector3 CalculateNextPosition()
        {
            return CalculateNextPosition(_target.position);
        }

        public Vector3 CalculateNextPosition(Vector3 refPosition)
        {
            // find the next position to move
            Vector3 nextPosition = refPosition;

            // clean the psotion
            nextPosition.x = Mathf.Clamp(_target.position.x + _distanceFollow, _distanceMinXDisplacement, _distanceMaxXDisplacement);
            nextPosition.y = _cameraHeight;
            nextPosition.z = Mathf.Clamp(nextPosition.z, _distanceMinZ, _distanceMaxZ);

            // return result
            return nextPosition;
        }

        public bool CanAutoFollowTarget { get => _canAutoFollowTarget; set => _canAutoFollowTarget = value; }
        public Vector3 Position { get => transform.position; set => transform.position = value; }

        public Transform Target { get => _target; set => _target = value; }
    }
}
