using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems;
using SmartMenuManagement.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus
{
    [Serializable]
    public class PreMatchMatchInfoSubMenu : BSubMenu
    {
        [SerializeField]
        Button _btnMatchSettings;

        [SerializeField]
        Button _btnTeamManagement;

        [SerializeField]
        Text _txtHalfLength;

        [SerializeField]
        Text _txtMatchDifficulty;

        [SerializeField]
        TeamInfo _cpuTeamInfo;

        [SerializeField]
        TeamInfo _userTeamInfo;

        public Button BtnMatchSettings { get => _btnMatchSettings; set => _btnMatchSettings = value; }
        public Button BtnTeamManagement { get => _btnTeamManagement; set => _btnTeamManagement = value; }

        public void Init(string halfLength, string matchDifficulty, TeamDto cpuTeamData, TeamDto userTeamData)
        {
            _txtHalfLength.text = string.Format("{0}mins", halfLength);
            _txtMatchDifficulty.text = matchDifficulty;

            // init the teams
            _cpuTeamInfo.Init(cpuTeamData);
            _userTeamInfo.Init(userTeamData);
        }
    }

    [Serializable]
    public class TeamInfo
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Text _teamName;

        [SerializeField]
        GameObject _playerItemsRoot;

        [SerializeField]
        private PlayerItem001 _playerItemPrefab;


        public void Init(TeamDto teamData)
        {
            // amount of players to render
            int count = 0;

            // init team details
            _teamName.text = teamData.Name;
            _icon.sprite = teamData.Icon;

            // clean the player itesm first
            _playerItemsRoot.transform.GetComponentsInChildren<PlayerItem001>().ToList()
                .ForEach(pI => GameObject.Destroy(pI.gameObject));

            // init team player details
            foreach (var tM in teamData.Players)
            {
                //get formation
                Formation formation = FormationManager.Instance.GetFormation(teamData.FormationType);

                // create new item
                var playerItem = GameObject.Instantiate(_playerItemPrefab, _playerItemsRoot.transform);
                playerItem.Init(tM.KitName, formation.GetPosition(count));

                // decrement count
                ++count;

                // if count is less than zero break out of loop
                if (count >= 11)
                    return;
            }
        }
    }
}
