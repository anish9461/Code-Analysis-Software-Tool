///////////////////////////////////////////////////////////////////////////
// Element.cs - Data Structure for holding Parser analysis results       //
// ver 1.0                                                               //
// Language:    C# 7.0, .Net Framework 4.6.1                             //
// Platform:    Dell XPS 8900, Win10                                     //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018  //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University       //
//              (315) 443-3948, jfawcett@twcny.rr.com                    //
///////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * ------------------
 * This packages consists of data structures for type table
 */
/* Required Files:
 *   Element.cs
 *   
 *   Public Interface Documentation:
 *   ====================================
 *   public class Elem                        // holds the scope information
 *   public override string ToString()
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 02 Nov 2018
 * 
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalysis
{
  public class Elem  
  {
    public string type { get; set; }
    public string filename { get; set; }
    public string name { get; set; }
    public string aliasName { get; set; }
    public int beginLine { get; set; }
    public int endLine { get; set; }
    public int beginScopeCount { get; set; }
    public int endScopeCount { get; set; }

    public override string ToString()
    {
      StringBuilder temp = new StringBuilder();
            
      temp.Append("{");
            temp.Append(filename);
            temp.Append(String.Format("{0,-10}", type)).Append(" : ");
      temp.Append(String.Format("{0,-10}", name)).Append(" : ");
      temp.Append(String.Format("{0,-5}", beginLine.ToString()));  // line of scope start
      temp.Append(String.Format("{0,-5}", endLine.ToString()));    // line of scope end
      temp.Append("}");
      return temp.ToString();
    }
  }
}

