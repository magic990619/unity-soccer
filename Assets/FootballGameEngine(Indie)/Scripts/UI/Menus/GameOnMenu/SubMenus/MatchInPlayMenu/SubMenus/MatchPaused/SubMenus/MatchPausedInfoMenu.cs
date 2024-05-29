using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus.MatchPaused.SubMenus
{
    [Serializable]
    public class MatchPausedInfoMenu : BSubMenu
    {
        [SerializeField]
        Button _btnResume;

        [SerializeField]
        Button _btnRestart;

        [SerializeField]
        Button _btnQuit;

        [SerializeField]
        Button _btnTeamManagement;

        [SerializeField]
        Text _txtInfo;

        public Text TxtInfo { get => _txtInfo; set => _txtInfo = value; }
        public Button BtnRestart { get => _btnRestart; set => _btnRestart = value; }
        public Button BtnResume { get => _btnResume; set => _btnResume = value; }
        public Button BtnQuit { get => _btnQuit; set => _btnQuit = value; }
        public Button BtnTeamManagement { get => _btnTeamManagement; set => _btnTeamManagement = value; }
    }
}
