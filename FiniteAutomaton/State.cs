namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class State
        where TSymbol : notnull
    {
        private string? _name;
        public string Name => _name ??= Automaton.States.IndexOf(this).ToString();

        private readonly Alphabet _alphabet;
        private FiniteAutomaton Automaton { get; }

        internal Dictionary<TSymbol, List<State>> Follow { get; }

        internal State(FiniteAutomaton e, Alphabet a, string? name = null)
        {
            Automaton = e;
            _alphabet = a;
            Follow = new Dictionary<TSymbol, List<State>>();
            _name = name;
        }

        public void AddFollow(State s, TSymbol o)
        {
            if (s.Automaton != Automaton) Environment.FailFast("Alphabet does not contain the added symbol.");
            if (!_alphabet.Symbols.Contains(o)) Environment.FailFast("Target State already contained");

            if (!Follow.TryGetValue(o, out var list)) Follow.Add(o, list = new List<State>());
            else if (Follow[o].Contains(s)) Environment.FailFast("Target State in wrong automaton.");

            list.Add(s);
        }
    }
}