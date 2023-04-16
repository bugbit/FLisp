// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Runtime.Serialization;

namespace FLisp.Core;

public class SExprParserException : Exception
{
    public int? Line { get; init; }
    public int? Column { get; init; }

    public SExprParserException()
    {
    }

    public SExprParserException(string? message) : base(message)
    {
    }

    public SExprParserException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SExprParserException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
