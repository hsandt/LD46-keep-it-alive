using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen ^= true;
        }
    }
}
