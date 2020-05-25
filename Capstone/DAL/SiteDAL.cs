using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    public class SiteDAL
    {
        int reservationId;
        private string connectionString;
        private const string SQL_MakeReservation = "insert into reservation (from_date, to_date, site_id , create_date, name) Values(@fromDate, @toDate, @siteId, @createDate,  @name);";
        private const string SQL_CheckAvailability = "select TOP 5 * from site where (site_id not in(select site_id from reservation where (from_date > @fromDate and from_date < @toDate) or (to_date > @fromDate and to_date < @toDate) or (from_date > @fromDate and from_date < @toDate) and (to_date > @fromDate and to_date < @toDate))) and (campground_id = @campgroundId);";
        public SiteDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int ReserveCampsite(int campgroundId, int siteId, DateTime startDate, DateTime endDate, string name)
        {

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    {
                        DateTime createDate = DateTime.Now;
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = SQL_MakeReservation;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@fromDate", startDate);
                        cmd.Parameters.AddWithValue("@toDate", endDate);
                        cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@createDate", createDate);
                        cmd.Parameters.AddWithValue("@siteId", siteId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("select reservation.reservation_id from reservation order by create_date desc;", conn);
                        reservationId = ((int)cmd.ExecuteScalar());
                        return reservationId;
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
        }

        public int GetReservationId()
        {
            return reservationId;
        }
        

        public List<Site> GetAvailableCampsites(int campgroundId, DateTime startDate, DateTime endDate)
        {

            List<Site> output = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = SQL_CheckAvailability;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@campgroundId", campgroundId);
                    cmd.Parameters.AddWithValue("@fromDate", startDate);
                    cmd.Parameters.AddWithValue("@toDate", endDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site s = new Site();
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.Site_id = Convert.ToInt32(reader["site_id"]);
                        s.Campground_id = Convert.ToInt32(reader["campground_id"]);
                        s.Site_number = Convert.ToInt32(reader["site_number"]);
                        s.Max_occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        s.Max_rv_length = Convert.ToInt32(reader["max_rv_length"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);

                        output.Add(s);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return output;
        }
    }
}
