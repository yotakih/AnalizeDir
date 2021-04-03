using System;
using System.Collections.Generic;
using System.IO;

namespace AnalizeDir
{
    class Program
    {
        const string DLM = "\t";
        string RootDir = "";
        string AllExtFile = "";
        string ExtDirFile = "";
        HashSet<string> AllExtSet = new HashSet<string>();
        HashSet<string> ExtSet = new HashSet<string>();
        StreamWriter SwAllExt;
        StreamWriter SwExt;
        static void Main(string[] args)
        {
            Console.WriteLine($"current dir:{System.Environment.CurrentDirectory}");
            new Program().Proc(args);
        }
        void Proc(string[] args)
        {
            try
            {
                if (!this.AnalizeArgs(args))
                    return;
                var rootdir = this.RootDir;
                if (!Directory.Exists(rootdir))
                {
                    Console.WriteLine($"not found: {rootdir}");
                    return;
                }
                this.SwAllExt = new StreamWriter(this.AllExtFile, false);
                this.SwExt = new StreamWriter(this.ExtDirFile, false);
                this.Analize(rootdir, new Action<string>((dir) => {
                    foreach (var ext in this.ExtSet)
                    {
                        this.SwExt.WriteLine(String.Format("{1}{0}{2}", DLM, dir, ext));
                    }
                }));
                foreach (var ext in this.AllExtSet)
                {
                    this.SwAllExt.WriteLine(ext);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
            }
            finally
            {
                if (!(this.SwAllExt is null))
                    this.SwAllExt.Close();
                if (!(this.SwExt is null))
                    this.SwExt.Close();
            }
        }
        bool AnalizeArgs(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    PrintUses();
                    return false;
                case 1:
                    this.RootDir = args[0];
                    this.AllExtFile = "allExt.txt";
                    this.ExtDirFile = "ext.txt";
                    return true;
                case 2:
                    PrintUses();
                    return false;
                case 3:
                    this.RootDir = args[0];
                    this.AllExtFile = args[1];
                    this.ExtDirFile = args[2];
                    return true;
                default:
                    PrintUses();
                    return false;
            }
        }
        void PrintUses()
        {
            Console.WriteLine(
                    $"Usege:\n" +
                    $" args1: select analize rootdir\n" +
                    $" args2: select all ext filepath\n" +
                    $" args3: select ext filepath\n"
                );
        }
        void Analize(string dir, Action<string> actExt)
        {
            foreach (var fl in Directory.GetFiles(dir))
            {
                var ext = Path.GetExtension(fl);
                this.AllExtSet.Add(ext.ToLower());
                this.ExtSet.Add(ext.ToLower());
            }
            actExt(dir);
            this.ExtSet.Clear();
            foreach (var dr in Directory.GetDirectories(dir))
            {
                this.Analize(dr, actExt);
            }
        }
    }
}
