using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FreePlayButton : MonoBehaviour {

    public void OnClick()
    {
        MenuController.WriteData();

        SceneManager.LoadScene("Free");
    }

}
