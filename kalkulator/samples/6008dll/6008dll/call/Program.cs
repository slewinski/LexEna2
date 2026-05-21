using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace call
{
    class Program
    {
        [DllImport(@"..\..\..\Debug\OdsetkiDll.dll"
        , EntryPoint = @"?fnMy6008dll@@YAHHH@Z"
        , CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnMy6008dll(int a, int b);


        [DllImport(@"..\..\..\Debug\OdsetkiDll.dll"
       , EntryPoint = @"?HelloWorld@@YAPADPAD@Z"
       , CallingConvention = CallingConvention.Cdecl)]
        public static extern string HelloWorld(char[] name);

        static void Main(string[] args)
        {
            int ret;
            string myString;
            char[] tab = new char [100];
            Console.WriteLine("1+8 = ?");
            ret = fnMy6008dll(1,8);
            Console.WriteLine(ret.ToString());
            tab = "Wacek  - ".ToCharArray(); 
            myString = HelloWorld(tab);
            Console.WriteLine(myString);
          // dump eksportów z dll'a  dumpbin.exe /EXPORTS c:\windows\system32\kernel32.dll


            Console.ReadKey();
        }
    }
}
