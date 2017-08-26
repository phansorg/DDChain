using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoreButton : MonoBehaviour
{
    LeaderBoard[] leaderBoard;
    string[] NameLabel = { "AllColorNameLabel", "SingleColorNameLabel" };
    string[] ScoreLabel = { "AllColorScoreLabel", "SingleColorScoreLabel" };

    public void Start()
    {
        leaderBoard = new LeaderBoard[ScoreDataV1.SCORE_KIND_MAX];
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            leaderBoard[scoreKind] = new LeaderBoard();
        }
    }

    public void Update()
    {
        Text label;
        Vector3 pos;

        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            if (leaderBoard[scoreKind].fetchFlag == false)
            {
                continue;
            }
            leaderBoard[scoreKind].fetchFlag = false;

            label = GameObject.Find("AllColorLabel").GetComponent<Text>();
            pos = label.transform.position;
            pos.x %= Screen.width;
            label.transform.position = pos;

            label = GameObject.Find("SingleColorLabel").GetComponent<Text>();
            pos = label.transform.position;
            pos.x %= Screen.width;
            label.transform.position = pos;

            for (int idx = 0; idx < LeaderBoard.RANK_MAX; idx++)
            {
                label = GameObject.Find(NameLabel[scoreKind] + idx).GetComponent<Text>();
                label.text = leaderBoard[scoreKind].scoreDataList[idx].Name;
                pos = label.transform.position;
                pos.x %= Screen.width;
                label.transform.position = pos;

                label = GameObject.Find(ScoreLabel[scoreKind] + idx).GetComponent<Text>();
                label.text = "" + leaderBoard[scoreKind].scoreDataList[idx].Score;
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
        
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            param.ScoreKindValue = scoreKind;
            leaderBoard[scoreKind].fetchTopRankers(param);
        }

    }

}
