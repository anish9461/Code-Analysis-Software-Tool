//////////////////////////////////////////////////////////////////////////
// NavigatorClient.xaml.cs - Demonstrates WPF for CodeAnalysis Tool     //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines WPF application processing by the client.  The client 
 * first tries to connect to the server and then navigates the folders in the server
 * to select the files required for code analysis. The server performs the code analysis
 * and gives back typtable result, dependency result and strong components result back to the client.
 * 
 * Required files
 * -------------------------
 * Environment.cs
 * FileMgr.cs
 * IMessagePassingCommService.cs
 * MessagePassingCommService.cs
 * TestUtilities.cs
 * 
 * Public Interface Documentation
 * ================================
 * public partial class MainWindow : Window
 * public MainWindow()
 * public void initializeEnvironment()
 * public void initializeMessageDispatcher()
 * public void rcvThreadProc()
 * private void Window_Closed(object sender, EventArgs e)
* private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
* public void get_files()
* public void upFolder()
* public void inFolder()
* public void connectServ()
* public void get_typetable()
* public void analyze()
* public void get_dependency()
* public void get_Scc()
* public void add_files()
* public void clear_list()
* public void automate()
* public void add_files_auto()
* 
 * ver 1.0 : 05 Dec 2018
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Threading;
using MessagePassingComm;
using System.Collections.ObjectModel;
using System.Globalization;


namespace Navigator
{

    public partial class MainWindow : Window
    {

        private IFileMgr fileMgr { get; set; } = null;  
        Comm comm { get; set; } = null;
        Dictionary<string, Action<CommMessage>> messageDispatcher = new Dictionary<string, Action<CommMessage>>();
        Thread rcvThread = null;
        List<string> x = new List<string>();

        //<----------  This constructor opens up the GUI ----------->

        public MainWindow()
        {
           // automate();
            //GetDependency.IsEnabled = false;
            //Typetable.IsEnabled = false;
            //GetScc.IsEnabled = false;
            //Add_Files.IsEnabled = false;
            //Analyze.IsEnabled = false;
            //Clear.IsEnabled = false;
            //RemoteUp.IsEnabled = true;
            //remoteDirs.IsEnabled = false;
            initializeEnvironment();
            Console.Title = "Code Analyzer Client";
            fileMgr = FileMgrFactory.create(FileMgrType.Local); // uses Environment
            comm = new Comm(ClientEnvironment.address, ClientEnvironment.port);
            initializeMessageDispatcher1();
            initializeMessageDispatcher2();
            initializeMessageDispatcher3();
            rcvThread = new Thread(rcvThreadProc);
            connectServ();
            rcvThread.Start();
        }

        //----< make Environment equivalent to ClientEnvironment >-------

        public void initializeEnvironment()
        {
            Environment.root = ClientEnvironment.root;
            Environment.address = ClientEnvironment.address;
            Environment.port = ClientEnvironment.port;
            Environment.endPoint = ClientEnvironment.endPoint;
        }

        //----< define how to process each message command >-------------

        public void initializeMessageDispatcher1()
        {          
            messageDispatcher["connect"] = (CommMessage msg) =>
         {            return;};
            messageDispatcher["FilesReceived"] = (CommMessage msg) =>
            {                return; };

            messageDispatcher["SentTypeTable"] = (CommMessage msg) =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (string text in msg.arguments)
                {
                    sb.Append(text).Append("\n");
                }
                typetable.Text = sb.ToString();
                //CodePopUp popup = new CodePopUp();
                //popup.Title = "typetable";
                //popup.codeView.Text = typetable.Text;
                //popup.Show();
            };

            messageDispatcher["SentDependency"] = (CommMessage msg) =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (string text in msg.arguments)
                {
                    sb.Append(text).Append("\n");
                }
                dependency.Text = sb.ToString();
                //CodePopUp popup = new CodePopUp();
                //popup.Title = "dependency result";
                //popup.codeView.Text = dependency.Text;
                //popup.Show();
            };

            messageDispatcher["Sentscc"] = (CommMessage msg) =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (string text in msg.arguments)
                {
                    sb.Append(text).Append("\n");
                }
                scc.Text = sb.ToString();
                //CodePopUp popup = new CodePopUp();
                //popup.Title = "SCC";
                //popup.codeView.Text = scc.Text;
                //popup.Show();
            };         
        }

        public void initializeMessageDispatcher2()
        {
            messageDispatcher["moveIntoFolderFiles"] = (CommMessage msg) =>
            {
                
                remoteFiles.Items.Clear();
                foreach (string file in msg.arguments)
                {
                    remoteFiles.Items.Add(file);                    
                }                
                if (remoteFiles.Items.Count != 0)
                    Add_Files.IsEnabled = true;
                else
                    Add_Files.IsEnabled = false;
            };
            messageDispatcher["moveIntoFolderDirs"] = (CommMessage msg) =>
            {
                remoteDirs.Items.Clear();
                fileMgr.currentPath = msg.lastPath;
                foreach (string dir in msg.arguments)
                {
                    remoteDirs.Items.Add(dir);
                }
                if (remoteDirs.Items.Count != 0)
                    remoteDirs.IsEnabled = true;
                else
                    remoteDirs.IsEnabled = false;
            };
            messageDispatcher["moveOutFolderDirs"] = (CommMessage msg) =>
            {
                remoteDirs.Items.Clear();
                fileMgr.currentPath = msg.lastPath;
                foreach (string dir in msg.arguments)
                {
                    remoteDirs.Items.Add(dir);
                }
                if (remoteDirs.Items.Count != 0)
                    remoteDirs.IsEnabled = true;
                else
                    remoteDirs.IsEnabled = false;
            };
        }

        public void initializeMessageDispatcher3()
        {
            messageDispatcher["getTopFiles"] = (CommMessage msg) =>
            {
                remoteFiles.Items.Clear();
                foreach (string file in msg.arguments)
                {
                    remoteFiles.Items.Add(file);
                    x.Add(file);
                }
                if (remoteFiles.Items.Count != 0)
                    Add_Files.IsEnabled = true;
                else
                    Add_Files.IsEnabled = false;
            };
            messageDispatcher["getTopDirs"] = (CommMessage msg) =>
            {
                remoteDirs.Items.Clear();
                foreach (string dir in msg.arguments)
                {
                    remoteDirs.Items.Add(dir);
                }
                if (remoteDirs.Items.Count != 0)
                    remoteDirs.IsEnabled = true;
                else
                    remoteDirs.IsEnabled = false;
            };

        }


        //----< define processing for GUI's receive thread >-------------

        public void rcvThreadProc()
        {
            Console.Write("\n  starting client's receive thread");
            while (true)
            {
                CommMessage msg = comm.getMessage();
                msg.show();
                if (msg.command == null)
                    continue;

                // pass the Dispatcher's action value to the main thread for execution

                Dispatcher.Invoke(messageDispatcher[msg.command], new object[] { msg });
            }
        }

        //----< shut down comm when the main window closes >-------------
        private void Window_Closed(object sender, EventArgs e)
        {
            
            comm.close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            
        }

         //-------------< gives files from root directory of server >---------------
         public void get_files()
        {
            RemoteUp.IsEnabled = true;
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;            
            msg1.command = "getTopFiles";
            msg1.arguments.Add("");
            msg1.show();
            comm.postMessage(msg1);            
            CommMessage msg2 = msg1.clone();
            msg2.command = "getTopDirs";
            msg2.show();
            comm.postMessage(msg2);            
        }
        //<---------button for get files >---------------
        private void RemoteTop_Click(object sender, RoutedEventArgs e)
        {
            get_files();
        }
         
        //------------< mouse click to move inside the server direcotries >-----------
        private void remoteDirs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            inFolder();
        }

        //--------------< click to move up the folders in server >----------------
        private void RemoteUp_Click(object sender, RoutedEventArgs e)
        {
            upFolder();
        }

        //--------------< comm message to move out of folder >-------- 
        public void upFolder()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "moveOutFolderDirs";
            msg1.arguments.Add("");
            msg1.lastPath = fileMgr.currentPath;
            msg1.show();
            if (!(msg1.lastPath == ""))
            {
                comm.postMessage(msg1);
                CommMessage msg2 = msg1.clone();
                msg2.command = "moveIntoFolderFiles";
                msg2.arguments.Add(fileMgr.currentPath);
                msg2.show();
                comm.postMessage(msg2);
            }
        }

        //----------< comm message to move in the folder >------------
        public void inFolder()
        {

            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "moveIntoFolderFiles";
            msg1.arguments.Add(remoteDirs.SelectedValue as string);
            msg1.show();
            comm.postMessage(msg1);
            CommMessage msg2 = msg1.clone();
            msg2.command = "moveIntoFolderDirs";
            msg2.show();
            comm.postMessage(msg2);
        }

        //------------< comm message to connect to server >---------------
        public void connectServ()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "connect";
            msg1.show();
            comm.postMessage(msg1);
        }

        //---------< comm message to get typetable >---------------
        public void get_typetable()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "getTypeTable";
            msg1.show();
            comm.postMessage(msg1);
        }

        //----------< mouse click to request typetable from server >----
        private void Typetable_Click(object sender, RoutedEventArgs e)
        {
            get_typetable();
        }

        //-----------< function to send files to server to analyze >-----------
        public void analyze()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "SelectedFiles";
            foreach (var file in remoteFiles1.Items)
            {
                msg1.arguments.Add(file.ToString());
            }
            msg1.show();
            comm.postMessage(msg1);
            GetDependency.IsEnabled = true;
            Typetable.IsEnabled = true;
            GetScc.IsEnabled = true;
            remoteFiles1.Items.Clear();
        }

        //-----< mouse click to analyze >----------
        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            analyze();            
        }
        //-----------< comm message to get dependency result from server >----
        public void get_dependency()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "getDependency";
            msg1.show();
            comm.postMessage(msg1);
        }

        //----------< mouse click for dependency >-------------
        private void GetDependency_Click(object sender, RoutedEventArgs e)
        {
            get_dependency();
        }

        //------------< comm message to get scc result from server >-----------
        public void get_Scc()
        {
            CommMessage msg1 = new CommMessage(CommMessage.MessageType.request);
            msg1.from = ClientEnvironment.endPoint;
            msg1.to = ServerEnvironment.endPoint;
            msg1.command = "getScc";
            msg1.show();
            comm.postMessage(msg1);
        }

        //-----------< mouse click for scc >-------------
        private void GetScc_Click(object sender, RoutedEventArgs e)
        {
            get_Scc();
        }

        //-------------< function to add selected files >------------
        public void add_files()
        {
            foreach (string file in remoteFiles.SelectedItems)
            {
                if (!remoteFiles1.Items.Contains(file))
                {
                    remoteFiles1.Items.Add(file);
                }
            }
            if (!remoteFiles1.Items.IsEmpty)
            {
                Analyze.IsEnabled = true;
            }
            Clear.IsEnabled = true;
        }

        //-----< Add files for automate GUI >----------
        public void add_files_auto()
        {
            remoteFiles1.Items.Add("File1.cs");
            remoteFiles1.Items.Add("File2.cs");
            remoteFiles1.Items.Add("File3.cs");
            remoteFiles1.Items.Add("File4.cs");

        }

        //--------< mouse click for adding files >------------- 
        private void Add_Files_Click(object sender, RoutedEventArgs e)
        {
            add_files();
        }

        //--------------< function to clear list >---------
        public void clear_list()
        {
            while (remoteFiles1.SelectedItems.Count > 0)
            {
                remoteFiles1.Items.Remove(remoteFiles1.SelectedItems[0]);
            }
            if (remoteFiles1.Items.IsEmpty)
            {
                Analyze.IsEnabled = false;
            }
        }

        //--------< mouse click for clearing list >-----------------
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            clear_list();
        }

        //------< function to automate GUI >----------
        public void automate()
        {
            InitializeComponent();
            initializeEnvironment();
            Console.Title = "Code Analyzer Client";
            Console.WriteLine("Listening on port: " + ClientEnvironment.port);
            fileMgr = FileMgrFactory.create(FileMgrType.Local); // uses Environment
            comm = new Comm(ClientEnvironment.address, ClientEnvironment.port);
            initializeMessageDispatcher1();
            initializeMessageDispatcher2();
            initializeMessageDispatcher3();            
            rcvThread = new Thread(rcvThreadProc);
            rcvThread.Start();
            Thread.Sleep(2000);
            connectServ();
            Thread.Sleep(2000);
            get_files();
            Thread.Sleep(3000);
            add_files_auto();
            Thread.Sleep(3000);
            analyze();
            Thread.Sleep(3000);
            get_typetable();
            RemoteGetTypetable.IsSelected = true;
            Thread.Sleep(3000);
            get_dependency();
            RemoteGetDependency.IsSelected = true;
            Thread.Sleep(3000);
            get_Scc();
            RemoteGetScc.IsSelected = true;
        }
    }   
}



