// Copyright © 2023 Oscar Hernandez Baño. All rights reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// This file is part of FLisp.

using System.Runtime.Serialization;

namespace FLisp.Core;

public class VariableAlreadyDeclaredException : Exception
{
    public VariableAlreadyDeclaredException()
    {
    }

    public VariableAlreadyDeclaredException(string? message) : base(message)
    {
    }

    public VariableAlreadyDeclaredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected VariableAlreadyDeclaredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
