using System.Collections.Generic;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;
using NUnit.Framework;

namespace GSTAppLogic.test.model
{
    [TestFixture]
    public class ComparisonModelTest
    {
        private const string RELATIVE_PATH_TO_TEST_FILES = "../../../test/applogic/{0}";
        private readonly IEnumerable<string> defaultTokenSet = new []
                                                          {
                                                              "INCREMENT", "ADDEQUAL", "ARRAY_ACCESS",
                                                              "ASSIGN", "CASE", "DECREMENT", "GOTO", "INCREMENT", "ASSIGN"
                                                          };

        [Test]
        public void Default()
        {
            var factory = new MutexTokenFactory();
            var model = new ComparisonModel(factory.GetTokenWrapperEnumerable(defaultTokenSet),
                new []{
                        new SourceEntityData("stud1", "assignment1", defaultTokenSet, "bla" ), 
                        new SourceEntityData("stud2", "assignment1", new[] {"CASE", "DECREMENT"}, "bla2")
                    }, factory);

            model.Calculate();

            Assert.AreEqual(100, model.MaximumSimilarity);
        }
    }
}
