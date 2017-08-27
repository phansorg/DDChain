using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public void Start()
    {
    }

    public void Update()
    {
        ScoreManager scoreManager = ScoreManager.Instance;
        if (scoreManager.fetchReplayData.flag == false)
        {
            return;
        }
        scoreManager.fetchReplayData.flag = false;

        if (scoreManager.fetchReplayData.replayData == null ||
            scoreManager.fetchReplayData.replayData.Version == 0)
        {
            return;
        }

        DataManager dataManager = DataManager.Instance;
        dataManager.ReplayData = scoreManager.fetchReplayData.replayData;

        dataManager.WriteReplay();

        SceneManager.LoadScene("Puzzle");
    }

    public void OnClick()
    {
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            if (name.Contains(TitleController.scoreKindName[scoreKind]) == false)
            {
                continue;
            }

            int idx = int.Parse(name.Substring(name.IndexOf("_") + 1));
            ReplayDataV1 param = HighScoreButton.replayData[scoreKind][idx];

            if (param.Version == 0)
            {
                continue;
            }

            ScoreManager scoreManager = ScoreManager.Instance;
            scoreManager.fetchReplay(param);
        }

    }

}
