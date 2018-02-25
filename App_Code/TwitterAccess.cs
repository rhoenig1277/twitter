using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using twitter.Models;

namespace twitter.App_Code
{
    public class TwitterAccess
    {
        private Dataaccess da;

        public TwitterAccess()
        {
            da = new Dataaccess();
        }

        public List<TweetsModel> GetTwitterData(string Query)
        {
            List<TweetsModel> rtnResponse = new List<TweetsModel>();
            rtnResponse = da.Select("Select * from twittercount");
            
            return rtnResponse;
        }

        public void SetTwitterData(string strTerm1, int intCount1, string strTerm2, int intCount2)
        {
            System.Guid tweetID = Guid.NewGuid();
            da.Insert("Insert into `mysqldatabase28603`.`twittercount` (`id`,`searchTerm1`,`searchTerm1Count`,`searchTerm2`,`searchTerm2Count`,`create_date`) VALUES ('"+ tweetID.ToString() +"','" + strTerm1+"','"+ intCount1 + "','"+ strTerm2+"','"+ intCount2+"','"+DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "')");
        }

        public int GetTweetCount(string strTerm)
        {
            string content = "";
            int tweetCount = 0;
            var AuthString = "OAuth oauth_consumer_key =\"" + System.Configuration.ConfigurationManager.AppSettings["oauth_consumer_key"] + "\",oauth_token=\"" + System.Configuration.ConfigurationManager.AppSettings["oauth_token"] + "\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1519445443\",oauth_nonce=\"Ho5QXNHTwM8\",oauth_version=\"1.0\",oauth_signature=\"wXsv1LeMO5Aor92%2BYxjJUG%2FmCJU%3D\"";
            //"OAuth oauth_consumer_key=\"KJEQsy0Rtc5g4fFu8WIIj781E\",oauth_token=\"17330945-FoFchUUjP2MyHcuLo2BNUgx54l3zU8EfVNr4zhGqF\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1519445443\",oauth_nonce=\"Ho5QXNHTwM8\",oauth_version=\"1.0\",oauth_signature=\"wXsv1LeMO5Aor92%2BYxjJUG%2FmCJU%3D\"");

            var client1 = new RestClient(strTerm);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("Cache-Control", "no-cache");
            request1.AddHeader("Authorization", AuthString);
            IRestResponse response1 = client1.Execute(request1);
            content = response1.Content; // raw content as string
            var serializer = new JavaScriptSerializer();
            dynamic tweets1 = serializer.Deserialize<object>(content);
            int strTweetCount = tweets1["statuses"];

            return tweetCount;
        }
    }
}