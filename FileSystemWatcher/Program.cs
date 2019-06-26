using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace FileSystemWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            int num = 0;
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher("C:\\");
            fileSystemWatcher.Changed += ShowMessage;
            Timer stateTimer = new Timer(fileSystemWatcher.CheckFolder, num, 0, 1);
            Console.ReadLine();
        }

        public static void ShowMessage(string path, DateTime time, string state)
        {
            Console.WriteLine(path + " was " + state + " :: " + time);
        }
    }

}
