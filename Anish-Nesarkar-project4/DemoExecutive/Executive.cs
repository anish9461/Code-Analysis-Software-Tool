//////////////////////////////////////////////////////////////////////////
// Executive.cs - Automated Test Unit for Project #3                    //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Application: Demonstration for CSE681, Project #3, Fall 2018         //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package takes input from command line arguments.
 * Command Line arguments accepts files as input
 * Displays type table, Dependency Analysis and Strongly connected Components for the files
 * 
 */
/* Required Files:
 *   DependencyAnalysis.cs
 *   TypeAnalysis.cs
 *   CsGraph.cs
 *   Display.cs
 *   Element.cs
 *   
 *   Public Interface Documentation:
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 03 Nov 2018
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DependencyAnalyzer;
using sccGraph;

namespace CodeAnalysis
{ 
  public static class Executive
  {
        //----< process commandline to get file references >-----------------
        
        static List<string> ProcessCommandline(string[] args)
        {
            List<string> files = new List<string>();
            string fullpath = Path.GetFullPath(args[0]);
            if(Directory.Exists(fullpath))
            {
                Console.WriteLine();
                Console.WriteLine("<================= Requirement 4 ===================>");
                Console.WriteLine("Directory files are: ");
                string fileFormat = "cs";
                DirectoryInfo dir = new DirectoryInfo(fullpath);
                
                foreach (FileInfo file in dir.GetFiles("*."+ fileFormat + "*", SearchOption.AllDirectories))
                {
                    files.Add(file.FullName);
                    Console.WriteLine(" "+file.Name);
                }
            }
            return files;
        }

        //----< Display commandline arguments and get files for Requirement 4 >-----------------
        static void ShowCommandLine(string[] args)
        {    
            Console.WriteLine("  Commandline args are:  ");           
            Console.WriteLine("  {0}", args[0]);           
            string dirPath = Path.GetFullPath(args[0]);
            Console.WriteLine();
            Console.WriteLine(" Directory containing files");
            Console.WriteLine(" "+dirPath);
        }

        //-------< Display Type Table for the Test files >-----------------
        static void displayRequirement1(List<List<Elem>> listOfTables)
        {           
            Console.WriteLine();
            Console.WriteLine("<======================== Requirement 5: Type Table and Dependency Analysis =========================> \n");
            Console.WriteLine("Type Table: ");
            Display.showMetricsNamespace(listOfTables);
            Display.showMetricsUsing(listOfTables);
            Display.showMetricsInterface(listOfTables);           
            Display.showMetricsClass(listOfTables);
            Display.showMetricsDelegate(listOfTables);
            Display.showMetricsStruct(listOfTables);
            Display.showMetricsEnum(listOfTables);
            Display.showMetricsFunction(listOfTables);
            Display.showMetricsAlias(listOfTables);
            Console.Write("\n\n");
        }
        //----< Display Dependency Analysis Requirement >-----------------

        static void displayRequirement2(List<CsNode<string, string>> nodes)
        {
            Console.WriteLine("Display dependency analysis for namespace,using and alias ");
            Console.WriteLine("The files are checked for different types like class,interface,delegate, struct, enum to check dependency by parsing the file twice");
            Console.WriteLine();
            Console.WriteLine("Dependency Analysis for the files: ");
            Display.showDependencies(nodes);
        }
        
        //----< Display strongly Connected Components requirements >-----------------
        static void displayRequirement3(List<string> sccNodes)
        {
            Console.WriteLine();
            Console.WriteLine("<======================== Requirement 6: =========================> \n");
            Console.WriteLine("Strongly Connected Components for the files: \n");
            Display.showsccGraph(sccNodes);
        }

       // public List<List<string>> static getTypeTable()

        static void Main(string[] args)
        {
            List<CsNode<string, string>> nodes = new List<CsNode<string, string>>();
            List<List<Elem>> listOfTables = new List<List<Elem>>();
            Console.WriteLine("<------------------------------ Demonstrating Project 3 : Type-Based Package Dependency Analysis ----------------------->");
            Console.WriteLine();
            ShowCommandLine(args);
            List<string> files = ProcessCommandline(args);
            TypeAnalysis typeAnalysisObj = new TypeAnalysis(files);
            listOfTables = typeAnalysisObj.generateTypeTable();
            nodes = DependencyAnalysis.GetDependency(listOfTables,files);
            GraphTest gt = new GraphTest();
            List<string> sccNodes = gt.Tarjan(nodes);           
            displayRequirement1(listOfTables);
            displayRequirement2(nodes); 
            displayRequirement3(sccNodes);
            Console.ReadLine();
        }
  }
}
