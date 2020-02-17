#pragma warning disable RCS1036, RCS1037
using System.Linq;
namespace FiniteAuto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class FiniteAutomaton : ICloneable
    {
        internal Alphabet alphabet { get; }
        internal List<State> States { get; }
        public State StartState { get; }
        private List<State> endStates;
        public List<State> EndStates { get => endStates.ToList(); }

        public FiniteAutomaton(Alphabet a)
        {
            alphabet = a;
            States = new List<State>();
            endStates = new List<State>();
        }

        public State AddState()
        {
            var state = new State(this, alphabet);
            States.Add(state);
            return state;
        }

        public string GetTable() => Extensions.TableFormat(TableData());
    
        private string[,] TableData()
        {
            const string title = "State";

            string[,] table = new string[alphabet.Symbols.Count + 1, States.Count + 1];
            table[0,0] = title;

            for(int i = 0; i < alphabet.Symbols.Count; i++)
            {
                table[i+1, 0] = alphabet.Symbols[i].ToString();
            }

            for(int i = 0; i < States.Count; i++)
            {
                State s = States[i];
                table[0,i+1] = s.Name;
                for(int u = 0; u < alphabet.Symbols.Count; u++)
                {
                    table[u + 1, i + 1] = s.Follow.TryGetValue(alphabet.Symbols[u], out List<State> follow)
                        ? string.Join(", ", follow.Select(x => x.Name))
                        : "-";
                }
            }
            return table;
        }

        private (FiniteAutomaton, Dictionary<State, State>) DeepCopyFull()
        {
            FiniteAutomaton Copy = new FiniteAutomaton(alphabet);
            Dictionary<State, State> Translate = new Dictionary<State, State>();
            foreach(State s in States)
            {
                State ss = Copy.AddState();
                Translate.Add(s, ss);
            }
            States
                .ForEach    (z => alphabet.Symbols
                .Where      (y => z.Follow.ContainsKey(y))
                .ToList     ()
                .ForEach    (x => z.Follow[x]
                .ForEach    (w => Translate[z]
                .AddFollow  (Translate[w], x))));
            
            return (Copy, Translate);
        }

        public FiniteAutomaton DeepCopy() => DeepCopyFull().Item1;

        public object Clone() => DeepCopy();

        public FiniteAutomaton ConvertToDFA()
        {
            FiniteAutomaton NFA = DeepCopy();
            
            FiniteAutomaton DFA = new FiniteAutomaton(alphabet);

            Dictionary<State, List<State>> Map = new Dictionary<State, List<State>>();
            for(bool finished = false; !finished;)
            {
                finished = true;
                foreach(State D in DFA.States)
                {
                    if(!Map.ContainsKey(D))
                    {
                        finished = false;
                        State s = DFA.AddState();
                        
                    }
                }
            } 
            return DFA;
        }

        public FiniteAutomaton Minimize()
        {
            List<List<State>> ListParts = new List<List<State>>{new List<State>(), new List<State>()};
            foreach(State s in States)
            {
                ListParts[EndStates.Contains(s)?0:1].Add(s);
            }
            foreach(object s in alphabet.Symbols)
            {
                List<Dictionary<State, List<State>>> bind = new List<Dictionary<State, List<State>>>();
                foreach(List<State> ss in ListParts)
                {
                    bind.Add(new Dictionary<State, List<State>>());
                    foreach(State sss in ss)
                    {
                        bind.Last().Add(sss, ListParts.First(x => x.Contains(sss)));
                    }
                }
                List<List<State>> NewListParts = new List<List<State>>();
                foreach(Dictionary<State, List<State>> ss in bind)
                {
                    Dictionary<List<State>, List<State>> bindbind = new Dictionary<List<State>, List<State>>();
                    foreach(List<State> sss in ss.Values.ToList())
                    {
                        bindbind.Add(sss, new List<State>());
                    }
                    foreach(State sss in ss.Keys)
                    {
                        bindbind[ListParts.First(x => x.Contains(sss.Follow[s][0]))].Add(sss);
                    }
                    NewListParts.AddRange(bindbind.Values);
                }
                ListParts = NewListParts;
            }

            FiniteAutomaton Minimized = new FiniteAutomaton(alphabet);
            foreach(List<State> s in ListParts)
            {
                //Minimized.AddStateWithoutLimitations()
            }
            return Minimized;
        }
    }
}
