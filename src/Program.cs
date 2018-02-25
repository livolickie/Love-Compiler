using System;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace LoveCompiler
{
    /*
     *   Livolickie
     *   https://github.com/livolickie/
     *   http://www.youtube.com/c/HavingTeam
     *   http://overgame-studio.ru
     *   https://vk.com/having_team 
    */
    class Program
    {
        public static object RedirectStandardInput { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Author: Livolickie | github.com/livolickie");
            string mainPath = "";
            string srcPath = "";
            if (File.Exists("config.txt"))
            {
                var dirs = File.ReadAllLines("config.txt");
                if (dirs.Length == 2)
                {
                    if (Directory.GetFiles(dirs[0]).Length > 0 && Directory.GetFiles(dirs[1]).Length > 0)
                    {
                        mainPath = @"" + dirs[0] + @"\";
                        srcPath = @"" + dirs[1] + @"\";
                    }
                }
            }
            else
            {
                File.Create("config.txt");
                return;
            }
            if (mainPath == "" || srcPath == "") return;
            File.Delete(mainPath + "game.love");
            File.Delete(srcPath + "game.love");
            File.Delete(srcPath + "game.exe");
            foreach(var file in Directory.GetFiles(srcPath))
            {
                if (Path.GetExtension(file) == ".dll")
                {
                    File.Delete(file);
                }    
            }
            string lovePath = mainPath + "game.love";
            ZipFile.CreateFromDirectory(srcPath, lovePath);
            string commands = @"cd " + mainPath + @"
                copy /b love.exe+"+ mainPath +"game.love "+ srcPath +"game.exe";
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    UseShellExecute = false
                }
            };
            process.Start();
            StreamWriter wr = process.StandardInput;
            if (wr.BaseStream.CanWrite)
            {
                foreach(var line in commands.Split('\n'))
                {
                    wr.WriteLine(line);
                }
            }
            wr.Close();
            var files = Directory.GetFiles(mainPath).Where((file) => Path.GetExtension(file) == ".dll");
            foreach(var file in files)
            {
                if (!File.Exists(srcPath + Path.GetFileName(file)))
                    File.Copy(file, srcPath + Path.GetFileName(file));
            }
        }
    }
}