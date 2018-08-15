using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    private bool valueChangeFlag = false;

    // Update is called once per frame
    void Update()
    {
        /*
        if (valueChangeFlag)
        {
            valueChangeFlag = false;
            FreeController.WriteData();
        }
        */
    }

    public void OnValueChanged(int result)
    {
        valueChangeFlag = true;
    }


}
