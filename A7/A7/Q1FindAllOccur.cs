using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A7
{
    public class Q1FindAllOccur : Processor
    {
        public Q1FindAllOccur(string testDataName) : base(testDataName)
        {
			this.VerifyResultWithoutOrder = true;
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<String, String, long[]>)Solve, "\n");

        protected virtual long[] Solve(string text, string pattern)
        {
            // write your code here
            string S = pattern + "$" + text ;
            int[] s = new int[S.Length];
            int border = 0 ;

            List<long> output = new List<long>();

            for (int i = 1; i < S.Length ; i++)
            {
                while(border > 0 && S[i] != S[border])
                    border = s[border -1];
                if (S[i] == S[border ])                
                    border ++ ;        
                else
                    border = 0 ;
                s[i] = border ;        

                if (border == pattern.Length)                
                    output.Add(i - 2 * pattern.Length);                
            }

            for (int i = 0; i < S.Length; i++)
            {
                
            }
            
            if (output.Count == 0)
            {
                long[] outp = {-1} ;
                return outp ;
            }
            else
            return output.ToArray() ;
        }
    }
}
