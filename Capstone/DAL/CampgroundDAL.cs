using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class CampgroundDAL
    {
        private string connectionString;
        private const string SQL_TotalCampGrounds = "select * from campground where park_id = @parkId";


        public CampgroundDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Campground GetCampground(int campgroundId)
        {
            Campground campground = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "SELECT * FROM campground WHERE campground_id = @campgroundId";
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Connection = connection;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground camp = new Campground();
                        camp.ParkId = Convert.ToInt32(reader["park_id"]);
                        camp.Name = Convert.ToString(reader["name"]);
                        camp.OpenFromMM = Convert.ToInt32(reader["open_from_mm"]);
                        camp.DailyFee = Convert.ToDouble(reader["daily_fee"]);
                        camp.OpenToMM = Convert.ToInt32(reader["open_to_mm"]);
                        camp.CampgroundId = Convert.ToInt32(reader["campground_id"]);

                        campground = camp;

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return campground;
        }


        public List<Campground> GetAllCampgrounds(int parkId)
        {
            List<Campground> campground = new List<Campground>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_TotalCampGrounds;
                    cmd.Parameters.AddWithValue("@parkId", parkId);
                    cmd.Connection = connection;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground camp = new Campground();
                        camp.ParkId = Convert.ToInt32(reader["park_id"]);
                        camp.Name = Convert.ToString(reader["name"]);
                        camp.OpenFromMM = Convert.ToInt32(reader["open_from_mm"]);
                        camp.DailyFee = Convert.ToDouble(reader["daily_fee"]);
                        camp.OpenToMM = Convert.ToInt32(reader["open_to_mm"]);
                        camp.CampgroundId = Convert.ToInt32(reader["campground_id"]);

                        campground.Add(camp);

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return campground;
        }
    }
}
