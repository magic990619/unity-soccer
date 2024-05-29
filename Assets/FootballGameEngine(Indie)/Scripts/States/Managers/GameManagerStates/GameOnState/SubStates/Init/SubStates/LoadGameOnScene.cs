using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using RobustFSM.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.Init.SubStates
{
    public class LoadGameOnScene : BState
    {
        bool canUpdate;
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set wait time
            canUpdate = false;
            waitTime = 1f;

            // register to the scene load event
            SceneManager.sceneLoaded += this.Instance_OnSceneLoaded;

            // load the game on scene
            SceneManager.LoadSceneAsync("GameOnScene", LoadSceneMode.Single);

            // enable the loading menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .LoadMenu
                .Root.
                gameObject
                .SetActive(true);
        }

        public override void Execute()
        {
            base.Execute();

            if(canUpdate)
            {
                // decrement the wit time
                waitTime -= Time.deltaTime;

                if(waitTime <= 0f)
                    Machine.ChangeState<InitializeSceneWithData>();
            }
        }

        void Instance_OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            // get the name of the loaded scene
            bool isGameOnSceneLoaded = arg0.name == "GameOnScene";

            if (isGameOnSceneLoaded)
                canUpdate = true;
        }
    }
}
