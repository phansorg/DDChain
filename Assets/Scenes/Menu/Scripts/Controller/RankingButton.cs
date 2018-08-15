using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingButton : MonoBehaviour
{

    public void OnClick()
    {
        MenuController.WriteData();

        SceneManager.LoadScene("Ranking");
    }

}
