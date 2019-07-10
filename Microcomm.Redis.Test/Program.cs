using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Redis.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //  LockTest.AsyncWithRedisLock();
            VolatileTest.Execute();
            Console.ReadLine();
        }
    }
}
