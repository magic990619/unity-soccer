using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.InfoWidgets.PlayerInfoWidget
{
    [Serializable]
    public class PlayerNameInfoWidget : MonoBehaviour
    {
        [SerializeField]
        Text _displayName;

        [SerializeField]
        GameObject _root;

        public Text DisplayNameMesh { get => _displayName; set => _displayName = value; }
        public GameObject Root { get => _root; set => _root = value; }

        public void Init(string name)
        {
            _displayName.text = name;
        }
    }
}
