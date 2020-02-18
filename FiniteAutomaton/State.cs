namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class State
    {
        readonly private string? name;
        public string Name { get => name ?? Automaton.States.IndexOf(this).ToString(); }

        private Alphabet aplphabet;
        private FiniteAutomaton Automaton { get; }

        internal Dictionary<object, List<State>> Follow { get; }

        internal State(FiniteAutomaton e, Alphabet a, string? name = null)
        {
            Automaton = e;
            aplphabet = a;
            Follow = new Dictionary<object, List<State>>();
            this.name = name;
        }

        public void AddFollow(State s, object o)
        {
            if (s.Automaton != Automaton) Environment.FailFast("");
            if (!aplphabet.Symbols.Any(x => x.Equals(o))) Environment.FailFast("");
            if (!Follow.ContainsKey(o)) Follow.Add(o, new List<State>());
            if (Follow[o].Contains(s)) Environment.FailFast("");
            Follow[o].Add(s);
        }
    }
}