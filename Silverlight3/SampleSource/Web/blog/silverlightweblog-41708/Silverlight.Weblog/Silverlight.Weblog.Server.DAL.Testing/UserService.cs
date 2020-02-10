using System.Collections.Generic;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.Testing;
using System.Linq;

namespace a
{
    [TestClass]
    public class UserService_Test : UnitTestBase<IUserService, UserService>
    {
        #region UnitTestBase methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase<IUserService, UserService>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase<IUserService, UserService>.ClassCleanup();
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
        public void GetUsersHasACollectionWith3Users_SendNameMatching2ndUser_Returns2ndUser()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                 Expect.Call(Mock<IRepository<User>>().Get()).Return(
                     new List<User>()
                         {
                             new User() { Username = TestStrings[0]},
                             new User() { Username = TestStrings[1]},
                             new User() { Username = TestStrings[2]}
                         }.AsQueryable());
            }
			
            // action
            var result = curElement.GetUserByUserName(TestStrings[1]);
            
            // assert
            Assert.IsTrue(result.Username == TestStrings[1]);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void IsValidWith3Users_SendWrongPassword_ReturnsFalse()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                 Expect.Call(Mock<IRepository<User>>().Get()).Return(
                     new List<User>()
                         {
                             new User() { Username = TestStrings[0], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[0], "sha1")},
                             new User() { Username = TestStrings[1], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[1], "sha1")},
                             new User() { Username = TestStrings[2], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[2], "sha1")},
                         }.AsQueryable());
            }
			
            // action
            var result = curElement.IsValid(TestStrings[0], "invalidpassword");
            
            // assert
            Assert.IsFalse(result);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void IsValidWith3Users_SendWrongUsername_ReturnsFalse()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<User>>().Get()).Return(
                    new List<User>()
                         {
                             new User() { Username = TestStrings[0], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[0], "sha1")},
                             new User() { Username = TestStrings[1], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[1], "sha1")},
                             new User() { Username = TestStrings[2], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[2], "sha1")},
                         }.AsQueryable());
            }

            // action
            var result = curElement.IsValid("invalidpassword", TestStrings[0]);

            // assert
            Assert.IsFalse(result);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void IsValidWith3Users_SendsCorrectUsernameAndPassword_ReturnsTrue()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<User>>().Get()).Return(
                    new List<User>()
                         {
                             new User() { Username = TestStrings[0], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[0], "sha1")},
                             new User() { Username = TestStrings[1], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[1], "sha1")},
                             new User() { Username = TestStrings[2], PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(TestStrings[2], "sha1")},
                         }.AsQueryable());
            }

            // action
            var result = curElement.IsValid(TestStrings[1], TestStrings[1]);

            // assert
            Assert.IsTrue(result);
            mocks.VerifyAll();
        }
    }
}