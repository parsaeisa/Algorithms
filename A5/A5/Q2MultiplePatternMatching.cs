using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q2MultiplePatternMatching : Processor
    {
        public Q2MultiplePatternMatching(string testDataName) : base(testDataName)
        {
            
            vocab = new int[85];
            vocab[(int)'A'] =  0;                
            vocab[(int)'T'] =  1; 
            vocab[(int)'G'] =  2;
            vocab[(int)'C'] =  3;
            vocab[(int)'$'] =  4;

        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, long, String[], long[]>)Solve);

        public long clock ;
        
        public int[] vocab ;

        public class Node {
            public long value ;

            public Node()
            {
                // this.value = value;                
                hasDollarSign = false ;
                neighbours = null ;
            }            

            public bool hasDollarSign ;
            
            public Node[] neighbours ;

            internal void AddNeighbour(Node newNeighbour, int currentSymbol_index)            
            {
                if(neighbours == null)                                    
                    neighbours = new Node[5] ;                
                neighbours[currentSymbol_index] = newNeighbour ;
            }
        }

        Node root ;

        public long[] Solve(string text, long n, string[] patterns)
        {
            // vocab = new Dictionary<char, int>();
            // vocab.Add('A' ,  0);                
            // vocab.Add('T' ,  1 ); 
            // vocab.Add('G' ,  2 );
            // vocab.Add('C' ,  3 );
            // vocab.Add('$' ,  4 );


            List<long> output = null ;

            long totalLength = text.Length ;
            
            clock = 0;            
            root = new Node();
            long c = 0 ;
            
            for (int j = 0; j < patterns.Length; j++)            
            {                
                Node currentNode = root;                                                    
                string pattern = patterns[j] + "$";

                for (int i = 0; i < pattern.Length; i++)
                {
                    char currentSymbol = pattern[i];                                                            

                    if ( currentNode.neighbours != null && currentNode.neighbours[vocab[(int)currentSymbol]] != null )                                        
                        currentNode = currentNode.neighbours[vocab[(int)currentSymbol]];                    
                                        
                    else 
                    {                        
                        currentNode.AddNeighbour( new Node(), vocab[(int)currentSymbol]);
                        if (currentSymbol == '$')                                                    
                            currentNode.hasDollarSign = true ;                                                
                        currentNode = currentNode.neighbours[vocab[(int)currentSymbol]] ;            
                    }                                                                           
                }
            }            
            
            for (int i = 0; i < totalLength ; i++)            
            {
                long res = PrefixTrieMatching(text , i) ;                
                if (res >= 0)                
                { 
                    if(output == null)            
                        output = new List<long>();
                    output.Add(res);                
                }
            }

            if (output == null)          
            {
                long[] outputB = {-1};
                return outputB ;
            }         

            return output.ToArray() ;
        }

        private long PrefixTrieMatching(string text , long starting)
        {            
            char symbol = text[(int)starting] ;
            clock = starting+1 ;            
            Node v = root ;
            while (true)            
            {                                
                if (v.hasDollarSign)
                    return starting ;

                if (clock > text.Length)
                {
                    return -1 ;
                }

                if(v.neighbours == null)
                    return -1 ;
                
                if (v.neighbours[vocab[(int)symbol]] != null )                    
                {             
                    v= v.neighbours[vocab[(int)symbol]];                    
                    try {                               
                        symbol = text[(int)clock];                                                
                    } catch {}                              
                    clock++ ;
                }
                else                 
                    return -1 ;                                
                                               
            }
        }
    }
}
