namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class State
    {
        private string? _name;
        public string Name => _name ??= Automaton.States.IndexOf(this).ToString();

        private readonly Alphabet _alphabet;
        private FiniteAutomaton Automaton { get; }

        internal Dictionary<object, List<State>> Follow { get; }

        internal State(FiniteAutomaton e, Alphabet a, string? name = null)
        {
            Automaton = e;
            _alphabet = a;
            Follow = new Dictionary<object, List<State>>();
            _name = name;
        }

        public void AddFollow(State s, object o)
        {
            if (s.Automaton != Automaton) throw new ArgumentException();
            if (!_alphabet.Symbols.Contains(o)) throw new ArgumentException();

            if (!Follow.TryGetValue(o, out var list)) Follow.Add(o, list = new List<State>());
            else if (Follow[o].Contains(s)) throw new ArgumentException();

            list.Add(s);
        }
    }
}