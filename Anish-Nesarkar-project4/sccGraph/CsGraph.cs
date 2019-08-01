////////////////////////////////////////////////////////////////////////////////////////////////////
// Graph.cs - Generate the graph for the dependent files and check for strong connected components//                                            //
// ver 1.0                                                                                        //
// Language:    C#, 2017, .Net Framework 4.7.1                                                    //
// Platform:    Dell Precision T8900, Win10                                                       //
// Application: Demonstration for CSE681, Project #3, Fall 2018                                   //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis                                     //
// Author:      Jim Fawcett, CST 4-187, Syracuse University                                       //
////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package generates nodes for the files that are dependent on other files
 * It also implements Tarjan's Algorithm to find strongly connected component
 */
/* Required Files:
 *   Executive.cs
 *   DependencyAnalyzer.cs
 *   
 * public Interface Documentation:
* public class GraphTest                                                                               // class graph test
* public List<CsNode<string, string>> getGraph(List<Tuple<string,string>> * graph, List<string> files) //function to get dependency nodes
* public List<string> Tarjan(List<CsNode<string, string>> m)                                           //function to get strongly connected components
* public class CsEdge<V, E>                                                                            // holds child node and instance of edge type E
* public class CsNode<V, E>                                                                            // holds the attributes of nodes
* virtual public bool doNodeOp(CsNode<V, E> node)                                                      // does operation on nodes
* virtual public bool doEdgeOp(E edgeVal)                                                              // does operation on edges
* public class CsGraph<V, E>                                                                           // graph class for generating graph 
* public CsGraph(string graphName)                                                                     // constructor for graph class      
* public void addNode(CsNode<V, E> node)                                                               // add child vertex and its associated edge value to vertex 
* public void walk()                                                                                   // traverse the graph
 * Maintenance History:
 * --------------------
 * ver 1.0 : 03 Nov 2018
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace sccGraph
{
    public class GraphTest
    {
        List<string> sccNodes = new List<string>();

        //----------------< function to get dependency nodes >--------------------
        public List<CsNode<string, string>> getGraph(List<Tuple<string,string>> graph, List<string> files)
        {
            List<CsNode<string, string>> nodes = new List<CsNode<string, string>>();
            for (int i = 0; i < files.Count; i++)
            {
                CsNode<string, string> node = new CsNode<string, string>(files[i]);
                node.name = Path.GetFileName(files[i]);
                nodes.Add(node);
            }

            int n = graph.Count;

            for(int i = 0;i<graph.Count;i++)
            {
                int j = 0;
                for(j = 0; j<nodes.Count;j++)
                {
                    if (nodes[j].name == graph[i].Item1)
                        break;
                }
                for(int k = 0;k < nodes.Count;k++)
                {
                    if (nodes[k].name == graph[i].Item2 && j < graph.Count)
                        nodes[j].addChild(nodes[k], " ");
                }
            }
            
            return nodes;
            
        }

        //----------------< function to get strongly connected components >--------------------
        public List<string> Tarjan(List<CsNode<string, string>> m)
        {
            var index = 0;
            var S = new Stack<CsNode<string, string>>();
            void StrongConnect(CsNode<string, string> v)
            {
                v.Index = index;
                v.LowLink = index;
                string sccFiles = null;
                index++;
                S.Push(v);
                CsNode<string, string> w = null;
                for (int i = 0; i < v.children.Count; i++)
                {
                    w = v.children[i].targetNode;
                    if (w.Index < 0)
                    {
                        StrongConnect(w);
                        v.LowLink = Math.Min(v.LowLink, w.LowLink);
                    }
                    else if (S.Contains(w))
                        v.LowLink = Math.Min(v.LowLink, w.Index);
                }
                if (v.LowLink == v.Index)
                {
                    do
                    {
                        w = S.Pop();
                        sccFiles = sccFiles + w.name + " , ";
                    } while (w != v);
                    sccNodes.Add(sccFiles);
                    sccFiles = null;
                }
            }
            foreach (var v in m)
                if (v.Index < 0)
                    StrongConnect(v);
            return sccNodes;
        }     
    }
    
    // holds child node and instance of edge type E

    public class CsEdge<V, E> 
    {
        public CsNode<V, E> targetNode { get; set; } = null;
        public E edgeValue { get; set; }

        public CsEdge(CsNode<V, E> node, E value)
        {
            targetNode = node;
            edgeValue = value;
        }
    };

    public class CsNode<V, E>
    {

        public V nodeValue { get; set; }
        public string name { get; set; }
        public int LowLink { get; set; }
        public int Index { get; set; }
        public List<CsEdge<V, E>> children { get; set; }
        public bool visited { get; set; }

        //----< construct a named node >---------------------------------------

        public CsNode(string nodeName)
        {
            name = nodeName;
            Index = -1;
            children = new List<CsEdge<V, E>>();
            visited = false;
        }
        //----< add child vertex and its associated edge value to vertex >-----

        public void addChild(CsNode<V, E> childNode, E edgeVal)
        {
            children.Add(new CsEdge<V, E>(childNode, edgeVal));
        }
        //----< find the next unvisited child >--------------------------------

        public CsEdge<V, E> getNextUnmarkedChild()
        {
            foreach (CsEdge<V, E> child in children)
            {
                if (!child.targetNode.visited)
                {
                    child.targetNode.visited = true;
                    return child;
                }
            }
            return null;
        }
        //----< has unvisited child? >-----------------------------------

        public bool hasUnmarkedChild()
        {
            foreach (CsEdge<V, E> child in children)
            {
                if (!child.targetNode.visited)
                {
                    return true;
                }
            }
            return false;
        }
        public void unmark()
        {
            visited = false;
        }
        public override string ToString()
        {
            return name;
        }
    }
    /////////////////////////////////////////////////////////////////////////
    // Operation<V,E> class

    class Operation<V, E>
    {
        //----< graph.walk() calls this on every node >------------------------

        virtual public bool doNodeOp(CsNode<V, E> node)
        {
            Console.Write("\n  {0}", node.ToString());
            return true;
        }
        //----< graph calls this on every child visitation >-------------------

        virtual public bool doEdgeOp(E edgeVal)
        {
            Console.Write(" {0}", edgeVal.ToString());
            return true;
        }
    }
    /////////////////////////////////////////////////////////////////////////
    // CsGraph<V,E> class

    public class CsGraph<V, E>
    {
        public CsNode<V, E> startNode { get; set; }
        public string name { get; set; }
        public bool showBackTrack { get; set; } = false;

        private List<CsNode<V, E>> adjList { get; set; }  // node adjacency list
        private Operation<V, E> gop = null;

        //----< construct a named graph >--------------------------------------

        public CsGraph(string graphName)
        {
            name = graphName;
            adjList = new List<CsNode<V, E>>();
            gop = new Operation<V, E>();
            startNode = null;
        }
        //----< register an Operation with the graph >-------------------------

        //public Operation<V, E> setOperation(Operation<V, E> newOp)
        //{
        //    Operation<V, E> temp = gop;
        //    gop = newOp;
        //    return temp;
        //}
        //----< add vertex to graph adjacency list >---------------------------

        public void addNode(CsNode<V, E> node)
        {
            adjList.Add(node);
        }
        //----< clear visitation marks to prepare for next walk >--------------

        public void clearMarks()
        {
            foreach (CsNode<V, E> node in adjList)
                node.unmark();
        }
        //----< depth first search from startNode >----------------------------

        public void walk()
        {
            if (adjList.Count == 0)
            {
                Console.Write("\n  no nodes in graph");
                return;
            }
            if (startNode == null)
            {
                Console.Write("\n  no starting node defined");
                return;
            }
            if (gop == null)
            {
                Console.Write("\n  no node or edge operation defined");
                return;
            }
            this.walk(startNode);
            foreach (CsNode<V, E> node in adjList)
                if (!node.visited)
                    walk(node);
            foreach (CsNode<V, E> node in adjList)
                node.unmark();
            return;
        }
        //----< depth first search from specific node >------------------------

        public void walk(CsNode<V, E> node)
        {
            // process this node

            gop.doNodeOp(node);
            node.visited = true;

            // visit children
            do
            {
                CsEdge<V, E> childEdge = node.getNextUnmarkedChild();
                if (childEdge == null)
                {
                    return;
                }
                else
                {
                    gop.doEdgeOp(childEdge.edgeValue);
                    walk(childEdge.targetNode);
                    if (node.hasUnmarkedChild() || showBackTrack)
                    {                         // popped back to predecessor node
                        gop.doNodeOp(node);     // more edges to visit so announce
                    }                         // location and next edge
                }
            } while (true);
        }
        public void showDependencies()
        {
            Console.Write("\n  Dependency Table:");
            Console.Write("\n -------------------");
            foreach (var node in adjList)
            {
                Console.Write("\n  {0}", node.name);
                for (int i = 0; i < node.children.Count; ++i)
                {
                    Console.Write("\n    {0}", node.children[i].targetNode.name);
                }
            }
        }
    }
 

    class demoOperation : Operation<string, string>
    {
        override public bool doNodeOp(CsNode<string, string> node)
        {
            Console.Write("\n -- {0}", node.name);
            return true;
        }
    }
    class Test
    {
#if (GRAPH_TEST)

       
        static void Main(string[] args)
        {
            Console.WriteLine("----------< Test stub for Strongly connected component >---------");
            

            CsNode<string, string> node1 = new CsNode<string, string>("node1");
            CsNode<string, string> node2 = new CsNode<string, string>("node2");
            CsNode<string, string> node3 = new CsNode<string, string>("node3");
            CsNode<string, string> node4 = new CsNode<string, string>("node4");
            CsNode<string, string> node5 = new CsNode<string, string>("node5");
            List<CsNode<string, string>> nodes = new List<CsNode<string, string>>();
            node1.addChild(node2, "edge12");
            node1.addChild(node3, "edge13");
            node2.addChild(node3, "edge23");
            node2.addChild(node4, "edge24");
            node3.addChild(node1, "edge31");
            node5.addChild(node1, "edge51");
            node5.addChild(node4, "edge54");
            List<string> sccNodes = new List<string>();
            CsGraph<string, string> graph = new CsGraph<string, string>("Fred");
            graph.addNode(node1);
            graph.addNode(node2);
            graph.addNode(node3);
            graph.addNode(node4);
            graph.addNode(node5);
            nodes.Add(node1);
            nodes.Add(node2);
            nodes.Add(node3);
            nodes.Add(node4);
            nodes.Add(node5);

            GraphTest T = new GraphTest();
            sccNodes = T.Tarjan(nodes);
            for (int i = 0; i < sccNodes.Count; i++)
            {
                Console.WriteLine(" scc" + (i + 1) + ": " + sccNodes[i]);
            }
            Console.Read();

        }
#endif
    }
}
