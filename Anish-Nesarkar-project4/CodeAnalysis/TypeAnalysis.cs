//////////////////////////////////////////////////////////////////////////
// TypeAnalysis.cs - Generating Type table                              //
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
 * This package takes files as input from the executive.cs
 * It creates type table for the input files using Parser and Rules and Action.
 * Type table detects types namespace, using, alias, enum, struct, Interface, delegates, Class, Functions. 
 */
/* Required Files:
 * ==================================
 *   Parser.cs
 *   RulesAndActions.cs
 *   IRuleAndActions.cs
 *   ScopeStack.cs
 *   Semi.cs
 *   Toker.cs
 *   ITokenCollection.cs
 *   Display.cs
 *   Element.cs
 *   Executive.cs
 *   
 *   Public Interface Documentation:
 *   =================================
 *   public class TypeAnalysis                                //Class for generating Type table
 *   public TypeAnalysis(List<string> files)                  //Constructor for above class
 *   public List<List<Elem>> generateTypeTable()              //function that generates Type table
 *   
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 02 Nov 2018
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexer;

namespace CodeAnalysis
{
    
    public class TypeAnalysis
    {
        List<string> files;

        //---------------< Function for Type Analysis to generate Type table >---------------
        public TypeAnalysis(List<string> files)
        {
            this.files = files;
        }
        
        public List<List<Elem>> generateTypeTable()
        {
            string nameofFile;
            List<List<Elem>> listOfTables = new List<List<Elem>>();
            foreach (string file in files)
            {
                nameofFile = System.IO.Path.GetFileName(file);
                ITokenCollection semi = Factory.create();
                //semi.displayNewLines = false;
                if (!semi.open(file as string))
                {
                    Console.Write("\n  Can't open {0}\n\n", nameofFile);
                    
                }

                BuildCodeAnalyzer builder = new BuildCodeAnalyzer(semi, nameofFile);
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
                listOfTables.Add(table);
                semi.close();
            }
            return listOfTables;
            
        }
#if(TEST_TYPE_ANALYSIS)

        static List<string> ProcessCommandline(string[] args)
        {
            List<string> files = new List<string>();
            string fullpath = Path.GetFullPath(args[0]);
            if (Directory.Exists(fullpath))
            {
                string fileFormat = "cs";
                DirectoryInfo dir = new DirectoryInfo(fullpath);
                foreach (FileInfo file in dir.GetFiles("*." + fileFormat + "*", SearchOption.AllDirectories))
                {
                    files.Add(file.FullName);
                }
            }
            return files;
        }

     

        public static void displayRequirement1(List<List<Elem>> listOfTables)
        {
            Console.WriteLine();
            //Console.WriteLine("<======================== Requirement 5: =========================> \n");
            Console.WriteLine("Type Table: ");
            Display.showMetricsUsing(listOfTables);
            Display.showMetricsNamespace(listOfTables);
            Display.showMetricsInterface(listOfTables);
            Display.showMetricsClass(listOfTables);
            Display.showMetricsFunction(listOfTables);
            Display.showMetricsEnum(listOfTables);
            Display.showMetricsStruct(listOfTables);
            Display.showMetricsDelegate(listOfTables);
            Display.showMetricsAlias(listOfTables);
            Console.Write("\n\n");
        }

        static void Main(string[] args)
        {  
            List<List<Elem>> listOfTables = new List<List<Elem>>();
            Console.WriteLine("<------------------------------ Type Analysis Test Stub ----------------------->");
            Console.WriteLine();
            List<string> files = ProcessCommandline(args);
            TypeAnalysis typeAnalysisObj = new TypeAnalysis(files);
            listOfTables = typeAnalysisObj.generateTypeTable();
            displayRequirement1(listOfTables);
            Console.ReadLine();
        }
#endif
    }
}
