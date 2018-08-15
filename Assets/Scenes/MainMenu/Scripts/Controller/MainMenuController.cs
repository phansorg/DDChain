using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    private const string PASSWORD_CHARS = "0123456789abcdefghijklmnopqrstuvwxyz";

    public static void WriteData()
    {
        DataManager dataManager = DataManager.Instance;

        InputField fieldWork;

        fieldWork = GameObject.Find("NameField").GetComponent<InputField>();
        dataManager.UserData.Name = fieldWork.text;

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

    }

    // Update is called once per frame
    void Update()
    {

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

        InputField fieldWork;

        fieldWork = GameObject.Find("NameField").GetComponent<InputField>();
        fieldWork.text = dataManager.UserData.Name;
    }
}
