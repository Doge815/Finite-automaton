#pragma warning disable RCS1037
using System.Collections;
namespace CLI
{
    using System;
    using System.Linq;
    using FiniteAuto;
    using System.IO;
    using System.Collections.Generic;

    public static class Program
    {
        private static void Main()
        {
            Alphabet a = new Alphabet(new List<object>{'a', 'b', 'c'});
            FiniteAutomaton automaton = new FiniteAutomaton(a);
            State startState = automaton.StartState;
            State two = automaton.AddState();
            State one = automaton.AddState();
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
