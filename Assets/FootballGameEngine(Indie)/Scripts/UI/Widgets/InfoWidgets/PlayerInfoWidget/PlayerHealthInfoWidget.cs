using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.InfoWidgets.PlayerInfoWidget
{
    [Serializable]
    public class PlayerHealthInfoWidget : MonoBehaviour
    {
        [SerializeField]
        Image _icon;

        [SerializeField]
        Image _meter;

        [SerializeField]
        GameObject _root;

        public Image Icon { get => _icon; set => _icon = value; }
        public Image Meter { get => _meter; set => _meter = value; }
        public GameObject Root { get => _root; set => _root = value; }
    }
}
