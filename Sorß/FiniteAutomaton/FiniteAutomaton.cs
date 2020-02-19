namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FiniteAutomaton : ICloneable
    {
        internal Alphabet Alphabet { get; }
        internal List<State> States { get; }

        private State? _startState = null;

        public State? StartState
        {
            get => _startState;
            set => _startState = States.Contains(value ?? throw new ArgumentNullException()) ? value : throw new ArgumentException();
        }

        private readonly List<State> _endStates;
        public IReadOnlyList<State> EndStates => _endStates;

        public FiniteAutomaton(Alphabet a)
        {
            Alphabet = a;
            States = new List<State>();
            _endStates = new List<State>();
        }

        public State AddState(string? name = null)
        {
            var state = new State(this, Alphabet, name);
            States.Add(state);
            return state;
        }

        public void AddEndstate(State state)
        {
            if (States.Contains(state))
            {
                _endStates.Add(state);
            }
            else
            {
                throw new ArgumentException(nameof(state));
            }
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
                State s = States[i];
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

        private (FiniteAutomaton, Dictionary<State, State>) DeepCopyFull()
        {
            FiniteAutomaton Copy = new FiniteAutomaton(Alphabet);
            Dictionary<State, State> Translate = new Dictionary<State, State>();
            foreach (State s in States)
            {
                State ss = Copy.AddState();
                Translate.Add(s, ss);
            }
            States
                .ForEach(z => Alphabet.Symbols
                    .Where(y => z.Follow.ContainsKey(y))
                    .ToList()
                    .ForEach(x => z.Follow[x]
                        .ForEach(w => Translate[z]
                            .AddFollow(x, Translate[w]))));

            return (Copy, Translate);
        }

        public FiniteAutomaton DeepCopy() => DeepCopyFull().Item1;

        public object Clone() => DeepCopy();

        public FiniteAutomaton ConvertToDFA()
        {
            FiniteAutomaton nfa = DeepCopy();

            var dfa = new FiniteAutomaton(Alphabet);

            var Map = new Dictionary<State, List<State>>();

            while (true)
            {
                bool finished = true;

                foreach (var state in dfa.States)
                {
                    if (!Map.ContainsKey(state))
                    {
                        finished = false;
                        State s = dfa.AddState();
                    }
                }

                if (finished) break;
            }

            return dfa;
        }

        public FiniteAutomaton Minimize()
        {
            var partitions = new List<State[]> { EndStates.ToArray(), States.Except(EndStates).ToArray(), State.GarbagePartition };

            while (true)
            {
                bool finished = true;

                foreach (var s in Alphabet.Symbols)
                {
                    var newPartitions = new List<State[]> { };

                    foreach (var partition in partitions)
                    {
                        if (partition == State.GarbagePartition || partition.Length == 1)
                        {
                            newPartitions.Add(partition);
                            continue;
                        }

                        var statesTargetingPartition = partitions.ToDictionary(x => x, _ => new List<State>());
                        foreach (var state in partition)
                        {
                            var followSet = state.GetFollow(s);

                            if (followSet.Count > 1) Environment.FailFast("Exceptional failure during P/Invoke. " + string.Join(", ", followSet.Select(x=>x.Name)));

                            if (followSet.Count == 0)
                            {
                                statesTargetingPartition[State.GarbagePartition].Add(state);
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

            var minimizedDfa = new FiniteAutomaton(Alphabet);
            var statesFromPartitions = partitions.ToDictionary(x => x, x => minimizedDfa.AddState($"{{ {string.Join(", ", x.Select(x => x.Name))} }}"));

            foreach (var stateAndPartition in statesFromPartitions)
            {
                foreach (object s in Alphabet.Symbols)
                {
                    var partition = stateAndPartition.Key;
                    var follow = partition[0].GetFollow(s)[0];
                    var followPartition = partitions.FindPartition(follow);
                    var followState = statesFromPartitions[followPartition];

                    stateAndPartition.Value.AddFollow(s, followState);
                }
            }

            return minimizedDfa;
        }
    }
}