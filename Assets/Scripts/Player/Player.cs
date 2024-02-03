using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    ILeftAttacker leftAttacker;


    //
    public bool isLeftClick;
    public bool isRightClick;
    public bool isLongPress;

    // Start is called before the first frame update
    void Start()
    {
        var leftAttack = gameObject.AddComponent<ClickAttack>();
        leftAttacker = leftAttack.GetComponent<ILeftAttacker>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray2D ray = new Ray2D(transform.position, transform.up);
        if (isLeftClick) 
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin , ray.direction ,1f);
            if (hit.collider) 
            {
                IButton button = hit.transform.GetComponent<IButton>();
                if (button != null)
                {
                    button.Pushed();
                }

            }

            //攻撃を使用
            leftAttacker.Attack(hit.transform);
        }
        if (isRightClick)
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1f);
            if (hit.collider)
            {

            }
        }
    }
}
