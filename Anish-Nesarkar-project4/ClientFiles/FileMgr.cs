/////////////////////////////////////////////////////////////////////
// FileMgr - provides file and directory handling for navigation   //
// ver 1.0                                                         //
// Jim Fawcett, CSE681 - Software Modeling and Analysis, Fall 2017 //
/////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NavigatorClient
{
  public struct ClientEnvironment
  {
    public static string localRoot { get; set; } = "../../../ClientFiles/";
  }

  public struct ServerEnvironment
  {
    public static string remoteRoot { get; set; } = "../../../ServerFiles/";
  }

  public enum FileMgrType { Local, Remote }

  ///////////////////////////////////////////////////////////////////
  // NavigatorClient uses only this interface and factory

  public interface IFileMgr
  {
    IEnumerable<string> getFiles();
    IEnumerable<string> getDirs();
    bool setDir(string dir);
    Stack<string> pathStack { get; set; }
    string currentPath { get; set; }
  }

  public class FileMgrFactory
  {
    static public IFileMgr create(FileMgrType type)
    {
      if (type == FileMgrType.Local)
        return new LocalFileMgr();
      else
        return null;  // eventually will have remote file Mgr
    }
  }

  ///////////////////////////////////////////////////////////////////
  // Concrete class for managing local files

  public class LocalFileMgr : IFileMgr
  {
    public string currentPath { get; set; } = "";
    public Stack<string> pathStack { get; set; } = new Stack<string>();

    public LocalFileMgr()
    {
      pathStack.Push(currentPath);  // stack is used to move to parent directory
    }
    //----< get names of all files in current directory >------------

    public IEnumerable<string> getFiles()
    {
      List<string> files = new List<string>();
      string path = Path.Combine(ClientEnvironment.localRoot, currentPath);
      string absPath = Path.GetFullPath(path);
      files = Directory.GetFiles(path).ToList<string>();
      for(int i=0; i<files.Count(); ++i)
      {
        files[i] = Path.Combine(currentPath, Path.GetFileName(files[i]));
      }
      return files;
    }
    //----< get names of all subdirectories in current directory >---

    public IEnumerable<string> getDirs()
    {
      List<string> dirs = new List<string>();
      string path = Path.Combine(ClientEnvironment.localRoot, currentPath);
      dirs = Directory.GetDirectories(path).ToList<string>();
      for (int i = 0; i < dirs.Count(); ++i)
      {
        string dirName = new DirectoryInfo(dirs[i]).Name;
        dirs[i] = Path.Combine(currentPath, dirName);
      }
      return dirs;
    }
    //----< sets value of current directory - not used >-------------

    public bool setDir(string dir)
    {
      if (!Directory.Exists(dir))
        return false;
      currentPath = dir;
      return true;
    }
  }

  class TestFileMgr
  {
    static void Main(string[] args)
    {
    }
  }
}
