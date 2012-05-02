using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private IEnumerable<string> defaultTokenSet = new []
                                                          {
                                                              "VOID_DATATYPE", "IDENTIFIER", "INTEGER_DATATYPE",
                                                              "IDENTIFIER", "INTEGER_DATATYPE", "POINTER", "IDENTIFIER"
                                                          };


        [Test]
        public void Default()
        {
            var model = new ComparisonModel(JoinString(defaultTokenSet).GetTokens(),
                new []{
                        new SourceEntityData("stud1", "assignment1", defaultTokenSet, "bla" ), 
                        new SourceEntityData("stud2", "assignment1", new[] {"IDENTIFIER", "INTEGER_DATATYPE"}, "bla2")
                    });

            model.Start();

            Assert.AreEqual(100, model.MaximumSimilarity);
        }

        private static string JoinString(IEnumerable<string> enumerable)
        {
            var builder = new StringBuilder();

            foreach (var str in enumerable)
                builder.AppendFormat("{0} ", str);


            return builder.ToString().Trim();
        }
    }
}
