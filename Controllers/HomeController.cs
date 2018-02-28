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

            Model.addressFrom = "Beloit, WI";
            Model.proximity = "10";

            return View(Model);
        }

        // GET: EventType
        [HttpPost, ActionName("Index")]
        public ActionResult SearchTwitterPost(string btnClearSearch, string txtSearchTerm1, string txtSearchTerm2, string txtAddressFrom, string txtProximity)
        {
            TweetsModel Model = new TweetsModel();
            if (txtProximity == "" || txtProximity == "0")
            {
                Model.strError = "Proximity must be Greater than 0 and not empty.";
            } else if (Convert.ToInt16(txtProximity) < 0)
            {
                Model.strError = "Proximity must be Greater than 0";
            }

            if (Model.strError != null && Model.strError != "")
            {
                return View(Model);
            }

            string strSearchOne = txtSearchTerm1;
            string strSearchTwo = txtSearchTerm2;
            int tweetCountTerm1 = 0;
            List<TweetsModel> rtnResponse = new List<TweetsModel>();
            TweetsModel tempModel = new TweetsModel();

            System.Guid tweetID = Guid.NewGuid();
            Model.tweetID = tweetID.ToString();
            Model.addressFrom = txtAddressFrom;
            Model.proximity = txtProximity;

            if (btnClearSearch != null && btnClearSearch.ToLower() == "clear")
            {
                return RedirectToAction("Index","Home");
            }

            if (strSearchOne != "")
            {
                tempModel = new TweetsModel();
                tempModel = GetTweets(strSearchOne, txtAddressFrom, txtProximity);
                if (tempModel.strError == null || tempModel.strError == "")
                {
                    tweetCountTerm1 = tempModel.searchTermCount1;
                    Model.searchTerm1 = strSearchOne;
                    Model.avgTermsPerHour1 = tempModel.tweetsPerhr;
                    Model.avgTermsPerMin1 = tempModel.tweetsPerMin;
                    Model.searchTermCount1 = tempModel.searchTermCount1;
                } else
                {
                    Model.strError = tempModel.strError;
                }
                
            }
            else
            {
                Model.strError = "Term 1 box needs to be populated.";
            }

            if (strSearchTwo != "")
            {
                tempModel = new TweetsModel();
                tempModel = GetTweets(strSearchTwo, txtAddressFrom, txtProximity);
                if (tempModel.strError == null || tempModel.strError == "")
                {
                    tweetCountTerm1 = tempModel.searchTermCount1;
                    Model.searchTerm2 = strSearchTwo;
                    Model.avgTermsPerHour2 = tempModel.tweetsPerhr;
                    Model.avgTermsPerMin2 = tempModel.tweetsPerMin;
                    Model.searchTermCount2 = tempModel.searchTermCount1;
                }
                else
                {
                    Model.strError = tempModel.strError;
                }
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

            // Insert Tweet Model into Database for History.
            bool blnSuccess = SetTweets(Model);

            // Get Tweets from Datbase
            rtnResponse = GetTweetList();

            Model.showTweets = true;
            Model.tweetsCount = rtnResponse;
            
            return View(Model);
        }
        
        public bool SetTweets(TweetsModel tweetModel)
        {
            twitterAccess = new TwitterAccess();

            return twitterAccess.SetTwitterData(tweetModel);
        }

        public TweetsModel GetTweets(string strTerm, string addressFrom, string proximity)
        {
            TweetsModel Model = new TweetsModel();
            twitterAccess = new TwitterAccess();

            TweetsModel tweetModel = twitterAccess.GetTweetCount(strTerm, addressFrom, proximity);
            return tweetModel;
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