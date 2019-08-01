/////////////////////////////////////////////////////////////////////////////
// NavigatorClient.xaml.cs - Demonstrates Directory Navigation in WPF App  //
// ver 1.0                                                                 //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017         //
/////////////////////////////////////////////////////////////////////////////

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

namespace NavigatorClient
{
  public partial class MainWindow : Window
  {
    private IFileMgr fileMgr = null;  // note: Navigator just uses interface declarations

    public MainWindow()
    {
      InitializeComponent();
      fileMgr = FileMgrFactory.create(FileMgrType.Local);
      getTopFiles();
    }
    //----< show files and dirs in root path >-----------------------

    public void getTopFiles()
    {
      List<string> files = fileMgr.getFiles().ToList<string>();
      localFiles.Items.Clear();
      foreach(string file in files)
      {
        localFiles.Items.Add(file);
      }
      List<string> dirs = fileMgr.getDirs().ToList<string>();
      localDirs.Items.Clear();
      foreach(string dir in dirs)
      {
        localDirs.Items.Add(dir);
      }
    }
    //----< move to directory root and display files and subdirs >---

    private void localTop_Click(object sender, RoutedEventArgs e)
    {
      fileMgr.currentPath = "";
      getTopFiles();
    }
    //----< show selected file in code popup window >----------------

    private void localFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      string fileName = localFiles.SelectedValue as string;
      try
      {
        string path = System.IO.Path.Combine(ClientEnvironment.localRoot, fileName);
        string contents = File.ReadAllText(path);
        CodePopUp popup = new CodePopUp();
        popup.codeView.Text = contents;
        popup.Show();
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
      }
    }
    //----< move to parent directory and show files and subdirs >----

    private void localUp_Click(object sender, RoutedEventArgs e)
    {
      if (fileMgr.currentPath == "")
        return;
      fileMgr.currentPath = fileMgr.pathStack.Peek();
      fileMgr.pathStack.Pop();
      getTopFiles();
    }
    //----< move into subdir and show files and subdirs >------------

    private void localDirs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      string dirName = localDirs.SelectedValue as string;
      fileMgr.pathStack.Push(fileMgr.currentPath);
      fileMgr.currentPath = dirName;
      getTopFiles();
    }
    //----< move to root of remote directories >---------------------

    private void RemoteTop_Click(object sender, RoutedEventArgs e)
    {
      // coming soon
    }
    //----< download file and display source in popup window >-------

    private void remoteFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      // comming soon
    }
    //----< move to parent directory of current remote path >--------

    private void RemoteUp_Click(object sender, RoutedEventArgs e)
    {
      // comming soon
    }
    //----< move into remote subdir and display files and subdirs >--

    private void remoteDirs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      // comming soon
    }
  }
}
