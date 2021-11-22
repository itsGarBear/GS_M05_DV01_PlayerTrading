using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TabSwitcher : MonoBehaviour
{
    public List<TMP_InputField> inputFields;
    int ndx = 0;
    bool canShiftTab = false;

    private void Start()
    {
        inputFields[ndx].Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ndx = 1;
            inputFields[ndx].Select();
            canShiftTab = true;
        }
        if(canShiftTab && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Tab))
        {
            ndx = 0;
            inputFields[ndx].Select();
            canShiftTab = false;
        }
    }
}
