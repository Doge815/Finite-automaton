#pragma warning disable RCS1036, RCS1037
using System.Linq;
namespace FiniteAutomaton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class FiniteAutomaton <SymbolType> : ICloneable
    where SymbolType : IComparable<SymbolType>, IEquatable<SymbolType>
    {
        internal List<State<SymbolType>> States { get; }
        public State<SymbolType> StartState {get => States[0];}
        public IReadOnlyList<SymbolType> Symbols{get;}

        public FiniteAutomaton(List<SymbolType> s)
        {
            Symbols = s ?? throw new ArgumentNullException(nameof(s));
            States = new List<State<SymbolType>> { new State<SymbolType>(StateType.Start, this) };
        }

        public State<SymbolType> AddState(StateType type)
        {
            if((type & StateType.Start) != 0) Environment.FailFast("");
            return AddStateWithoutLimitations(type);
        }

        private State<SymbolType> AddStateWithoutLimitations(StateType type)
        {
            var state = new State<SymbolType>(type, this);
            States.Add(state);
            return state;
        }

        private FiniteAutomaton<SymbolType> ClearFiniteAutomaton(List<SymbolType> s)
        {
            FiniteAutomaton<SymbolType> newAutomaton = new FiniteAutomaton<SymbolType>(s);
            newAutomaton.States.Clear();
            return newAutomaton;
        }

        public string GetTable() => TableData().TableFormat();
    
        private string[,] TableData()
        {
            const string title = "State";

            string[,] table = new string[Symbols.Count + 1, States.Count + 1];
            table[0,0] = title;

            for(int i = 0; i < Symbols.Count; i++)
            {
                table[i+1, 0] = Symbols[i].ToString();
            }

            for(int i = 0; i < States.Count; i++)
            {
                State<SymbolType> s = States[i];
                table[0,i+1] = s.Name;
                for(int u = 0; u < Symbols.Count; u++)
                {
                    table[u + 1, i + 1] = s.Follow.TryGetValue(Symbols[u], out List<State<SymbolType>> follow)
                        ? string.Join(", ", follow.Select(x => x.Name))
                        : "-";
                }
            }
            return table;
        }

        private (FiniteAutomaton<SymbolType>, Dictionary<State<SymbolType>, State<SymbolType>>) DeepCopyFull()
        {
            FiniteAutomaton<SymbolType> Copy = ClearFiniteAutomaton(Symbols.ToList());
            Dictionary<State<SymbolType>, State<SymbolType>> Translate = new Dictionary<State<SymbolType>, State<SymbolType>>();
            foreach(State<SymbolType> s in States)
            {
                State<SymbolType> ss = Copy.AddStateWithoutLimitations(s.Type);
                Translate.Add(s, ss);
            }
            States
                .ForEach    (z => Symbols
                .Where      (y => z.Follow.ContainsKey(y))
                .ToList     ()
                .ForEach    (x => z.Follow[x]
                .ForEach    (w => Translate[z]
                .AddFollow  (Translate[w], x))));
            
            return (Copy, Translate);
        }

        public FiniteAutomaton<SymbolType> DeepCopy() => DeepCopyFull().Item1;

        public object Clone() => DeepCopy();

        public FiniteAutomaton<SymbolType> ConvertToDFA()
        {
            FiniteAutomaton<SymbolType> NFA = DeepCopy();
            
            FiniteAutomaton<SymbolType> DFA = new FiniteAutomaton<SymbolType>(Symbols.ToList());

            Dictionary<State<SymbolType>, List<State<SymbolType>>> Map = new Dictionary<State<SymbolType>, List<State<SymbolType>>>();
            for(bool finished = false; !finished;)
            {
                finished = true;
                foreach(State<SymbolType> D in DFA.States)
                {
                    if(!Map.ContainsKey(D))
                    {
                        finished = false;
                        State<SymbolType> s = new State<SymbolType>(StateType.None, DFA);
                        
                    }
                }
            } 
            return DFA;
        }

        public FiniteAutomaton<SymbolType> Minimize()
        {
            List<List<State<SymbolType>>> ListParts = new List<List<State<SymbolType>>>{new List<State<SymbolType>>(), new List<State<SymbolType>>()};
            foreach(State<SymbolType> s in States)
            {
                ListParts[(s.Type & StateType.End)!=0?0:1].Add(s);
            }
            foreach(SymbolType s in Symbols)
            {
                List<Dictionary<State<SymbolType>, List<State<SymbolType>>>> bind = new List<Dictionary<State<SymbolType>, List<State<SymbolType>>>>();
                foreach(List<State<SymbolType>> ss in ListParts)
                {
                    bind.Add(new Dictionary<State<SymbolType>, List<State<SymbolType>>>());
                    foreach(State<SymbolType> sss in ss)
                    {
                        bind.Last().Add(sss, ListParts.First(x => x.Contains(sss)));
                    }
                }
                List<List<State<SymbolType>>> NewListParts = new List<List<State<SymbolType>>>();
                foreach(Dictionary<State<SymbolType>, List<State<SymbolType>>> ss in bind)
                {
                    Dictionary<List<State<SymbolType>>, List<State<SymbolType>>> bindbind = new Dictionary<List<State<SymbolType>>, List<State<SymbolType>>>();
                    foreach(List<State<SymbolType>> sss in ss.Values.ToList())
                    {
                        bindbind.Add(sss, new List<State<SymbolType>>());
                    }
                    foreach(State<SymbolType> sss in ss.Keys)
                    {
                        bindbind[ListParts.First(x => x.Contains(sss.Follow[s][0]))].Add(sss);
                    }
                    NewListParts.AddRange(bindbind.Values);
                }
                ListParts = NewListParts;
            }

            FiniteAutomaton<SymbolType> Minimized = ClearFiniteAutomaton(Symbols.ToList());
            foreach(List<State<SymbolType>> s in ListParts)
            {
                Minimized.AddStateWithoutLimitations()
            }
            return Minimized;
        }
    }
}
