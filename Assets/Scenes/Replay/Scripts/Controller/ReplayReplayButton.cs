using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReplayReplayButton : MonoBehaviour
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
        Debug.Log("Replay Update");
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
        int idx = int.Parse(name.Substring(name.IndexOf("_") + 1));
        ReplayDataV1 param = ReplayQueryDropdown.replayData[idx];

        if (param.Version == 0)
        {
            return;
        }

        ScoreManager scoreManager = ScoreManager.Instance;
        scoreManager.fetchReplay(param);
    }

}
