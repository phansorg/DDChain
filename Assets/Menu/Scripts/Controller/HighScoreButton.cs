using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreButton : MonoBehaviour
{
    string[] NameLabel = { "AllColorNameLabel", "SingleColorNameLabel" };
    string[] ScoreLabel = { "AllColorScoreLabel", "SingleColorScoreLabel" };

    public void Start()
    {
    }

    public void Update()
    {
        Text label;
        Vector3 pos;

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            if (scoreManager.fetchData[scoreKind].flag == false)
            {
                continue;
            }
            scoreManager.fetchData[scoreKind].flag = false;

            label = GameObject.Find("AllColorLabel").GetComponent<Text>();
            pos = label.transform.position;
            pos.x %= Screen.width;
            label.transform.position = pos;

            label = GameObject.Find("SingleColorLabel").GetComponent<Text>();
            pos = label.transform.position;
            pos.x %= Screen.width;
            label.transform.position = pos;

            for (int idx = 0; idx < ScoreManager.RANK_MAX; idx++)
            {
                label = GameObject.Find(NameLabel[scoreKind] + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Name;
                pos = label.transform.position;
                pos.x %= Screen.width;
                label.transform.position = pos;

                label = GameObject.Find(ScoreLabel[scoreKind] + idx).GetComponent<Text>();
                label.text = "" + scoreManager.fetchData[scoreKind].scoreDataList[idx].Score;
                pos = label.transform.position;
                pos.x %= Screen.width;
                label.transform.position = pos;
            }
        }
    }

    public void OnClick()
    {
        TitleController.WriteData();

        DataManager dataManager = DataManager.Instance;

        ScoreDataV1 param = new ScoreDataV1();

        param.Id = dataManager.UserData.Id;
        param.Name = dataManager.UserData.Name;
        param.Row = dataManager.PuzzleData.Row;
        param.Col = dataManager.PuzzleData.Col;
        param.Color = dataManager.PuzzleData.Color;
        param.Link = dataManager.PuzzleData.Link;
        param.Direction = dataManager.PuzzleData.Direction;
        param.Time = dataManager.PuzzleData.Time;
        param.Stop = dataManager.PuzzleData.Stop;
        param.CountDisp = dataManager.PuzzleData.CountDisp;
        param.Garbage = dataManager.PuzzleData.Garbage;

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            param.ScoreKindValue = scoreKind;
            scoreManager.fetchTopRankers(scoreKind, param);
        }

    }

}
