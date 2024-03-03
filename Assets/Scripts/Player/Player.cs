using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Command 
{
    Empty,
    Open,
    FallText,
    FireWall,
    Scan,
    Delete,
    EnemyPaste,
    InstallTxt,
    InstallPng,
    InstallZip
}

public enum Element 
{
    Empty,
    Select,
    Normal,
    Fire,
    Ice,
    Thunder,
    Wind,
    FireAndIce,
    ThunderAndWind
}

public enum Range 
{
    Right,
    Left,
    Mix
}


public class Player : MonoBehaviour
{
    ILeftAttacker leftAttacker;
    ILongPressAttacker longPressAttacker;
    public List<IRightAttacker> rightAttackers = new List<IRightAttacker>();

    Transform cursorTrans;
    Vector2 cursorPos;

    //
    public bool isLeftClick;
    public bool isRightClick;
    public bool isLongPressDown;
    public bool isLongPress;

    bool isRange;

    bool isGrabbing;
    IGrabbable grabbable;
    int grabCnt = 0;
    const int GRAB_FRAME_INTERVAL = 15;

    bool beforeLongPress;
    Vector2 longPressStartPos;
    public List<IDamagable> withinRangeDamagable;
    public List<GameObject> withinRangeEnemies;
    Range range;

    const float MIX_INTERVAL_FRAME = 10;
    const int MIX_CNT = 10;
    int mixCnt = 0;

    enum CheckAxis { Begin, Virtical,Horizontal }

    enum Virtical {Empty,Upper, Lower}
    enum Horizontal {Empty,Right,Left }
    CheckAxis checkAxis = CheckAxis.Begin;
    Virtical beforeVirtical;
    Horizontal beforeHorizontal;
    bool isMix;

    public bool isSelectEnemy;
    public List<GameObject> selectingEnemies = new List<GameObject>();

    [SerializeField] GameObject longPressDisplay;
    [SerializeField] GameObject commandMenu;

    // Start is called before the first frame update
    void Start()
    {
        //仮
        var leftAttack = gameObject.AddComponent<LoadTunderFirstAttack>();
        var longPressAttack = gameObject.AddComponent<RangeSelect>();
        var rightAttack0 = gameObject.AddComponent<FallTextAttack>();
        var rightAttack1 = gameObject.AddComponent<InstallTxtTallet>();
        var rightAttack2 = gameObject.AddComponent<OpenAttack>();
        var rightAttack3 = gameObject.AddComponent<DeleteAttack>();
        var rightAttack4 = gameObject.AddComponent<InstallPngBuster>();
        var rightAttack5 = gameObject.AddComponent<FireWallFirstAttack>();
        var rightAttack6 = gameObject.AddComponent<ScanWeakPoint>();
        leftAttacker = leftAttack.GetComponent<ILeftAttacker>();
        longPressAttacker = longPressAttack.GetComponent<ILongPressAttacker>();
        rightAttackers.Add(rightAttack0);
        rightAttackers.Add(rightAttack1);
        rightAttackers.Add(rightAttack2);
        rightAttackers.Add(rightAttack3);
        rightAttackers.Add(rightAttack4);
        rightAttackers.Add(rightAttack5);
        rightAttackers.Add(rightAttack6);

        cursorTrans = GetComponent<Transform>();

        withinRangeDamagable = longPressDisplay.GetComponent<LongPressDisplay>().withinRangeDamagable;
        withinRangeEnemies = longPressDisplay.GetComponent<LongPressDisplay>().withinRangeEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        cursorPos = cursorTrans.position;

        Ray2D ray = new Ray2D(transform.position, transform.up);
        RaycastHit2D hit = new RaycastHit2D();
        if (isRightClick || isLeftClick || isLongPress || beforeLongPress) 
        {
            int layerMask = ~(1 << LayerMask.NameToLayer("RangeCheck"));
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1f,layerMask);
        }
        IButton button = null;
        IDamagable damagable = null;
        ISelectable selectable = null;
        if (hit.collider) 
        {
            button = hit.transform.GetComponent<IButton>();
            damagable = hit.transform.GetComponent<IDamagable>();
            selectable = hit.transform.GetComponent<ISelectable>();
        }

        if (selectingEnemies.Count == 0)
        {
            isSelectEnemy = false;
        }
        else 
        {
            isSelectEnemy = true;
        }

        //左クリックしたとき
        if (isLeftClick) 
        {
            bool alreadyAttack = false;

            if (hit.collider)
            {
                if (button != null)
                {
                    Command command = button.Pushed();
                    ActionCommand(command);
                    alreadyAttack = true;
                }
            }

            //選択を解除
            selectingEnemies = new List<GameObject>();
            commandMenu.SetActive(false);

            //攻撃を使用
            if (!alreadyAttack)
            {
                Transform hitTrans = null;
                if (hit.collider) 
                {
                    hitTrans = hit.transform.GetComponent<Transform>();
                }
                leftAttacker.Attack(cursorTrans, hitTrans);
            }
        }

        //右クリックした時
        if (isRightClick)
        {
            if (hit.collider)
            {
                if (button != null)
                {
                    Command command = button.Pushed();
                    ActionCommand(command);

                    //選択を解除
                    selectingEnemies = new List<GameObject>();
                    commandMenu.SetActive(false);
                }
                else 
                {
                    commandMenu.transform.position = cursorPos;
                    commandMenu.SetActive(true);
                }

                if (selectable != null) 
                {
                    bool already = false;
                    for (int i = 0; i < selectingEnemies.Count; i++)
                    {
                        if (hit.transform.gameObject == selectingEnemies[i])
                        {
                            already = true;
                        }
                    }

                    if (!already)
                    {
                        selectingEnemies.Add(hit.transform.gameObject);
                    }
                }
            }
            else 
            {
                //選択を解除
                selectingEnemies = new List<GameObject>();

                commandMenu.transform.position = cursorPos;
                commandMenu.SetActive(true);
            }
        }

        if (isLongPressDown)
        {
            longPressStartPos = cursorTrans.position;
            grabCnt = 0;

        }

        if (isLongPress)
        {

            if (!isRange && !isGrabbing) 
            {
                if (hit.collider)
                {
                    grabbable = hit.transform.GetComponent<IGrabbable>();
                    if (grabbable != null)
                    {
                        grabCnt += 1;
                        if (grabCnt >= GRAB_FRAME_INTERVAL) 
                        {
                            isGrabbing = true;
                        }
                    }
                    else
                    {
                        isRange = true;
                        isGrabbing = false;
                    }
                }
                else
                {
                    isRange = true;
                    isGrabbing = false;
                }
            }

            //範囲選択
            if (isRange) 
            {
                float width = Mathf.Abs(cursorPos.x - longPressStartPos.x);
                float height = Mathf.Abs(cursorPos.y - longPressStartPos.y);
                Vector2 centerPos = longPressStartPos + ((cursorPos - longPressStartPos) / 2);

                longPressDisplay.transform.position = centerPos;
                longPressDisplay.transform.localScale = new Vector2(width, height);
                longPressDisplay.SetActive(true);

                if (cursorPos.x >= longPressStartPos.x)
                {
                    range = Range.Right;
                }
                else if(cursorPos.x <= longPressStartPos.x)
                {
                    range = Range.Left;
                }


                //Mix判定
                Virtical virtical = Virtical.Upper;
                if (cursorPos.y >= longPressStartPos.y)
                {
                    virtical = Virtical.Upper;
                }
                else if (cursorPos.y <= longPressStartPos.y)
                {
                    virtical = Virtical.Lower;
                }

                Horizontal horizontal = Horizontal.Right;
                if (cursorPos.x >= longPressStartPos.x) 
                {
                    horizontal = Horizontal.Right;
                }
                else if(cursorPos.x <= longPressStartPos.x) 
                {
                    horizontal = Horizontal.Left;
                }

                if (beforeVirtical == Virtical.Empty) 
                {
                    beforeVirtical = virtical;
                }
                if (beforeHorizontal == Horizontal.Empty) 
                {
                    beforeHorizontal = horizontal;
                }


                if (checkAxis == CheckAxis.Begin) 
                {
                    if (beforeHorizontal != horizontal)
                    {
                        checkAxis = CheckAxis.Virtical;
                        mixCnt += 1;
                    }
                    else if(beforeVirtical != virtical)
                    {
                        checkAxis = CheckAxis.Horizontal;
                        mixCnt += 1;
                    }
                }
                else if (checkAxis == CheckAxis.Virtical)
                {
                    if (beforeVirtical != virtical)
                    {
                        checkAxis = CheckAxis.Horizontal;
                        mixCnt += 1;
                    }
                }
                else if (checkAxis == CheckAxis.Horizontal)
                {
                    if (beforeHorizontal != horizontal)
                    {
                        checkAxis = CheckAxis.Virtical;
                        mixCnt += 1;
                    }
                }

                beforeVirtical = virtical;
                beforeHorizontal = horizontal;

                if (mixCnt >= MIX_CNT)
                {
                    isMix = true;
                }
                else 
                {
                    isMix = false;
                }

            }

            //つかむ
            if (isGrabbing)
            {
                grabbable.Grabbing(cursorTrans);
                for (int i = 0; i < selectingEnemies.Count; i++)
                {
                    IGrabbable grabbable = selectingEnemies[i].GetComponent<IGrabbable>();
                    if (grabbable != null) 
                    {
                        grabbable.Grabbing(cursorTrans);
                    }
                }
            }
        }
        else 
        {
            if (beforeLongPress)
            {
                Debug.Log("長押し解除");

                //範囲選択攻撃
                if (isMix)
                {
                    if (longPressAttacker.GetElementKind(Range.Mix) != Element.Empty)
                    {
                        range = Range.Mix;
                    }
                }
                for (int i = 0; i < withinRangeEnemies.Count; i++)
                {
                    longPressAttacker.Attack(withinRangeEnemies[i], range);
                }
                //Mix関連の情報の初期化
                beforeHorizontal = Horizontal.Empty;
                beforeVirtical = Virtical.Empty;
                isMix = false;
                mixCnt = 0;


                //コマンド選択
                if (hit.collider)
                {
                    if (button != null)
                    {
                        Command command = button.Pushed();
                        ActionCommand(command);
                        selectingEnemies = new List<GameObject>();
                    }
                }
                commandMenu.SetActive(false);

                if (isGrabbing) 
                {
                    grabbable.Putting();
                    for (int i = 0; i < selectingEnemies.Count; i++)
                    {
                        IGrabbable grabbable = selectingEnemies[i].GetComponent<IGrabbable>();
                        if (grabbable != null)
                        {
                            grabbable.Grabbing(cursorTrans);
                        }
                    }
                    selectingEnemies = new List<GameObject>();
                }
            }
            isRange = false;
            isGrabbing = false;

            longPressDisplay.SetActive(false);
        }
        beforeLongPress = isLongPress;
    }

    void ActionCommand(Command command) 
    {
        for (int i = 0; i < rightAttackers.Count; i++)
        {
            if (command == rightAttackers[i].GetCommandKind()) 
            {
                List<ISelectable> selectables = new List<ISelectable>();
                for (int j = 0; j < selectingEnemies.Count; j++)
                {
                    ISelectable selectable = selectingEnemies[j].GetComponent<ISelectable>();
                    if (selectable != null) 
                    {
                        selectables.Add(selectable);
                    }
                }

                rightAttackers[i].Command(commandMenu.transform.position,selectables);
            }
        }
    }
}
