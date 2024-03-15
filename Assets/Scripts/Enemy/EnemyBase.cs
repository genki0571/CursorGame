using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    [System.NonSerialized]public float hp;
    public float maxHp;

    [System.NonSerialized] public SpriteRenderer renderer;

    EnemyUIGenerator uiGenerator => EnemyUIGenerator.instance;
    [SerializeField] List<DamageDisplay> damageDisplays;
    [System.NonSerialized] public Image hpImage;

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
        renderer.enabled = true;
        hp = maxHp;

        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(true);
    }

    public void Reset()
    {
        state = State.Sleep;
        renderer.enabled = false;

        GameObject canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);

        transform.position = new Vector3(0,50,0);
    }

    /// <summary>
    /// ダメージの数値を表記する
    /// </summary>
    /// <param name="pos">表示場所</param>
    /// <param name="damage">ダメージ量</param>
    public void DamageDisplay(Vector3 pos, float damage)
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

    /// <summary>
    /// HPを表示する
    /// </summary>
    public void HpDisplay() 
    {
        hpImage.fillAmount = (hp/maxHp);
    }
}
