namespace FiniteAuto
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class FiniteAutomaton<TSymbol> : ICloneable
        where TSymbol : notnull
    {
        internal Alphabet<TSymbol> Alphabet { get; }
        internal List<State<TSymbol>> States { get; }

        private State<TSymbol> _startState;
        public State<TSymbol> StartState
        {
            get => _startState;
            set => _startState = States.Contains(value) ? value : throw new ArgumentException();
        }

        private readonly List<State<TSymbol>> _endStates;
        public IReadOnlyList<State<TSymbol>> EndStates => _endStates;

        public FiniteAutomaton(Alphabet<TSymbol> a)
        {
            Alphabet = a;
            States = new List<State<TSymbol>>();
            _endStates = new List<State<TSymbol>>();
        }

        public State<TSymbol> AddState()
        {
            var state = new State<TSymbol>(this, Alphabet);
            States.Add(state);
            return state;
        }

        public string GetTable() => TableData().TableFormat();

        private string[,] TableData()
        {
            const string title = "State";

            string[,] table = new string[Alphabet.Symbols.Count + 1, States.Count + 1];
            table[0, 0] = title;

            for (int i = 0; i < Alphabet.Symbols.Count; i++)
            {
                table[i + 1, 0] = Alphabet.Symbols[i].ToString();
            }

            for (int i = 0; i < States.Count; i++)
            {
                State<TSymbol> s = States[i];
                table[0, i + 1] = s.Name;
                for (int u = 0; u < Alphabet.Symbols.Count; u++)
                {
                    table[u + 1, i + 1] = s.Follow.TryGetValue(Alphabet.Symbols[u], out var follow)
                        ? string.Join(", ", follow.Select(x => x.Name))
                        : "-";
                }
            }
            return table;
        }

        private (FiniteAutomaton<TSymbol>, Dictionary<State<TSymbol>, State<TSymbol>>) DeepCopyFull()
        {
            FiniteAutomaton<TSymbol> Copy = new FiniteAutomaton<TSymbol>(Alphabet);
            Dictionary<State<TSymbol>, State<TSymbol>> Translate = new Dictionary<State<TSymbol>, State<TSymbol>>();
            foreach (State<TSymbol> s in States)
            {
                State<TSymbol> ss = Copy.AddState();
                Translate.Add(s, ss);
            }
            States
                .ForEach(z => Alphabet.Symbols
            .Where(y => z.Follow.ContainsKey(y))
            .ToList()
            .ForEach(x => z.Follow[x]
        .ForEach(w => Translate[z]
    .AddFollow(Translate[w], x))));

            return (Copy, Translate);
        }

        public FiniteAutomaton<TSymbol> DeepCopy() => DeepCopyFull().Item1;

        public object Clone() => DeepCopy();

        public FiniteAutomaton<TSymbol> ConvertToDFA()
        {
            FiniteAutomaton<TSymbol> nfa = DeepCopy();

            var dfa = new FiniteAutomaton<TSymbol>(Alphabet);

            var Map = new Dictionary<State<TSymbol>, List<State<TSymbol>>>();

            while (true)
            {
                bool finished = true;

                foreach (var state in dfa.States)
                {
                    if (!Map.ContainsKey(state))
                    {
                        finished = false;
                        State<TSymbol> s = dfa.AddState();
                    }
                }

                if (finished) break;
            }

            return dfa;
        }

        public FiniteAutomaton<TSymbol> Minimize()
        {
            var partitions = new List<State<TSymbol>[]?> { EndStates.ToArray(), States.Except(EndStates).ToArray(), null };

            while (true)
            {
                bool finished = true;

                foreach (TSymbol s in Alphabet.Symbols)
                {
                    var newPartitions = new List<State<TSymbol>[]?> { null };

                    foreach (var partition in partitions)
                    {
                        if (partition == null || partition.Length == 1) continue;

                        var statesTargetingPartition = partitions.ToDictionary(x => x, _ => new List<State<TSymbol>>());
                        foreach (var state in partition)
                        {
                            var followSet = state.Follow[s];

                            if (followSet.Count > 1) Environment.FailFast("Exceptional failure during P/Invoke.");

                            if (followSet.Count == 0)
                            {
                                statesTargetingPartition[null].Add(state);
                            }
                            else
                            {
                                var follow = followSet[0];
                                var targetedPartition = partitions.FindPartition(follow);
                                statesTargetingPartition[targetedPartition].Add(state);
                            }
                        }

                        var addedPartitions = statesTargetingPartition.Values
                            .Select(x => x.ToArray())
                            .Where(x => x.Length > 0)
                            .ToList();

                        if (addedPartitions.Count > 1) finished = false;

                        newPartitions.AddRange(addedPartitions);
                    }

                    partitions = newPartitions;
                }

                if (finished) break;
            }

            var minimizedDfa = new FiniteAutomaton<TSymbol>(Alphabet);
            var statesFromPartitions = partitions.ToDictionary(x => x, _ => minimizedDfa.AddState());

            foreach (var stateAndPartition in statesFromPartitions)
            {
                foreach (TSymbol s in Alphabet.Symbols)
                {
                    var partition = stateAndPartition.Key;
                    var follow = partition?[0].Follow[s][0];
                    var followPartition = follow == null ? null : partitions.FindPartition(follow);
                    var followState = statesFromPartitions[followPartition];

                    stateAndPartition.Value.AddFollow(followState, s);
                }
            }

            return minimizedDfa;
        }
    }
}