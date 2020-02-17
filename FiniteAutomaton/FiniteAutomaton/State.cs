#nullable enable
#pragma warning disable RCS1037
namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class State
    {
        readonly private string? name;
        public string Name {get => name ?? Automaton.States.IndexOf(this).ToString(); }

        private Aplphabet<T> aplphabet;
        internal Dictionary<T, List<State> Follow {get; }

        internal State(FiniteAutomaton e, string? name = null)
        {
            Automaton = e;
            Follow = new Dictionary<T, List<State>>();
            this.name = name;
        }

        private FiniteAutomaton Automaton{get;}

        public void AddFollow(State s, T t)
        {
            if(s.Automaton != Automaton) Environment.FailFast("");
            if(!Automaton.Symbols.Any(x => x.Equals(t))) Environment.FailFast("");
            if(!Follow.ContainsKey(t)) Follow.Add(t, new List<State>());
            if(Follow[t].Contains(s)) Environment.FailFast("");
            Follow[t].Add(s);
        }
    }
}