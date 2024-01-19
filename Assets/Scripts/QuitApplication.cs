using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    private void Update() {
        Quit();
    }

    private void Quit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Quitting application...");
            Application.Quit();
        }
    }
}
