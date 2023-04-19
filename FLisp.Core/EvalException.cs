// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Runtime.Serialization;

namespace FLisp.Core;

public class EvalException : Exception
{
    public int? NumberArgument { get; init; }
    public object? Expr { get; init; }

    public EvalException()
    {
    }

    public EvalException(string? message) : base(message)
    {
    }

    public EvalException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected EvalException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}