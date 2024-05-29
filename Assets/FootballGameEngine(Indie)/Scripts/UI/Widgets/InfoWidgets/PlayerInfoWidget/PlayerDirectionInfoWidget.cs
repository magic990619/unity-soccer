using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.InfoWidgets.PlayerInfoWidget
{
    [Serializable]
    public class PlayerDirectionInfoWidget : MonoBehaviour
    {
        [SerializeField]
        GameObject _root;

        public GameObject Root { get => _root; set => _root = value; }
    }
}
