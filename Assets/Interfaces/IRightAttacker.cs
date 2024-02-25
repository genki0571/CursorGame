using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRightAttacker 
{
    public void Command(Transform cursorTrans) { }

    public Command GetCommandKind() 
    {
        Command command = global::Command.Empty;
        return command;
    }
}
