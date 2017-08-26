using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    private const string PASSWORD_CHARS = "0123456789abcdefghijklmnopqrstuvwxyz";

    public static void WriteData()
    {
        DataManager dataManager = DataManager.Instance;

        InputField fieldWork;

        fieldWork = GameObject.Find("NameField").GetComponent<InputField>();
        dataManager.UserData.Name = fieldWork.text;

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

        FindObjectOfType<UserAuth>().logOut();

        DataManager dataManager = DataManager.Instance;
        UserAuth userAuth = FindObjectOfType<UserAuth>();
        if (dataManager.UserData.Id == string.Empty)
        {
            InitUserData();
            userAuth.signUp(dataManager.UserData.Id, dataManager.UserData.Password);
        }
        else
        {
            if (userAuth.currentPlayer() == string.Empty)
            {
                userAuth.logIn(dataManager.UserData.Id, dataManager.UserData.Password);
            }
        }
        /*
        dataManager.UserData.Id = "guest";
        dataManager.UserData.Password = "guest";
        dataManager.Write();
        */


        LoadGameData();

        InitHighScore();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitUserData()
    {
        DataManager dataManager = DataManager.Instance;

        long idBinary;
        idBinary = (long)((ulong)DateTime.Now.ToBinary() / 10000);
        idBinary %= 80000000000000L;

        dataManager.UserData.Id = Base32Utility.DecimalToBase32(idBinary).ToString();
        dataManager.UserData.Password = GeneratePassword(UserDataV1.PASSWORD_LENGTH);

        dataManager.Write();
    }

    private string GeneratePassword(int length)
    {
        var sb = new System.Text.StringBuilder(length);
        var r = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int pos = r.Next(PASSWORD_CHARS.Length);
            char c = PASSWORD_CHARS[pos];
            sb.Append(c);
        }

        return sb.ToString();
    }

    private void LoadGameData()
    {
        DataManager dataManager = DataManager.Instance;

        Text textWork;
        textWork = GameObject.Find("IdPassLabel").GetComponent<Text>();
        textWork.text = string.Empty;        

        InputField fieldWork;

        fieldWork = GameObject.Find("NameField").GetComponent<InputField>();
        fieldWork.text = dataManager.UserData.Name;

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

        label = GameObject.Find("AllColorLabel");
        pos = label.transform.position;
        pos.x += Screen.width;
        label.transform.position = pos;

        label = GameObject.Find("SingleColorLabel");
        pos = label.transform.position;
        pos.x += Screen.width;
        label.transform.position = pos;

        for (int idx = 0; idx < ScoreManager.RANK_MAX; idx++)
        {
            int dy = (int)Screen.height / 32;

            // AllColorNameLabel
            prefab = (GameObject)Resources.Load("Prefabs/NameLabel");
            label = (GameObject)Instantiate(prefab);
            label.name = "AllColorNameLabel" + idx;
            label.transform.SetParent(canvas.transform, false);

            pos = label.transform.position;
            pos.x += Screen.width;
            pos.y -= idx * dy;
            label.transform.position = pos;

            // AllColorScoreLabel
            prefab = (GameObject)Resources.Load("Prefabs/ScoreLabel");
            label = (GameObject)Instantiate(prefab);
            label.name = "AllColorScoreLabel" + idx;
            label.transform.SetParent(canvas.transform, false);

            pos = label.transform.position;
            pos.x += Screen.width;
            pos.y -= idx * dy;
            label.transform.position = pos;

            // SingleColorNameLabel
            prefab = (GameObject)Resources.Load("Prefabs/NameLabel");
            label = (GameObject)Instantiate(prefab);
            label.name = "SingleColorNameLabel" + idx;
            label.transform.SetParent(canvas.transform, false);

            pos = label.transform.position;
            pos.x += (int)(Screen.width * 1.45);
            pos.y -= idx * dy;
            label.transform.position = pos;

            // SingleColorScoreLabel
            prefab = (GameObject)Resources.Load("Prefabs/ScoreLabel");
            label = (GameObject)Instantiate(prefab);
            label.name = "SingleColorScoreLabel" + idx;
            label.transform.SetParent(canvas.transform, false);

            pos = label.transform.position;
            pos.x += (int)(Screen.width * 1.45);
            pos.y -= idx * dy;
            label.transform.position = pos;

        }
    }
}
