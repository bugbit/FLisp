using System.Text;

namespace FLisp.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task TestMethod1()
    {
        var sexprsStr = new StringBuilder();

        sexprsStr.AppendLine("1");
        sexprsStr.AppendLine("a");
        sexprsStr.AppendLine(@"""Hola mundo""");

        using var stream = new StringReader(sexprsStr.ToString());

        var parser = new SExprParser(stream);
        var result = await parser.ReadSExprs();

        Assert.AreEqual(result.Length, 3);
        Assert.AreEqual(result[0], (Int128)1);
        Assert.AreEqual(result[1], "a");
        Assert.AreEqual(result[2], new SString("Hola mundo"));
    }
}