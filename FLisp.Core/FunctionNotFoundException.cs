// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Runtime.Serialization;

namespace FLisp.Core;

public class FunctionNotFoundException : Exception
{
    public FunctionNotFoundException()
    {
    }

    public FunctionNotFoundException(string? message) : base(message)
    {
    }

    public FunctionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FunctionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
