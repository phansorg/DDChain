using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayController : MonoBehaviour {

    public static string ReplayButtonName = "ReplayButton_";
    public static string NameLabelName = "NameLabel_";
    public static string RowLabelName = "RowLabel_";
    public static string ColLabelName = "ColLabel_";
    public static string ColorLabelName = "ColorLabel_";
    public static string LinkLabelName = "LinkLabel_";
    public static string CountDispLabelName = "CountDispLabel_";
    public static string GarbageLabelName = "GarbageLabel_";
    public static string ScoreLabelName = "ScoreLabel_";

    public static string scoreKindName = "AllColor";

    public static int RANK_MAX = 20;
    public static int SCORE_KIND_MAX = 1;

    // Use this for initialization
    void Start () {
        InitHighScore();
    }

    // Update is called once per frame
    void Update () {

    }

    private void InitHighScore()
    {
        GameObject prefab;
        Vector3 pos;
        GameObject canvas = GameObject.Find("Canvas");

        for (int idx = 0; idx < ReplayController.RANK_MAX; idx++)
        {
            Action<string, string, int, int> InitObject = (prefabName, objectName, posX, posY) =>
            {
                GameObject gameObject;

                prefab = (GameObject)Resources.Load(prefabName);
                gameObject = (GameObject)Instantiate(prefab);
                gameObject.name = objectName;
                gameObject.transform.SetParent(canvas.transform, false);

                pos = gameObject.transform.position;
                pos.x += posX;
                pos.y -= posY;
                gameObject.transform.position = pos;
            };

            int dy = (int)Screen.height / 31;

            InitObject("Prefabs/Replay/ReplayReplayButton", ReplayButtonName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayNameLabel", NameLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayRowLabel", RowLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayColLabel", ColLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayColorLabel", ColorLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayLinkLabel", LinkLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayCountDispLabel", CountDispLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayGarbageLabel", GarbageLabelName + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Replay/ReplayScoreLabel", ScoreLabelName + idx, Screen.width, idx * dy);
        }
    }
}
