using System;
using System.Collections.Generic;
using System.Linq;
using twitter.Models;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Google.Maps;
using Google.Maps.Geocoding;

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

        public bool SetTwitterData(TweetsModel tweetModel)
        {                        
            return da.Insert("putTweetSearch", tweetModel);
        }

        public TweetsModel GetTweetCount(string strTerm, string addressFrom, string proximity)
        {
            TweetsModel tweetModel = new TweetsModel();
            string oauth_consumer_key = System.Configuration.ConfigurationManager.AppSettings["oauth_consumer_key"];
            string oauth_consumer_secret = System.Configuration.ConfigurationManager.AppSettings["oauth_consumer_secret"];
            string oauth_access_token = System.Configuration.ConfigurationManager.AppSettings["oauth_access_token"];
            string oauth_token_secret = System.Configuration.ConfigurationManager.AppSettings["oauth_token_secret"];
            string longitude = "";
            string latitude = "";
            tweetModel.strError = "";

            string rtnMessage = GetCoordinates(ref longitude, ref latitude, addressFrom);

            if (rtnMessage != "success")
            {
                tweetModel.strError = rtnMessage;

                return tweetModel;
            } else
            {
                try {
                    // Create a new set of credentials for the application.
                    var appCredentials = new TwitterCredentials(oauth_consumer_key, oauth_consumer_secret, oauth_access_token, oauth_token_secret);

                    // Use the user credentials in your application
                    Auth.SetCredentials(appCredentials);

                    var searchParameter = new SearchTweetsParameters(strTerm)
                    {
                        //GeoCode = new GeoCode(42.523506, -89.01766, Convert.ToDouble(proximity), DistanceMeasure.Miles),
                        GeoCode = new GeoCode(Convert.ToDouble(latitude), Convert.ToDouble(longitude), Convert.ToDouble(proximity), DistanceMeasure.Miles),
                        MaximumNumberOfResults = 5000
                    };

                    // Simple Search
                    var tweets = Search.SearchTweets(searchParameter);

                    if (tweets.Count() == 0)
                    {
                        tweetModel.searchTermCount1 = tweets.Count();
                    } else
                    {
                        List<DateTime> tweetDateList = new List<DateTime>();

                        foreach (ITweet itm in tweets.ToList())
                        {
                            tweetDateList.Add(Convert.ToDateTime(itm.CreatedAt));
                        }

                        tweetDateList.Sort();
                        DateTime tweetDate = new DateTime();
                        tweetDate = tweetDateList.First();

                        double minDiff = (DateTime.Now - tweetDate).TotalMinutes;
                        double hoursDiff = (DateTime.Now - tweetDate).TotalHours;
                        int avgPerMin = (int)minDiff / (int)tweets.Count();
                        int avgPerHour = (int)hoursDiff / (int)tweets.Count();

                        tweetModel.tweetsPerhr = avgPerHour;
                        tweetModel.tweetsPerMin = avgPerMin;
                        tweetModel.searchTermCount1 = tweets.Count();
                    }
                    
                    // Return the count of tweets
                    return tweetModel;
                }
                catch (Exception ex)
                {
                    string strError = "";
                    strError = ex.ToString();
                    tweetModel.strError = strError;
                    return tweetModel;
                }
            }
        }

        public string GetCoordinates(ref string longitude, ref string latitude, string addressFrom)
        {
            string google_api_key = System.Configuration.ConfigurationManager.AppSettings["google_api_key"];
            string strMessage = "";

            try
            {
                //always need to use YOUR_API_KEY for requests.  Do this in App_Start.
                GoogleSigned.AssignAllServices(new GoogleSigned(google_api_key));

                var request = new GeocodingRequest();
                request.Address = addressFrom;
                var response = new GeocodingService().GetResponse(request);

                //The GeocodingService class submits the request to the API web service, and returns the
                //response strongly typed as a GeocodeResponse object which may contain zero, one or more results.

                //Assuming we received at least one result, let's get some of its properties:
                if (response.Status == ServiceResponseStatus.Ok && response.Results.Count() > 0)
                {
                    var result = response.Results.First();

                    longitude = result.Geometry.Location.Longitude.ToString();
                    latitude = result.Geometry.Location.Latitude.ToString();

                    return "success";
                }
                else
                {
                    throw new System.ArgumentException();
                }
                //End Google Geocoding
            }
            catch (Exception ex)
            {
                string strError = "";
                strError = "No Longitude/Latitude Found for " + addressFrom + ". Please enter a valid City, State.";
                return strError;
            }
        }
    }
}