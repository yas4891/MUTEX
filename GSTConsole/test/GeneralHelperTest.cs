using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace GSTConsole.test
{
    [TestFixture]
    public class GeneralHelperTest
    {
        private const string EXAMPLE_1 = "UPLOAD/mop/Pawelczak+Dieter/59067/V1/aufgabe2_10.c&HTML/mop/_plag/V1/aufgabe2_10&main";

        [Test]
        public void GetStudentIdentifier()
        {
            Assert.AreEqual("Pawelczak+Dieter", GeneralHelper.GetStudentIdentifier(EXAMPLE_1));
        }

        [Test]
        public void GetAssignmentIdentifier()
        {
            Assert.AreEqual("aufgabe2_10", GeneralHelper.GetAssignmentIdentifier(EXAMPLE_1));
        }
    }
}
