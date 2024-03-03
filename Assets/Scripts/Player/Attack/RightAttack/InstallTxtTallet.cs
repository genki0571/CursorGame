using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallTxtTallet : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.InstallTxt;
    PCFieldController pcFieldController => PCFieldController.instance;

    List<TxtTallet> txtTallets;

    // Start is called before the first frame update
    void Start()
    {

        txtTallets = pcFieldController.txtTallets;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables) 
    {
        for (int i = 0; i < txtTallets.Count; i++)
        {
            if (txtTallets[i].isSleep) 
            {
                txtTallets[i].Initialize(pos);
                break;
            }
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
