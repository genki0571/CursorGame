using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public enum LobbyItemType
{
    Dustbin,        // ゴミ箱.
    Email,          // メール.
    Browser,        // ブラウザ.
    Setting,        // 設定.
    EmptyFolder,    // 空フォルダ.
    NonEmptyFolder, // 空でないフォルダ.
    Item1,          // 持ち物1.
    Item2,          // 持ち物2.
    Item3,          // 持ち物3.
    None,           // 何もない
}
public class LobbyItemBase : MonoBehaviour
{
    public LobbyItemType itemType = LobbyItemType.None;
    public LobbyItemBase itemObj;
}