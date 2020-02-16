#nullable enable
#pragma warning disable RCS1037
namespace FiniteAutomaton
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Flags]
    public enum StateType
    {
        None = 0,
        Start =1<<1,
        End=1<<2,
        Garbage=1<<3
    }
    
    public class State<SymbolType>
    where SymbolType : IComparable<SymbolType>, IEquatable<SymbolType>
    {
        readonly private string? name;
        public string Name {get => name ?? Automaton.States.IndexOf(this).ToString();}
        internal Dictionary<SymbolType, List<State<SymbolType>>> Follow {get; }
        internal StateType Type {get;}

        internal State(StateType type, FiniteAutomaton<SymbolType> e, string? name = null)
        {
            Type = type;
            Automaton = e;
            Follow = new Dictionary<SymbolType, List<State<SymbolType>>>();
            this.name = name;
        }

        private FiniteAutomaton<SymbolType> Automaton{get;}

        public void AddFollow(State<SymbolType> s, SymbolType t)
        {
            if(s.Automaton != Automaton) Environment.FailFast("");
            if(!Automaton.Symbols.Any(x => x.Equals(t))) Environment.FailFast("");
            if(!Follow.ContainsKey(t)) Follow.Add(t, new List<State<SymbolType>>());
            if(Follow[t].Contains(s)) Environment.FailFast("");
            Follow[t].Add(s);
        }
    }
}