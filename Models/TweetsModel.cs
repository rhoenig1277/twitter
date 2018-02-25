using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace twitter.Models
{
    public class TweetsModel
    {

        public string tweetID { get; set; }
        public string searchTerm1 { get; set; }
        public int searchTermCount1 { get; set; }
        public string searchTerm2 { get; set; }
        public int searchTermCount2 { get; set; }
        public IList<TweetsModel> tweetsCount { get; set; }
        public bool showTweets { get; set; }
    }
}