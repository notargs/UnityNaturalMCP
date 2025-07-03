using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ModelContextProtocol.Server;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [McpServerToolType]
    public static class RunTestsTool
    {
        [McpServerTool, Description(
             "Run Edit Mode tests on Unity Test Runner. Recommend filtering by assemblyNames, groupNames, or testNames to narrow down the tests to be executed to the scope of changes.")]
        public static async ValueTask<string> RunEditModeTests(
            [Description(
                "The name of assemblies included in the run. That is the assembly name, without the .dll file extension. E.g., MyTestAssembly")]
            string[] assemblyNames = null,
            [Description(
                "The name of a Category to include in the run. Any test or fixtures runs that have a Category matching the string.")]
            string[] categoryNames = null,
            [Description(
                "The same as testNames, except that it allows for Regex. This is useful for running specific fixtures or namespaces. E.g. \"^MyNamespace\\.\" Runs any tests where the top namespace is MyNamespace.")]
            string[] groupNames = null,
            [Description(
                "The full name of the tests to match the filter. This is usually in the format FixtureName.TestName. If the test has test arguments, then include them in parenthesis. E.g. MyTestClass2.MyTestWithMultipleValues(1).")]
            string[] testNames = null,
            CancellationToken cancellationToken = default)
        {
            return await RunTests(TestMode.EditMode, assemblyNames, categoryNames, groupNames, testNames,
                cancellationToken);
        }

        [McpServerTool, Description(
             "Run Play Mode tests on Unity Test Runner. Recommend filtering by assemblyNames, groupNames, or testNames to narrow down the tests to be executed to the scope of changes.")]
        public static async ValueTask<string> RunPlayModeTests(
            [Description(
                "The name of assemblies included in the run. That is the assembly name, without the .dll file extension. E.g., MyTestAssembly")]
            string[] assemblyNames = null,
            [Description(
                "The name of a Category to include in the run. Any test or fixtures runs that have a Category matching the string.")]
            string[] categoryNames = null,
            [Description(
                "The same as testNames, except that it allows for Regex. This is useful for running specific fixtures or namespaces. E.g. \"^MyNamespace\\.\" Runs any tests where the top namespace is MyNamespace.")]
            string[] groupNames = null,
            [Description(
                "The full name of the tests to match the filter. This is usually in the format FixtureName.TestName. If the test has test arguments, then include them in parenthesis. E.g. MyTestClass2.MyTestWithMultipleValues(1).")]
            string[] testNames = null,
            CancellationToken cancellationToken = default)
        {
            return await RunTests(TestMode.PlayMode, assemblyNames, categoryNames, groupNames, testNames,
                cancellationToken);
        }

        private static async ValueTask<string> RunTests(
            TestMode testMode,
            string[] assemblyNames = null,
            string[] categoryNames = null,
            string[] groupNames = null,
            string[] testNames = null,
            CancellationToken cancellationToken = default)
        {
            var filter = new Filter
            {
                assemblyNames = assemblyNames, categoryNames = categoryNames,
                groupNames = groupNames, testNames = testNames, testMode = testMode,
            };
            Debug.Log($"Running tests, {filter}");

            TestResultCollector testResultCollector = null;
            TestRunnerApi testRunner = null;
            string testJobGuid = null;

            try
            {
                await UniTask.SwitchToMainThread();

                testResultCollector = new TestResultCollector();
                TestRunnerApi.RegisterTestCallback(testResultCollector);

                testRunner = ScriptableObject.CreateInstance<TestRunnerApi>();
                testJobGuid = testRunner.Execute(new ExecutionSettings(filter));

                return await testResultCollector.WaitForRunFinished(cancellationToken);
            }
            catch (OperationCanceledException e)
            {
                Debug.LogWarning(e.Message);
                throw;
            }
            finally
            {
                if (testJobGuid != null)
                    TestRunnerApi.CancelTestRun(testJobGuid);
                if (testResultCollector != null)
                    TestRunnerApi.UnregisterTestCallback(testResultCollector);
                if (testRunner != null)
                    Object.DestroyImmediate(testRunner);
            }
        }
    }
}
