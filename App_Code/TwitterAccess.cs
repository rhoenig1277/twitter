using System;
using System.Collections.Generic;
using System.Linq;
using twitter.Models;
using Tweetinvi;
using Tweetinvi.Models;

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
            rtnResponse = da.Select("getTweetCount");

            return rtnResponse;
        }

        public bool SetTwitterData(string strTerm1, int intCount1, string strTerm2, int intCount2)
        {
            System.Guid tweetID = Guid.NewGuid();
            TweetsModel tweetModel = new TweetsModel();

            tweetModel.tweetID = tweetID.ToString();
            tweetModel.searchTerm1 = strTerm1;
            tweetModel.searchTerm2 = strTerm2;
            tweetModel.searchTermCount1 = intCount1;
            tweetModel.searchTermCount2 = intCount2;
                        
            return da.Insert("putTweetSearch", tweetModel);
        }

        public int GetTweetCount(string strTerm)
        {
            string oauth_consumer_key = System.Configuration.ConfigurationManager.AppSettings["oauth_consumer_key"];
            string oauth_consumer_secret = System.Configuration.ConfigurationManager.AppSettings["oauth_consumer_secret"];
            string oauth_access_token = System.Configuration.ConfigurationManager.AppSettings["oauth_access_token"];
            string oauth_token_secret = System.Configuration.ConfigurationManager.AppSettings["oauth_token_secret"];

            try { 
                // Create a new set of credentials for the application.
                var appCredentials = new TwitterCredentials(oauth_consumer_key, oauth_consumer_secret, oauth_access_token, oauth_token_secret);

                // Use the user credentials in your application
                Auth.SetCredentials(appCredentials);

                // Simple Search
                var matchingTweets = Search.SearchTweets(strTerm);

                // Return the count of tweets
                return matchingTweets.Count();
            }
            catch(Exception ex)
            {
                string strError = "";
                strError = ex.ToString();
                return 0;
            }
        }
    }
}