using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using GSTAppLogic.templating;

namespace GSTAppLogic.test.templating
{
    [TestFixture]
    public class TemplatingHelperTest
    {
        private string sourcePath;
        private string templatePath;

        [SetUp]
        public void SetupFixture()
        {
            sourcePath = Path.GetTempFileName();
            templatePath = Path.GetTempFileName();

            using(var sourceWriter = new StreamWriter(sourcePath, false, Encoding.UTF8))
            {
                sourceWriter.WriteLine("void main(void)");
                sourceWriter.WriteLine("{");
                sourceWriter.WriteLine("printf(\"%s\", \"Hallo\");");
                sourceWriter.WriteLine("}");
            }

            using (var templateWriter = new StreamWriter(templatePath, false, Encoding.UTF8))
            {
                templateWriter.WriteLine("printf(\"%s\", \"Hallo\");");
            }
        }


        [Test]
        public void TestTemplating()
        {
            var stripped = File.ReadAllText(TemplatingHelper.StripTemplateFromSourceFile(sourcePath, templatePath));
            var split = stripped.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            Assert.AreEqual("void main(void)", split[0]);
        }
    }
}
