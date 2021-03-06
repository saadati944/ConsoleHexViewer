﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEXviewer
{
    class Program
    {
        static string[] hex = new string[256];
        static int linesPerScroll=3;
        enum encodingMode {utf8,utf16,ascii };
        static encodingMode encMode = encodingMode.utf16;
        static void Main(string[] args)
        {
            Console.WriteLine("width : " + Console.WindowWidth.ToString() + "height : " + Console.WindowHeight.ToString());
            while (Console.WindowWidth<100||Console.WindowHeight<20)
            {
                Console.WriteLine("width : "+Console.WindowWidth.ToString()+"height : "+Console.WindowHeight.ToString());
                Console.Write("increase the windows size then press enter.\nmin width : 100 , min height : 20");
                Console.ReadLine();
            }
            //menu(new string[] { "item 1","item 2","item 3","item 4" },"select an item then press enter")
            createArray();
            string fileName = "";
            if (args.Length != 0 && File.Exists(args[0])) 
            {
                fileName = args[0];
            }
            else
            {
                do
                {
                    Console.Write("Enter file name : ");
                    fileName = Console.ReadLine();
                    if (fileName.StartsWith("'")|| fileName.StartsWith("\"")) fileName = fileName.Substring(1);
                    if (fileName.EndsWith("'")|| fileName.EndsWith("\"")) fileName = fileName.Substring(0, fileName.Length - 1);
                } while (!File.Exists(fileName));
            }
            Console.Clear();
            Console.Title = "H : help, file name : " + fileName;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            if(fs.Length<=999999999)
            {
                long k = 0;
                bool exit = false;
                while (!exit)
                {
                    //Console.Write(fixSize(k.ToString(), 9));
                    fs.Position = k;
                    Console.Clear();
                    for (int l = 0; l < Console.WindowHeight; l++)
                    {
                        Console.Write(fixSize((fs.Position+1).ToString(), 9)+":");
                        byte[] by = new byte[30];
                        int rl = fs.Length - 1 - fs.Position < 30 ? (int)(fs.Length - 1 - fs.Position) : 30;
                        fs.Read(by, 0, rl);
                        for (int i = 0; i < rl; i++)
                        {
                            Console.Write(fixSize(hex[by[i]], 3));
                        }
                        if (Console.WindowWidth > 130)
                        {
                            Console.Write('|');
                            switch (encMode)
                            {
                                case encodingMode.utf8:
                                    Console.Write(replaceenter(UTF8Encoding.UTF8.GetString(by, 0, 30)));
                                    break;
                                case encodingMode.utf16:
                                    Console.Write(replaceenter(UnicodeEncoding.Unicode.GetString(by, 0, 30)));
                                    break;
                                case encodingMode.ascii:
                                    Console.Write(replaceenter(ASCIIEncoding.ASCII.GetString(by, 0, 30)));
                                    break;
                            }
                        }
                        if (l < Console.WindowHeight - 1)
                            Console.WriteLine();
                    }
                    ConsoleKeyInfo ck = Console.ReadKey(true);
                    if (ck.Key == ConsoleKey.DownArrow)
                    {
                        k += linesPerScroll*30;
                        if (k > fs.Length-1)
                            k = fs.Length - 1;
                    }
                    else if (ck.Key == ConsoleKey.UpArrow)
                    {
                        k -= linesPerScroll * 30;
                        if (k < 0)
                            k = 0;
                    }
                    else if (ck.Key == ConsoleKey.Home)
                    {
                        k = 0;
                    }
                    else if (ck.Key == ConsoleKey.End)
                    {
                        k = fs.Length - Console.WindowHeight * 30;
                        if (k < 0)
                            k = 0;
                    }
                    else if (ck.Key == ConsoleKey.PageUp)
                    {
                        k -= Console.WindowHeight * 30;
                        if (k < 0)
                            k = 0;
                    }
                    else if (ck.Key == ConsoleKey.PageDown)
                    {
                        k += Console.WindowHeight * 30;
                        if (k > fs.Length - 1)
                            k = fs.Length - 1;
                    }
                    else if (ck.Key == ConsoleKey.Tab)
                    {
                        switch (menu(new string[] {"back",  "goto" , "open another file", "lines per scroll", "Text Encoding Mode" , "Exit" },"select an item then press enter\n"))
                        {
                            case "back":

                                break;
                            case "goto":
                                Console.Clear();
                                Console.Write("Enter byte number : ");
                                long bn = 0;
                                if (long.TryParse(Console.ReadLine(), out bn))
                                {
                                    bn -= 1;
                                    if (bn < fs.Length && bn > -1)
                                    {
                                        k = bn;
                                    }
                                }
                                break;
                            case "open another file":
                                string fname = "";
                                Console.Clear();
                                do
                                {
                                    Console.Write("Enter file name : ");
                                    fname = Console.ReadLine();
                                } while (!File.Exists(fname));
                                fileName = fname;
                                fs.Close();
                                fs = new FileStream(fileName, FileMode.Open);
                                Console.Title = "press tab to open menu ,file name : " + fileName;
                                k = 0;
                                break;
                            case "lines per scroll":
                                Console.Clear();
                                Console.Write("Enter a number : ");
                                int ln = 0;
                                if (int.TryParse(Console.ReadLine(), out ln))
                                {
                                    linesPerScroll = ln;
                                }
                                break;
                            case "Text Encoding Mode":
                                string answer = menu(new string[] { "UTF-8", "ASCII","UTF-16" }, "select an item then press enter\n");
                                if (answer == "UTF-8")
                                    encMode = encodingMode.utf8;
                                else if (answer == "ASCII")
                                    encMode = encodingMode.utf8;
                                else if (answer == "UTF-16")
                                    encMode = encodingMode.utf16;
                                break;
                            case "Exit":
                                fs.Close();
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("file is very big");
                Console.ReadLine();
                fs.Close();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        static string replaceenter(string v)
        {
            string r="";
            for(int i=0;i<v.Length;i++)
            {
                r += v[i] == '\n' || v[i] == '\r' ? ' ' : v[i];
            }
            return r;
        }
        static string emptyString(int len)
        {
            string s = "";
            for (int i = 0; i < len; i++)
                s += " ";
            return s;
        }
        static int entersCount(string v)
        {
            int i = 0;
            foreach (char x in v)
                if (x == '\n')
                    i++;
            return i;
        }
        static string menu(string[] items, string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            int enterCount = entersCount(title) + 1;
            int selected = 0, last = 0;
            for (int i = 0; i < items.Length; i++)
                Console.WriteLine((i == selected ? "  -->  " : "") + items[i] + (i == selected ? "  <--" : ""));
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey(true);
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > items.Length - 1)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected < 0)
                        selected = items.Length - 1;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    if (selected == -1)
                        return "";
                    else return items[selected];
                }
                if (selected != last)
                {
                    Console.SetCursorPosition(0, enterCount + last);
                    Console.Write(emptyString(12 + items[last].Length));
                    Console.SetCursorPosition(0, enterCount + last);
                    Console.Write(items[last]);
                    Console.SetCursorPosition(0, enterCount + selected);
                    Console.Write("  -->  " + items[selected] + "  <--");
                }
                last = selected;
            }
        }
        static void createArray()
        {
            string[] vs = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            int k = 0;
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    hex[k] = vs[i] + vs[j];
                    k++;
                }
        }
        static string fixSize(string s,int size)
        {
            while (s.Length < size)
                s = " " + s;
            return s;
        }
    }
}
