using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAttack : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.Delete;

    const float PROPABILLITY = 20;

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
            int rand = Random.Range(0,101);
            if (rand <= PROPABILLITY)
            {
                selectDamagables[i].Delete();
            }
        }
    }

    public Command GetCommandKind()
    {
        return command;
    }
}
