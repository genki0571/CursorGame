using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButton
{
    public Command Pushed()
    {
        Command command = Command.Empty;
        return command;
    }
}
