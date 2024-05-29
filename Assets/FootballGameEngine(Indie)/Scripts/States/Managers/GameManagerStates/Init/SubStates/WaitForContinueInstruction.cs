using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.SubStates
{
    public class WaitForContinueInstruction : BState
    {
        public override void Enter()
        {
            base.Enter();

            // enable the wait for instruction menu
            GraphicsManager.Instance.InitMainMenu.EnableWaitForContinueInstructionSubMenu();
        }

        public override void Execute()
        {
            base.Execute();

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (ControlFreak2.CF2Input.anyKeyDown)
            {
                SuperMachine.ChangeState<HomeMainState>();
                GameManager.Instance.OnButtonClicked();
            }
#else
            if(ControlFreak2.CF2Input.anyKeyDown)
            {                
                SuperMachine.ChangeState<HomeMainState>();
                GameManager.Instance.OnButtonClicked();
            }
#endif

        }

        public override void Exit()
        {
            base.Exit();

            // disable the wait for instruction menu
            GraphicsManager.Instance.InitMainMenu.DisableWaitForContinueInstructionSubMenu();
        }
    }
}
