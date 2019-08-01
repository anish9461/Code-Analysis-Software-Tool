///////////////////////////////////////////////////////////////////////////
// Parser.cs - Parser detects code constructs defined by rules           //
// ver 1.0                                                              //
// Language:    C#, 2008, .Net Framework 4.0                           //
// Author:      Chandana Rao                                          //
// Source:      Jim Fawcett                                          //
//////////////////////////////////////////////////////////////////////
/*
 * Module Operations:
 * ------------------
 * This module defines the following class:
 *   Parser  - a collection of IRules
 */
/* Required Files:
 *   IRulesAndActions.cs, RulesAndActions.cs, Parser.cs, Semi.cs, Toker.cs
 *   Display.cs
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 30 Aug 2018
 * - first release
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lexer;

namespace CodeAnalysis
{
    /////////////////////////////////////////////////////////
    // rule-based parser used for code analysis


    public class Parser
  {
    private List<IRule> Rules;

    public Parser()
    {
      Rules = new List<IRule>();
    }
    public void add(IRule rule)
    {
      Rules.Add(rule);
    }
    public void parse(Lexer.ITokenCollection semi)
    {
      // Note: rule returns true to tell parser to stop
      //       processing the current semiExp
      
      Display.displaySemiString(semi.ToString());

      foreach (IRule rule in Rules)
      {
        if (rule.test(semi))
          break;
      }
    }
  }

  class TestParser
  {

    //----< process commandline to get file references >-----------------

    static List<string> ProcessCommandline(string[] args)
    {
      List<string> files = new List<string>();
      if (args.Length == 0)
      {
        Console.Write("\n  Please enter file(s) to analyze\n\n");
        return files;
      }
      string path = args[0];
      path = Path.GetFullPath(path);
      for (int i = 1; i < args.Length; ++i)
      {
        string filename = Path.GetFileName(args[i]);
        files.AddRange(Directory.GetFiles(path, filename));
      }
      return files;
    }

    static void ShowCommandLine(string[] args)
    {
      Console.Write("\n  Commandline args are:\n  ");
      foreach (string arg in args)
      {
        Console.Write("  {0}", arg);
      }
      Console.Write("\n  current directory: {0}", System.IO.Directory.GetCurrentDirectory());
      Console.Write("\n");
    }

    //----< Test Stub >--------------------------------------------------

#if(TEST_PARSER)

    static void Main(string[] args)
    {
            List<List<Elem>> allTables = new List<List<Elem>>();
            Console.Write("\n  Demonstrating Parser");
      Console.Write("\n ======================\n");

      ShowCommandLine(args);

      List<string> files = TestParser.ProcessCommandline(args);
      foreach (string file in files)
      {
        Console.Write("\n  Processing file {0}\n", System.IO.Path.GetFileName(file));

        ITokenCollection semi = Factory.create();
        //semi.displayNewLines = false;
        if (!semi.open(file as string))
        {
          Console.Write("\n  Can't open {0}\n\n", args[0]);
          return;
        }

        Console.Write("\n  Type and Function Analysis");
        Console.Write("\n ----------------------------");

        BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi, file);
        Parser parser = builder.build();
              
                try
        {
          while (semi.get().Count > 0)
            parser.parse(semi);
        }
        catch (Exception ex)
        {
          Console.Write("\n\n  {0}\n", ex.Message);
        }
        Repository rep = Repository.getInstance();
        List<Elem> table = rep.locations;
                allTables.Add(table);
                Console.Write("\n");

        semi.close();
      }
            Display.showMetricsNamespace(allTables);
            Display.showMetricsClass(allTables);
            Display.showMetricsFunction(allTables);
            Display.showMetricsAlias(allTables);
            Display.showMetricsEnum(allTables);
            Display.showMetricsStruct(allTables);
            Display.showMetricsDelegate(allTables);
            Display.showMetricsUsing(allTables);
            Display.showMetricsInterface(allTables);
            Console.Write("\n\n");
    }
#endif
  }
}
