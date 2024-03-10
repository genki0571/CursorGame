using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public float hp;
    public float maxHp;

    EnemyUIGenerator uiGenerator => EnemyUIGenerator.instance;
    [SerializeField] List<DamageDisplay> damageDisplays;
    public Image hpSlider;

    public enum State
    {
        None,
        StateDecide,
        Sleep,
        Grabed,
        GoServer,
        Attack,
        Stop,
        Death
    }
    public State state;

    public Element haveElement;


    public void Initialize()
    {
        state = State.StateDecide;
    }

    public void Reset()
    {
        state = State.Sleep;
    }

    public void HpDisplay(Vector3 pos, float damage)
    {
        damageDisplays = uiGenerator.damageDisplays;

        for (int i = 0; i < damageDisplays.Count; i++)
        {
            if (damageDisplays[i].isSleep)
            {
                damageDisplays[i].Initialize(pos,damage);
                break;
            }
        }
    }
}
