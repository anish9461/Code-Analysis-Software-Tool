////////////////////////////////////////////////////////////////////////////
// NavigatorServer.cs - File Server for WPF NavigatorClient Application   //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines a single NavigatorServer class that returns file
 * and directory information about its rootDirectory subtree.  It uses
 * a message dispatcher that handles processing of all incoming and outgoing
 * messages.
 * 
 * Required files -
 * ===========================
 * Environment.cs
 * FileMgr.cs
 * IMessagePassingCommService.cs
 * MessagePassingCommService.cs
 * TestUtilities.cs
 * TypeAnalysis.cs
 * DependencyAnalyzer.cs
 * Display.cs
 * sccGraph.cs
 * 
 * Public Interface Documentation -
 * ================================
 * public class NavigatorServer
 * public NavigatorServer()
 * 
 * Maintanence History:
 * --------------------
 * ver 1.0 - 05 Dec 2018
 * - first release
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePassingComm;
using CodeAnalysis;
using DependencyAnalyzer;
using sccGraph;
using System.IO;

namespace Navigator
{
  
  public class NavigatorServer
  {
    IFileMgr localFileMgr { get; set; } = null;
    Comm comm { get; set; } = null;
    Dictionary<string, Func<CommMessage, CommMessage>> messageDispatcher = 
    new Dictionary<string, Func<CommMessage, CommMessage>>();
    List<string> receivedfiles = new List<string>();

    /*----< initialize server processing >-------------------------*/

    public NavigatorServer()
    {
      initializeEnvironment();
      Console.Title = "Code Analyzer Server";
      localFileMgr = FileMgrFactory.create(FileMgrType.Local);
    }
    /*----< set Environment properties needed by server >----------*/

    void initializeEnvironment()
    {
      Environment.root = ServerEnvironment.root;
      Environment.address = ServerEnvironment.address;
      Environment.port = ServerEnvironment.port;
      Environment.endPoint = ServerEnvironment.endPoint;
    }
    /*----< define how each message will be processed >------------*/

    void initializeDispatcher1()
    {    
      Func<CommMessage, CommMessage> moveIntoFolderDirs = (CommMessage msg) =>
      {
        if (msg.arguments.Count() == 1)
          localFileMgr.currentPath = msg.arguments[0];
        localFileMgr.pathStack.Push(localFileMgr.currentPath);
          CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
        reply.to = msg.from;
        reply.from = msg.to;
        reply.command = "moveIntoFolderDirs";
        reply.arguments = localFileMgr.getDirs().ToList<string>();
        reply.lastPath = msg.arguments[0];     
          return reply;
      };
      messageDispatcher["moveIntoFolderDirs"] = moveIntoFolderDirs;
            
            Func<CommMessage, CommMessage> moveOutFolderDirs = (CommMessage msg) =>
            {
                if (msg.lastPath == "")
                    return null;
                localFileMgr.pathStack.Pop();
                localFileMgr.currentPath = localFileMgr.pathStack.Peek();
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "moveOutFolderDirs";
                reply.arguments = localFileMgr.getDirs().ToList<string>();
                reply.lastPath = localFileMgr.currentPath;
                return reply;
            };
            messageDispatcher["moveOutFolderDirs"] = moveOutFolderDirs;
        }
    void initializeDispatcher2()
        {
            Func<CommMessage, CommMessage> analyze = (CommMessage msg) =>
            {
                receivedfiles = new List<string>();
                receivedfiles.AddRange(msg.arguments);
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "FilesReceived";
                return reply;
            };
            messageDispatcher["SelectedFiles"] = analyze;

            Func<CommMessage, CommMessage> typetable = (CommMessage msg) =>
            {

                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "SentTypeTable";
                reply.arguments = getTypetable(receivedfiles);
                return reply;
            };
            messageDispatcher["getTypeTable"] = typetable;

            Func<CommMessage, CommMessage> dependency = (CommMessage msg) =>
            {

                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "SentDependency";
                reply.arguments = getDependency(receivedfiles);
                return reply;
            };
            messageDispatcher["getDependency"] = dependency;
        }
    void initializeDispatcher3()
        {
            Func<CommMessage, CommMessage> scc = (CommMessage msg) =>
            {

                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "Sentscc";
                reply.arguments = getscc(receivedfiles);
                return reply;
            };
            messageDispatcher["getScc"] = scc;

            Func<CommMessage, CommMessage> getTopFiles = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getTopFiles";
                reply.arguments = localFileMgr.getFiles().ToList<string>();
                return reply;
            };
            messageDispatcher["getTopFiles"] = getTopFiles;

            Func<CommMessage, CommMessage> getTopDirs = (CommMessage msg) =>
            {
                localFileMgr.currentPath = "";
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "getTopDirs";
                reply.arguments = localFileMgr.getDirs().ToList<string>();
                return reply;
            };
            messageDispatcher["getTopDirs"] = getTopDirs;
        }
    void initializeDispatcher4()
        {
            Func<CommMessage, CommMessage> connectServ = (CommMessage msg) =>
            {
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "connect";
                return reply;
            };
            messageDispatcher["connect"] = connectServ;

            Func<CommMessage, CommMessage> moveIntoFolderFiles = (CommMessage msg) =>
            {
                if (msg.arguments.Count() == 1)
                    localFileMgr.currentPath = msg.arguments[0];
                CommMessage reply = new CommMessage(CommMessage.MessageType.reply);
                reply.to = msg.from;
                reply.from = msg.to;
                reply.command = "moveIntoFolderFiles";
                reply.arguments = localFileMgr.getFiles().ToList<string>();
                return reply;
            };
            messageDispatcher["moveIntoFolderFiles"] = moveIntoFolderFiles;
        }
     //---------< get the file path of the selected files >-------------
        List<string> getfilePath(List<string> files)
        {
            string x = files.ToString();   
            List<string> selectedFiles = new List<string>();
            string fullpath = Path.GetFullPath(ServerEnvironment.root);
            if (Directory.Exists(fullpath))
            {
                string fileFormat = "cs";
                DirectoryInfo dir = new DirectoryInfo(fullpath);
                foreach (FileInfo serverfile in dir.GetFiles("*." + fileFormat + "*", SearchOption.AllDirectories))
                {                   
                    foreach(string file in files)
                    {
                        if (serverfile.FullName.Contains(file))
                            selectedFiles.Add(serverfile.FullName);
                    }
                }
            }
            return selectedFiles;
        }

    //---------< get the typetable for selected files >-------------
        List<string> getTypetable(List<string> files)
        {
            List<List<Elem>> typetable = new List<List<Elem>>();
            List<string> selectedfiles = new List<string>();
            List<string> typetableList = new List<string>();
            selectedfiles = getfilePath(files);
            TypeAnalysis typetableobj = new TypeAnalysis(selectedfiles);
            typetable = typetableobj.generateTypeTable();
            typetableList = Display.showTypetable(typetable);
            return typetableList;
        }

    //---------< get the depedency table for selected files >------------
        List<string> getDependency(List<string> files)
        {
            List<CsNode<string, string>> nodes = new List<CsNode<string, string>>();
            List<List<Elem>> dependency = new List<List<Elem>>();
            List<string> selectedfiles = new List<string>();
            List<string> dependencyList = new List<string>();
            selectedfiles = getfilePath(files);
            TypeAnalysis typeAnalysisObj = new TypeAnalysis(selectedfiles);
            dependency = typeAnalysisObj.generateTypeTable();
            nodes = DependencyAnalysis.GetDependency(dependency, selectedfiles);
            dependencyList= Display.showDependencies(nodes);
            return dependencyList;
        }
    //---------< get the scc for the selected files >----------------
        List<string> getscc(List<string> files)
        {
            List<CsNode<string, string>> nodes = new List<CsNode<string, string>>();
            List<List<Elem>> dependency = new List<List<Elem>>();
            List<string> selectedfiles = new List<string>();
            List<string> dependencyList = new List<string>();
            selectedfiles = getfilePath(files);
            TypeAnalysis typeAnalysisObj = new TypeAnalysis(selectedfiles);
            dependency = typeAnalysisObj.generateTypeTable();
            nodes = DependencyAnalysis.GetDependency(dependency, selectedfiles);
            dependencyList = Display.showDependencies(nodes);
            GraphTest gt = new GraphTest();
            List<string> sccNodes1 = gt.Tarjan(nodes);
            List<string> sccNodes = Display.showsccGraph(sccNodes1);
            return sccNodes;
        }

    static void Main(string[] args)
    {
      TestUtilities.title("Starting Navigation Server", '=');
      try
      {
        NavigatorServer server = new NavigatorServer();
        server.initializeDispatcher1();
        server.initializeDispatcher2();
        server.initializeDispatcher3();
        server.initializeDispatcher4();
        server.comm = new MessagePassingComm.Comm(ServerEnvironment.address, ServerEnvironment.port);      
        while (true)
        {
          CommMessage msg = server.comm.getMessage();
          if (msg.type == CommMessage.MessageType.closeReceiver)
            break;
          msg.show();
          if (msg.command == null)
            continue;                
          CommMessage reply = server.messageDispatcher[msg.command](msg);                    
          if (reply == null)
              continue;
          reply.show();
          server.comm.postMessage(reply);                   
        }               
      }
      catch(Exception ex)
      {
        Console.Write("\n  exception thrown:\n{0}\n\n", ex.Message);
      }
    }
  }
}
