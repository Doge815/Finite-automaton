#nullable enable
#pragma warning disable RCS1037
using System;
namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class State
    {
        Type T;
        readonly private string? name;
        public string Name {get => name ?? Automaton.States.IndexOf(this).ToString(); }

        private Aplphabet<T> aplphabet;
        private FiniteAutomaton Automaton {get;}S

        internal Dictionary<T, List<State>> Follow {get; }

        internal State(FiniteAutomaton e, string? name = null, Type t)
        {
            T = t;

            Automaton = e;
            Follow = new Dictionary<T, List<State>>();
            this.name = name;
        }

        public State(FiniteAutomaton automaton) 
        {
            this.Automaton = automaton;
               
        }

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