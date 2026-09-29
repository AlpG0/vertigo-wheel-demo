using System;
using System.Collections.Generic;
using UnityEngine;

namespace VertigoWheel.Core
{
    /// <summary>
    /// State'leri tutar ve sadece izin verilen gecislere izin verir. Tanimlanmamis bir gecis denenirse hata loglar ve state degismez.
    /// </summary>
    public class GameStateMachine
    {
        private readonly Dictionary<Type, GameState> states = new Dictionary<Type, GameState>();
        private readonly Dictionary<Type, HashSet<Type>> allowedTransitions = new Dictionary<Type, HashSet<Type>>();
        private GameState current;

        public GameState Current { get { return current; } }

        public void AddState(GameState state)
        {
            states[state.GetType()] = state;
        }

        public void AllowTransition<TFrom, TTo>() where TFrom : GameState where TTo : GameState
        {
            HashSet<Type> targets;
            if (!allowedTransitions.TryGetValue(typeof(TFrom), out targets))
            {
                targets = new HashSet<Type>();
                allowedTransitions[typeof(TFrom)] = targets;
            }

            targets.Add(typeof(TTo));
        }

        public void Start<T>() where T : GameState // ilk state, gecis kontrolu olmadan
        {
            current = states[typeof(T)];
            current.Enter();
        }

        public void ChangeState<T>() where T : GameState
        {
            Type from = current.GetType();
            Type to = typeof(T);

            HashSet<Type> targets;
            if (!allowedTransitions.TryGetValue(from, out targets) || !targets.Contains(to))
            {
                Debug.LogError(string.Format("Gecersiz state gecisi: {0} -> {1}", from.Name, to.Name));
                return;
            }

            current.Exit();
            current = states[to];
            current.Enter();
        }
    }
}
