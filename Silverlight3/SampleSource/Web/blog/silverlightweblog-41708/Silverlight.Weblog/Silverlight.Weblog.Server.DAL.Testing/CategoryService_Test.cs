using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.Testing;
using Silverlight.Weblog.Server.UI.Web.Testing;

namespace a
{
    [TestClass]
    public class CategoryService_Test : UnitTestBase<ICategoryService, CategoryService>
    {
        #region UnitTestBase methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase<ICategoryService, CategoryService>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase<ICategoryService, CategoryService>.ClassCleanup();
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestCleanup]
        public override void TestCleanup()
        {
            base.TestCleanup();
        }

        #endregion UnitTestBase methods

        [TestMethod]
        public void GetCatrgoryOrCreateNewOne_InvokeWithExistingCategoryName_ReturnThatCategory()
        {
            // setup expectations and results
            var category = new Category() { Name = TestStrings[0] };
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<Category>>().Get()).Return(
                     new List<Category>() { category }.AsQueryable());
            }

            // action
            var Result = curElement.GetCatrgoryOrCreateNewOne(TestStrings[0]);

            // assert
            Assert.IsTrue(Result == category);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void GetCatrgoryOrCreateNewOne_InvokeWithNewCategoryName_NewCategoryReturned()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<Category>>().Get()).Return(
                    new List<Category>().AsQueryable());
            }

            // action
            var Result = curElement.GetCatrgoryOrCreateNewOne(TestStrings[0]);

            // assert
            Assert.IsTrue(Result.Name == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void GetCatrgoryOrCreateNewOne_InvokeWithNewCategoryName_NewCategoryInserted()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<Category>>().Get()).Return(
                    new List<Category>().AsQueryable());

                Mock<IRepository<Category>>().Insert(null);
                LastCall.Callback((Category c) => c.Name == TestStrings[0]);
            }

            // action
            var Result = curElement.GetCatrgoryOrCreateNewOne(TestStrings[0]);

            // assert
            mocks.VerifyAll();
        }
    }

    
}