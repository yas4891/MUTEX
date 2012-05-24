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
        private const string EXAMPLE_1 = "path=UPLOAD/mop/Pawelczak+Dieter/59067/V1/aufgabe2_10.c&repository=aufgabe2_10&function=main&pin=59067&thres=80";

        [Test]
        public void GetStudentIdentifier()
        {
            Assert.AreEqual("59067", GeneralHelper.GetStudentIdentifier(EXAMPLE_1));
        }

        [Test]
        public void GetAssignmentIdentifier()
        {
            Assert.AreEqual("aufgabe2_10", GeneralHelper.GetAssignmentIdentifier(EXAMPLE_1));
        }
    }
}
