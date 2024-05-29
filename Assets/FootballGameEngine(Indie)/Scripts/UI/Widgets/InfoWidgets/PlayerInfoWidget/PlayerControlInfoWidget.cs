using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.InfoWidgets.PlayerInfoWidget
{
    [Serializable]
    public class PlayerControlInfoWidget : MonoBehaviour
    {
        [SerializeField]
        Image _controlIcon;

        [SerializeField]
        Image _receiverIcon;

        [SerializeField]
        GameObject _root;

        public Image Icon { get => _controlIcon; set => _controlIcon = value; }
        public Image Meter { get => _receiverIcon; set => _receiverIcon = value; }
        public GameObject Root { get => _root; set => _root = value; }
    }
}
