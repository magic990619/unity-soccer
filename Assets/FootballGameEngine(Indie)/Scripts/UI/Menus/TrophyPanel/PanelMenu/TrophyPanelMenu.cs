using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.TrophyPanel.PanelMenu
{
    [Serializable]
    public class TrophyPanelMenu : BMenu
    {
        [SerializeField]
        Button _btnCancelTrophy;

        public Button BtnCancelTrophy { get => _btnCancelTrophy; set => _btnCancelTrophy = value; }
    }
}
