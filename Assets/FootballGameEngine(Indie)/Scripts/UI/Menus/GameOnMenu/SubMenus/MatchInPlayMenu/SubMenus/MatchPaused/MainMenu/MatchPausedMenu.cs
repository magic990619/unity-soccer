using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus.MatchPaused.SubMenus;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus
{
    [Serializable]
    public class MatchPausedMenu : BSubMenu
    {
        [SerializeField]
        MatchPausedInfoMenu _matchPausedInfoMenu;

        [SerializeField]
        PreMatchTeamManagementSubMenu _teamManagementMenu;

        public MatchPausedInfoMenu MatchPausedInfoMenu { get => _matchPausedInfoMenu; set => _matchPausedInfoMenu = value; }
        public PreMatchTeamManagementSubMenu TeamManagementMenu { get => _teamManagementMenu; set => _teamManagementMenu = value; }

        public void DisableChildren()
        {
            _matchPausedInfoMenu.Root.SetActive(false);
            _teamManagementMenu.Root.SetActive(false);
        }
    }
}
