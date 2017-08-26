using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    public void OnClick()
    {
        if (PuzzleController.currentState != PuzzleController.PuzzleState.SendScoreData)
        {
            iTween.tweens.Clear();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
