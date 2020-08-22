using System;
using System.Collections.Generic;
using System.Text.Json;
using StackExchange.Redis;
using EachOther.Api.Models;

namespace EachOther.Api.Services
{
    public class ArticleService
    {
        private IDatabase database;
        private static string key = "articles";

        public ArticleService()
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            database = redis.GetDatabase();
        }

        public List<Article> GetArticles(int start, int end)
        {
            RedisValue[] values = database.ListRange(key,start,end);
            List<Article> articles = new List<Article>();
            foreach (var value in values)
            {
                articles.Add(JsonSerializer.Deserialize<Article>(value.ToString()));
            }
            return articles;
        }

        public Article GetArticle(string id)
        {
            RedisValue[] values = database.ListRange(key);
            foreach (var value in values)
            {
                Article article = JsonSerializer.Deserialize<Article>(value.ToString());
                if(article.Id == id)
                {
                    return article;
                }
            }
            return null;
        }

        public bool AddArticle(Article article)
        {
            try
            {
                string jsonStr = JsonSerializer.Serialize(article);
                database.ListRightPush(key,new RedisValue(jsonStr));
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool RemoveArticle(string id)
        {
            RedisValue[] values = database.ListRange(key);
            foreach (var value in values)
            {
                Article article = JsonSerializer.Deserialize<Article>(value.ToString());
                if(article.Id == id)
                {
                    database.ListRemove(key,value);
                    return true;
                }
            }
            return false;
        }

        public bool EditArticle(Article newArticle)
        {
            RedisValue[] values = database.ListRange(key);
            for (var i=0; i<values.Length; i++)
            {
                Article article = JsonSerializer.Deserialize<Article>(values[i].ToString());
                if(article.Id == newArticle.Id)
                {
                    database.ListSetByIndex(key,i,JsonSerializer.Serialize(newArticle));
                    return true;
                }
            }
            return false;
        }

    }
}