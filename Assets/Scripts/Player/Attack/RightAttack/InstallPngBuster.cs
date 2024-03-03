using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallPngBuster : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.InstallPng;
    PCFieldController pcFieldController => PCFieldController.instance;

    List<PngBuster> pngBusters;

    // Start is called before the first frame update
    void Start()
    {

        pngBusters = pcFieldController.pngBusters;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        for (int i = 0; i < pngBusters.Count; i++)
        {
            if (pngBusters[i].isSleep)
            {
                pngBusters[i].Initialize(pos);
                break;
            }
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
