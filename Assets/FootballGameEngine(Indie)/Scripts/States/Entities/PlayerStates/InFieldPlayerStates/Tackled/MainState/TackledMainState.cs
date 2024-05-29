using System;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState
{
    public class TackledMainState : BState
    {
        bool _hasExitedStartTackledState;
        float _currWaitTime;
        float _waitTime = 3f;
        FinishTackledAnimBehaviour _finishTackledAnimBehaviour;

        public override void Initialize()
        {
            base.Initialize();

            _finishTackledAnimBehaviour = Owner.Animator.GetBehaviour<FinishTackledAnimBehaviour>();
            _finishTackledAnimBehaviour.OnExit += Instance_OnExit;
        }

        private void Instance_OnExit()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
        }

        public override void Enter()
        {
            base.Enter();

            //set the wait time
            _hasExitedStartTackledState = false;
            _currWaitTime = _waitTime;

            // set the animator
            Owner.Animator.SetTrigger("Idle");
            Owner.Animator.SetTrigger("Tackled");

            // show the injured icon
            Owner.PlayerHealthInfoWidget.Root.SetActive(true);
        }

        public override void Execute()
        {
            base.Execute();

            if (_hasExitedStartTackledState == false)
            {
                // update the meter
                Owner.PlayerHealthInfoWidget.Meter.fillAmount = _currWaitTime / _waitTime;

                //decrement time
                _currWaitTime -= Time.deltaTime;

                //if time if exhausted trigger approprite state transation
                if (_currWaitTime <= 0)
                {
                    _hasExitedStartTackledState = true;

                    // trigger the exit state
                    Owner.Animator.SetTrigger("ExitStartTackled");
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
            Owner.Animator.ResetTrigger("Idle");
            Owner.Animator.ResetTrigger("Tackled");

            // show the injured icon
            Owner.PlayerHealthInfoWidget.Root.SetActive(false);
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
