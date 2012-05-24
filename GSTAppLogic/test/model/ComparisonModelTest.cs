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
                                                              "T__24", "T__23", "T__25",
                                                              "T__26", "T__27", "T__28", "T__29"
                                                          };

        [Test]
        public void Default()
        {
            var model = new ComparisonModel(defaultTokenSet.GetTokens(),
                new []{
                        new SourceEntityData("stud1", "assignment1", defaultTokenSet, "bla" ), 
                        new SourceEntityData("stud2", "assignment1", new[] {"T__27", "T__28"}, "bla2")
                    });

            model.Calculate();

            Assert.AreEqual(100, model.MaximumSimilarity);
        }
    }
}
