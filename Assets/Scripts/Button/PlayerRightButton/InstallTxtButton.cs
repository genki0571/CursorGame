using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallTxtButton : MonoBehaviour,IButton
{
    Command command = Command.InstallTxt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Command Pushed()
    {
        return command;
    }
}
