using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace debugdetector
{
    class Mydetector
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
        void Start()
        {
            //method 1: blacklist(sometimes evil thing is not only debugger)
            Process[] localByName = Process.GetProcessesByName("OLLYDBG");
            if (localByName != null && localByName.Length>0)
            {
                Console.Write("We are being fucked!!Ollydbg is so good\n");

            }

            //method 2: don't attach me (even you are a good debugger)
            bool isDebuggerPresent = false;
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            if (isDebuggerPresent)
            {
                Console.WriteLine("Debugger Attached: " + isDebuggerPresent+"\n");
                Console.ReadLine();
            }
            //method 3: it is what I said in 2 (this is for .net debugger XD)
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("We are being fucked by .net debugger!!\n");
            }

        }

        static void Main(string[] args)
        {
            Thread.Sleep(10000);
            Mydetector mydetector = new Mydetector();
            mydetector.Start();
            Console.ReadLine();
        }
    }
}
