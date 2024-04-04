using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private LobbyItemBase[,] lobbyItems = new LobbyItemBase[8,5];
    private bool isFirstDisplay = true;

    public static bool IsOpenwindow { get; private set; }

    private readonly float[] ITEM_DISPLAY_POS_X = { 7.7f, 5.5f, 3.3f, 1.1f, -1.1f, -3.3f, -5.5f, -7.7f };//配列でconstは使えないのでreadonlyで定数を宣言している.
    private readonly float[] ITEM_DISPLAY_POS_Y = { 4, 2, 0, -2, -4 };

    [Header("アイコンのprefab")]
    [Tooltip("ゴミ箱")]
    [SerializeField] private LobbyItemBase dustbin;
    [Tooltip("ブラウザ")]
    [SerializeField] private LobbyItemBase browser;
    [Tooltip("メール")]
    [SerializeField] private LobbyItemBase email;
    [Tooltip("設定")]
    [SerializeField] private LobbyItemBase setting;
    [Tooltip("空のフォルダ")]
    [SerializeField] private LobbyItemBase emptyFolder;
    [Tooltip("空でないフォルダ")]
    [SerializeField] private LobbyItemBase nonEmptyFolder;
    [Tooltip("アイテム1")]
    [SerializeField] private LobbyItemBase item1;
    [Tooltip("アイテム2")]
    [SerializeField] private LobbyItemBase item2;
    [Tooltip("アイテム3")]
    [SerializeField] private LobbyItemBase item3;

    [Header("アプリケーションウィンドウ")]
    [Tooltip("ゴミ箱")]
    [SerializeField] private GameObject dustbinWindow;
    [Tooltip("ブラウザ")]
    [SerializeField] private GameObject browserWindow;
    [Tooltip("メール")]
    [SerializeField] private GameObject emailWindow;
    [Tooltip("設定")]
    [SerializeField] private GameObject settingWindow;


    void Start()
    {
        if (isFirstDisplay)
        {
            //配列内のアイテムをNoneに初期化する.
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (lobbyItems[i,j] == null)lobbyItems[i, j] = gameObject.AddComponent<LobbyItemBase>();
                    lobbyItems[i, j].itemType = LobbyItemType.None;
                }
            }
            //画面の初期配置を以下に記述.
            lobbyItems[7, 0].itemType = LobbyItemType.Dustbin;
            lobbyItems[7, 1].itemType = LobbyItemType.Browser;
            lobbyItems[7, 2].itemType = LobbyItemType.Email;
            lobbyItems[7, 3].itemType = LobbyItemType.Setting;

            isFirstDisplay = false;
        }
        RefreshScreenIcons();
    }

    public LobbyItemType GetItemType(LobbyPosition pos)
    {
        return lobbyItems[pos.x, pos.y].itemType;
    }
    public void SetItemType(LobbyPosition pos, LobbyItemType itemType)
    {
        lobbyItems[pos.x, pos.y].itemType = itemType;
    }
    /// <summary>
    /// アイコンの表示状況を更新する.
    /// 多分重いのであまり高頻度で使わないようにすること.
    /// </summary>
    public void RefreshScreenIcons()
    {
        //画面からアイコンを消去.
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (lobbyItems[i, j].itemObj != null) LobbyItemPool.instance.ReleaseGameObject(lobbyItems[i, j].itemObj);
            }
        }
        //アイコンを画面に描画.
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                switch (lobbyItems[i,j].itemType)
                {
                    case LobbyItemType.Dustbin:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(dustbin,new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j],0), Quaternion.identity);
                        break;
                    case LobbyItemType.Browser:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(browser, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.Email:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(email, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.Setting:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(setting, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.EmptyFolder:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(emptyFolder, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.NonEmptyFolder:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(nonEmptyFolder, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.Item1:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(item1, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.Item2:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(item2, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                    case LobbyItemType.Item3:
                        lobbyItems[i, j].itemObj = LobbyItemPool.instance.GetGameObject(item3, new Vector3(ITEM_DISPLAY_POS_X[i], ITEM_DISPLAY_POS_Y[j], 0), Quaternion.identity);
                        break;
                }
            }
        }
    }
}
