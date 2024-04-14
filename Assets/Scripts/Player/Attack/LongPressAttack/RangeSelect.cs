using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelect : MonoBehaviour,IHoldAttacker
{
    const float MAX_LENGTH = 7;

    PlayerAttack playerAttack;

    GameObject holdDisplay;

    // Start is called before the first frame update
    void Start()
    {
        holdDisplay = GetComponent<PlayerAttack>().holdDisplay;
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(GameObject selectEnemy, Range range)
    {
        ISelectable selectable = selectEnemy.GetComponent<ISelectable>();
        if (selectable != null) 
        {
            bool already = false;
            for (int i = 0; i < playerAttack.selectingEnemies.Count; i++)
            {
                if (selectEnemy == playerAttack.selectingEnemies[i]) 
                {
                    already = true;
                }
            }

            if (!already)
            {
                playerAttack.selectingEnemies.Add(selectEnemy);
            }
        }
    }

    public Element GetElementKind(Range range)
    {
        return Element.Empty;
    }

    public float GetMaxLength()
    {
        return MAX_LENGTH;
    }
}
