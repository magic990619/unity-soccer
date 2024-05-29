using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.Background.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.ExitMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.HomeMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.Init.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.SettingsMenu.MainMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.TrophyPanel.PanelMenu;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.UtilityMenu.MainMenu;
using Patterns.Singleton;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Managers
{
   [RequireComponent(typeof(MenuManager))]
    public class GraphicsManager : Singleton<GraphicsManager>
    {
        [SerializeField]
        BackgroundMainMenu _backgroundMainMenu;

        [SerializeField]
        ExitMainMenu _exitMainMenu;

        [SerializeField]
        GameOnMainMenu _gameOnMainMenu;

        [SerializeField]
        HomeMainMenu _homeMainMenu;

        [SerializeField]
        InitMainMenu _initMainMenu;
        
        [SerializeField]
        PrepareForMatchMainMenu _prepareForMatchMainMenu;

        [SerializeField]
        SettingsMainMenu _settingsMainMenu;

        [SerializeField]
        UtilityMainMenu _utilityMainMenu;

        // Me Added
        [SerializeField]
        TrophyPanelMenu _trophyPanel;


        MenuManager _menuManager;

        public InitMainMenu InitMainMenu { get => _initMainMenu; set => _initMainMenu = value; }
        public MenuManager MenuManager { get => _menuManager; set => _menuManager = value; }
        public BackgroundMainMenu BackgroundMainMenu { get => _backgroundMainMenu; set => _backgroundMainMenu = value; }
        public HomeMainMenu HomeMainMenu { get => _homeMainMenu; set => _homeMainMenu = value; }
        public ExitMainMenu ExitMainMenu { get => _exitMainMenu; set => _exitMainMenu = value; }
        public TrophyPanelMenu TrophyPanel { get => _trophyPanel; set => _trophyPanel = value; }
     
        public SettingsMainMenu SettingsMainMenu { get => _settingsMainMenu; set => _settingsMainMenu = value; }
        public UtilityMainMenu UtilityMainMenu { get => _utilityMainMenu; set => _utilityMainMenu = value; }
        public PrepareForMatchMainMenu PrepareForMatchMainMenu { get => _prepareForMatchMainMenu; set => _prepareForMatchMainMenu = value; }
        public GameOnMainMenu GameOnMainMenu { get => _gameOnMainMenu; set => _gameOnMainMenu = value; }

        public override void Awake()
        {
            base.Awake();

            _menuManager = GetComponent<MenuManager>();
        }
    }
}
