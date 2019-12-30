using System;
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
        static void Main(string[] args)
        {
            Console.WriteLine("width : " + Console.WindowWidth.ToString() + "height : " + Console.WindowHeight.ToString());
            while (Console.WindowWidth<100||Console.WindowHeight<20)
            {
                Console.WriteLine("width : "+Console.WindowWidth.ToString()+"height : "+Console.WindowHeight.ToString());
                Console.Write("increase the windows size then press enter.\nmin width : 100 , min height : 20");
                Console.ReadLine();
            }
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
                } while (!File.Exists(fileName));
            }
            Console.Clear();
            Console.Title = "file name : " + fileName;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            if(fs.Length<=999999999)
            {
                long k = 0;
                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    //Console.Write(fixSize(k.ToString(), 9));
                    fs.Position = k;
                    for (int l = 0; l < Console.WindowHeight; l++)
                    {
                        Console.Write(fixSize(fs.Position.ToString(), 9)+":");
                        for (int i = 0; i < 30; i++)
                        {
                            if (fs.Position < fs.Length)
                                Console.Write(fixSize(hex[fs.ReadByte()], 3));
                            else
                                break;
                        }
                        if (l < Console.WindowHeight - 1)
                            Console.WriteLine();
                    }
                    ConsoleKeyInfo ck = Console.ReadKey(true);
                    if (ck.Key == ConsoleKey.DownArrow)
                    {
                        k += 90;
                        if (k > fs.Length-1)
                            k = fs.Length - 1;
                    }
                    else if (ck.Key == ConsoleKey.UpArrow)
                    {
                        k -= 90;
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
                        Console.Write("Enter byte number : ");
                        long bn = 0;
                        if (long.TryParse(Console.ReadLine(), out bn))
                            if (bn < fs.Length && bn > -1)
                            {

                                k = bn;
                            }
                    }
                }
            }
            else
            {
                Console.WriteLine("file is very big");
                Console.ReadLine();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        static string menu(string[] items,string title)
        {
            Console.Clear();
            for (int i = 0; i < items.Length; i++)
                Console.WriteLine(items[i]);
            int selected = -1;
            while (true)
            {
                ConsoleKeyInfo ck = Console.ReadKey(true);
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected > items.Length)
                        selected = 0;
                }
                else if (ck.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected <0)
                        selected = items.Length;
                }
                else if (ck.Key == ConsoleKey.Enter)
                {
                    if (selected == -1)
                        return "";
                    else return items[selected];
                }
                Console.WriteLine(title);
                for (int i = 0; i < items.Length; i++)
                {
                    if (selected == i)
                        Console.Write("#");
                    Console.WriteLine(items[i]);
                }
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
