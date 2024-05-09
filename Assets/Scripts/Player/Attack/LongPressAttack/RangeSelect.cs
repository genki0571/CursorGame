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

    public void Attack(List<GameObject> selectEnemies, Range range)
    {
        for (int i = 0; i < selectEnemies.Count; i++)
        {
            ISelectable selectable = selectEnemies[i].GetComponent<ISelectable>();
            if (selectable != null)
            {
                bool already = false;
                for (int j = 0; j < playerAttack.selectingEnemies.Count; j++)
                {
                    if (selectEnemies[i] == playerAttack.selectingEnemies[j])
                    {
                        already = true;
                    }
                }

                if (!already)
                {
                    playerAttack.selectingEnemies.Add(selectEnemies[i]);
                }
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
