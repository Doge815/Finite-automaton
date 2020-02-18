namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class State<TSymbol>
        where TSymbol : notnull
    {
        private string? _name;
        public string Name => _name ??= Automaton.States.IndexOf(this).ToString();

        private readonly Alphabet<TSymbol> _alphabet;
        private FiniteAutomaton<TSymbol> Automaton { get; }

        internal Dictionary<TSymbol, List<State<TSymbol>>> Follow { get; }

        internal State(FiniteAutomaton<TSymbol> e, Alphabet<TSymbol> a, string? name = null)
        {
            Automaton = e;
            _alphabet = a;
            Follow = new Dictionary<TSymbol, List<State<TSymbol>>>();
            _name = name;
        }

        public void AddFollow(State<TSymbol> s, TSymbol o)
        {
            if (s.Automaton != Automaton) Environment.FailFast("Alphabet does not contain the added symbol.");
            if (!_alphabet.Symbols.Contains(o)) Environment.FailFast("Target State already contained");

            if (!Follow.TryGetValue(o, out var list)) Follow.Add(o, list = new List<State<TSymbol>>());
            else if (Follow[o].Contains(s)) Environment.FailFast("Target State in wrong automaton.");

            list.Add(s);
        }
    }
}