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
            while(Console.WindowWidth<100||Console.WindowHeight<20)
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
            Console.WriteLine("file name : " + fileName);
            FileStream fs = new FileStream(fileName, FileMode.Open);
            if(fs.Length<=15360)
            {

            }
            else
            {
                Console.WriteLine("file is very big");
                Console.ReadLine();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
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
    }
}
