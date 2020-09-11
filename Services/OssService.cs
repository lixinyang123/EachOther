using System.IO;
using System.Web;
using Aliyun.OSS;
using EachOther.Models;

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

        public string UploadCover(string fileName, Stream stream)
        {
            string path = "cover/";
            client.PutObject(config.BucketName, path + fileName, stream);
            return config.BucketDomainName + path + HttpUtility.UrlEncode(fileName);
        }

        public string UploadPic(string fileName, Stream stream)
        {
            string path = "images/";
            client.PutObject(config.BucketName, path + fileName, stream);
            return config.BucketDomainName + path + HttpUtility.UrlEncode(fileName);
        }
    }
}