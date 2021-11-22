using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A7
{
    public class Q3PatternMatchingSuffixArray 
    {

        public static int Main()
        {
            string text = System.Console.ReadLine();   
            long n = long.Parse(System.Console.ReadLine());
            string[] patterns ;
            string p = System.Console.ReadLine();
            patterns = p.Split();
            // for (int i = 0; i < n; i++)            
            //     patterns[i] = System.Console.Read();            

            Q3PatternMatchingSuffixArray obj = new Q3PatternMatchingSuffixArray();

            long[] ans =obj.Solve(text , n , patterns) ;
            for (int i = 0; i < ans.Length; i++)
            {
                System.Console.Write(ans[i] + " ");
            }            

            return 0 ;
        } 

        public Q3PatternMatchingSuffixArray()
        {
            vocab = new Dictionary<char, int>();
            vocab.Add( 'A',  1) ;                
            vocab.Add( 'C',  2) ; 
            vocab.Add( 'G',  3) ;
            vocab.Add( 'T',  4) ;
            vocab.Add( '$',  0) ;            
        }

        List<long> output ;

        Dictionary<char , int> vocab ;
        
        protected virtual long[] Solve(string text, long n, string[] patterns)
        {
            // write your code here
            output  = new List<long>();
            text += '$';
            long[] suffixArray = ComputeSuffixArray(text);            

            for (int i = 0; i < patterns.Length; i++)
            {
                string pattern = patterns[i] ;
                // pattern += '$' ;
                PatternMatchingWithSuffixArray(text, pattern ,suffixArray );
            }            

            if (output.Count == 0)
            {
                long[] outp = {-1};
                return outp ;
            }                
            

            return output.ToArray() ;
        }

        private void PatternMatchingWithSuffixArray(string text, string pattern, long[] suffixArray)
        {            
            long minIndex = 0 , maxIndex = text.Length ;
            while (minIndex < maxIndex)
            {
                long midIndex = (minIndex + maxIndex)/2 ;
                string b = text.Substring((int)suffixArray[midIndex]) ;
                if(isAfter(pattern ,b ))
                    minIndex = midIndex + 1 ;
                else 
                    maxIndex = midIndex ;
            }
            long start = minIndex ;
            maxIndex = text.Length ;
            while(minIndex < maxIndex)
            {
                long midIndex = (minIndex + maxIndex)/2 ;
                string b = text.Substring((int)suffixArray[midIndex]) ;
                if( isAfter(b, pattern , 0 ))
                    maxIndex = midIndex ;
                else 
                    minIndex = midIndex +1 ;
            }

            long end = maxIndex ;

        
            if(start <=  end)                                           
                for (int i = (int)start ; i < end; i++)   
                    if(!output.Contains(suffixArray[i]))            
                        output.Add(suffixArray[i]);                            
        }

        private bool isAfter(string pattern, string v, int i=0 )
        {                   
            for (int j = 0; j < Math.Min(pattern.Length , v.Length); j++)            
                if(pattern[j] != v[j])
                    return pattern[j] > v[j] ;                
            return false ;
        }

        private long[] ComputeSuffixArray(string text)
        {
            long[] order = SortCharacters(text);

            long[] Class = ComputeCharClasses(text, order);

            int L = 1;
            while (L < text.Length)
            {
                order = sortDoubled(text, ref order, Class, L);

                Class = UpateClasses(order, Class, L);

                L *= 2;
            }

            return order;
        }

        private static long[] ComputeCharClasses(string text, long[] order)
        {
            long[] Class = new long[text.Length];
            Class[order[0]] = 0;

            for (int i = 1; i < text.Length; i++)
            {
                if (text[(int)order[i]] != text[(int)order[i - 1]])
                    Class[order[i]] = Class[order[i - 1]] + 1;
                else
                    Class[order[i]] = Class[order[i - 1]];
            }

            return Class;
        }

        private long[] SortCharacters(string text)
        {
            long[] order = new long[text.Length];
            long[] count = new long[5];
            // sort characters ----------------------------------------
            for (int i = 0; i < text.Length; i++)
                count[vocab[text[i]]]++;

            for (int i = 1; i < 5; i++)
                count[i] += count[i - 1];

            for (int i = text.Length - 1; i >= 0; i--)
            {
                int a = vocab[text[i]];
                count[a]--;
                order[count[a]] = i;
            }

            return order;
        }

        private static long[] UpateClasses(long[] newOrder, long[] Class, int L)
        {
            int n = newOrder.Length;
            long[] newClass = new long[newOrder.Length];
            newClass[newOrder[0]] = 0;
            for (int i = 1; i < n; i++)
            {
                long cur = newOrder[i], prev = newOrder[i - 1],
                    mid = (cur + L) % n, midPrev = (prev + L) % n;

                if (Class[(int)cur] != Class[(int)prev] || Class[mid] != Class[midPrev])
                    newClass[cur] = newClass[prev] + 1;
                else
                    newClass[cur] = newClass[prev];
            }

            // Class = newClass;
            return newClass;
        }

        private static long[] sortDoubled(string text, ref long[] order, long[] Class, int L)
        {
            long[] count = new long[text.Length];
            long[] newOrder = new long[text.Length];

            for (int i = 0; i < text.Length; i++)
                count[Class[i]]++;
            for (int i = 1; i < text.Length; i++)
                count[i] += count[i - 1];

            int start;
            long cl;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                start = ((int)order[i] - L + text.Length) % text.Length;
                cl = Class[start];
                count[(int)cl]--;
                newOrder[count[(int)cl]] = start;
            }

            // order = newOrder;
            return newOrder;
        }
    }
}
