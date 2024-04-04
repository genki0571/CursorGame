using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallZipButton : MonoBehaviour,IButton
{
    Command command = Command.InstallZip;

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
