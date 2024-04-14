using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTextAttack : MonoBehaviour,IRightAttacker
{
    PCFieldController pcFieldController => PCFieldController.instance;

    Command command = global::Command.FallText;

    List<FallText> fallTexts;

    // Start is called before the first frame update
    void Start()
    {
        fallTexts = pcFieldController.fallTexts;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables) 
    {
        for (int j = 0; j < fallTexts.Count; j++)
        {
            if (fallTexts[j].isSleep)
            {
                fallTexts[j].Initialize(pos);
                break;
            }
        }
    }

    public Command GetCommandKind() 
    {
        return command;
    }
}
