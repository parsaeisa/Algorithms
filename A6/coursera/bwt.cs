using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace A6
{
    public class Q1ConstructBWT 
    {
        public static int Main()
        {
            string text = System.Console.ReadLine();                                                            

            Q1ConstructBWT obj = new Q1ConstructBWT();

            System.Console.WriteLine(obj.Solve(text));

            return 0 ;
        }            

        public string Solve(string text)
        {
            
            SortedSet <string> words = new SortedSet<string>(); 
            int len = text.Length ;            
            StringBuilder output = new StringBuilder();

            for (int i = 0; i < len; i++)             
                words.Add(text.Substring(i) + text.Substring(0,i));            
            
            foreach (var item in words)            
                output.Append(item[len-1]);            
                        
            return output.ToString() ;
        }
    }
}
