using System;

using System.Threading.Tasks;
using StackExchange.Redis;

namespace Microcomm.Redis
{
    public static class DatabaseExtension
    {
        public static RedisValue[] SortedSetRangeByPaging(this IDatabase database,RedisKey key,   int pageIndex, int pageSize=10, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return  database.SortedSetRangeByRank(key,
                  pageIndex * pageSize,
                  pageIndex * pageSize + pageSize - 1,
                  order,
                  flags
              );

        }


        public static async Task<RedisValue[]> SortedSetRangeByPagingAsync(this IDatabase database, RedisKey key, int pageIndex, int pageSize = 10, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            return await database.SortedSetRangeByRankAsync(key,
                  pageIndex * pageSize,
                  pageIndex * pageSize + pageSize - 1,
                  order,
                  flags
              );

        }

    }
}
