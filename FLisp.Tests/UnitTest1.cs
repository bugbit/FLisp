// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Text;

namespace FLisp.Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public async Task TestMethod1()
    {
        var sexprsStr = new StringBuilder();
        var cancellationToken = CancellationToken.None;

        sexprsStr.AppendLine("   ; test 1");
        sexprsStr.AppendLine("1");
        sexprsStr.AppendLine("a    ; method a");
        sexprsStr.AppendLine(@"""Hola mundo""");
        sexprsStr.AppendLine(@"(func1 a b 100 ""sum"")");

        using var stream = new StringReader(sexprsStr.ToString());

        var parser = new SExprParser(stream);
        var sexpr = await parser.ReadSExpr(cancellationToken);

        //Assert.AreEqual(result.Length, 3);
        Assert.AreEqual(sexpr, (Int128)1);

        sexpr = await parser.ReadSExpr(cancellationToken);
        Assert.AreEqual(sexpr, "a");

        sexpr = await parser.ReadSExpr(cancellationToken);
        Assert.AreEqual(sexpr, new SString("Hola mundo"));

        sexpr = await parser.ReadSExpr(cancellationToken);
        Assert.AreEqual(sexpr, new SList(new object?[] { "func1", "a", "b", 100, new SString("sum") }));
    }
}