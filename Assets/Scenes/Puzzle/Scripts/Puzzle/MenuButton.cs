using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    public void OnClick()
    {
        if (PuzzleController.currentState != PuzzleController.PuzzleState.SendScoreData)
        {
            PuzzleController.currentState = PuzzleController.PuzzleState.Wait; // 暫定対策
            iTween.tweens.Clear();

            DataManager dataManager = DataManager.Instance;
            PuzzleController.currentState = PuzzleController.PuzzleState.Wait; // 暫定対策
            SceneManager.LoadScene(dataManager.UserData.Scene);
        }
    }
}
