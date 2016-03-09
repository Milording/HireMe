using System;
using NUnit.Framework;

namespace HireMe.BL.Tests
{
    /// <summary>
    /// Tests for TriangleWorker
    /// </summary>
    [TestFixture]
    public class TriangleWorkerTests
    {
        private ITriangleWorker _triangleWorker;

        /// <summary>
        /// Initialize local variables.
        /// </summary>
        [SetUp]
        public void Init()
        {
            this._triangleWorker = new TriangleWorker();
        }

        /// <summary>
        /// Common test for checking simple example.
        /// </summary>
        [Test]
        public void CalculateAreaTriangle_Common()
        {
            var area = _triangleWorker.CalculateAreaTriangle(2, 3);

            Assert.AreEqual(area, 3);
        }

        /// <summary>
        /// Exception should thrown when first argument is negative.
        /// </summary>
        [Test]
        public void CalculateAreaTriangle_NegativeFirst()
        {
            try
            {
                _triangleWorker.CalculateAreaTriangle(-2, 2);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Doesn't satisfy to restrictions of first cathenus."));
            }
        }

        /// <summary>
        /// Exeption should thrown when second argument is negative.
        /// </summary>
        [Test]
        public void CalculateAreaTriangle_NegativeSecond()
        {
            try
            {
                _triangleWorker.CalculateAreaTriangle(2, -2);

                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Doesn't satisfy to restrictions  of second cathenus."));
            }
        }
    }
}
