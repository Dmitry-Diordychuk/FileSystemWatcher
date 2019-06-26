using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileSystemWatcher
{
    class FileInfo
    {
        public string Path { get; set; }
        public DateTime ChangeTime { get; set; }
    }

    class FileSystemWatcher
    {
        readonly string _path;

        private List<FileInfo> FilesInfo { get; set; }

        //private List<string> FilesPath { get; set; }

        //private List<DateTime> FilesLastWriteTime { get; set; } = new List<DateTime>();

        public FileSystemWatcher(string path)
        {
            _path = path;
        }

        public void CheckFolder(Object StateInfo)
        {
            List<FileInfo> filesInfo = new List<FileInfo>();
            List<string> filesPath = Directory.GetFiles(_path).ToList();
            foreach (var path in filesPath)
            {
                filesInfo.Add(new FileInfo() { Path = path,
                                     ChangeTime = File.GetLastWriteTime(path) });
            }

            List<DateTime> filesLastWriteTime = new List<DateTime>();
            
            if (FilesInfo == null)
            {
                FilesInfo = filesInfo;
            }
            else
            {
                if(!filesInfo.SequenceEqual(FilesInfo))
                {
                    if(filesInfo.Count > FilesInfo.Count)
                    {
                        var changes = filesInfo.Where(p => !FilesInfo.Any(l => p.Path == l.Path));
                        foreach(var fileinfo in changes)
                            Changed(fileinfo.Path, DateTime.Now, "Added");
                    }
                    else if (FilesInfo.Count > filesInfo.Count)
                    {
                        var changes = FilesInfo.Where(p => !filesInfo.Any(l => p.Path == l.Path));
                        foreach(var fileinfo in changes)
                            Changed(fileinfo.Path, DateTime.Now, "Deleted");
                    }
                    else
                    {
                        var comp = FilesInfo.Where(p => !filesInfo.Any(l => p.Path == l.Path));
                        if (!comp.Any())
                        {
                            var changes = filesInfo.Where(p => FilesInfo.Any(l => p.Path == l.Path && p.ChangeTime != l.ChangeTime));
                            foreach (var fileinfo in changes)
                                Changed(fileinfo.Path, DateTime.Now, "Edited");
                        }
                        else
                            Changed(comp.First().Path, DateTime.Now, "Renamed");
                    }

                    FilesInfo = filesInfo;
                }
            }
        }

        //Делегат прикрепляемый к событию
        public delegate void FolderHandler(string path, DateTime time, string state);//callback
        //Событие 
        public event FolderHandler Changed;
    }
}
