using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRightAttacker 
{
    public void Command(Vector3 pos,List<ISelectable> selectables) { }

    public Command GetCommandKind() 
    {
        Command command = global::Command.Empty;
        return command;
    }
}
