// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

namespace FLisp.Core;

using System;
using System.IO;
using System.Text;

public class SExprParser
{
    private enum EReadSExprFlags
    {
        SExpr, BeginList, EndList
    }

    //private static readonly Regex sExprRegEx = new Regex(@"(?<token>[\(\)]|""(?<texto>[^""]*)""|\S+)", RegexOptions.Compiled);
    private TextReader reader;
    private string? line;
    private int lineNumber = 0;
    private int position = 0;

    public bool EOF { get; private set; }

    public SExprParser(TextReader reader) => this.reader = reader;

    public async Task<object?> ReadSExpr(CancellationToken cancellationToken) => (await ReadSExprInternal(EReadSExprFlags.SExpr, cancellationToken)).sexpr;

    private async Task<(EReadSExprFlags flagsEnd, object? sexpr)> ReadSExprInternal(EReadSExprFlags flags, CancellationToken cancellationToken)
    {
        var token = await ReadToken(cancellationToken);

        if (token == null)
            return (flags, null);

        if (string.Equals(token, SExpr.NullStr, StringComparison.OrdinalIgnoreCase))
            return (flags, null);

        if (token.Length > 0)
        {
            var car1 = token[0];

            if (char.IsDigit(car1))
            {
                if (Int128.TryParse(token, out var numInt))
                    return (flags, numInt);

                throw new NotImplementedException();
            }

            return car1 switch
            {
                SExpr.DobleQuoteChar => (flags, new SString(token.Substring(1, token.Length - 2))),
                SExpr.BeginListChar => (flags, await ReadList(cancellationToken)),
                SExpr.EndListChar => flags == EReadSExprFlags.BeginList ? (EReadSExprFlags.EndList, null) : throw new SExprParserException($"Not expected '{token}'") { Line = lineNumber, Column = position },
                _ => (flags, token)
            };

            //if (car1 == SExpr.DobleQuoteChar)
            //    return (flags, new SString(token.Substring(1, token.Length - 2)));
        }

        return (flags, token);
    }

    private async Task<SList> ReadList(CancellationToken cancellationToken)
    {
        (int line, int col) lineCol0 = (lineNumber, position++);
        var sexprs = new List<object?>();
        (EReadSExprFlags flagsEnd, object? sexpr) sexpr = (EReadSExprFlags.BeginList, null);
        bool exit;

        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            sexpr = await ReadSExprInternal(sexpr.flagsEnd, cancellationToken);
            if (EOF)
                throw new SExprParserException($"Last opening parenthesis probably in line {lineCol0.line} column {lineCol0.col}") { Line = lineNumber, Column = position };

            exit = sexpr.flagsEnd == EReadSExprFlags.EndList;
            if (!exit)
                sexprs.Add(sexpr.sexpr);
        } while (!exit);

        return new(sexprs);
    }

    private async Task<string?> ReadToken(CancellationToken cancellationToken)
    {
        for (; ; )
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (line == null)
            {
                if (!await ReadLine())
                    return null;
            }
            while (position < line!.Length && char.IsSeparator(line[position])) position++;
            if (position < line!.Length)
            {
                var car1 = line[position];

                if (car1 == SExpr.CommentChar)
                {
                    line = null;

                    continue;
                }

                return car1 switch
                {
                    SExpr.BeginListChar or SExpr.EndListChar => car1.ToString(),
                    SExpr.DobleQuoteChar => ReadString(cancellationToken),
                    _ => ReadWord(cancellationToken)
                };
            }
            else
                line = null;
        }
    }

    private string ReadWord(CancellationToken cancellationToken)
    {
        var word = new StringBuilder();
        char car;

        while (position < line!.Length && !char.IsSeparator((car = line[position])))
        {
            cancellationToken.ThrowIfCancellationRequested();

            word.Append(car);
            position++;
        }

        return word.ToString();
    }

    private string ReadString(CancellationToken cancellationToken)
    {
        var theString = new StringBuilder();

        if (position < line!.Length)
        {
            theString.Append(line[position++]);
            while (position < line!.Length)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var car = line[position++];

                if (car == SExpr.DobleQuoteChar)
                {
                    theString.Append(car);

                    break;
                }

                if (car == '\\' && position < line!.Length && line[position] == SExpr.DobleQuoteChar)
                    car = line[position++];

                theString.Append(car);
            }
        }

        return theString.ToString();
    }

    private async Task<bool> ReadLine()
    {
        if (EOF)
            return false;

        line = await reader.ReadLineAsync();

        if (line == null)
        {
            EOF = true;

            return false;
        }

        lineNumber++;
        position = 0;

        return true;
    }
}