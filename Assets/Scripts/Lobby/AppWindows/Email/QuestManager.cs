using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QuestManager : MonoBehaviour
{
    [SerializeField] CSVReader csvReader;
    [SerializeField] private TextAsset QUEST_DATA_FILE;
    static List<string[]> questDataList = new List<string[]>();
    private static int releasedQuestNum;
    private static int selectedQuestNum;
    private static bool isFirstOpen = true;

    [SerializeField] QuestBoard boardTop;
    [SerializeField] QuestBoard boardCenter;
    [SerializeField] QuestBoard boardBottom;
    [SerializeField] QuestBoard boardMain;
    QuestData[] questDatas;

    void Start()
    {
        if (isFirstOpen)
        {
            releasedQuestNum = 0;
            questDataList = csvReader.ReadCSV(QUEST_DATA_FILE);
            questDatas = QuestDataConverter(questDataList);
            isFirstOpen = false;
        }
        RefleshQuestBoard(selectedQuestNum);
        Debug.Log(questDatas[0].questTitle);
    }
    private void Update()
    {
        /*
        Debug.Log("r" + releasedQuestNum);
        Debug.Log("s" + selectedQuestNum);*/
        if (Input.GetKeyDown(KeyCode.Q) && releasedQuestNum < questDatas.Length-1)
        {
            releasedQuestNum++;
        }
        if (Input.GetKeyDown(KeyCode.W)&&releasedQuestNum>1)
        {
            releasedQuestNum--;
        }
    }
    private void RefleshQuestBoard(int currentNum)
    {
        boardCenter.SetQuestData(questDatas[currentNum]);
        boardCenter.gameObject.SetActive(true);
        if (currentNum == 0)
        {
            boardTop.gameObject.SetActive(false);
        }
        else
        {
            boardTop.SetQuestData(questDatas[currentNum - 1]);
            boardTop.gameObject.SetActive(true);
        }
        if (currentNum == releasedQuestNum)
        {
            boardBottom.gameObject.SetActive(false);
        }
        else
        {
            boardBottom.SetQuestData(questDatas[currentNum + 1]);
            boardBottom.gameObject.SetActive(true);
        }
        boardMain.SetQuestData(questDatas[currentNum]);
    }

    private QuestData[] QuestDataConverter(List<string[]> data)
    {
        int listCount = data.Count;
        QuestData[] convertedQuestData = new QuestData[listCount];

        // クエスト情報の書式に応じて変更する.
        for (int i = 0; i < listCount; i++)
        {
            convertedQuestData[i].questNum = Convert.ToInt32(questDataList[i][0]);
            convertedQuestData[i].questTitle     = questDataList[i][1];
            convertedQuestData[i].questOverview  = questDataList[i][2];
        }

        return convertedQuestData;
    }

    public void ClickUpButton()
    {
        if (selectedQuestNum > 0)
        {
            selectedQuestNum -= 1;
        }
        RefleshQuestBoard(selectedQuestNum);
    }
    public void ClickDownButton()
    {
        if (selectedQuestNum < releasedQuestNum)
        {
            selectedQuestNum += 1;
        }
        RefleshQuestBoard(selectedQuestNum);
    }
}
