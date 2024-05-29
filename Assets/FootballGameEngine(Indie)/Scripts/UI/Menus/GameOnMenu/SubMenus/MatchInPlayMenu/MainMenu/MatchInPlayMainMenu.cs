using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.MainMenu
{
    [Serializable]
    public class MatchInPlayMainMenu : BSubMenu
    {
        //[SerializeField]
        //HalfTimeMenu _halfTimeMenu;

        [SerializeField]
        InfoMenu _infoMenu;

        [SerializeField]
        MatchOverMenu _matchOverMenu;

        //[SerializeField]
        //MatchPausedMenu _matchPausedMenu;

        [SerializeField]
        MatchPlayMenu _matchPlayMenu;

        [SerializeField]
        NormalTimeOverMenu _normalTimeOverMenu;

        public InfoMenu InfoMenu { get => _infoMenu; set => _infoMenu = value; }
        public MatchPlayMenu MatchPlayMenu { get => _matchPlayMenu; set => _matchPlayMenu = value; }
        //public HalfTimeMenu HalfTimeMenu { get => _halfTimeMenu; set => _halfTimeMenu = value; }
        public MatchOverMenu MatchOverMenu { get => _matchOverMenu; set => _matchOverMenu = value; }
        //public MatchPausedMenu MatchPausedMenu { get => _matchPausedMenu; set => _matchPausedMenu = value; }
        public NormalTimeOverMenu NormalTimeOverMenu { get => _normalTimeOverMenu; set => _normalTimeOverMenu = value; }

        public void DisableChildren()
        {
            //_halfTimeMenu.Root.SetActive(false);
            _infoMenu.Root.SetActive(false);
            _matchOverMenu.Root.SetActive(false);
            //_matchPausedMenu.Root.SetActive(false);
            _matchPlayMenu.Root.SetActive(false);
            _normalTimeOverMenu.Root.SetActive(false);
        }
    }
}
