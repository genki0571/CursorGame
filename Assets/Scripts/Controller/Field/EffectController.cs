using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EffectController : MonoBehaviour
{
    static public EffectController instance;
    PlayerSkill playerSkill => PlayerSkill.instance;

    //元のObjPool
    [SerializeField] Transform effectPool;

    //Prefabs
    [SerializeField] VisualEffectAsset clickEffectAsset;
    [SerializeField] VisualEffectAsset clickCriticalEffectAsset;
    [SerializeField] VisualEffectAsset hammerEffectAsset;

    [SerializeField] VisualEffectAsset fireAreaEffectAsset;


    //List
    private List<VisualEffect> originalVisualEffects = new List<VisualEffect>();

    public List<VisualEffect> clickEffects = new List<VisualEffect>();
    public List<VisualEffect> clickCriticalEffects = new List<VisualEffect>();
    public List<VisualEffect> hammerEffects = new List<VisualEffect>();

    public List<VisualEffect> fireAreaEffects = new List<VisualEffect>();

    private void Awake()
    {
        instance = this;

        VisualEffect[] effects = effectPool.GetComponentsInChildren<VisualEffect>();
        for (int i = 0; i < effects.Length; i++)
        {
            originalVisualEffects.Add(effects[i]);
        }

        int num = 0;

        //設置系のものを生成しておく
        if (playerSkill.rightLevel >= 0)
        {
            //FallText
        }
        if (playerSkill.rightLevel >= 1 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //TXTタレット
        }
        if (playerSkill.rightLevel >= 2 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //PNGバスター
        }
        if (playerSkill.rightLevel >= 3 && playerSkill.rightSkill == RightSkill.Installation)
        {
            //ZIPファイル
        }
        if (playerSkill.rightSkill == RightSkill.FireWall)
        {
            //ファイアウォール
        }

        //LeftClick
        if (playerSkill.leftSkill == LeftSkill.Hammer)
        {
            //ハンマー
            for (int i = 0; i < 5; i++)
            {
                hammerEffects.Add(originalVisualEffects[num + i]);
                hammerEffects[i].visualEffectAsset = hammerEffectAsset;
            }
            num += hammerEffects.Count;
        }
        if (playerSkill.leftSkill == LeftSkill.LoadThunder)
        {
            //ロードサンダー
        }

        //クリック
        for (int i = 0; i < 10; i++)
        {
            clickEffects.Add(originalVisualEffects[num + i]);
            clickEffects[i].visualEffectAsset = clickEffectAsset;
            clickEffects[i].transform.localScale = new Vector3(2, 2, 2);
        }
        num += clickEffects.Count;

        //クリティカルクリック
        for (int i = 0; i < 10; i++)
        {
            clickCriticalEffects.Add(originalVisualEffects[num + i]);
            clickCriticalEffects[i].visualEffectAsset = clickCriticalEffectAsset;
            clickCriticalEffects[i].transform.localScale = new Vector3(2,2,2);
        }
        num += clickCriticalEffects.Count;

        if (playerSkill.longPressSkill == LongPressSkill.FireAndIce) 
        {
            for (int i = 0; i < 5; i++)
            {
                fireAreaEffects.Add(originalVisualEffects[num + i]);
                VisualEffect eff = fireAreaEffects[i];
                eff.visualEffectAsset = fireAreaEffectAsset;
                eff.transform.GetComponent<Renderer>().sortingOrder = -1;
                eff.transform.localEulerAngles = new Vector3(50,0,0);
                eff.Stop();
            }
            num += fireAreaEffects.Count;
        }

        //WeekPoint


        //SelectTarget


        //EnemyAttack

    }

    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(List<VisualEffect> list ,Vector3 pos) 
    {
        list[0].transform.position = pos;
        list[0].Play();
        VisualEffect temp = list[0];
        list.Remove(temp);
        list.Add(temp);
    }
}
