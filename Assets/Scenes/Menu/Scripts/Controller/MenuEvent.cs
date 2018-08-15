using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvent : MonoBehaviour {

    public void OnClickFreePlayButton()
    {
        MenuController.WriteData();

        SceneManager.LoadScene("Free");
    }

    public void OnClickReplayButton()
    {
        MenuController.WriteData();

        SceneManager.LoadScene("Replay");
    }

}
