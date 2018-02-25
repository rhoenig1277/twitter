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
            TweetsModel Model = new TweetsModel { tweetsCount = rtnResponse };

            if (strSearchOne != "")
            { 
                tweetCountTerm1 = GetTweets(strSearchOne);
            }
            else
            {
                Model.strError = "Term 1 box needs to be populated.";
            }

            if (strSearchTwo != "")
            {
                tweetCountTerm2 = GetTweets(strSearchTwo);
            }
            else
            {
                if (Model.strError != null && Model.strError != "")
                {
                    Model.strError += "<br>";
                }
                Model.strError += "Term 2 box needs to be populated.";
            }

            if (Model.strError != null && Model.strError != "")
            {
                return View(Model);
            }

            bool blnSuccess = SetTweets(strSearchOne, tweetCountTerm1, strSearchTwo, tweetCountTerm2);

            rtnResponse = GetTweetList();

            Model.searchTerm1 = strSearchOne;
            Model.searchTermCount1 = tweetCountTerm1;
            Model.searchTerm2 = strSearchTwo;
            Model.searchTermCount2 = tweetCountTerm2;
            Model.showTweets = true;
            Model.tweetsCount = rtnResponse;

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

        public bool SetTweets(string strTerm1, int intCount1, string strTerm2, int intCount2)
        {
            twitterAccess = new TwitterAccess();

            return twitterAccess.SetTwitterData(strTerm1, intCount1, strTerm2, intCount2);
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