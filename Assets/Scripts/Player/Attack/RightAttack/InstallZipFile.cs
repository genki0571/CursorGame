using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallZipFile : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.InstallZip;
    PCFieldController pcFieldController => PCFieldController.instance;

    List<ZipFile> zipFiles;

    // Start is called before the first frame update
    void Start()
    {

        zipFiles = pcFieldController.zipFiles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables)
    {
        for (int i = 0; i < zipFiles.Count; i++)
        {
            if (zipFiles[i].isSleep)
            {
                zipFiles[i].Initialize(pos);
                break;
            }
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
