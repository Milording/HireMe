using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace HireMe.BL.Tests
{
    [TestFixture]
    public class TriangleWorkerTests
    {
        private ITriangleWorker _triangleWorker;

        [SetUp]
        public void Init()
        {
            this._triangleWorker = new TriangleWorker();
        }

        [Test]
        public void CalculateAreaTriangle_Common()
        {
            var area = _triangleWorker.CalculateAreaTriangle(2, 3);

            Assert.AreEqual(area, 3);
        }

        [Test]
        public void CalculateAreaTriangle_NegativeFirst()
        {
            try
            {
                var area = _triangleWorker.CalculateAreaTriangle(-2, 2);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Doesn't satisfy to restrictions of first cathenus."));
            }
        }

        [Test]
        public void CalculateAreaTriangle_NegativeSecond()
        {
            try
            {
                var area = _triangleWorker.CalculateAreaTriangle(2, -2);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Doesn't satisfy to restrictions  of second cathenus."));
            }
        }
    }
}
