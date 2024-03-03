using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTextAttack : MonoBehaviour,IRightAttacker
{
    Command command = global::Command.FallText;

    const float ATTACK_INTERVAL = 0.5f;
    float attackTimer = 0;

    Vector2 FallPos = Vector2.zero;
    bool isFall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFall) 
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= ATTACK_INTERVAL) 
            {

                isFall = false;
            }
        }
    }

    public void Command(Vector3 pos, List<ISelectable> selectDamagables) 
    {
        Debug.Log("FallText");
        FallPos = pos;
        isFall = true;
    }

    public Command GetCommandKind() 
    {
        return command;
    }
}
