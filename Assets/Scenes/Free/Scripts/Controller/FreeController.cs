using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreeController : MonoBehaviour {

    public static string[] LabelName = { "AllColorLabel", "SingleColorLabel" };

    public static string[] ReplayButtonName = { "AllColorReplayButton_", "SingleColorReplayButton_" };
    public static string[] NameLabelName = { "AllColorNameLabel_", "SingleColorNameLabel_" };
    public static string[] ScoreLabelName = { "AllColorScoreLabel_", "SingleColorScoreLabel_" };

    public static string[] scoreKindName = { "AllColor", "SingleColor" };

    public static int RANK_MAX = 10;

    public static void WriteData()
    {
        DataManager dataManager = DataManager.Instance;

        Slider sliderWork;

        sliderWork = GameObject.Find("RowSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Row = (int)sliderWork.value;

        sliderWork = GameObject.Find("ColSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Col = (int)sliderWork.value;

        sliderWork = GameObject.Find("ColorSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Color = (int)sliderWork.value;

        sliderWork = GameObject.Find("LinkSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Link = (int)sliderWork.value;

        sliderWork = GameObject.Find("DirectionSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Direction = (int)sliderWork.value;

        sliderWork = GameObject.Find("TimeSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Time = (int)sliderWork.value * 10;

        sliderWork = GameObject.Find("CountDispSlider").GetComponent<Slider>();
        dataManager.PuzzleData.CountDisp = (int)sliderWork.value;

        sliderWork = GameObject.Find("GarbageSlider").GetComponent<Slider>();
        dataManager.PuzzleData.Garbage = (int)sliderWork.value;

        if (dataManager.PuzzleData.WriteCount == int.MaxValue)
        {
            dataManager.PuzzleData.WriteCount = int.MinValue;
        }
        dataManager.PuzzleData.WriteCount++;

        dataManager.Write();
    }

	// Use this for initialization
	void Start () {

        LoadGameData();

        InitHighScore();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadGameData()
    {
        DataManager dataManager = DataManager.Instance;

        Text textWork;
        textWork = GameObject.Find("IdPassLabel").GetComponent<Text>();
        textWork.text = string.Empty;        

        Slider sliderWork;

        sliderWork = GameObject.Find("RowSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Row;

        sliderWork = GameObject.Find("ColSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Col;

        sliderWork = GameObject.Find("ColorSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Color;

        sliderWork = GameObject.Find("LinkSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Link;

        sliderWork = GameObject.Find("DirectionSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Direction;

        sliderWork = GameObject.Find("TimeSlider").GetComponent<Slider>();
//        sliderWork.value = dataManager.PuzzleData.Time / 10;
        sliderWork.value = 60 / 10;

        sliderWork = GameObject.Find("CountDispSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.CountDisp;

        sliderWork = GameObject.Find("GarbageSlider").GetComponent<Slider>();
        sliderWork.value = dataManager.PuzzleData.Garbage;

    }

    private void InitHighScore()
    {
        GameObject prefab;
        GameObject label;
        Vector3 pos;
        GameObject canvas = GameObject.Find("Canvas");

        label = GameObject.Find(LabelName[0]);
        pos = label.transform.position;
        pos.x += Screen.width;
        label.transform.position = pos;

        label = GameObject.Find(LabelName[1]);
        pos = label.transform.position;
        pos.x += Screen.width;
        label.transform.position = pos;

        for (int idx = 0; idx < FreeController.RANK_MAX; idx++)
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

            InitObject("Prefabs/Free/FreeReplayButton", ReplayButtonName[0] + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Free/FreeNameLabel", NameLabelName[0] + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Free/FreeScoreLabel", ScoreLabelName[0] + idx, Screen.width, idx * dy);
            InitObject("Prefabs/Free/FreeReplayButton", ReplayButtonName[1] + idx, (int)(Screen.width * 1.45), idx * dy);
            InitObject("Prefabs/Free/FreeNameLabel", NameLabelName[1] + idx, (int)(Screen.width * 1.45), idx * dy);
            InitObject("Prefabs/Free/FreeScoreLabel", ScoreLabelName[1] + idx, (int)(Screen.width * 1.45), idx * dy);
        }
    }
}
