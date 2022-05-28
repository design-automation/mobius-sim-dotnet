using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using GraphCollection;

namespace GraphTest;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        Graph gr = new Graph();
        gr.AddNode("big", new Dictionary<string, object>());
        gr.AddNode("bang", new Dictionary<string, object>());
        gr.AddEdgeType("aaa", Graph.M2M);
        gr.AddEdge("big", "bang", "aaa");
        int deg = gr.Degree("big", "aaa");
        Assert.AreEqual(deg, 1);
    }
}