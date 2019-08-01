/////////////////////////////////////////////////////////////////////
// IMPCommService.cs - service interface for MessagePassingComm    //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 */
/*
 * This package provides:
 * ----------------------
 * - ClientEnvironment   : client-side path and address
 * - ServiceEnvironment  : server-side path and address
 * - IMessagePassingComm : interface used for message passing and file transfer
 * - CommMessage         : class representing serializable messages
 * 
 * Required Files:
 * ---------------
 * - IPCommService.cs         : Service interface and Message definition
 * 
 * Public Interface Documentation -
 * public interface IMessagePassingComm
 * public class CommMessage
 * public enum MessageType
 * public MessageType type { get; set; } = MessageType.connect;
 * public string to { get; set; }
 * public string from { get; set; }
 * public Command command { get; set; }
 * public List<Argument> arguments { get; set; } = new List<Argument>();
 * public int threadId { get; set; } = Thread.CurrentThread.ManagedThreadId;
 * public ErrorMessage errorMsg { get; set; } = "no error";
 * public string lastPath { get; set; }
 * public void show()
 * public CommMessage clone()
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
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Threading;

namespace MessagePassingComm
{
  using Command = String;             
  using EndPoint = String;            
  using Argument = String;      
  using ErrorMessage = String;

  [ServiceContract(Namespace = "MessagePassingComm")]
  public interface IMessagePassingComm
  {
    /*----< support for message passing >--------------------------*/

    [OperationContract(IsOneWay = true)]
    void postMessage(CommMessage msg);

    // private to receiver so not an OperationContract
    CommMessage getMessage();

    /*----< support for sending file in blocks >-------------------*/

    [OperationContract]
    bool openFileForWrite(string name);

    [OperationContract]
    bool writeFileBlock(byte[] block);

    [OperationContract(IsOneWay = true)]
    void closeFile();
  }

  [DataContract]
  public class CommMessage
  {
    public enum MessageType
    {
      [EnumMember]
      connect,           
      [EnumMember]
      request,          
      [EnumMember]
      reply,            
      [EnumMember]
      closeSender,       
      [EnumMember]
      closeReceiver      
    }

    /*----< constructor requires message type >--------------------*/

    public CommMessage(MessageType mt)
    {
      type = mt;
    }
    /*----< data members - all serializable public properties >----*/

    [DataMember]
    public MessageType type { get; set; } = MessageType.connect;

    [DataMember]
    public string to { get; set; }

    [DataMember]
    public string from { get; set; }

    [DataMember]
    public Command command { get; set; }

    [DataMember]
    public List<Argument> arguments { get; set; } = new List<Argument>();

    [DataMember]
    public int threadId { get; set; } = Thread.CurrentThread.ManagedThreadId;

    [DataMember]
    public ErrorMessage errorMsg { get; set; } = "no error";

    [DataMember]
    public string lastPath { get; set; }

    public void show()
    {
      Console.Write("\n  CommMessage:");
      Console.Write("\n    MessageType : {0}", type.ToString());
      Console.Write("\n    to          : {0}", to);
      Console.Write("\n    from        : {0}", from);
      Console.Write("\n    command     : {0}", command);
      Console.Write("\n    arguments   :");
      if (arguments.Count > 0)
        Console.Write("\n      ");
      foreach (string arg in arguments)
        Console.Write("{0} ", arg);
      Console.Write("\n    ThreadId    : {0}", threadId);
      Console.Write("\n    errorMsg    : {0}\n", errorMsg);
    }

    public CommMessage clone()
    {
      CommMessage msg = new CommMessage(MessageType.request);
      msg.type = type;
      msg.to = to;
      msg.from = from;
      msg.command = command;
      foreach (string arg in arguments)
        msg.arguments.Add(arg);
      return msg;
    }
  }
}
