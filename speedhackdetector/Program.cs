/*
 * 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Threading;

namespace speedhackdetector
{
    class speedDetector
    {
        private const int THRESHOLD = 5000000;
        private long ticksOnStart = 0;
        private long ticksOnStartVulnerable = 0;
        private int errorsCount=0;
        private const int maxFalsePositives = 3;
        public void Start()
        {
            errorsCount = 0;
            ticksOnStart = DateTime.UtcNow.Ticks;
            ticksOnStartVulnerable = Environment.TickCount * TimeSpan.TicksPerMillisecond;
            Timer t = new Timer(OnTimer, null, 0, 2000);
        }
        private void OnTimer(Object o)
        {
            long ticks = 0;
            long ticksVulnerable = 0;


            ticks = DateTime.UtcNow.Ticks;
            ticksVulnerable = Environment.TickCount * TimeSpan.TicksPerMillisecond;

           // Console.WriteLine(ticksVulnerable - ticksOnStartVulnerable);
            //Console.WriteLine(ticks - ticksOnStart);
            if (Math.Abs((ticksVulnerable - ticksOnStartVulnerable) - (ticks - ticksOnStart)) > THRESHOLD)
            {
                errorsCount++;
                if (errorsCount > maxFalsePositives)
                {
                    Console.WriteLine("Sry Baby,bad things happened!");
                }
            }
            ticksOnStart = DateTime.UtcNow.Ticks;
            ticksOnStartVulnerable = Environment.TickCount * TimeSpan.TicksPerMillisecond;
        }
        static void Main(string[] args)
        {
            speedDetector myspeedDetector = new speedDetector();
            myspeedDetector.Start();
            //Console.WriteLine("Pass maybe?");
            //Console.ReadLine();
        }
    }
}
