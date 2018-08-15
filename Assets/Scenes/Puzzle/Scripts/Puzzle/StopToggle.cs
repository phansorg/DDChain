using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopToggle : MonoBehaviour {

    public void OnValueChanged(int result)
    {
        Toggle stopToggle = GameObject.Find("StopToggle").GetComponent<Toggle>();

        PuzzleController puzzleController = GameObject.Find("PuzzleController").GetComponent<PuzzleController>();
        puzzleController.replayStop = stopToggle.isOn;
    }
}
