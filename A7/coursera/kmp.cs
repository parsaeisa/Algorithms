using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A7
{
    public class Q1FindAllOccur 
    {    
        
        public static int Main()
        {
            string text = System.Console.ReadLine();                                                            
            string pattern = System.Console.ReadLine();                                                            

            Q1FindAllOccur obj = new Q1FindAllOccur();

            long[] ans = obj.Solve(pattern, text) ;
            for (int i = 0; i < ans.Length; i++)
            {
                System.Console.Write(ans[i] + " ");
            }            

            return 0 ;
        } 

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
                    border =0 ;
                s[i] = border ;        

                if (border == pattern.Length)                
                    output.Add(i - 2 * pattern.Length);                
            }
        
            return output.ToArray() ;
        }
    }
}
