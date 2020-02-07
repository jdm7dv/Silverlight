using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Silverlight.Weblog.Common.IoC;

namespace Silverlight.Weblog.Server.Testing
{
    [TestClass]
    public class UnitTestBase<TInterface, TInstance>
        where TInstance : TInterface
    {
        public UnitTestBase()
        {

        }

        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }

        protected MockRepository mocks;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            // clear IoC
            IoC.Clear();

            // clear mocks
            TypeInstanceMocks = new Dictionary<Type, object>();
            if (mocks != null)
                ((IDisposable)mocks).Dispose();
            mocks = new MockRepository();

            // initialize mocks
            foreach (Type mockType in MockTypes)
            {
                AddTypeMockInstance(mockType);     
            }

            // setup IoC and mocks based on current element
            SetMockTypesAsFirstLevelDependenciesOf<TInstance>();
            curElement = IoC.Get<TInstance>();

            // perform custom actions
            foreach (TestInitializeActionAttribute actionAttribute in GetCurrentActionAttributes())
            {
                actionAttribute.Execute(this);
            }
        }

        protected TInterface curElement { get; private set; }

        private void AddTypeMockInstance(Type mockType)
        {
            AddTypeMockInstance(mockType, mocks.DynamicMock(mockType));
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {

        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        //[TestMethod]
        //public void TestMethod1()
        //{
        //    //
        //    // Add test logic	here
        //    //
        //}

        #endregion

        protected virtual Type[] MockTypes
        {
            get
            {
                return new Type[0];
            }
        }

        protected void SetMockTypesAsFirstLevelDependenciesOf<T>()
        {
            foreach (PropertyInfo propInfo in typeof(T).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(DependencyAttribute), true).Length == 1))
            {
                AddTypeMockInstance(propInfo.PropertyType);
            }

        }

        private void AddTypeMockInstance(Type mockType, object mock)
        {
            TypeInstanceMocks[mockType] = mock;
            IoC.Register(mockType, mock);
        }

        private Dictionary<Type, object> TypeInstanceMocks = new Dictionary<Type, object>();

        public T Mock<T>()
        {
            if (!TypeInstanceMocks.ContainsKey(typeof(T)))
                AddTypeMockInstance(typeof(T));

            return (T)TypeInstanceMocks[typeof(T)];
        }

        protected ReadOnlyCollection<string> TestStrings
        {
            get
            {
                return new ReadOnlyCollection<string>(new[] { "hello", "world", "foo", "bar" });
            }
        }

        protected ReadOnlyCollection<int> TestInts
        {
            get
            {
                return new ReadOnlyCollection<int>(new [] { 111, 222, 333, 444 });
            }
        }

        protected ReadOnlyCollection<DateTime> TestDateTimes
        {
            get
            {
                return new ReadOnlyCollection<DateTime>(new List<DateTime>()
                                                            {
                                                                new DateTime(2009, 1, 1, 1, 1, 1), 
                                                                new DateTime(2009, 2, 2, 2, 2, 2), 
                                                                new DateTime(2009, 3, 3, 3, 3, 3), 
                                                                new DateTime(2010, 1, 1, 1, 1, 1), 
                                                            });
            }
        }

        private MethodInfo CurrentTestMethod
        {
            get
            {
                return this.GetType().GetMethod(TestContext.TestName);
            }
        }

        private IEnumerable<TestInitializeActionAttribute> GetCurrentActionAttributes()
        {
            foreach (object curAttribute in CurrentTestMethod.GetCustomAttributes(true))
            {
                if (curAttribute is TestInitializeActionAttribute)
                    yield return (TestInitializeActionAttribute) curAttribute;
            }
        }
    }


    public abstract class TestInitializeActionAttribute : Attribute
    {
        public abstract void Execute(object unitTest);

    }
}