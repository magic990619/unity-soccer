using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Utilities.Misc
{
    public class BillBoard : MonoBehaviour
    {

        private void Update()
        {
            // always look at the main camera
            transform.LookAt(Camera.main.transform);
        }
    }
}
