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
    public class ParksTest
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
        public void GetAllParksTest()
        {
            using (SqlConnection conn = new SqlConnection(connectionstring))
            {
                DateTime establishDate = Convert.ToDateTime("01/01/99");
                conn.Open();
                SqlCommand cmd = new SqlCommand($"insert into park(name, location, establish_date, area, visitors, description)values('parkity park', 'park land','1900', 1234, 1, 'a rustic park');", conn);
                cmd.ExecuteNonQuery();

            }

            ParksDAL dal = new ParksDAL(connectionstring);
            List<Park> output = dal.GetAllParks();

            Assert.IsTrue(output.Count > 0);
        }
    }
}
