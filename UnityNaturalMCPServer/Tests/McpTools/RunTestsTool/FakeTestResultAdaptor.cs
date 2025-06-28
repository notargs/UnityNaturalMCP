using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework.Interfaces;
using UnityEditor.TestTools.TestRunner.Api;
using TestStatus = UnityEditor.TestTools.TestRunner.Api.TestStatus;

namespace UnityNaturalMCP.Editor.McpTools.RunTestsTool
{
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
    public class FakeTestResultAdaptor : ITestResultAdaptor
    {
        public TNode ToXml()
        {
            throw new NotImplementedException();
        }

        public ITestAdaptor Test { get; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string ResultState { get; set; }
        public TestStatus TestStatus { get; set; }
        public double Duration { get; set; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int AssertCount { get; }
        public int FailCount { get; set; }
        public int PassCount { get; set; }
        public int SkipCount { get; set; }
        public int InconclusiveCount { get; set; }
        public bool HasChildren { get; set; }
        public IEnumerable<ITestResultAdaptor> Children { get; }
        public string Output { get; set; }
    }
}
