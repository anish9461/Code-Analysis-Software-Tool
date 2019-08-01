///////////////////////////////////////////////////////////////////////
// ITokenCollection.cs - Interface for Semiexpression                //
// ver 1.0                                                           //
// Language:    C#, 2017, .Net Framework 4.7.1                       //
// Platform:    Dell Precision T8900, Win10                          //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis        //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University   //      
///////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package consists of function declarations for semiExpression
 * 
 */
/* Required Files:
 *  -------------------
 *  
 *  public Interface Documentation:
 *  public interface ITokenCollection : IEnumerable<Token>
 *   
 * Maintenance History:
 * --------------------
 * ver 1.0 : 02 Nov 2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
  using Token = String;
  using TokColl = List<String>;

  public interface ITokenCollection : IEnumerable<Token>
  {
    bool open(string source);                 
    void close();                             
    TokColl get();                            
    int size();                               
    Token this[int i] { get; set; }           
    ITokenCollection add(Token token);        
    bool insert(int n, Token tok);            
    void clear();                             
    bool contains(Token token);               
    bool find(Token tok, out int index);      
    Token predecessor(Token tok);             
    bool hasSequence(params Token[] tokSeq);  
    bool hasTerminator();                     
    bool isDone();                            
    int lineCount();                          
    string ToString();                        
    void show();                              
  }
}
