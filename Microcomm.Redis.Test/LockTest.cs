using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using StackExchange.Redis;

namespace Microcomm.Redis.Test
{
    
   

    public static class LockTest
    {
        private static IDatabase database;
        private const string KEY = "string_lock_key";
        private static RedisValue LockToken = Environment.MachineName;
        private static object lockvalue = new object();

        static LockTest()
        {
           var conn = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisServer"]);
           database =conn.GetDatabase(0);

            if (!database.KeyExists(KEY))
                database.StringSet(KEY,0);
        }

        public static void Sync()
        {

            for (int i = 0; i < 100; i++)
            {
                var currValue = int.Parse(database.StringGet(KEY));
                Console.WriteLine($"current value is {currValue}");
                currValue += 1;
                database.StringSet(KEY, currValue);
            }
        
        }

        public static void AsyncWithNoLock()
        {

            Parallel.For(0, 100, i =>
              {
                  var currValue = int.Parse(database.StringGet(KEY));                
                  currValue += 1;
                  var result= database.StringSet(KEY, currValue,null,When.Exists);
                  Print($"current value is {currValue}",result?ConsoleColor.White:ConsoleColor.Red);
              });
            
        }

        public static void AsyncWithRedisLock()
        {

            Parallel.For(0, 100, i =>
            {
                if (database.LockTake(KEY, LockToken,TimeSpan.FromMilliseconds(1)))
                {
                    try
                    {


                        var currValue = int.Parse(database.StringGet(KEY));
                        currValue += 1;
                        var result = database.StringSet(KEY, currValue);
                        Print($"current value is {currValue}", result ? ConsoleColor.White : ConsoleColor.Red);
                    }
                    finally
                    {
                        database.LockRelease(KEY,LockToken);
                    }
                }

            });

            Console.WriteLine("finish");

        }

        private static void DoAsyncWithNativeLock()
        {
            lock (lockvalue)
            {
            
                var currValue = int.Parse(database.StringGet(KEY))+1;
                var result = database.StringSet(KEY, currValue);
                Print($"current value is {currValue}", result ? ConsoleColor.White : ConsoleColor.Red);
            }
        }


        public static void AsyncWithNativeLock()
        {

            Parallel.For(0, 100, i =>
            {
             
                DoAsyncWithNativeLock();
            });

            Console.WriteLine("finish");

        }

        private static void Print(string text,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
