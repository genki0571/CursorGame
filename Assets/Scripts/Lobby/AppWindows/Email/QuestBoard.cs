using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct QuestData
{
    public int questNum;            // クエスト番号(0スタート)
    public string questTitle;       // クエストタイトル.
    public string questOverview;    // クエスト概要(説明文).
}
public class QuestBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questTitileText;
    [SerializeField] TextMeshProUGUI questOverviewText;

    private QuestData questData;

    public void SetQuestData(QuestData data)
    {
        questData = data;
    }

    public QuestData GetQuestData()
    {
        return questData;
    }

}
