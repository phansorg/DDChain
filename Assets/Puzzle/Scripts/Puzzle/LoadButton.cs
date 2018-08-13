using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour {

    public void OnClick()
    {
        PuzzleController puzzleController = GameObject.Find("PuzzleController").GetComponent<PuzzleController>();
        puzzleController.LoadPractice();
    }

}
