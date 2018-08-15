using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEvent : MonoBehaviour {

    public void OnClickFreePlayButton()
    {
        MenuController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.UserData.Scene = "Free";
        dataManager.Write();

        SceneManager.LoadScene("Free");
    }

    public void OnClickReplayButton()
    {
        MenuController.WriteData();

        DataManager dataManager = DataManager.Instance;
        dataManager.UserData.Scene = "Replay";
        dataManager.Write();

        SceneManager.LoadScene("Replay");
    }

}
