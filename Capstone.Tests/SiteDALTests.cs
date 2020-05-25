using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Transactions;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace Capstone.Tests
{
    [TestClass]
    public class SiteDALTests
    {
        private string connectionstring = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campgrounds;User ID=te_student;Password=sqlserver1";
        TransactionScope t;

        [TestInitialize]
        public void Initialize()
        {

            t = new TransactionScope();
        }
        [TestCleanup]
        public void CleanUp()
        {

            t.Dispose();
        }

        [TestMethod]
        public void GetAvailableCampgroundsTest()
        {
    

            int siteId;

            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into site (accessible, max_occupancy, campground_id, site_number, utilities) values ('true', 100, 1, 1000, 'false');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("SELECT CAST(SCOPE_IDENTITY() as int);", conn);
                siteId = Convert.ToInt32(cmd.ExecuteScalar());
            }
                
            Reservation r = new Reservation();
            r.Name = "Peter";
            r.Site_id = siteId;
            r.From_date = Convert.ToDateTime ("01/01/01");
            r.To_date = Convert.ToDateTime("01/05/01");
            r.Create_date = DateTime.Now;

            SiteDAL dal = new SiteDAL(connectionstring);

            List<Site> output = dal.GetAvailableCampsites(1, Convert.ToDateTime("01/02/01"), Convert.ToDateTime("01/03/01"));

            Assert.IsTrue(output.Count > 0);

        }
    }
}
