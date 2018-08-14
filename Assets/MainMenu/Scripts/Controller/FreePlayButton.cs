using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreePlayButton : MonoBehaviour {

    public void OnClick()
    {
        MainMenuController.WriteData();

        SceneManager.LoadScene("FreePlayMenu");
    }

}
