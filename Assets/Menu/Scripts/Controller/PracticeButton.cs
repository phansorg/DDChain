using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PracticeButton : MonoBehaviour {

    public void OnClick()
    {
        TitleController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.PuzzleData.Practice = 1;
        dataManager.PuzzleData.WriteCount++;
        dataManager.Write();

        SceneManager.LoadScene("Puzzle");
    }
}
