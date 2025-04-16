using System;

public class DebugCommandAttribute : Attribute
{
    public string commandName;

    public DebugCommandAttribute(string commandName)
    {
        this.commandName = commandName;
    }
}
