using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExtendedError: Exception
{
    private string message;
    private object data;

    public ExtendedError(string message, object data) : base(message)
    {
        this.message = message;
        this.data = data;
    }

    public override string ToString()
    {
        return message + ", data: " + data.ToString();
    }
}
