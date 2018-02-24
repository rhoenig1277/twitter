using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace twitter.App_Code
{
    public class Dataaccess
    {
        private MySqlConnection connection;
  
        //Constructor
        public Dataaccess()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            connection = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["twitterConn"].ConnectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        ////Insert statement
        public void Insert(string query)
        {
            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
        
        ////Select statement
        public List<twitter.Models.TweetsModel> Select(string query)
        {
            List<twitter.Models.TweetsModel> rtnResponse = new List<twitter.Models.TweetsModel>();

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                //cmd.ExecuteNonQuery();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        twitter.Models.TweetsModel tweetModel = new twitter.Models.TweetsModel();
                        tweetModel.searchTerm1 = reader.GetString(reader.GetOrdinal("searchTerm1")).ToString();
                        tweetModel.searchTermCount1 = reader.GetString(reader.GetOrdinal("searchTerm1Count")).ToString();
                        tweetModel.searchTerm2 = reader.GetString(reader.GetOrdinal("searchTerm2")).ToString();
                        tweetModel.searchTermCount2 = reader.GetString(reader.GetOrdinal("searchTerm2Count")).ToString();
                        rtnResponse.Add(tweetModel);
                    }
                }

                //close connection
                this.CloseConnection();
            }

            return rtnResponse;
        }
    }
}