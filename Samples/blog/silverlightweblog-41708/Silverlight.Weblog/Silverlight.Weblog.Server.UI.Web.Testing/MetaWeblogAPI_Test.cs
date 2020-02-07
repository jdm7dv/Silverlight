using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using CookComputing.XmlRpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Silverlight.Weblog.Common.IoC;
using Silverlight.Weblog.Server.DAL;
using Silverlight.Weblog.Server.DAL.Services.Interfaces;
using Silverlight.Weblog.Server.Testing;
using Silverlight.Weblog.UI.Web;
using Silverlight.Weblog.UI.Web.MetaWeblog;
using Category = Silverlight.Weblog.UI.Web.MetaWeblog.Category;

namespace Silverlight.Weblog.Server.UI.Web.Testing
{
    [TestClass]
    public class MetaWeblogAPI_Test : UnitTestBase<IMetaWeblog, MetaWeblogAPI>
    {
        #region UnitTestBase methods
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            UnitTestBase<IMetaWeblog, MetaWeblogAPI>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            UnitTestBase<IMetaWeblog, MetaWeblogAPI>.ClassCleanup();
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
        public void Login_SubmittingUserCredentials_CheckIUserServiceInvokedWithParameters()
        {
            // setup expectations and results
            using (mocks.Record())
                Expect.Call(Mock<IUserService>().IsValid(TestStrings[0], TestStrings[1])).Return(true);

            // action
            curElement.newPost(null, TestStrings[0], TestStrings[1], new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(XmlRpcFaultException), "Could not authenticate user.")]
        public void Login_SubmittingInvalidUserCredentials_CheckIUserServiceAndThrowExpection()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().IsValid(TestStrings[0], TestStrings[1])).Return(false);
            }

            // action
            curElement.newPost(0, TestStrings[0], TestStrings[1], new Post(), false);

            // assert
            mocks.VerifyAll();
        }


        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithTitle_SavesBlogPostTitle()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => b.Title == TestStrings[0]);
            }

            // action
            curElement.newPost(0, null, null, new Post() { title = TestStrings[0] }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithDescription_SavesBlogPostHTML()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => b.HTML == TestStrings[0]);
            }

            // action
            curElement.newPost(0, null, null, new Post() { description = TestStrings[0] }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithUsername_CallsUserServiceToFindUser()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().GetUserByUserName(TestStrings[0])).Return(null);
            }

            // action
            curElement.newPost(0, TestStrings[0], null, new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithUsername_SavesReturnValueFromIUserServiceAsBlogPostUser()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                User ReturnUser = new User();
                Expect.Call(Mock<IUserService>().GetUserByUserName(null)).IgnoreArguments().Return(ReturnUser);

                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => b.User == ReturnUser);
            }

            // action
            curElement.newPost(0, TestStrings[0], null, new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithStringTitle_SavedAndFormattedCorrectlyToPermLink()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => b.Permlink == "HelloWorld42");
            }

            // action
            curElement.newPost(0, null, null, new Post() { title = "hello woRld ./,-!@#$%^&*()+~ 42" }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithYearBefore2000_SavesBlogPostWithTodaysDate()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) =>
                    (DateTime.Now - b.DateCreated.Value).Seconds < 5);
            }

            // action
            curElement.newPost(0, null, null, new Post() { dateCreated = new DateTime(1999, 1, 1) }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithDateCreated_SavesBlogPostWithDateCreated()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => b.DateCreated == TestDateTimes[0]);
            }

            // action
            curElement.newPost(0, null, null, new Post() { dateCreated = TestDateTimes[0] }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWithDateToday_SavesDateAsDateNow()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => (DateTime.Now - b.DateCreated.Value).Seconds < 5);
            }

            // action
            curElement.newPost(0, null, null, new Post() { dateCreated = DateTime.Today }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWith2Categories_InvokesCategoryServiceGetTwice()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<ICategoryService>().GetCatrgoryOrCreateNewOne(null)).IgnoreArguments()
                    .Repeat.Twice().Return(new DAL.Category());
            }

            // action
            curElement.newPost(0, null, null, 
                new Post() { categories = new[] { TestStrings[0], TestStrings[1] } }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SendPostWith2Categories_AddTheResultsFromCategoryServiceoToBlogPostSave()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<ICategoryService>().GetCatrgoryOrCreateNewOne(null)).IgnoreArguments()
                    .Return(new DAL.Category() { Name = TestStrings[2] });

                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.IgnoreArguments().Callback((BlogPost b) => 
                    b.PostCategories.All(c => c.Category.Name == TestStrings[2]));
            }

            // action
            curElement.newPost(0, null, null, 
                new Post() { categories = new[] { TestStrings[0], TestStrings[1] } }, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_Invoked_DBContextCommitsChanges()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IDBContextContainer>().CommitChanges();
            }

            // action
            curElement.newPost(0, null, null, new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void NewPost_SetIDAfterSave_ConfirmTheIDIsReturned()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Mock<IRepository<BlogPost>>().Insert(null);
                LastCall.Callback((BlogPost b) =>
                {
                    b.ID = TestInts[0];
                    return true;
                });
            }

            // action
            var Result = curElement.newPost(0, null, null, new Post(), false);

            // assert
            Assert.IsTrue(Result == TestInts[0].ToString());
            mocks.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(XmlRpcException), "Invalid post id passed: invalidstring")]
        [IgnoreLoginAttribute]
        public void EditPost_InvokeWithNonNumericID_ExpcetionIsThrown()
        {
            using(mocks.Record())
            {
                
            }

            // action
            curElement.editPost("invalidstring", null, null, new Post(), false);
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        [ExpectedException(typeof(XmlRpcException), "Blog post with ID 111 was not found")]
        public void EditPost_BlogPostServiceDoesNotRecognizeID_ExceptionIsThrown()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(111)).Return(null);
            }

            // action
            curElement.editPost(111.ToString(), null, null, new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void EditPost_BlogPostFound_ClearCategoriesWasInvokedOnBlogPost()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                var blogPost = new BlogPost();
                Expect.Call(Mock<IBlogPostService>().Find(0)).IgnoreArguments().Return(blogPost);

                Mock<IBlogPostService>().ClearCategoriesOn(blogPost);
            }

            // action
            curElement.editPost(TestInts[0].ToString(), null, null, new Post(), false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void EditPost_Invoke_ReturnsTrue()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(0)).IgnoreArguments().Return(new BlogPost());
            }

            // assert
            Assert.IsTrue(curElement.editPost(TestInts[0].ToString(), null, null, new Post(), false) == true);
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void DeletePost_FoundBlogPost_BlogPostIsDeleted()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                var blogPost = new BlogPost();
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments().Return(blogPost);

                Mock<IRepository<BlogPost>>().Delete(blogPost);
            }

            // action
            curElement.deletePost(null, TestInts[0].ToString(), null, null, false);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void GetCategories_RepositoryReturns3Categories_Returns3Results()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<DAL.Category>>().Get()).Return(
                    new List<DAL.Category>{
                         new DAL.Category(), new DAL.Category(), new DAL.Category()
                     }.AsQueryable());
            }

            // action
            var Results = curElement.getCategories(0, null, null);

            // assert
            Assert.IsTrue(Results.Count() == 3);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void GetCategories_RepositoryReturnsCategoriesNames_ReturnedCorrectTitles()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<DAL.Category>>().Get()).Return(
                    new List<DAL.Category>{
                         new DAL.Category() { Name = TestStrings[0]},
                         new DAL.Category() { Name = TestStrings[0]}
                     }.AsQueryable());
            }

            // action
            var Results = curElement.getCategories(0, null, null);

            // assert
            Assert.IsTrue(Results.All(c => c.title == TestStrings[0]));
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void GetCategories_RepositoryReturnsCategoriesNames_ReturnedCorrectDescription()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<DAL.Category>>().Get()).Return(
                    new List<DAL.Category>{
                         new DAL.Category() { Name = TestStrings[0]},
                         new DAL.Category() { Name = TestStrings[0]}
                     }.AsQueryable());
            }

            // action
            var Results = curElement.getCategories(0, null, null);

            // assert
            Assert.IsTrue(Results.All(c => c.description == TestStrings[0]));
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void GetCategories_RepositoryReturnsCategoriesIDs_ReturnedCorrectIDs()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<DAL.Category>>().Get()).Return(
                    new List<DAL.Category>{
                         new DAL.Category() { ID = TestInts[0]},
                         new DAL.Category() { ID = TestInts[0]}
                     }.AsQueryable());
            }

            // action
            var Results = curElement.getCategories(0, null, null);

            // assert
            Assert.IsTrue(Results.All(c => c.categoryid == TestInts[0].ToString()));
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLogin]
        public void getUsersBlogs_InvokedWithUsername_CallsUserServiceFindForUsername()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().GetUserByUserName(TestStrings[0])).Return(new User());
            }

            // action
            curElement.getUsersBlogs(null, TestStrings[0], null);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        [ExpectedException(typeof(XmlRpcException), "User username does not have access to any blogs.")]
        public void getUsersBlogs_UserServiceReturnsNull_ExpectionIsThrown()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().GetUserByUserName(null)).IgnoreArguments().Return(null);
            }

            // action
            curElement.getUsersBlogs(null, "username", null);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getUsersBlogs_UserServiceReturnsUserWithID_IDValueIsReturnedAsBlogID()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().GetUserByUserName(null)).IgnoreArguments()
                    .Return(new User() { ID = TestInts[0] });
            }

            // action
            var Results = curElement.getUsersBlogs(null, null, null);

            // assert
            Assert.IsTrue(Results[0].blogid == TestInts[0].ToString());
            mocks.VerifyAll();
        }


        [TestMethod]
        [IgnoreLoginAttribute]
        public void getUsersBlogs_UserServiceReturnsUserWithBlogName_BlogNameValueIsReturnedAsBlogName()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IUserService>().GetUserByUserName(null)).IgnoreArguments().Return(new User() { BlogName = TestStrings[0] });
            }

            // action
            var Results = curElement.getUsersBlogs(null, null, null);

            // assert
            Assert.IsTrue(Results[0].blogName == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_Invoked_BlogPostServiceFindIsCalledWithPostID()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments().Return(new BlogPost());
            }

            // action
            curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithTitle_TitleIsEqualToReturnTitle()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments()
                    .Return(new BlogPost() { Title = TestStrings[0] });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.title == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithHTML_HtmlIsEqualToReturnDescription()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments()
                    .Return(new BlogPost() { HTML = TestStrings[0] });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.description == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithDate_DateIsEqualToReturnDate()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments()
                    .Return(new BlogPost() { DateCreated = TestDateTimes[0] });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.dateCreated == TestDateTimes[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithPermlink_PermlinkIsEqualToReturnPermlink()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments()
                    .Return(new BlogPost() { Permlink = TestStrings[0] });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.permalink == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithPermlink_PermlinkAndLinkIsEqualToReturnPermlink()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments()
                    .Return(new BlogPost() { Permlink = TestStrings[0] });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.link == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getPost_FoundBlogPostWithCategories_ReturnCategoriesAreEqualToCategories()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IBlogPostService>().Find(TestInts[0])).IgnoreArguments().Return(
                    new BlogPost()
                    {
                        PostCategories = new EntityCollection<DAL.PostCategory>()
                                                      {
                                                          new PostCategory()
                                                              {
                                                                  Category = new DAL.Category() { Name = TestStrings[0] }
                                                              }
                                                      }
                    });
            }

            // action
            var Result = curElement.getPost(TestInts[0].ToString(), null, null);

            // assert
            Assert.IsTrue(Result.categories.Count() == 1);
            Assert.IsTrue(Result.categories[0] == TestStrings[0]);
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getRecentPosts_Invoked_InvokedBlogPostRepositoryGet()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<BlogPost>>().Get()).Return(new List<BlogPost>().AsQueryable());
            }

            // action
            curElement.getRecentPosts(null, null, null, 0);

            // assert
            mocks.VerifyAll();
        }

        [TestMethod]
        [IgnoreLoginAttribute]
        public void getRecentPostsInvokedWith5PostsRequests_BlogPostServiceReturns10Post_Returns5BlogPost()
        {
            // setup expectations and results
            using (mocks.Record())
            {
                Expect.Call(Mock<IRepository<BlogPost>>().Get()).Return(new List<BlogPost>()
                                                                            {
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost(),
                                                                                new BlogPost()
                                                                            }.AsQueryable());
            }

            // action
            var results = curElement.getRecentPosts(null, null, null, 5);

            // assert
            Assert.IsTrue(results.Length == 5);
            mocks.VerifyAll();
        }
    }

    public class IgnoreLoginAttribute : TestInitializeActionAttribute
    {
        #region Overrides of TestInitializeActionAttribute

        public override void Execute(object unitTest)
        {
            var unitTestBase = (UnitTestBase<IMetaWeblog, MetaWeblogAPI>) unitTest;

            SetupResult.For(unitTestBase.Mock<IUserService>().IsValid(null, null)).IgnoreArguments().Return(true);
        }

        #endregion
    }

}
