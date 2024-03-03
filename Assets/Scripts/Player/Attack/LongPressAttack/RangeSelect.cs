using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSelect : MonoBehaviour,ILongPressAttacker
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
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
            for (int i = 0; i < player.selectingEnemies.Count; i++)
            {
                if (selectEnemy == player.selectingEnemies[i]) 
                {
                    already = true;
                }
            }

            if (!already)
            {
                player.selectingEnemies.Add(selectEnemy);
            }
        }
    }

    public Element GetElementKind(Range range)
    {
        return Element.Empty;
    }
}
