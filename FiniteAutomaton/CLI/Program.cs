#pragma warning disable RCS1037
using System.Collections;
namespace CLI
{
    using System;
    using System.Linq;
    using FiniteAutomaton;
    using System.IO;
    using System.Collections.Generic;

    public static class Program
    {
        private static void Main()
        {
            FiniteAutomaton<char> automaton = new FiniteAutomaton<char>(new List<char>{'a', 'b'});
            //FiniteAutomaton<char> automaton = new FiniteAutomaton<char>(Enumerable.Range(0, char.MaxValue).Select(x => (char)x).Where(char.IsLetter).ToList());
            State<char> startState = automaton.StartState;
            State<char> one = automaton.AddState(StateType.None);
            State<char> two = automaton.AddState(StateType.End);
            startState.AddFollow(one, 'a');
            startState.AddFollow(two, 'b');
            one.AddFollow(two, 'a');
            one.AddFollow(one, 'a');
            one.AddFollow(one, 'b');
            two.AddFollow(startState, 'a');
            two.AddFollow(two, 'a');
            
            //FiniteAutomaton<char> DFA = automaton.ConvertToDFA();
            Console.WriteLine(automaton.GetTable()+"\n");
            //Console.WriteLine(DFA.GetTable()+"\ndone");
        }
    }
}
