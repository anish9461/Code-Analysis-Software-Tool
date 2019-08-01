/////////////////////////////////////////////////////////////////////
// FileMgr - provides file and directory handling for navigation   //
// ver 1.0                                                              //
// Language:    C#, 2017, .Net Framework 4.7.1                          //
// Platform:    Dell Precision T8900, Win10                             //
// Anish Nesarkar , CSE 681 - Software Modelling and Analysis, Fall2018 //
// Source Author:      Jim Fawcett, CST 4-187, Syracuse University      //
//////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package defines IFileMgr interface, FileMgrFactory, and LocalFileMgr
 * classes.  Clients use the FileMgrFactory to create an instance bound to
 * an interface reference.
 * 
 * The FileManager finds files and folders at the root path and in any
 * subdirectory in the tree rooted at that path.
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
using System.IO;

namespace Navigator
{
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
      string path = Path.Combine(Environment.root, currentPath);
      string absPath = Path.GetFullPath(path);
            string fileFormat = "cs";
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles("*." + fileFormat))
            {
                files.Add(Path.Combine(currentPath,file.Name));
                
            }
        
            return files;
    }
    //----< get names of all subdirectories in current directory >---

    public IEnumerable<string> getDirs()
    {
      List<string> dirs = new List<string>();
      string path = Path.Combine(Environment.root, currentPath);
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
