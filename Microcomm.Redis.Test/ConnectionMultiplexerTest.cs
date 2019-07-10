using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using StackExchange.Redis;
using System.Configuration;

namespace Microcomm.Redis.Test
{

     

    public static class ConnectionMultiplexerTest
    {


        public static void Connect()
        {
            var connStr = ConfigurationManager.AppSettings["RedisServer"];
            var instance= ConnectionMultiplexer.Connect(connStr, Console.Out);
            Console.WriteLine(instance?.ClientName);
        }
    }
}
