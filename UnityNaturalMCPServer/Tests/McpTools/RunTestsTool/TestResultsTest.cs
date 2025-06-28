using NUnit.Framework;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [TestFixture]
    public class TestResultsTest
    {
        [Test]
        public void Success_WithFail_ReturnsFalse()
        {
            var sut = new TestResults
            {
                failCount = 1,
                passCount = 1, // Does not affect the actual
            };

            Assert.That(sut.success, Is.False);
        }

        [Test]
        public void Success_WithInconclusive_ReturnsFalse()
        {
            var sut = new TestResults
            {
                inconclusiveCount = 1,
                passCount = 1, // Does not affect the actual
            };

            Assert.That(sut.success, Is.False);
        }

        [Test]
        public void Success_WithoutPass_ReturnsFalse()
        {
            var sut = new TestResults
            {
                failCount = 0,
                passCount = 0,
                skipCount = 1, // Note: Does not affect the actual
                inconclusiveCount = 0
            };

            Assert.That(sut.success, Is.False);
        }

        [Test]
        public void Success_WithPassAndWithoutFailOrInconclusive_ReturnsTrue()
        {
            var sut = new TestResults
            {
                failCount = 0,
                passCount = 1,
                inconclusiveCount = 0
            };

            Assert.That(sut.success, Is.True);
        }
    }
}
