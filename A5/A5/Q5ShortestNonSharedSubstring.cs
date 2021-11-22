using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q5ShortestNonSharedSubstring : Processor
    {        
        private Node root;
        private int[] vocab;

        public Q5ShortestNonSharedSubstring(string testDataName) : base(testDataName)
        {
            // for (int i = 1; i < 3; i++)            
            this.ExcludeTestCases(50);

            vocab = new int[85];
            vocab[(int)'A'] =  0;                
            vocab[(int)'T'] =  1; 
            vocab[(int)'G'] =  2;
            vocab[(int)'C'] =  3;
            vocab[(int)'$'] =  4;
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String, String>)Solve);

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

        private string Solve(string text2, string text)
        {            

            long totalLength = text.Length ;
                        
            root = new Node();            
            
            for (int j = 0; j < text.Length; j++)            
            {                                
                Node currentNode = root;                                                                    

                for (int i = 0; i < totalLength - j; i++)
                {
                    char currentSymbol = text[i+j];                                                            

                    if ( currentNode.neighbours != null && currentNode.neighbours[vocab[(int)currentSymbol]] != null )                                        
                        currentNode = currentNode.neighbours[vocab[(int)currentSymbol]];                    
                                        
                    else 
                    {                        
                        currentNode.AddNeighbour( new Node(), vocab[(int)currentSymbol]);                        
                        currentNode = currentNode.neighbours[vocab[(int)currentSymbol]] ;            
                    }                                                                           
                }
            }

            long shortestNonSharedLength = text.Length + text2.Length;
            string shortestNonShared = "" ;

            for (int j = 0; j < text2.Length; j++)            
            {                                
                Node currentNode = root;                                                                    
          
                for (int i = 0; i < text2.Length - j; i++)
                {
                    char currentSymbol = text2[i+j];                                      

                    if ( currentNode.neighbours != null && currentNode.neighbours[vocab[(int)currentSymbol]] != null )                                                            
                        currentNode = currentNode.neighbours[vocab[(int)currentSymbol]];                                                            
                                        
                    else 
                    {                        
                        if( i+1 < shortestNonSharedLength  ) 
                        {
                            shortestNonShared = text2.Substring(j,i+1) ;
                            shortestNonSharedLength = shortestNonShared.Length ;
                        }                            

                        if (shortestNonShared.Length == 1)                        
                            return shortestNonShared ;                        
                                             
                        break ;                        
                    }                                                                           
                }
            }            

            return shortestNonShared ;
        }
    }
}
