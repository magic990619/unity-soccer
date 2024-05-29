using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus
{
    [Serializable]
    public class HalfTimeInfoMenu : BSubMenu
    {
        [SerializeField]
        Button _btnContinueToSecondHalf;

        [SerializeField]
        Button _btnRestart;

        [SerializeField]
        Button _btnTeamManagement;

        [SerializeField]
        Text _txtInfo;

        public Text TxtInfo { get => _txtInfo; set => _txtInfo = value; }
        public Button BtnRestart { get => _btnRestart; set => _btnRestart = value; }
        public Button BtnContinueToSecondHalf { get => _btnContinueToSecondHalf; set => _btnContinueToSecondHalf = value; }
        public Button BtnTeamManagement { get => _btnTeamManagement; set => _btnTeamManagement = value; }
    }
}
