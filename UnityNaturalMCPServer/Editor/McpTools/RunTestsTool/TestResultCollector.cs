using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.TestTools.TestRunner.Api;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    /// <summary>
    /// Waiting for the test run to finish and collecting results.
    /// </summary>
    public class TestResultCollector : IErrorCallbacks
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal readonly TestResults _testResults = new TestResults();

        private string _abortMessage;
        private bool _runFinished;

        /// <inheritdoc/>
        public void RunStarted(ITestAdaptor testsToRun)
        {
            // nop
        }

        /// <inheritdoc/>
        public void RunFinished(ITestResultAdaptor result)
        {
            _testResults.failCount = result.FailCount;
            _testResults.passCount = result.PassCount;
            _testResults.skipCount = result.SkipCount;
            _testResults.inconclusiveCount = result.InconclusiveCount;
            _runFinished = true;
        }

        /// <inheritdoc/>
        public void TestStarted(ITestAdaptor test)
        {
            // nop
        }

        /// <inheritdoc/>
        public void TestFinished(ITestResultAdaptor result)
        {
            if (result.HasChildren)
            {
                return;
            }

            if (result.TestStatus == TestStatus.Failed || result.TestStatus == TestStatus.Inconclusive)
            {
                _testResults.failedTests.Add(new FailedTestResult(result));
            }
        }

        /// <inheritdoc/>
        public void OnError(string message)
        {
            _abortMessage = message;
            _runFinished = true;
        }

        /// <summary>
        /// Wait until the run is finished or the cancellation token is triggered.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Test results by JSON string or abort message.</returns>
        public async ValueTask<string> WaitForRunFinished(CancellationToken cancellationToken = default)
        {
            while (_runFinished == false && !cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500, cancellationToken);
            }

            return _abortMessage ?? _testResults.ToJson();
        }
    }
}
