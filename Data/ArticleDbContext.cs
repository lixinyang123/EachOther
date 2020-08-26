using StackExchange.Redis;

namespace EachOther.Data
{
    public class ArticleDbContext
    {
        public IDatabase database;
        public readonly string key = "articles";

        public ArticleDbContext()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            database = redis.GetDatabase();
        }
    }
}