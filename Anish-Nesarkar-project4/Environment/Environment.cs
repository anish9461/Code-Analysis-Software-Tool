///////////////////////////////////////////////////////////////////////////
// Environment.cs - defines environment properties for Client and Server //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 * This package assigns the root, address, port to the client and server environment
 * 
 * public Interface Documentation -
 * public struct Environment
 * public static string root { get; set; }
 * public const long blockSize = 1024;
 * public static string endPoint { get; set; }
 * public static string address { get; set; }
 * public static int port { get; set; }
 * public static bool verbose { get; set; }
 * public struct ClientEnvironment
 * public static string root { get; set; } = "../../../ClientFiles/";
 * public static string endPoint { get; set; } = "http://localhost:8090/IMessagePassingComm";
 * public static string address { get; set; } = "http://localhost";
 * public static int port { get; set; } = 8090;
 * public static bool verbose { get; set; } = true;
 * public struct ServerEnvironment
 * public static string root { get; set; } = "../../../ServerFiles/";
 * public static string endPoint { get; set; } = "http://localhost:8080/IMessagePassingComm";
 * public static string address { get; set; } = "http://localhost";
 * public static int port { get; set; } = 8080;
 * 
 * Maintenance History:
 * --------------------
 * ver 1.0 : 05 Dec 2018
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigator
{
  public struct Environment
  {
    public static string root { get; set; }
    public const long blockSize = 1024;
    public static string endPoint { get; set; }
    public static string address { get; set; }
    public static int port { get; set; }
    public static bool verbose { get; set; }
  }

  public struct ClientEnvironment
  {
    public static string root { get; set; } = "../../../ClientFiles/";
    public const long blockSize = 1024;
    public static string endPoint { get; set; } = "http://localhost:8090/IMessagePassingComm";
    public static string address { get; set; } = "http://localhost";
    public static int port { get; set; } = 8090;
    public static bool verbose { get; set; } = true;
  }

  public struct ServerEnvironment
  {
    public static string root { get; set; } = "../../../ServerFiles/";
    public const long blockSize = 1024;
    public static string endPoint { get; set; } = "http://localhost:8080/IMessagePassingComm";
    public static string address { get; set; } = "http://localhost";
    public static int port { get; set; } = 8080;
    public static bool verbose { get; set; } = true;
  }
}
