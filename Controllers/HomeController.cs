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
            TweetsModel Model = new TweetsModel();
            Model.tweetsCount = new List<TweetsModel>();
            return View(Model);
        }

        // GET: EventType
        [HttpPost, ActionName("Index")]
        public ActionResult SearchTwitterPost(string txtSearchTerm1, string txtSearchTerm2)
        {
            string strSearchOne = txtSearchTerm1;
            string strSearchTwo = txtSearchTerm2;
            int tweetCountTerm1 = 0;
            int tweetCountTerm2 = 0;
            List<TweetsModel> rtnResponse = new List<TweetsModel>();
                        
            tweetCountTerm1 = GetTweets(strSearchOne);
            tweetCountTerm2 = GetTweets(strSearchTwo);

            SetTweets(strSearchOne, tweetCountTerm1, strSearchTwo, tweetCountTerm2);

            rtnResponse = GetTweetList();

            TweetsModel Model = new TweetsModel { tweetsCount = rtnResponse };

            Model.searchTerm1 = strSearchOne;
            Model.searchTermCount1 = tweetCountTerm1;
            Model.searchTerm2 = strSearchTwo;
            Model.searchTermCount2 = tweetCountTerm2;
            Model.showTweets = true;

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