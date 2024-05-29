using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Utilities
{
    public class ActionUtility
    {
        public static void Invoke_Action(Action action)
        {
            //get the action
            Action temp = action;

            //run action if not null
            if (temp != null) temp.Invoke();
        }

        public static void Invoke_Action(Pass pass, Action<Pass> action)
        {
            Action<Pass> temp = action;
            if (temp != null) temp.Invoke(pass);
        }

        public static void Invoke_Action(string message, Action<string> action)
        {
            Action<string> temp = action;
            if (temp != null) temp.Invoke(message);
        }

        public static void Invoke_Action(Vector3 position, Action<Vector3> action)
        {
            Action<Vector3> temp = action;
            if (temp != null) temp.Invoke(position);
        }

        public static void Invoke_Action(int currHalf, int minutes, int seconds, Action<int, int, int> action)
        {
            Action<int, int, int> temp = action;
            if (temp != null) temp.Invoke(currHalf, minutes, seconds);
        }

        public static void Invoke_Action(Shot shot, Action<Shot> action)
        {
            Action<Shot> temp = action;
            if (temp != null) temp.Invoke(shot);
        }
    }
}
