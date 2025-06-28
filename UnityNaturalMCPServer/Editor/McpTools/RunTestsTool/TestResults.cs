using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using UnityEditor.TestTools.TestRunner.Api;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TestResults
    {
        /// <summary>
        /// The number of test cases that failed when running the test and all its children.
        /// </summary>
        public int failCount { get; set; }

        /// <summary>
        /// The number of test cases that passed when running the test and all its children.
        /// </summary>
        public int passCount { get; set; }

        /// <summary>
        /// The number of test cases that were skipped when running the test and all its children.
        /// </summary>
        public int skipCount { get; set; }

        /// <summary>
        ///The number of test cases that were inconclusive when running the test and all its children.
        /// </summary>
        public int inconclusiveCount { get; set; }

        /// <summary>
        /// Failed or inconclusive tests.
        /// </summary>
        [SuppressMessage("ReSharper", "CollectionNeverQueried.Global")]
        public List<FailedTestResult> failedTests { get; } = new List<FailedTestResult>();

        /// <summary>
        /// Returns true if all tests passed.
        /// </summary>
        public bool success => (failCount + inconclusiveCount) == 0 && passCount > 0;

        /// <summary>
        /// Returns a JSON representation of the test results.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class FailedTestResult
    {
        public string name { get; }
        public string fullName { get; }
        public string resultState { get; }
        public string testStatus { get; }
        public double duration { get; }
        public string message { get; }
        public string stackTrace { get; }
        public string output { get; }

        public FailedTestResult(ITestResultAdaptor result)
        {
            name = result.Name;
            fullName = result.FullName;
            resultState = result.ResultState;
            testStatus = result.TestStatus.ToString();
            duration = result.Duration;
            message = result.Message;
            stackTrace = result.StackTrace;
            output = result.Output;
        }
    }
}
