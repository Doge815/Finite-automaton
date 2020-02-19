namespace CLI
{
    using FiniteAuto;
    using System;
    using System.Collections.Generic;

    public static class Program
    {
        private static void Main()
        {
            Alphabet a = new Alphabet(new List<object> { 'a', 'b', 'c' });
            FiniteAutomaton automaton = new FiniteAutomaton(a);

            State s0 = automaton.AddState("0");
            State s1 = automaton.AddState("1");
            State s2 = automaton.AddState("2");

            s0.AddFollow('a', s2);
            s0.AddFollow('c', s1);
            s1.AddFollow('a', s2);
            s1.AddFollow('c', s1);
            s2.AddFollow('b', s0);

            automaton.AddEndstate(s2);

            FiniteAutomaton minimized = automaton.Minimize();

            Console.WriteLine(automaton.GetTable() + "\n");
            Console.WriteLine(minimized.GetTable() + "\n");
        }
    }
}