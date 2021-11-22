using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TestCommon;

namespace A6
{
    public class Q1ConstructBWT : Processor
    {
        public Q1ConstructBWT(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String>)Solve);

        /// <summary>
        /// Construct the Burrows–Wheeler transform of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> BWT(Text) </returns>                

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
