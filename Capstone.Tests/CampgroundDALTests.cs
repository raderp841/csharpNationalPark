using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.DAL;
using Capstone.Models;
using System.Transactions;
using System.Collections.Generic;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundDALTests
    {
        private string connectionsrting = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campgrounds;User ID=te_student;Password=sqlserver1";
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
        public void GetAllCampgroundsTest()
        {
            Campground camp = new Campground();
            camp.Name = "fake ground";
            camp.OpenFromMM = 01;
            camp.OpenToMM = 01;
            camp.DailyFee = 5;
            camp.ParkId = 1;

            CampgroundDAL dal = new CampgroundDAL(connectionsrting);
            List<Campground> output = dal.GetAllCampgrounds(1);
            Assert.IsTrue(output.Count > 0);
        }

    }
}
