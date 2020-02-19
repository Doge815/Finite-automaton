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

        private readonly bool IsGarbageState;

        public static readonly State Garbage = new State(null!, null!, "Garbage", true);
        public static readonly State[] GarbagePartition = new [] {Garbage};

        public State(FiniteAutomaton e, Alphabet a, string? name = null) : this(e, a, name, false) {}

        private State(FiniteAutomaton e, Alphabet a, string? name = null, bool isGarbageState = false)
        {
            if (isGarbageState)
            {
                IsGarbageState = true;
                Automaton = null!;
                _alphabet = null!;
                Follow = null!;
                _name = name;
            }
            else{
                Automaton = e;
                _alphabet = a;
                Follow = new Dictionary<object, List<State>>();
                _name = name;
            }
        }

        public void AddFollow(object o, State s)
        {
            if (IsGarbageState) Environment.FailFast("No thats haram");
            if (s.Automaton != Automaton && !s.IsGarbageState) throw new ArgumentException();
            if (!_alphabet.Symbols.Contains(o)) throw new ArgumentException();

            if (!Follow.TryGetValue(o, out var list)) Follow.Add(o, list = new List<State>());
            else if (Follow[o].Contains(s)) throw new ArgumentException();

            list.Add(s);
        }

        public IReadOnlyList<State> GetFollow(object o)
        {
            return Follow == null
                ? GarbagePartition
                : Follow.TryGetValue(o, out var follow)
                    ? follow
                    : (IReadOnlyList<State>)GarbagePartition;
        }

        public override string ToString() => Name;
    }
}