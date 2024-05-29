using System;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Patterns.Singleton;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    public class Ball : Singleton<Ball>
    {
        [SerializeField]
        [Min(0)]
        float _friction = 3f;

        [SerializeField]
        string _groundMaskName;

        [SerializeField]
        GameObject _ballModel;

        [SerializeField]
        IKTargets _ballIkTargets;

        bool _isAutoRotatingBall;
        bool _isGrounded;
        float _rayCastDistance;
        int _groundMask;
        RaycastHit _hit;
        Vector3 _frictionVector;
        Vector3 _rayCastStartPosition;

        public Action<Shot> OnBallShot;

        public delegate void BallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target);

        public BallLaunched OnBallLaunched;

        public float Friction { get => _friction; set => _friction = value; }

        public Player CurrentOwner { get; set; }
        public Player OwnerWithLastTouch { get; set; }

        public Rigidbody Rigidbody { get; set; }
        public SphereCollider SphereCollider { get; set; }

        public override void Awake()
        {
            base.Awake();

            //get the components
            Rigidbody = GetComponent<Rigidbody>();
            SphereCollider = GetComponent<SphereCollider>();

            //init some variables
            _groundMask = LayerMask.GetMask(_groundMaskName);
            _rayCastDistance = SphereCollider.radius + 0.05f;

            // set the friction
            _friction = _friction < 0 ? _friction : -1 * _friction;
        }

        private void FixedUpdate()
        {
            ApplyFriction();
        }

        private void Update()
        {
            if(_isAutoRotatingBall == true)
            {
                
                // get ball velocity
                float ballVelocity = Rigidbody.velocity.magnitude;
                if (ballVelocity <= 0.01f) ballVelocity = 0f;

                // rotate ball
                RotateBallModel(_ballModel.transform.right, ballVelocity * 200f);
                
            }
        }

        /// <summary>
        /// Applies friction to this instance
        /// </summary>
        public void ApplyFriction()
        {
            //get the direction the ball is travelling
            _frictionVector = Rigidbody.velocity.normalized;
            _frictionVector.y = 0f;

            //calculate the actual friction
            _frictionVector *=  _friction;

            //calculate the raycast start positiotn
            _rayCastStartPosition = transform.position + SphereCollider.radius * Vector3.up;

            //check if the ball is touching with the pitch
            //if yes apply the ground friction force
            //else apply the air friction
            _isGrounded = Physics.Raycast(_rayCastStartPosition,
                Vector3.down,
                out _hit,
                _rayCastDistance,
                _groundMask);

            //apply friction if grounded
            if (_isGrounded)
                Rigidbody.AddForce(_frictionVector);

#if UNITY_EDITOR
            Debug.DrawRay(_rayCastStartPosition, 
                Vector3.down * _rayCastDistance, 
                Color.red);
#endif

        }

        public void AttachBallToParent(Transform parentTransform, Quaternion rotation)
        {
            transform.SetParent(parentTransform);
            transform.rotation = rotation;
        }

        public void DetachBallToParent()
        {
            transform.SetParent(null);
        }

        public void DisablePhysics()
        {
            Rigidbody.isKinematic = true;
            Rigidbody.useGravity = false;
        }

        public void EnablePhysics()
        {
            Rigidbody.isKinematic = false;
            Rigidbody.useGravity = true;
        }

        public Vector3 FuturePosition(float time)
        {
            //get the velocities
            Vector3 velocity = Rigidbody.velocity;
            Vector3 velocityXZ = velocity;
            velocityXZ.y = 0f;

            //find the future position on the different axis
            float futurePositionY = Position.y + (velocity.y * time + (0.5f * Physics.gravity.y * Mathf.Pow(time, 2)));
            Vector3 futurePositionXZ = Vector3.zero;

            //get the ball future position
            futurePositionXZ = Position + velocityXZ.normalized * velocityXZ.magnitude * time;

            //bundle the future positions to together
            Vector3 futurePosition = futurePositionXZ;
            futurePosition.y = futurePositionY;

            //return the future position
            return futurePosition;
        }

        /// <summary>
        /// Finds the power needed to kick an entity to reach it's destination
        /// with the specifed velocity
        /// </summary>
        /// <param name="from">The initial position</param>
        /// <param name="to">The final position</param>
        /// <param name="finalVelocity">The initial velocity</param>
        /// <returns></returns>
        public float FindPower(Vector3 from, Vector3 to, float finalVelocity)
        {
            // v^2 = u^2 + 2as => u^2 = v^2 - 2as => u = root(v^2 - 2as)
            return Mathf.Sqrt(Mathf.Pow(finalVelocity, 2f) - (2 * _friction * Vector3.Distance(from, to)));
        }

        public void KickInDirection(Vector3 direction, float power)
        {
            // normalize direction
            direction.Normalize();

            // kick in direction
            Rigidbody.velocity = direction * power;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(0f, 0f, new Vector3(), new Vector3());
        }

        /// <summary>
        /// Kicks the ball to the target
        /// </summary>
        /// <param name="to"></param>
        /// <param name="power"></param>
        public void KickToPoint(Vector3 to, float power)
        {
            Vector3 direction = to - NormalizedPosition;
            direction.Normalize();

            //change the velocity
            direction.y = 0.015f;
            Rigidbody.velocity = direction * power;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(0f, power, NormalizedPosition, to);
        }

        public void LaunchToPoint(Vector3 final, float power)
        {
            //set the initial position
            Vector3 initial = Position;

            //find the direction vectors
            Vector3 toTarget = final - initial;
            Vector3 toTargetXZ = toTarget;
            toTargetXZ.y = 0;

            //find the time to target
            float time = toTargetXZ.magnitude / power;

            // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
            // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
            // so xz = v0xz * t => v0xz = xz / t
            // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
            toTargetXZ = toTargetXZ.normalized * toTargetXZ.magnitude / time;

            //set the y-velocity
            Vector3 velocity = toTargetXZ;
            velocity.y = toTarget.y / time + (0.5f * -Physics.gravity.y * time);

            //return the velocity
            Rigidbody.velocity = velocity;

            //invoke the ball launched event
            BallLaunched temp = OnBallLaunched;
            if (temp != null)
                temp.Invoke(time, power, initial, final);
        }

        public void RotateBallModel(Vector3 axis, float rotSpeed)
        {
            // rotate ball model
            _ballModel.transform.Rotate(axis, rotSpeed * 200f);
        }

        public void Trap()
        {
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
        }

        public float TimeToCoverDistance(Vector3 from, Vector3 to, float initialVelocity, bool factorInFriction = true)
        {
            //find the distance
            float distance = Vector3.Distance(from, to);

            //if I'm not factoring friction or I'm factoring in friction but no friction has been specified
            //simply assume there is no friction(ball is self accelerating)
            if(!factorInFriction || (factorInFriction && _friction == 0))
            {
                return distance / initialVelocity;
            }
            else
            {
                // v^2 = u^2 + 2as
                float v_squared = Mathf.Pow(initialVelocity, 2f) + (2 * _friction * Vector3.Distance(from, to));

                //if v_squared is less thatn or equal to zero it means we can't reach the target
                if (v_squared <= 0)
                    return -1.0f;

                // t = v-u
                //     ---
                //      a
                return (Mathf.Sqrt(v_squared) - initialVelocity) / (_friction);
            }
        }

        /// <summary>
        /// Get the normalized ball position
        /// </summary>
        public Vector3 NormalizedPosition
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

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
            }
        }

        public IKTargets BallIkTargets { get => _ballIkTargets; set => _ballIkTargets = value; }
        public GameObject BallModel { get => _ballModel; set => _ballModel = value; }
        public bool IsAutoRotatingBall { get => _isAutoRotatingBall; set => _isAutoRotatingBall = value; }
    }

    [Serializable]
    public struct IKTargets
    {
        [SerializeField]
        Transform _ikTargetBack;

        [SerializeField]
        Transform _ikTargetBottom;

        [SerializeField]
        Transform _ikTargetFront;

        [SerializeField]
        Transform _ikTargetLeft;

        [SerializeField]
        Transform _ikTargetRight;

        [SerializeField]
        Transform _ikTargetTop;

        public Transform IkTargetBack { get => _ikTargetBack; set => _ikTargetBack = value; }
        public Transform IkTargetBottom { get => _ikTargetBottom; set => _ikTargetBottom = value; }
        public Transform IkTargetFront { get => _ikTargetFront; set => _ikTargetFront = value; }
        public Transform IkTargetLeft { get => _ikTargetLeft; set => _ikTargetLeft = value; }
        public Transform IkTargetRight { get => _ikTargetRight; set => _ikTargetRight = value; }
        public Transform IkTargetTop { get => _ikTargetTop; set => _ikTargetTop = value; }
    }

}
