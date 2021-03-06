﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using twitter.Models;

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
                //Need to send this to a log
                string strError = "";
                strError = ex.ToString();
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
                //Need to send this to a log
                string strError = "";
                strError = ex.ToString();
                return false;
            }
        }

        ////Insert statement
        public bool Insert(string strQuery, TweetsModel tweetModel)
        {
            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand
                    {
                        Connection = connection,
                        CommandText = strQuery,
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@tweetID", tweetModel.tweetID);
                    cmd.Parameters["@tweetID"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@searchTerm1", tweetModel.searchTerm1);
                    cmd.Parameters["@searchTerm1"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@searchTerm1Count", tweetModel.searchTermCount1);
                    cmd.Parameters["@searchTerm1Count"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@searchTerm2", tweetModel.searchTerm2);
                    cmd.Parameters["@searchTerm2"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@searchTerm2Count", tweetModel.searchTermCount2);
                    cmd.Parameters["@searchTerm2Count"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@proximity", tweetModel.proximity);
                    cmd.Parameters["@proximity"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@addressFrom", tweetModel.addressFrom);
                    cmd.Parameters["@addressFrom"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@avgTermsPerMin1", tweetModel.avgTermsPerMin1);
                    cmd.Parameters["@avgTermsPerMin1"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@avgTermsPerMin2", tweetModel.avgTermsPerMin2);
                    cmd.Parameters["@avgTermsPerMin2"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@avgTermsPerHour1", tweetModel.avgTermsPerHour1);
                    cmd.Parameters["@avgTermsPerHour1"].Direction = ParameterDirection.Input;

                    cmd.Parameters.AddWithValue("@avgTermsPerHour2", tweetModel.avgTermsPerHour2);
                    cmd.Parameters["@avgTermsPerHour2"].Direction = ParameterDirection.Input;

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //Need to send this to a log
                string strError = "";
                strError = "Error Occured Inserting tweets:"+ ex.ToString();

                return false;
            }

            return true;
        }
        
        ////Select statement
        public List<twitter.Models.TweetsModel> Select(string strQuery)
        {
            List<twitter.Models.TweetsModel> rtnResponse = new List<twitter.Models.TweetsModel>();

            try
            {
                //open connection
                if (this.OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = connection;
                    cmd.CommandText = strQuery;
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Execute command
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            twitter.Models.TweetsModel tweetModel = new twitter.Models.TweetsModel();
                            tweetModel.searchTerm1 = "";
                            tweetModel.searchTermCount1 = 0;
                            tweetModel.searchTerm2 = "";
                            tweetModel.searchTermCount2 = 0;
                            tweetModel.proximity = "";
                            tweetModel.addressFrom = "";
                            tweetModel.avgTermsPerHour1 = 0;
                            tweetModel.avgTermsPerHour2 = 0;
                            tweetModel.avgTermsPerMin1 = 0;
                            tweetModel.avgTermsPerMin2 = 0;

                            if (!reader.IsDBNull(reader.GetOrdinal("searchTerm1")))
                            {
                                tweetModel.searchTerm1 = reader.GetString(reader.GetOrdinal("searchTerm1")).ToString();
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("searchTerm1Count")))
                            {
                                tweetModel.searchTermCount1 = int.Parse(reader.GetString(reader.GetOrdinal("searchTerm1Count")));
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("searchTerm2")))
                            {
                                tweetModel.searchTerm2 = reader.GetString(reader.GetOrdinal("searchTerm2")).ToString();
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("searchTerm2Count")))
                            {
                                tweetModel.searchTermCount2 = int.Parse(reader.GetString(reader.GetOrdinal("searchTerm2Count")));
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("proximity")))
                            {
                                tweetModel.proximity = reader.GetString(reader.GetOrdinal("proximity")).ToString();
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("addressFrom")))
                            {
                                tweetModel.addressFrom = reader.GetString(reader.GetOrdinal("addressFrom")).ToString();
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("avgTermsPerHour1")))
                            {
                                tweetModel.avgTermsPerHour2 = int.Parse(reader.GetString(reader.GetOrdinal("avgTermsPerHour2")));
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("avgTermsPerMin1")))
                            {
                                tweetModel.avgTermsPerMin1 = int.Parse(reader.GetString(reader.GetOrdinal("avgTermsPerMin1")));
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("avgTermsPerMin2")))
                            {
                                tweetModel.avgTermsPerMin2 = int.Parse(reader.GetString(reader.GetOrdinal("avgTermsPerMin2")));
                            }
                            rtnResponse.Add(tweetModel);
                        }
                    }

                    //close connection
                    this.CloseConnection();
                }

                return rtnResponse;
            }
            catch (Exception ex)
            {
                //Need to send this to a log
                string strError = "";
                strError = ex.ToString();
                
                rtnResponse = new List<twitter.Models.TweetsModel>();
                TweetsModel tweetModel = new TweetsModel();

                tweetModel.strError += "Error Occured Select tweets: "+strError;

                rtnResponse.Add(tweetModel);
                return rtnResponse;
            }
        }
    }
}