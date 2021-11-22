using System;
using System.Collections.Generic;

namespace A6
{
    public class Q4ConstructSuffixArray 
    {      
        public static int Main()
        {
            string text = System.Console.ReadLine();                                                            

            Q4ConstructSuffixArray obj = new Q4ConstructSuffixArray();

            foreach (var item in obj.Solve(text))
            {
                System.Console.WriteLine(item);   
            }            

            return 0 ;
        }

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
