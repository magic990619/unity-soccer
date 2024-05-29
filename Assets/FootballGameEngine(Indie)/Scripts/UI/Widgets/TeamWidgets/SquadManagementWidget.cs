using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.TeamWidgets
{
    [Serializable]
    public class SquadManagementWidget
    {
        [SerializeField]
        PlayerItem002 _playerItemPrefab;

        [SerializeField]
        Button _swapButton;

        [SerializeField]
        Transform _firstElevenRoot;

        [SerializeField]
        Transform _substitutesRoot;

        public Transform FirstElevenRoot { get => _firstElevenRoot; set => _firstElevenRoot = value; }
        public Transform SubstitutesRoot { get => _substitutesRoot; set => _substitutesRoot = value; }
        public Button SwapButton { get => _swapButton; set => _swapButton = value; }

        public void Init(TeamDto teamData, UnityAction<PlayerItem002> onClickPlayerItemButton, UnityAction onClickSwapButton)
        {
            // init stuff
            InitSquadPlayersList(teamData, onClickPlayerItemButton);
            InitSwapButton(onClickSwapButton);
        }

        private void InitSquadPlayersList(TeamDto teamDto, UnityAction<PlayerItem002> unityAction)
        {
            // prepare data
            int index = 0;

            // clear it first
            _firstElevenRoot.GetComponentsInChildren<PlayerItem002>().ToList()
                .ForEach(pI => GameObject.Destroy(pI.gameObject));
            _substitutesRoot.GetComponentsInChildren<PlayerItem002>().ToList()
                .ForEach(pI => GameObject.Destroy(pI.gameObject));

            // loop through each player an create his item
            foreach (var tP in teamDto.Players)
            {
                if (index <= 10)
                {
                    // create the first eleven
                    var playerItem = GameObject.Instantiate(_playerItemPrefab, _firstElevenRoot);
                    playerItem.Init(tP.Id, tP.KitNumber, tP.KitName, FormationManager.Instance.GetFormation(teamDto.FormationType).GetPosition(index));
                    InitPlayerItem(playerItem, unityAction);
                }
                else
                {
                    // create the substitues
                    var playerItem = GameObject.Instantiate(_playerItemPrefab, _substitutesRoot);
                    playerItem.Init(tP.Id, tP.KitNumber, tP.KitName, tP.PlayerPosition.ToString());
                    InitPlayerItem(playerItem, unityAction);
                }

                // update inex
                ++index;
            }
        }

        private void InitPlayerItem(PlayerItem002 playerItem, UnityAction<PlayerItem002> unityAction)
        {
            playerItem.Button.onClick.RemoveAllListeners();
            playerItem.Button.onClick.AddListener(delegate { unityAction.Invoke(playerItem); });
        }

        private void InitSwapButton(UnityAction onClickSwapButton)
        {
            _swapButton.onClick.RemoveAllListeners();
            _swapButton.onClick.AddListener(onClickSwapButton);

            // disable it
            _swapButton.gameObject.SetActive(false);
        }
    }
}
