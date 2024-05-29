using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    public class SupportSpot : MonoBehaviour
    {
        [SerializeField]
        bool _isPickedOut;

        public Vector3 Position { get => transform.position; set => transform.position = value; }
        public MeshRenderer MeshRenderer { get; set; }
        public Player Owner { get; set; }

        private void Awake()
        {
#if UNITY_EDITOR
            MeshRenderer = GetComponent<MeshRenderer>();
            if(MeshRenderer != null)
                MeshRenderer.material.color = Color.red;
#endif
        }

        public void SetIsNotPickedOut()
        {
#if UNITY_EDITOR
            if (MeshRenderer != null)
                MeshRenderer.material.color = Color.red;
#endif
            // set is picked out
            _isPickedOut = false;

            // set the owner
            Owner = null;
        }

        public void SetIsPickedOut(Player player)
        {
#if UNITY_EDITOR
            if (MeshRenderer != null)
                MeshRenderer.material.color = Color.green;
#endif

            // set is picked out
            _isPickedOut = true;

            // set the owner
            Owner = player;
        }

        public bool IsPickedOut(Player player)
        {
            // if there is no owner then I'm not picked out
            // if the owner is not equal to the testing player then I'm picked out
            if (Owner == player)
                return false;
            else
                return _isPickedOut;
        }
    }
}
