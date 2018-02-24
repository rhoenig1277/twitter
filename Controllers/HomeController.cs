using System;
using System.Collections.Generic;
using System.Web.Mvc;
using twitter.App_Code;
using twitter.Models;

namespace twitter.Controllers
{
    public class HomeController : Controller
    {
        private TwitterAccess twitterAccess;

        public ActionResult Index()
        {
            string strSearchOne = "Cool Runnings";
            string strSearchTwo = "Bobsled Jamaica";
            int tweetCountTerm1 = 0;
            int tweetCountTerm2 = 0;
            List<TweetsModel> rtnResponse = new List<TweetsModel>();
            string searchQ1 = System.Configuration.ConfigurationManager.AppSettings["twitterAPI"] + strSearchOne;
            string searchQ2 = System.Configuration.ConfigurationManager.AppSettings["twitterAPI"] + strSearchTwo;

            try
            {
                tweetCountTerm1 = GetTweets(searchQ1);
                tweetCountTerm2 = GetTweets(searchQ2);

                SetTweets(strSearchOne, tweetCountTerm1, strSearchTwo, tweetCountTerm2);

                rtnResponse = GetTweetList();
            }
            catch (Exception ex)
            {
                string strError = "";
                strError = ex.ToString();
            }

            TweetsModel Model = new TweetsModel { tweetsCount = rtnResponse };

            Model.searchTerm1 = strSearchOne;
            Model.searchTermCount1 = tweetCountTerm1.ToString();
            Model.searchTerm2 = strSearchTwo;
            Model.searchTermCount2 = tweetCountTerm2.ToString();
            return View(Model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public void SetTweets(string strTerm1, int intCount1, string strTerm2, int intCount2)
        {
            twitterAccess = new TwitterAccess();

            twitterAccess.SetTwitterData(strTerm1, intCount1, strTerm2, intCount2);
        }

        public int GetTweets(string strTerm)
        {
            int tweetCountTerm1;
            twitterAccess = new TwitterAccess();

            tweetCountTerm1 = twitterAccess.GetTweetCount(strTerm);
            return tweetCountTerm1;
        }

        public List<TweetsModel> GetTweetList()
        {
            List<TweetsModel> rtnResponse = new List<TweetsModel>();

            twitterAccess = new TwitterAccess();

            rtnResponse = twitterAccess.GetTwitterData("");

            return rtnResponse;
        }
    }
}