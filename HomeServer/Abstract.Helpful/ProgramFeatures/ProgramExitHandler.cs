using System;
using System.Collections.Generic;

namespace Abstract.Helpful.Lib.ProgramFeatures
{
    public static class ProgramExitHandler
    {
        public static void Start()
        {
            AppDomain.CurrentDomain.ProcessExit += (s,e) => OnProcessExit();   
        }

        private static readonly List<Action> subscribers = new();
        
        private static void OnProcessExit()
        {
            foreach (var subscriber in subscribers)
                subscriber();
        }

        public static void SubscribeOnExit(Action action)
        {
            Start();
            subscribers.Add(action);
        }
    }
}