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


public class PlayerAttack : MonoBehaviour
{
    static public PlayerAttack instance;
    PlayerState playerState => PlayerState.instance;

    PlayerInput playerInput => PlayerInput.instance;
    PCFieldController pcFieldController => PCFieldController.instance;

    ILeftAttacker leftAttacker;
    IHoldAttacker holdAttacker;
    public List<IRightAttacker> rightAttackers = new List<IRightAttacker>();

    Transform cursorTrans;
    Vector2 cursorPos;

    //
    public bool isLeftClick;
    public bool isRightClick;
    public bool isHoldStart;
    public bool isHold;
    public bool isHoldEnd;

    bool isRange;

    bool isGrabbing;
    IGrabbable grabbable;
    int grabCnt = 0;
    const int GRAB_FRAME_INTERVAL = 15;

    bool beforeHold;
    public Vector2 holdStartPos;
    public List<IDamagable> withinRangeDamagable;
    public List<GameObject> withinRangeEnemies;
    Range range;

    const int MIX_CNT = 10;
    [SerializeField]int mixCnt = 0;
    [SerializeField]bool isMix;

    enum CheckAxis { Begin, Virtical,Horizontal }

    enum Virtical {Empty,Upper, Lower}
    enum Horizontal {Empty,Right,Left }
    CheckAxis checkAxis = CheckAxis.Begin;
    Virtical beforeVirtical;
    Horizontal beforeHorizontal;

    public bool isSelectEnemy;
    public List<GameObject> selectingEnemies = new List<GameObject>();

    [SerializeField] public GameObject holdDisplay;
    [SerializeField] GameObject commandMenu;

    [System.NonSerialized]public bool isGrabSlider;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        leftAttacker = GetComponent<ILeftAttacker>();
        holdAttacker = GetComponent<IHoldAttacker>();
        IRightAttacker[] rAttackers = GetComponents<IRightAttacker>();
        for (int i = 0; i < rAttackers.Length; i++)
        {
            rightAttackers.Add(rAttackers[i]);
        }

        cursorTrans = GetComponent<Transform>();

        withinRangeDamagable = holdDisplay.GetComponent<HoldDisplay>().withinRangeDamagable;
        withinRangeEnemies = holdDisplay.GetComponent<HoldDisplay>().withinRangeEnemies;
    }

    // Update is called once per frame
    void Update()
    {

        //マウスの入力状態の取得
        GetMouseState();
        if (playerState.IsVirus(Virus.ClickInvert) && !playerState.isDead) 
        {
            bool temp = isRightClick;
            isRightClick = isLeftClick;
            isLeftClick = temp;
        }


        cursorPos = cursorTrans.position;

        if (!playerState.isDead) 
        {
            Ray2D ray = new Ray2D(transform.position, transform.forward);
            RaycastHit2D hit = new RaycastHit2D();
            if (isRightClick || isLeftClick || isHold || beforeHold)
            {
                int layerMask = ~(1 << LayerMask.NameToLayer("RangeCheck"));
                hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, layerMask);
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


                if (!playerState.IsVirus(Virus.CannotLeftClick))
                {
                    //攻撃を使用
                    if (!alreadyAttack)
                    {
                        Transform hitTrans = null;
                        GameObject hitObj = null;
                        if (hit.collider)
                        {
                            hitTrans = hit.transform;
                            hitObj = hit.transform.gameObject;
                        }

                        if (selectingEnemies.Count >= 2)
                        {
                            bool multiplePossible = false;
                            for (int i = 0; i < selectingEnemies.Count; i++)
                            {
                                if (selectingEnemies[i] == hitObj)
                                {
                                    multiplePossible = true;
                                    break;
                                }
                            }
                            Debug.Log(multiplePossible);
                            if (multiplePossible)
                            {
                                for (int i = 0; i < selectingEnemies.Count; i++)
                                {
                                    leftAttacker.Attack(cursorTrans, selectingEnemies[i].transform);
                                }
                            }
                        }
                        else
                        {
                            leftAttacker.Attack(cursorTrans, hitTrans);
                        }
                    }

                }


                //選択を解除
                selectingEnemies.Clear();
                commandMenu.SetActive(false);
            }

            //右クリックした時
            if (isRightClick)
            {
                if (!playerState.IsVirus(Virus.CannotRightClick))
                {
                    if (hit.collider)
                    {
                        if (button != null)
                        {
                            Command command = button.Pushed();
                            ActionCommand(command);

                            //選択を解除
                            selectingEnemies.Clear();
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
                        selectingEnemies.Clear();

                        commandMenu.transform.position = cursorPos;
                        commandMenu.SetActive(true);
                    }
                }

            }

            if (isHoldStart)
            {
                holdStartPos = cursorTrans.position;
                grabCnt = 0;
            }

            if (isHold)
            {
                if (!isRange && !isGrabbing && !isGrabSlider)
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

                if (playerState.IsVirus(Virus.CannnotHold))
                {
                    isRange = false;
                }
                if (playerState.IsVirus(Virus.CannotGrab))
                {
                    isGrabbing = false;
                }

                //範囲選択
                if (isRange)
                {
                    Vector2 dirX = new Vector2((cursorPos.x - holdStartPos.x), 0).normalized;
                    Vector2 dirY = new Vector2(0, (cursorPos.y - holdStartPos.y)).normalized;
                    float width = Mathf.Abs(cursorPos.x - holdStartPos.x);
                    float height = Mathf.Abs(cursorPos.y - holdStartPos.y);
                    float maxLength = holdAttacker.GetMaxLength();
                    if (width >= maxLength)
                    {
                        width = maxLength;
                    }
                    if (height >= maxLength)
                    {
                        height = maxLength;
                    }
                    Vector2 centerVec = new Vector2(dirX.x * (width / 2), dirY.y * (height / 2));
                    Vector2 centerPos = holdStartPos + centerVec;

                    holdDisplay.transform.position = centerPos;
                    holdDisplay.transform.localScale = new Vector2(width, height);
                    holdDisplay.SetActive(true);

                    if (cursorPos.x >= holdStartPos.x)
                    {
                        range = Range.Right;
                    }
                    else if (cursorPos.x <= holdStartPos.x)
                    {
                        range = Range.Left;
                    }

                    //Mix判定
                    IsMix();

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
                if (beforeHold)
                {
                    Debug.Log("長押し解除");

                    if (isRange)
                    {
                        //範囲選択攻撃
                        if (isMix)
                        {
                            if (holdAttacker.GetElementKind(Range.Mix) != Element.Empty)
                            {
                                range = Range.Mix;
                            }
                            holdAttacker.Attack(null, range);
                        }
                        else
                        {
                            for (int i = 0; i < withinRangeEnemies.Count; i++)
                            {
                                holdAttacker.Attack(withinRangeEnemies[i], range);
                            }
                        }
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
                                grabbable.Putting();
                            }
                        }
                        selectingEnemies.Clear();
                    }
                }
                isRange = false;
                isGrabbing = false;

                holdDisplay.SetActive(false);
            }
            beforeHold = isHold;

            int num = 0;
            if (selectingEnemies.Count != 0)
            {
                for (int i = 0; i < selectingEnemies.Count; i++)
                {
                    pcFieldController.selectTargets[i].position = selectingEnemies[i].transform.position;
                }
                num = selectingEnemies.Count;
            }

            for (int i = num; i < pcFieldController.selectTargets.Count; i++)
            {
                pcFieldController.selectTargets[i].localPosition = Vector3.zero;
            }

        }
    }

    void ActionCommand(Command command) 
    {
        if (command != Command.Empty)
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

                    rightAttackers[i].Command(commandMenu.transform.position, selectables);
                    break;
                }
            }
        }
        
    }

    private void GetMouseState() 
    {
        isLeftClick = playerInput.IsLeftClick();
        isRightClick = playerInput.IsRightClick();
        isHold = playerInput.IsHold();
        isHoldStart = playerInput.IsHoldStart();
        isHoldEnd = playerInput.IsHoldEnd();
    }

    private void IsMix() 
    {
        Virtical virtical = Virtical.Upper;
        if (cursorPos.y >= holdStartPos.y)
        {
            virtical = Virtical.Upper;
        }
        else if (cursorPos.y <= holdStartPos.y)
        {
            virtical = Virtical.Lower;
        }

        Horizontal horizontal = Horizontal.Right;
        if (cursorPos.x >= holdStartPos.x)
        {
            horizontal = Horizontal.Right;
        }
        else if (cursorPos.x <= holdStartPos.x)
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
            else if (beforeVirtical != virtical)
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
}
