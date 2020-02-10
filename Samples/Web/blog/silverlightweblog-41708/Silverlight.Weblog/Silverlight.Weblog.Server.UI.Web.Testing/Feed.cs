using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.Testing;
using Silverlight.Weblog.UI.Web.Feeds;
using System.Linq;

namespace a
{
    [TestClass]
    public class Feed_Test : UnitTestBase<IFeed, Feed>
    {
        #region UnitTestBase methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase<IFeed, Feed>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase<IFeed, Feed>.ClassCleanup();
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
        [ExpectedException(typeof(ApplicationException), "Unknown username: invaliduser")]
        public void GetBlog_UsernameDoesNotExistInUserRepository_ExceptionIsThrown()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<BlogPost>>().Get()).Return(new List<BlogPost>()
                         {
                             new BlogPost() { User =  new User() { Username = TestStrings[0] }},
                             new BlogPost() { User =  new User() { Username = TestStrings[1] }},
                         }.AsQueryable());

                 Expect.Call(Mock<IRepository<User>>().Get()).Return(new List<User>()
                         {
                             new User() { Username = TestStrings[0] },
                             new User() { Username = TestStrings[1] }
                         }.AsQueryable());
            }
			
            // action
            curElement.GetBlog(null, "invaliduser");
            
            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        [EmptyBlogPostRepository]
        public void GetBlog_SendInvalidFormat_ReturnsNull()
        {
            // setup expectations and results
            using (mocks.Record())
            {
            }

            // action
            curElement.GetBlog("invalidformat", null);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        [EmptyBlogPostRepository]
        public void GetBlog_InvokeRssFormat_ReturnsRssFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
            }

            // action
            var result = curElement.GetBlog("rss", null);

            // assert
            Assert.IsTrue(result is Rss20FeedFormatter);
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        [EmptyBlogPostRepository]
        public void GetBlog_InvokeAtomFormat_ReturnsAtomFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
            }

            // action
            var result = curElement.GetBlog("atom", null);

            // assert
            Assert.IsTrue(result is Atom10FeedFormatter);
            mocks.VerifyAll();
        }


        [TestMethod]
        [EmptyUserRepository]
        [EmptyBlogPostRepository]
        public void GetMainFormat_InvokeRssFormat_ReturnsRssFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
            }

            // action
            var result = curElement.GetMainFormat("rss");

            // assert
            Assert.IsTrue(result is Rss20FeedFormatter);
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        [EmptyBlogPostRepository]
        public void GetMainFormat_InvokeAtomFormat_ReturnsAtomFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
            }

            // action
            var result = curElement.GetMainFormat("atom");

            // assert
            Assert.IsTrue(result is Atom10FeedFormatter);
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        public void GetCategory_InvokeRssFormat_ReturnsRssFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<Category>>().Get()).Return(new List<Category>()
                        {
                           new Category() { Name = TestStrings[0]} 
                        }.AsQueryable());
            }

            // action
            var result = curElement.GetCategory("rss", null, TestStrings[0]);

            // assert
            Assert.IsTrue(result is Rss20FeedFormatter);
            mocks.VerifyAll();
        }

        [TestMethod]
        [EmptyUserRepository]
        public void GetCategory_InvokeAtomFormat_ReturnsAtomFormatter()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<Category>>().Get()).Return(new List<Category>()
                        {
                           new Category() { Name = TestStrings[0]} 
                        }.AsQueryable());
            }

            // action
            var result = curElement.GetCategory("atom", null, null);

            // assert
            Assert.IsTrue(result is Atom10FeedFormatter);
            mocks.VerifyAll();
        }

    }

    public class EmptyBlogPostRepository : TestInitializeActionAttribute
    {
        public override void Execute(object unitTest)
        {
            var unitTestBase = (UnitTestBase<IFeed, Feed>)unitTest;

            Expect.Call(unitTestBase.Mock<IRepository<BlogPost>>().Get()).Return(new List<BlogPost>().AsQueryable());
        }
    }

    public class EmptyUserRepository : TestInitializeActionAttribute
    {
        public override void Execute(object unitTest)
        {
            var unitTestBase = (UnitTestBase<IFeed, Feed>)unitTest;

            Expect.Call(unitTestBase.Mock<IRepository<User>>().Get()).Return(new List<User>().AsQueryable());
        }
    }

    public class EmptyCategoryRepository : TestInitializeActionAttribute
    {
        public override void Execute(object unitTest)
        {
            var unitTestBase = (UnitTestBase<IFeed, Feed>)unitTest;

            Expect.Call(unitTestBase.Mock<IRepository<Category>>().Get()).Return(new List<Category>().AsQueryable());
        }
    }
}