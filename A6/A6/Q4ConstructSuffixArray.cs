using System;
using System.Collections.Generic;
// using System.Text;
using TestCommon;

namespace A6
{
    public class Q4ConstructSuffixArray : Processor
    {
        public Q4ConstructSuffixArray(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, long[]>)Solve);

        /// <summary>
        /// Construct the suffix array of a string
        /// </summary>
        /// <param name="text"> A string Text ending with a “$” symbol </param>
        /// <returns> SuffixArray(Text), that is, the list of starting positions
        /// (0-based) of sorted suffixes separated by spaces </returns>                

        long[] outp ;

        public long[] Solve(string text)
        {
            outp = new long[text.Length];
            
            SortedSet<string> words = new SortedSet<string>();                        

            int len = text.Length;
            for (int i = 0; i < len; i++)                
                words.Add(text.Substring(i));
            
            SortedSet<string>.Enumerator e = words.GetEnumerator();
            NewMethod(text, e);
           

            return outp;
        }

        private void NewMethod(string text, SortedSet<string>.Enumerator e)
        {            
            for (int i = 0; i < text.Length; i++)
            {
                if (e.MoveNext())
                {
                    int t = e.Current.Length;
                    outp[i] = text.Length - t;
                }
            }
        }
        
    }
    
}
