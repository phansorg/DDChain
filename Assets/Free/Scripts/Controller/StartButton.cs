using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	public void OnClick () {

        FreeController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.PuzzleData.Practice = 0;
        dataManager.PuzzleData.WriteCount++;
        dataManager.Write();

        SceneManager.LoadScene("Puzzle");
    }

}
