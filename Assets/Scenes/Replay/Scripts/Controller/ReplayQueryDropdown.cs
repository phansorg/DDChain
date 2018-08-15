using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayQueryDropdown : MonoBehaviour {

    public static List<ReplayDataV1> replayData;

    // Use this for initialization
    void Start()
    {
        replayData = new List<ReplayDataV1>();
        for (int idx = 0; idx < ReplayController.RANK_MAX; idx++)
        {
            replayData.Add(new ReplayDataV1());
        }

        OnValueChanged(0);
    }

    // Update is called once per frame
    void Update()
    {
        Button button;
        Text label;

        ScoreManager scoreManager = ScoreManager.Instance;
        for (int scoreKind = 0; scoreKind < ScoreDataV1.SCORE_KIND_MAX; scoreKind++)
        {
            if (scoreManager.version == 0 ||
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

            for (int idx = 0; idx < ReplayController.RANK_MAX; idx++)
            {
                // スコア表示
                button = GameObject.Find(ReplayController.ReplayButtonName + idx).GetComponent<Button>();
                SetPosition(button.transform, true);

                label = GameObject.Find(ReplayController.NameLabelName + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Name;
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.RowLabelName + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Row.ToString();
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.ColLabelName + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Col.ToString();
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.ColorLabelName + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Color.ToString();
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.LinkLabelName + idx).GetComponent<Text>();
                label.text = scoreManager.fetchData[scoreKind].scoreDataList[idx].Link.ToString();
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.CountDispLabelName + idx).GetComponent<Text>();
                switch (scoreManager.fetchData[scoreKind].scoreDataList[idx].CountDisp)
                {
                    case 0:
                        label.text = "Off";
                        break;
                    case 1:
                        label.text = "On";
                        break;
                }
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.GarbageLabelName + idx).GetComponent<Text>();
                switch (scoreManager.fetchData[scoreKind].scoreDataList[idx].Garbage)
                {
                    case 0:
                        label.text = "Off";
                        break;
                    case 1:
                        label.text = "On";
                        break;
                    case 2:
                        label.text = "Hard";
                        break;
                    case 3:
                        label.text = "Dark";
                        break;
                }
                SetPosition(label.transform, true);

                label = GameObject.Find(ReplayController.ScoreLabelName + idx).GetComponent<Text>();
                label.text = "" + scoreManager.fetchData[scoreKind].scoreDataList[idx].Score;
                SetPosition(label.transform, true);

                // リプレイデータ
                List<ReplayDataV1> replayDataWork = replayData;

                replayDataWork[idx].Version = scoreManager.fetchData[scoreKind].scoreDataList[idx].Version;
                replayDataWork[idx].Id = scoreManager.fetchData[scoreKind].scoreDataList[idx].Id;
                replayDataWork[idx].PlayDateTime = scoreManager.fetchData[scoreKind].scoreDataList[idx].PlayDateTime;
                replayDataWork[idx].ScoreKindValue = scoreManager.fetchData[scoreKind].scoreDataList[idx].ScoreKindValue;

                replayDataWork[idx].Row = scoreManager.fetchData[scoreKind].scoreDataList[idx].Row;
                replayDataWork[idx].Col = scoreManager.fetchData[scoreKind].scoreDataList[idx].Col;
                replayDataWork[idx].Color = scoreManager.fetchData[scoreKind].scoreDataList[idx].Color;
                replayDataWork[idx].Link = scoreManager.fetchData[scoreKind].scoreDataList[idx].Link;
                replayDataWork[idx].Direction = scoreManager.fetchData[scoreKind].scoreDataList[idx].Direction;
                replayDataWork[idx].Time = scoreManager.fetchData[scoreKind].scoreDataList[idx].Time;
                replayDataWork[idx].Stop = scoreManager.fetchData[scoreKind].scoreDataList[idx].Stop;
                replayDataWork[idx].CountDisp = scoreManager.fetchData[scoreKind].scoreDataList[idx].CountDisp;
                replayDataWork[idx].Garbage = scoreManager.fetchData[scoreKind].scoreDataList[idx].Garbage;

                replayDataWork[idx].Name = scoreManager.fetchData[scoreKind].scoreDataList[idx].Name;

            }
        }
    }

    public void OnValueChanged(int result)
    {
        DataManager dataManager = DataManager.Instance;

        ScoreDataV1 param = new ScoreDataV1();

        param.Version = CommonDefine.VERSION;
        param.ScoreCategoryValue = (int)ScoreDataV1.ScoreCategory.NewArrivals;

        Dropdown queryDropdown = GameObject.Find("QueryDropdown").GetComponent<Dropdown>();
        int scoreKind = queryDropdown.value;

        ScoreManager scoreManager = ScoreManager.Instance;
        param.ScoreKindValue = scoreKind;
        scoreManager.fetchNewArrivals(scoreKind, param, ReplayController.RANK_MAX);
        //scoreManager.setNewCol();
    }

}
