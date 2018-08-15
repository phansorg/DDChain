using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FreeEvent : MonoBehaviour {

    public void OnClickPracticeButton()
    {
        FreeController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.PuzzleData.Practice = 1;
        dataManager.PuzzleData.WriteCount++;
        dataManager.Write();

        SceneManager.LoadScene("Puzzle");
    }

    public void OnClickStartButton()
    {

        FreeController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.PuzzleData.Practice = 0;
        dataManager.PuzzleData.WriteCount++;
        dataManager.Write();

        SceneManager.LoadScene("Puzzle");
    }

    public void OnClickIdPassButton()
    {
        Text textWork;
        textWork = GameObject.Find("IdPassLabel").GetComponent<Text>();

        if (textWork.text == string.Empty)
        {
            DataManager dataManager = DataManager.Instance;

            textWork.text = dataManager.UserData.Id + " / " + dataManager.UserData.Password;
        }
        else
        {
            textWork.text = string.Empty;
        }
    }
}
