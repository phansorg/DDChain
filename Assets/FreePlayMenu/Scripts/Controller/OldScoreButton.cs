using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OldScoreButton : MonoBehaviour
{
    public void Start()
    {
    }

    public void Update()
    {
        Button button;
        Text label;

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            if (scoreManager.version != 0 ||
                scoreManager.fetchData[scoreKind].flag == false)
            {
                continue;
            }
            scoreManager.fetchData[scoreKind].flag = false;

            Action<Transform, bool> SetPosition = (transform, visible) =>
            {
                Vector3 pos;
                pos = transform.position;
                pos.x %= Screen.width;
                if (visible == false)
                {
                    pos.x += Screen.width;
                }
                transform.position = pos;
            };

            label = GameObject.Find(FreePlayMenuController.LabelName[scoreKind]).GetComponent<Text>();
            SetPosition(label.transform, true);

            for (int idx = 0; idx < ScoreManager.RANK_MAX; idx++)
            {
                // スコア表示
                button = GameObject.Find(FreePlayMenuController.ReplayButtonName[scoreKind] + idx).GetComponent<Button>();
                SetPosition(button.transform, false);

                label = GameObject.Find(FreePlayMenuController.NameLabelName[scoreKind] + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Name;
                SetPosition(label.transform, true);

                label = GameObject.Find(FreePlayMenuController.ScoreLabelName[scoreKind] + idx).GetComponent<Text>();
                label.text = "" + scoreManager.fetchData[scoreKind].scoreDataList[idx].Score;
                SetPosition(label.transform, true);
            }
        }
    }


    public void OnClick()
    {
        FreePlayMenuController.WriteData();

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
        param.Version = 0;

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            param.ScoreKindValue = scoreKind;
            scoreManager.fetchTopRankers(scoreKind, param);
        }

    }

}
