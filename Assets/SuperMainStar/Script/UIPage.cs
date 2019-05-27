using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using GWebUtility;
using SimpleJSON;
using UnityEngine.EventSystems;

public class UIPage : MonoBehaviour
{

    public enum PageType
    {
        LOGIN,
        LOADING,
        LOBBY,
        GAMEPLAY
    }
    public PageType currentPageType;
    public GameObject gQuitPanel;


    public virtual void OnEnter()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public virtual void OnExit()
    {
        // transform.GetChild(0).gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    public virtual void PlayAnimation()
    {

    }

    #region Minimize Game 
    [DllImportAttribute("user32.dll")]
    public extern static bool ShowWindow(IntPtr hwnd, int nCmdShow);
    //public static boolean{} ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImportAttribute("user32.dll")]
    public extern static IntPtr GetForegroundWindow();
    [DllImportAttribute("user32.dll")]
    public extern static IntPtr GetActiveWindow();

    public void Minimize()
    {
        //Minimize the window
        ShowWindow(GetActiveWindow(), 2);
    }

   #endregion


    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Exit()
    {
        GameObject eventSystem = EventSystem.current.gameObject;
        eventSystem.SetActive(false);
        Web.Create()
            .SetUrl(Constant.LogOutURL, Web.RequestType.POST, Web.ResponseType.TEXT)
            .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
            .SetOnSuccessDelegate((Web _web, Response _response) =>
            {
                eventSystem.SetActive(true);
                Debug.Log("Success " + _response.GetText());
                JSONNode _jsonNode = JSON.Parse(_response.GetText());
                if (_jsonNode["status"].Value == "1")
                {
                    UIController.instance.TransitionTo(UIPage.PageType.LOGIN);

                }


                _web.Close();
            })
            .SetOnFailureDelegate((Web _web, Response _response) =>
            {
                eventSystem.SetActive(true);
                Debug.Log("Found Error " + _response.GetError());
                if (_response.GetError() == Constant.NoInternet || _response.GetError() == Constant.ResolveDestinationHost || _response.GetError() == Constant.ConnectDestinationHost)
                {
                    UIController.instance.NoInterNetPopUp();
                }

                _web.Close();


            })
            .Connect();

    }

    //
    public virtual void Update()
    {
#if UNITY_ANDROID
        if(Input.GetKeyDown(KeyCode.Escape) && gQuitPanel != null)
        {
            if(gQuitPanel.activeSelf)
                gQuitPanel.SetActive(false);
            else
                gQuitPanel.SetActive(true);
        }
#endif
    }
    //

    public virtual void YesClicked()
    {

    }

    public virtual void NoClicked()
    {
        gQuitPanel.SetActive(false);
    }
}
