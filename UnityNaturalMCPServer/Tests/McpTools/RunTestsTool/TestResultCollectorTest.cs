using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [TestFixture]
    public class TestResultCollectorTest
    {
        [Test]
        public void RunFinished_SetTestCounts()
        {
            var finish = new FakeTestResultAdaptor
            {
                FailCount = 2,
                PassCount = 3,
                SkipCount = 5,
                InconclusiveCount = 7
            };

            var sut = new TestResultCollector();
            sut.RunFinished(finish);

            Assert.That(sut._testResults.failCount, Is.EqualTo(2));
            Assert.That(sut._testResults.passCount, Is.EqualTo(3));
            Assert.That(sut._testResults.skipCount, Is.EqualTo(5));
            Assert.That(sut._testResults.inconclusiveCount, Is.EqualTo(7));
        }

        [Test]
        public void TestFinished_Failed_AddToFailedTests()
        {
            var failed = new FakeTestResultAdaptor
            {
                TestStatus = TestStatus.Failed,
                Name = "FailedTest",
                FullName = "Fake.FailedTest",
                ResultState = "Failed:Error",
                Duration = 1.23d,
                Message = "Message of Fake.FailedTest",
                StackTrace = "Stack trace of Fake.FailedTest",
                Output = "Output of Fake.FailedTest"
            };

            var sut = new TestResultCollector();
            sut.TestFinished(failed);
            sut.TestFinished(failed); // twice

            Assert.That(sut._testResults.ToJson(), Does.Contain(
                "\"failedTests\":[{\"name\":\"FailedTest\",\"fullName\":\"Fake.FailedTest\",\"resultState\":\"Failed:Error\",\"testStatus\":\"Failed\",\"duration\":1.23,\"message\":\"Message of Fake.FailedTest\",\"stackTrace\":\"Stack trace of Fake.FailedTest\",\"output\":\"Output of Fake.FailedTest\"},{\"name\":\"FailedTest\",\"fullName\":\"Fake.FailedTest\",\"resultState\":\"Failed:Error\",\"testStatus\":\"Failed\",\"duration\":1.23,\"message\":\"Message of Fake.FailedTest\",\"stackTrace\":\"Stack trace of Fake.FailedTest\",\"output\":\"Output of Fake.FailedTest\"}]"));
        }

        [Test]
        public void TestFinished_Inconclusive_AddToFailedTests()
        {
            var inconclusive = new FakeTestResultAdaptor
            {
                TestStatus = TestStatus.Inconclusive,
                Name = "InconclusiveTest",
                FullName = "Fake.InconclusiveTest",
                ResultState = "Inconclusive",
                Duration = 1.23d,
                Message = "Message of Fake.InconclusiveTest",
                StackTrace = "Stack trace of Fake.InconclusiveTest",
                Output = "Output of Fake.InconclusiveTest"
            };

            var sut = new TestResultCollector();
            sut.TestFinished(inconclusive);
            sut.TestFinished(inconclusive); // twice

            Assert.That(sut._testResults.ToJson(), Does.Contain(
                "\"failedTests\":[{\"name\":\"InconclusiveTest\",\"fullName\":\"Fake.InconclusiveTest\",\"resultState\":\"Inconclusive\",\"testStatus\":\"Inconclusive\",\"duration\":1.23,\"message\":\"Message of Fake.InconclusiveTest\",\"stackTrace\":\"Stack trace of Fake.InconclusiveTest\",\"output\":\"Output of Fake.InconclusiveTest\"},{\"name\":\"InconclusiveTest\",\"fullName\":\"Fake.InconclusiveTest\",\"resultState\":\"Inconclusive\",\"testStatus\":\"Inconclusive\",\"duration\":1.23,\"message\":\"Message of Fake.InconclusiveTest\",\"stackTrace\":\"Stack trace of Fake.InconclusiveTest\",\"output\":\"Output of Fake.InconclusiveTest\"}]"));
        }

        [Test]
        [Timeout(5000)]
        public async Task WaitForRunFinished_RunFinished_LeaveAwaiting()
        {
            var sut = new TestResultCollector();
            sut.RunFinished(new FakeTestResultAdaptor());

            var result = await sut.WaitForRunFinished();
            Debug.Log(result);
        }

        [Test]
        [Timeout(5000)]
        public async Task WaitForRunFinished_OnError_LeaveAwaiting()
        {
            var message = "Error occurred!";
            var sut = new TestResultCollector();
            sut.OnError(message);

            var result = await sut.WaitForRunFinished();
            Assert.That(result, Is.EqualTo(message));
        }

        [Test]
        [Timeout(5000)]
        public async Task WaitForRunFinished_Cancel_LeaveAwaiting()
        {
            var sut = new TestResultCollector();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(1000);

            var result = await sut.WaitForRunFinished(cts.Token);
            Assert.That(result, Is.Null);
        }
    }
}
