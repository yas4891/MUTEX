﻿using System.Collections.Generic;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;
using NUnit.Framework;
using Antlr.Runtime;
using System;
using GSTLibrary.token;
using Tokenizer;
using GSTLibrary.tile;
using GSTAppLogic.ext;

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

        [Test]
        public void ANTLRITokenVersusMutexTokenImpl()
        {
            var factory = new MutexTokenFactory();
            var itokens = factory.GetTokenWrapperListFromSource("void main(int argc, char** argv)").ToGSTTokenList();

            var tokennames = new[] { "VOID", "IDENTIFIER", "INTEGER_DATATYPE", "IDENTIFIER", "POINTER_DATATYPE", "IDENTIFIER" }; 
            var mutextokens = factory.GetTokenWrapperEnumerable(tokennames).ToGSTTokenList();

            for (int i = 0; i < itokens.Count; i++)
            {
                Assert.AreEqual(itokens[i].GetHashCode(), mutextokens[i].GetHashCode());
            }

            var algo = new HashingGSTAlgorithm<GSTToken<TokenWrapper>>(itokens, mutextokens)
            {
                MinimumMatchLength = 3
            };

            algo.RunToCompletion();


            Assert.AreEqual(100, algo.Similarity);

            
        }
    }
}
