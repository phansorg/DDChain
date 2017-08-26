using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IdPassButton : MonoBehaviour {

    public void OnClick()
    {

        Text textWork;
        textWork = GameObject.Find("IdPassLabel").GetComponent<Text>();

        if (textWork.text == string.Empty)
        {
            DataManager dataManager = DataManager.Instance;

            textWork.text = dataManager.UserData.Id + " / " + dataManager.UserData.Password;
        }
        else
        {
            textWork.text = string.Empty;
        }
    }
}
