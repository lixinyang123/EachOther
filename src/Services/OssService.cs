using Aliyun.OSS;
using EachOther.Models;
using System;
using System.IO;
using System.Web;

namespace EachOther.Services
{
    public class OssService
    {
        private readonly OssClient client;
        private readonly OssConfig config;

        public OssService(OssConfig config)
        {
            client = new OssClient(config.EndPoint, config.AccessKeyId, config.AccessKeySecret);
            this.config = config;
        }

        public string UploadCover(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "covers/";
            client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path) + HttpUtility.UrlEncode(fileName);
        }

        public string UploadPic(Stream stream)
        {
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = "articles/";
            client.PutObject(config.BucketName, Path.Combine(path, fileName), stream);
            return Path.Combine(config.BucketDomainName, path) + HttpUtility.UrlEncode(fileName);
        }
    }
}