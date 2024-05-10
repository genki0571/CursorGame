using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CSVReader : MonoBehaviour
{

    TextAsset csvFile; // CSVファイル
    List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト;

    public List<string[]> ReadCSV(TextAsset textAsset)
    {
        try
        {
            //csvFile = Resources.Load(csvFileName) as TextAsset; // Resouces下のCSV読み込み
            if (textAsset != null)
            {
                csvFile = textAsset;
            }
            else
            {
                throw new  NullReferenceException("CSVファイルが指定されていません");
            }
            StringReader reader = new StringReader(csvFile.text);
            // , で分割しつつ一行ずつ読み込み
            // リストに追加していく
            while (reader.Peek() != -1) // reader.Peaekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                csvDatas.Add(line.Split(',')); // , 区切りでリストに追加
            }
            return csvDatas;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}