using System;
using System.Collections.Generic;
using TestCommon;

namespace A6
{
    public class Q3MatchingAgainCompressedString : Processor
    {
        public Q3MatchingAgainCompressedString(string testDataName) 
        : base(testDataName) { }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        /// <summary>
        /// Implement BetterBWMatching algorithm
        /// </summary>
        /// <param name="text"> A string BWT(Text) </param>
        /// <param name="n"> Number of patterns </param>
        /// <param name="patterns"> Collection of n strings Patterns </param>
        /// <returns> A list of integers, where the i-th integer corresponds
        /// to the number of substring matches of the i-th member of Patterns
        /// in Text. </returns>        

        int vocab(char a)
        {            
            if(a=='A') return 1 ;
            if(a=='C') return 2 ;
            if(a=='G') return 3 ;
            if(a=='T') return 4 ;
            if(a=='$') return 0 ;
            return 0 ;
        }        
        int[,] Count ;        

        public long[] Solve(string text, long n, String[] patterns)
        {
            int[] repeats = new int[5];
            List<long> output = new List<long>();
            Count = new int[text.Length + 1 ,5];                    

            Count[1,vocab(text[0])] = 1 ;

            for (int i = 0; i < text.Length; i++)            
            {                    
                int a = vocab(text[i]);                
                if(i>0)
                {
                    for (int j = 0; j < 5; j++)                
                        if(j != a)
                            Count[i+1,j] = Count[i,j];                
                    Count[i+1 , a] = Count[i , a] + 1 ;
                }                             
                repeats[a] ++ ;                                                    
            }                    

            int[] starting_points = {0,0,0,0,0};
            for (int i = 1; i < 5; i++)            
                starting_points[i] = starting_points[i-1] + repeats[i-1] ;   

            for (int i = 0; i < n; i++)                  
                    output.Add( BetterBWmatching (starting_points ,  text , patterns[i] ) );                                                 

            return output.ToArray();               
        }

        private int BetterBWmatching( int[] starting_points , string text, string p)
        {
            int top = 0 ;
            int bottom = text.Length - 1 ;            
            while(top <= bottom)
            {                
                if (p.Length > 0)
                {
                    char symbol = p[p.Length-1] ;
                    int a = vocab(symbol) ;
                            
                    p = p.Remove(p.Length - 1);

                    // if(Count[top,a] == 0 )                            
                        top = starting_points[a] + Count[top , a];
                    // else
                        // top = starting_points[a] + Count[top,a] -1 ;

                    bottom = starting_points[a] + Count[bottom +1 ,a] -1  ;
                }
                else
                    return bottom - top + 1 ;                                
            }            
            return 0 ;
        }
    }
}
