///////////////////////////////////////////////////////////////////////////
// Display.cs  -  Manage Display properties                              //
// ver 1.0                                                               //
// Language:    C#, Visual Studio 2013, .Net Framework 4.5               //
// Platform:    Dell XPS 2720 , Win 8.1 Pro                              //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018  //
// Source Author:      Jim Fawcett, CST 2-187, Syracuse University       //
//              (315) 443-3948, jfawcett@twcny.rr.com                    //
///////////////////////////////////////////////////////////////////////////
/*
 * Package Operations
 * ==================
 * Display manages static public properties used to control what is displayed and
 * provides static helper functions to send information to MainWindow and Console.
 * 
 */
/*
 * Required Files:
 * =============
 *   Element.cs
 *   CsGraph.cs
 *   Executive.cs
 *   
 * Public Interface Documentation:
 * ===================================
 * public static class StringExt                                            
 * public static string Truncate(this string value, int maxLength)
 * static public class Display
 * static public void showMetricsUsing(List<List<Elem>> tableList)
 * static public void showMetricsInterface(List<List<Elem>> tableList)		
 * static public void showMetricsClass(List<List<Elem>> tableList)
 * static public void showMetricsFunction(List<List<Elem>> tableList)
 * static public void showMetricsEnum(List<List<Elem>> tableList)
 * static public void showMetricsStruct(List<List<Elem>> tableList)
 * static public void showMetricsDelegate(List<List<Elem>> tableList)
 * static public void showMetricsAlias(List<List<Elem>> tableList)
 * static public void showDependencies(List<CsNode<string, string>> nodes)
 * static public void showsccGraph(List<string> sccNodes)
 * static public void displaySemiString(string semi)
 * static public void displayString(Action<string> act, string str)
 * static public void displayString(string str, bool force=false)
 * static public void displayRules(Action<string> act, string msg)
 * static public void displayActions(Action<string> act, string msg)
 * static public void displayFiles(Action<string> act, string file)
 * static public void displayDirectory(Action<string> act, string file) 
 * 
 * Maintenance History
 * ===================
 * ver 1.0 : 02 Nov 2018
 *   - first release
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using sccGraph;

namespace CodeAnalysis
{
  ///////////////////////////////////////////////////////////////////
  // StringExt static class
  // - extension method to truncate strings

  public static class StringExt
  {
    public static string Truncate(this string value, int maxLength)
    {
      if (string.IsNullOrEmpty(value)) return value;
      return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }
  }

  static public class Display
  {
    static Display()
    {
      showFiles = true;
      showDirectories = true;
      showActions = false;
      showRules = false;
      useFooter = false;
      useConsole = false;
      goSlow = false;
      width = 33;
    }
    static public bool showFiles { get; set; }
    static public bool showDirectories { get; set; }
    static public bool showActions { get; set; }
    static public bool showRules { get; set; }
    static public bool showSemi { get; set; }
    static public bool useFooter { get; set; }
    static public bool useConsole { get; set; }
    static public bool goSlow { get; set; }
    static public int width { get; set; }
    static public List<string> typetableList = new List<string>();
    

    //----< display using type and filename >-----------------------

    static public void showMetricsUsing(List<List<Elem>> tableList)
    {
            //Console.WriteLine();
            //Console.WriteLine("Using: ");
            typetableList.Add("Using:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach(Elem e in table)
                {
                    if(e.type == "using")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ "+e.filename + " , " + e.name + " ]");
                    }
                }
            }
    }

    //----< display namespace type and filename >-----------------------
    static public void showMetricsNamespace(List<List<Elem>> tableList)
        {
            typetableList = new List<string>();
            //Console.WriteLine();
            //Console.WriteLine("Namespace: ");
            typetableList.Add("Namespace:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "namespace")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" +"\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display interface type and filename >-----------------------
    static public void showMetricsInterface(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Interface: ");
            typetableList.Add("Interface:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "interface")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display class type and filename >-----------------------
    static public void showMetricsClass(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Class: ");
            typetableList.Add("Class:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "class")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display function type and filename >-----------------------
    static public void showMetricsFunction(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Function: ");
            typetableList.Add("Function:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "function")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display enum type and filename >-----------------------
    static public void showMetricsEnum(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Enum: ");
            typetableList.Add("Enum:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "enum")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display struct type and filename >-----------------------
    static public void showMetricsStruct(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Struct: ");
            typetableList.Add("Struct:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "struct")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display delegate type and filename >-----------------------
    static public void showMetricsDelegate(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Delegate: ");
            typetableList.Add("Delegate:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "delegate")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
        }

    //----< display Alias type and filename >-----------------------
    static public List<string> showMetricsAlias(List<List<Elem>> tableList)
        {
            //Console.WriteLine();
            //Console.WriteLine("Alias: ");
            typetableList.Add("Alias:\n");
            foreach (List<Elem> table in tableList)
            {
                foreach (Elem e in table)
                {
                    if (e.type == "alias")
                    {
                        typetableList.Add(" [ " + e.filename + " , " + e.name + " ]" + "\n");
                        //Console.WriteLine(" [ " + e.filename + " , " + e.name + " ]");
                    }
                }
            }
            return typetableList;
        }

    //----< display dependency file names >-----------------------
    static public List<string> showDependencies(List<CsNode<string, string>> nodes)
        {
            List<string> listOfDep = new List<string>();
            foreach (var node in nodes)
            {
                //Console.Write("\n File  {0} depends on Files : ", node.name);
                listOfDep.Add("File " + node.name + "depends on: ");
                for (int i = 0; i < node.children.Count; ++i)
                {
                    //Console.Write("\n       {0}: {1}", (i + 1), node.children[i].targetNode.name);
                    listOfDep.Add("\n        " + node.children[i].targetNode.name + "\n");
                }
                //Console.WriteLine();
                listOfDep.Add("\n");
            }
            return listOfDep;
        }
    
    static public List<string> showTypetable(List<List<Elem>> tablelist)
        {
            List<string> generatetypetable = new List<string>();
            showMetricsNamespace(tablelist);
            Display.showMetricsUsing(tablelist);
            Display.showMetricsInterface(tablelist);
            Display.showMetricsClass(tablelist);
            Display.showMetricsDelegate(tablelist);
            Display.showMetricsStruct(tablelist);
            Display.showMetricsEnum(tablelist);
            Display.showMetricsFunction(tablelist);
            generatetypetable = showMetricsAlias(tablelist);
            return generatetypetable;
        }

    //----< display connect components >-----------------------
    static public List<string> showsccGraph(List<string> sccNodes)
        {
            List<string> listOfscc = new List<string>();
            for (int i = 0; i < sccNodes.Count; i++)
            {
                //Console.WriteLine(" scc" + (i + 1) + ": " + sccNodes[i]);
                listOfscc.Add(" scc" + (i + 1) + ": " + sccNodes[i]+"\n");
            }
            return listOfscc;
        }

        //----< display a semiexpression on Console >--------------------

        static public void displaySemiString(string semi)
    {
      if (showSemi && useConsole)
      {
        Console.Write("\n");
        System.Text.StringBuilder sb = new StringBuilder();
        for (int i = 0; i < semi.Length; ++i)
          if (!semi[i].Equals('\n'))
            sb.Append(semi[i]);
        Console.Write("\n  {0}", sb.ToString());
      }
    }
    //----< display, possibly truncated, string >--------------------

    static public void displayString(Action<string> act, string str)
    {
      if (goSlow) Thread.Sleep(200);  //  here only to support visualization
      if (act != null && useFooter)
        act.Invoke(str.Truncate(width));
      if (useConsole)
        Console.Write("\n  {0}", str);
    }
    //----< display string, possibly overriding client pref >--------

    static public void displayString(string str, bool force=false)
    {
      if (useConsole || force)
        Console.Write("\n  {0}", str);
    }
    //----< display rules messages >---------------------------------

    static public void displayRules(Action<string> act, string msg)
    {
      if (showRules)
      {
        displayString(act, msg);
      }
    }
    //----< display actions messages >-------------------------------

    static public void displayActions(Action<string> act, string msg)
    {
      if (showActions)
      {
        displayString(act, msg);
      }
    }
    //----< display filename >---------------------------------------

    static public void displayFiles(Action<string> act, string file)
    {
      if (showFiles)
      {
        displayString(act, file);
      }
    }
    //----< display directory >--------------------------------------

    static public void displayDirectory(Action<string> act, string file)
    {
      if (showDirectories)
      {
        displayString(act, file);
      }
    }

#if(TEST_DISPLAY)
    static void Main(string[] args)
    {
      Console.Write("\n  Tested by use in Parser\n\n");
    }
#endif
  }
}
