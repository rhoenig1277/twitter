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
            //rtnResponse = da.Select("SELECT * FROM mysqldatabase28603.twittercount order by create_date desc;");
            rtnResponse = da.Select("getTweetCount");

            return rtnResponse;
        }

        public void SetTwitterData(string strTerm1, int intCount1, string strTerm2, int intCount2)
        {
            System.Guid tweetID = Guid.NewGuid();
            TweetsModel tweetModel = new TweetsModel();

            tweetModel.tweetID = tweetID.ToString();
            tweetModel.searchTerm1 = strTerm1;
            tweetModel.searchTerm2 = strTerm2;
            tweetModel.searchTermCount1 = intCount1;
            tweetModel.searchTermCount2 = intCount2;
                        
            //da.Insert("Insert into `mysqldatabase28603`.`twittercount` (`id`,`searchTerm1`,`searchTerm1Count`,`searchTerm2`,`searchTerm2Count`,`create_date`) VALUES ('"+ tweetID.ToString() +"','" + strTerm1+"','"+ intCount1 + "','"+ strTerm2+"','"+ intCount2+"','"+DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "')");
            da.Insert("putTweetSearch", tweetModel);
        }

        public int GetTweetCount(string strTerm)
        {
            // Create a new set of credentials for the application.
            var appCredentials = new TwitterCredentials("KJEQsy0Rtc5g4fFu8WIIj781E", "xiOFX2nfkA7FRUjdAQHSPXSKuypxNn5WGlrGDLMMmJxVWpmyDV", "17330945-FoFchUUjP2MyHcuLo2BNUgx54l3zU8EfVNr4zhGqF", "rKwgchEMFn2FY6wqyucmlLHaDxXe4K9NIFetXu6yhxC76");

            // Use the user credentials in your application
            Auth.SetCredentials(appCredentials);

            // Simple Search
            var matchingTweets = Search.SearchTweets(strTerm);

            // Return the count of tweets
            return matchingTweets.Count();
        }
    }
}