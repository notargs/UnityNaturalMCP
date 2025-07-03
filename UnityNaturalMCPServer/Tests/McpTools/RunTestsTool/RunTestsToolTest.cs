using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [TestFixture]
    public class RunTestsToolTest
    {
        private const string NotExistCategoryName = "Dummy";
        private const string CanceledMessage = "A task was canceled.";

        [Test]
        public async Task RunEditModeTests_TaskCancel_CancelledTestRun()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var testTask = RunTestsTool.RunEditModeTests(
                categoryNames: new[] { NotExistCategoryName },
                cancellationToken: cancellationTokenSource.Token);

            await Task.Yield();
            cancellationTokenSource.Cancel();

            await testTask;
            LogAssert.Expect(LogType.Warning, CanceledMessage);
        }

        [Test]
        public async Task RunPlayModeTests_TaskCancel_CancelledTestRun()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var testTask = RunTestsTool.RunPlayModeTests(
                categoryNames: new[] { NotExistCategoryName },
                cancellationToken: cancellationTokenSource.Token);

            await Task.Yield();
            cancellationTokenSource.Cancel();

            await testTask;
            LogAssert.Expect(LogType.Warning, CanceledMessage);
        }
    }
}
