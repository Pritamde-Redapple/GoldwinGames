using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoInternetPopUp : MonoBehaviour {


    private void Start()
    {
        Camera.main.GetComponent<AudioListener>().enabled = false;
    }

    public void OkClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
