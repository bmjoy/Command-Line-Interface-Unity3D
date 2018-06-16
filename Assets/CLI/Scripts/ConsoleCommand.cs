using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method)]
public class ConsoleCommand : Attribute
{

    public ConsoleCommand() { }

    public ConsoleCommand(string customPath)
    {
        this.customPath = customPath;
    }

    public string customPath = String.Empty;

}
