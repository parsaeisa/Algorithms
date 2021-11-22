using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace A6
{
    public class Q2ReconstructStringFromBWT 
    {
        
        public static int Main()
        {
            string text = System.Console.ReadLine();                                                            

            Q2ReconstructStringFromBWT obj = new Q2ReconstructStringFromBWT();

            System.Console.WriteLine(obj.Solve(text));

            return 0 ;
        }  

        int vocab(char a)
        {
            if(a=='$') return 0 ;
            if(a=='A') return 1 ;
            if(a=='C') return 2 ;
            if(a=='G') return 3 ;
            if(a=='T') return 4 ;
            return 0 ;
        }

        public string Solve(string bwt)
        {                
            
            int[] repeats = new int[5];

            int k = 0 ;
            int[] bwt_index = new int[bwt.Length];
            foreach (var c in bwt)       
            {     
                int a = vocab(c);
                repeats[a] ++ ;                              
                bwt_index[k] = repeats[a];
                k++ ;
            }            
            

            int[] starting_points = {0,0,0,0,0};
            for (int i = 1; i < 5; i++)            
                starting_points[i] = starting_points[i-1] + repeats[i-1] ;        

            StringBuilder output= new StringBuilder();
            int ptr = 0;            

            while(output.Length != bwt.Length - 1 )
            {
                output.Append( bwt[ptr]);
                ptr = starting_points[vocab(bwt[ptr])] + bwt_index[ptr] -1 ;                
            }
                        
            string s =  new string(output.ToString().Reverse().ToArray()) + '$';
            return s; 
        }
        
    }
}
