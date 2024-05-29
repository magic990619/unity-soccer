using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.HalfTimeMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
//
using Assets.FootballGameEngine_Indie_.Scripts.Managers;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.MainMenu
{
    [Serializable]
    public class GameOnMainMenu : BMenu
    {
        [SerializeField]
        HalfTimeMainMenu _halfTimeMainMenu;

        [SerializeField]
        InitMenu _initMenu;

        [SerializeField]
        MatchInPlayMainMenu _matchInPlayMainMenu;

        [SerializeField]
        MatchPausedMenu _matchPausedMenu;

        public InitMenu InitMenu { get => _initMenu; set => _initMenu = value; }
        public MatchInPlayMainMenu MatchInPlayMainMenu { get => _matchInPlayMainMenu; set => _matchInPlayMainMenu = value; }
        public MatchPausedMenu MatchPausedMenu { get => _matchPausedMenu; set => _matchPausedMenu = value; }
        public HalfTimeMainMenu HalfTimeMainMenu { get => _halfTimeMainMenu; set => _halfTimeMainMenu = value; }

        public void DisableChildren()
        {
            _halfTimeMainMenu.Root.SetActive(false);
            _initMenu.Root.SetActive(false);
            _matchInPlayMainMenu.Root.SetActive(false);
            _matchPausedMenu.Root.SetActive(false);
        }
    }
}
