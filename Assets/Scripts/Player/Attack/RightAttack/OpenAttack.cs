using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAttack : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.Open;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        for (int i = 0; i < selectDamagables.Count; i++)
        {
            selectDamagables[i].Open();
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
