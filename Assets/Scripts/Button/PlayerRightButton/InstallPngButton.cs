using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallPngButton : MonoBehaviour,IButton
{
    Command command = Command.InstallPng;

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
