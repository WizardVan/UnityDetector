using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

public class DebuggerDetector : MonoBehaviour {


    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool IsDebuggerPresent();
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetCurrentThread();
    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern int NtSetInformationThread(IntPtr threadHandle, int threadInformationClass, IntPtr threadInformation, int threadInformationLength);
    [DllImport("ntdll.dll", SetLastError = true)]
    static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, IntPtr processInformation, uint processInformationLength,IntPtr returnLength);
    
    private bool Dflag;
    private IntPtr NoDebugInherit = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(UInt32)));
    private long status;
    private long status2;
    private IntPtr hDebugObject = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)));
	
    void Start () {
        Dflag = false;
	}
	
	// Update is called once per frame
	void Update () {
        //method 1:
        NtSetInformationThread(GetCurrentThread(), 0x11, IntPtr.Zero, 0);
        //UnityEngine.Debug.Log();
        //UnityEngine.Debug.Log(GetCurrentThread().ToString());
        
        //method 2: don't attach me (even you are a good debugger)
        bool isDebuggerPresent = false;
        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
        
        if (isDebuggerPresent || IsDebuggerPresent())
        {
            //Console.WriteLine("Debugger Attached: " + isDebuggerPresent + "\n");
            //Console.ReadLine();
            Dflag = true;
        }
        //method 3: it is what I said in 2 (this is for .net debugger XD)
        if (System.Diagnostics.Debugger.IsAttached)
        {
            //Console.Write("We are being fucked by .net debugger!!\n");
            Dflag = true;
        }
        //method4:
        status = NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 0x1f, NoDebugInherit, 4, IntPtr.Zero);
        if (((uint)Marshal.PtrToStructure(NoDebugInherit, typeof(uint)))==0)
        {
            Dflag = true;
        }
        //UnityEngine.Debug.Log(status.ToString());
        //UnityEngine.Debug.Log(NoDebugInherit.ToString());
        status2 = NtQueryInformationProcess(Process.GetCurrentProcess().Handle, 0x1e, hDebugObject, 4, IntPtr.Zero);
        if(status2==0)
        {
            Dflag = true;
        }
        //UnityEngine.Debug.Log(status.ToString());
        //UnityEngine.Debug.Log(hDebugObject.ToString());
       
	}
    void OnGUI()
    {
        GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2-100, 150, 30), status.ToString());
        GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 - 50, 150, 30), status2.ToString());
        GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2, 150, 30), ((uint)Marshal.PtrToStructure(NoDebugInherit,typeof(uint))).ToString());
        GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 + 50, 150, 30), ((IntPtr)Marshal.PtrToStructure(hDebugObject, typeof(IntPtr))).ToString());
        if (Dflag == true)
        {
            GUI.TextField(new Rect(Screen.width / 2, Screen.height / 2 +100, 150, 30), "Hacked");
        }
    }
}
