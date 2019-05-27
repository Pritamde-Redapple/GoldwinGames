using System.Collections;
using System.Collections.Generic;
using GWebUtility;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SuperJoker;
using System;

public class Lobby : UIPage {

    public Text tName;
    public Text tPointBalance;

    public GameObject gLoadingPanel;
    public GameObject gLoadingPanelForAPI;
    public GameObject lobbyPanel;

    public Image fillBar;
    public RectTransform star;

    public float speed = 0.3f;

    private bool bIsWebLoaded;
    private float fEndRandomValue = 0.1f;
    Web web;

    [Header("Change Password")]
    public GameObject changePasswordPanel;
    public InputField newPasswordField;
    public InputField oldPasswordField;
    public Text errorText;

    public Text nextGameName;

    public Text dateText;
    public Text timeText;

    private void Start()
    {
        UIController.instance.Addpage(this, false);
    }

    public override void OnExit()
    {
        base.OnExit();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public override void Update()
    {
        SetDateTime();
        base.Update();
    }

    public void TripleChanceClicked()
    {
        nextGameName.text = "TRIPLE CHANCE";
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gLoadingPanel.SetActive(true);
        lobbyPanel.SetActive(false);

        StartCoroutine(Load());

        Debug.Log(Constant.CurrentAccessToken);
        web = Web.Create()
           .SetUrl(Constant.GameDetailsURL, Web.RequestType.POST, Web.ResponseType.TEXT)

           .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
                    .SetOnSuccessDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Success " + _response.GetText());
                        JSONNode _jsonNode = JSON.Parse(_response.GetText());
                        if (_jsonNode["status"].Value == "1")
                        {
                            Constant.CurrentGameSession = _jsonNode["result"]["game_session"].Value;
                            Constant.PointBalance = double.Parse(_jsonNode["result"]["currentblance"].Value);
                            Constant.TimerForGame = float.Parse(_jsonNode["result"]["timer"].Value);
                            Constant.GameStatus = _jsonNode["result"]["game_status"].Value;
                            //  UIController.instance.GetResultForGame(_jsonNode["result"]);

                            /*  UIController.instance.sAllResult = new List<string>();
                              UIController.instance.sAllResult.Clear();

                              int count = _jsonNode["result"]["winning_details"].Count;

                              for (int i = 0; i < count; i++)
                              {
                                    UIController.instance.sAllResult.Add(_jsonNode["result"]["winning_details"][i]["winning_number"].Value);
                              }*/

                            UIController.instance.GetResultForGame(_response.GetText());
                            bIsWebLoaded = true;
                            {
                                fEndRandomValue = 0.4f;
                            }

						  if(fillBar.fillAmount == 1)
                             UIController.instance.TransitionTo(PageType.GAMEPLAY);
                        }
                        else
                        {
                            StopAllCoroutines();
                           // show error popup
                        }

                        _web.Close();
                    })
                    .SetOnFailureDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Found Error " + _response.GetError());
                       // if (_response.GetError() == Constant.NoInternet || _response.GetError() == Constant.ResolveDestinationHost)
                        {
                            UIController.instance.NoInterNetPopUp();
                        }

                        _web.Close();
                    })
           .Connect();
          
        
       
    }

    public void SuperJokerClicked()
    {
        gLoadingPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        StartCoroutine(Load(true));
        nextGameName.text = "SUPER JOKER";
        /*
         * {
    "status": "1",
    "message": "Game result",
    "result": {
        "currentblance": "-418.00",
        "curenttime": "2018-12-10 15:42:35",
        "sessiontime": "2018-12-10 12:19:32",
        "game_session": "1544424572787",
        "game_status": "resultdecliare",
        "timer": 12183,
        "previous_win_arr": [{
                "winning_number": {
                    "card_id": "3",
                    "card_suite": "4"
                },
                "multiplier": "1"
            } ]
    }
}
 *
 */
        web = Web.Create()
            .SetUrl(Constant.SJGameDetailsURL, Web.RequestType.POST, Web.ResponseType.TEXT)
            .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)            
            .SetOnSuccessDelegate((Web _web, Response _response) =>
            {
                Debug.Log(System.DateTime.Now + "Lobby: " + _response.GetText());
                JSONNode _jsonNode = JSON.Parse(_response.GetText());
                if (_jsonNode["status"].Value == "1")
                {
                    Constant.CurrentGameSession = _jsonNode["result"]["game_session"].Value;
                    Constant.PointBalance = double.Parse(_jsonNode["result"]["currentblance"].Value);
                    Constant.TimerForGame = float.Parse(_jsonNode["result"]["timer"].Value);
                    Constant.GameStatus = _jsonNode["result"]["game_status"].Value;
                    SuperJokerConstants.PointBalance = Constant.PointBalance;
                   // List<Vector3> historyCards = new List<Vector3>();
                    int outerTarget = _jsonNode["result"]["previous_win_arr"][0]["winning_number"]["card_id"].AsInt;
                    int innerTarget = _jsonNode["result"]["previous_win_arr"][0]["winning_number"]["card_suite"].AsInt;
                    int cardSlot = _jsonNode["result"]["previous_win_arr"][0]["bumper_number"]["card_id"].AsInt;                   
                    int cardSuite = _jsonNode["result"]["previous_win_arr"][0]["bumper_number"]["card_suite"].AsInt;
                    //for (int i = 0; i < _jsonNode["result"]["previous_win_arr"].Count; i++)
                    //{
                    //    historyCards.Add(new Vector3(_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"].AsFloat, _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsFloat, _jsonNode["result"]["previous_win_arr"][i]["multiplier"].AsFloat));
                    //   // Debug.Log("Winning Number"+_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"] + _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsFloat);
                    //}

                    #region NewModule
                    List<CardHistoryData> historyCards = new List<CardHistoryData>();

                    for (int i = 0; i < _jsonNode["result"]["previous_win_arr"].Count; i++)
                    {
                        historyCards.Add(new CardHistoryData(_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"].AsInt , _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsInt, _jsonNode["result"]["previous_win_arr"][i]["multiplier"].AsInt, System.Convert.ToBoolean(_jsonNode["result"]["previous_win_arr"][i]["is_bumper"].AsInt)));

                        
                        //(_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"].AsFloat, _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsFloat, _jsonNode["result"]["previous_win_arr"][i]["multiplier"].AsFloat));
                        // Debug.Log("Winning Number"+_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"] + _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsFloat);
                    }


                    #endregion
                    
                    if ((60-Constant.TimerForGame) <= 0)
                    {
                        if(Constant.GameStatus == "resultdecliare")
                        {
                            GameObject newResultDeclareActivator = new GameObject("ResultDeclareActivator");//Resources.Load<GameObject>("SJ_Resources/SpinActivator");
                           
                            GameObject newRA = (GameObject) Instantiate(newResultDeclareActivator);
                            ResultDeclareActivator resultDeclareActivator = newRA.AddComponent<ResultDeclareActivator>();
                            resultDeclareActivator.HistoryDeckData(historyCards);
                            resultDeclareActivator.SetWinningResults(outerTarget, innerTarget,cardSlot,cardSuite);
                        }
                        else if(Constant.GameStatus == "running")
                        {
                            //Result Declare
                            Debug.Log("Running...........");
                            GameObject newSpinActivator = new GameObject("SpinActivator");//Resources.Load<GameObject>("SJ_Resources/SpinActivator");
                            
                            GameObject newSA = (GameObject)Instantiate(newSpinActivator);
                            newSA.GetComponent<SlotWheelActivator>().HistoryDeckData(historyCards); 
                            
                        }
                    }
                    else
                    {
                        GameObject newHistoryDeckActivator = new GameObject("HistoryDeckActivator");
                        newHistoryDeckActivator.AddComponent<HistoryDeckActivator>();
                        GameObject newHA = (GameObject)Instantiate(newHistoryDeckActivator);
                        HistoryDeckActivator historyDeckActivator = newHA.GetComponent<HistoryDeckActivator>();
                        historyDeckActivator.HistoryDeckData(historyCards);
                        historyDeckActivator.SetWinningResults(outerTarget, innerTarget, cardSlot,cardSuite);
                    }
                 //   UIController.instance.GetResultForGame(_response.GetText());
                    bIsWebLoaded = true;
                    {
                        fEndRandomValue = 0.4f;
                    }

                    StartCoroutine(ChangeSceneAfterFill());
                }
                else
                {
                    StopAllCoroutines();
                    // show error popup
                }

                _web.Close();
            })
                    .SetOnFailureDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Found Error " + _response.GetError());
                        // if (_response.GetError() == Constant.NoInternet || _response.GetError() == Constant.ResolveDestinationHost)
                        {
                            UIManager.inst.ShowNoInternetPopup();
                        }

                        _web.Close();
                    })
           .Connect();
       // Invoke("ChangeScene", 1.8f);
    }

    IEnumerator ChangeSceneAfterFill()
    {
        yield return new WaitUntil(() => fillBar.fillAmount == 1);
        ChangeScene();
    }

    void ChangeScene()
    {
#if UNITY_ANDROID
        SceneManager.LoadScene(4);
#else
        SceneManager.LoadScene(3);
#endif
    }

    public void ExitClicked()
    {
        UIController.instance.TransitionTo(PageType.LOGIN);
    }

    public void ChangePasswordClicked()
    {
        changePasswordPanel.SetActive(true);
        newPasswordField.text = "";
        oldPasswordField.text = "";
        errorText.text = "";
    }

    public void SubmitForChangePasswordClicked()
    {
        if(oldPasswordField.text.Trim().Length > 0)
        {
            if (newPasswordField.text.Trim().Length > 0)
            {
                ChangePasswordAPI();
            }
            else
            {
                errorText.text = "Enter New Password";
            }
        }
        else
        {
            errorText.text = "Enter Old Password";
        }
    }

    private void ChangePasswordAPI()
    {
        gLoadingPanelForAPI.SetActive(true);
        web = Web.Create()
          .SetUrl(Constant.ChangePasswordURL, Web.RequestType.POST, Web.ResponseType.TEXT)

          .AddField(Constant.Username, PlayerPrefs.GetString(Constant.LoginUserName))
          .AddField(Constant.Current_Password, oldPasswordField.text.Trim())
          .AddField(Constant.Password, newPasswordField.text.Trim())
          .AddField(Constant.ConfirmPassword, newPasswordField.text.Trim())
                   .SetOnSuccessDelegate((Web _web, Response _response) =>
                   {
                       Debug.Log("Success " + _response.GetText());
                       JSONNode _jsonNode = JSON.Parse(_response.GetText());
                       if (_jsonNode["status"].Value == "1")
                       {
                           UIController.instance.TransitionTo(PageType.LOGIN);
                       }
                       else
                       {
                           errorText.text = _jsonNode["message"].Value;
                       }

                       _web.Close();
                   })
                   .SetOnFailureDelegate((Web _web, Response _response) =>
                   {
                       Debug.Log("Found Error " + _response.GetError());
                        {
                           UIController.instance.NoInterNetPopUp();
                        }

                       _web.Close();
                   })
          .Connect();


    }

    public override void OnEnter()
	{
        base.OnEnter();
        Debug.Log("Name : " + Constant.Name);
        tName.text = Constant.Name.ToUpper();
        tPointBalance.text = Constant.PointBalance.ToString();
        SoundController.instance.StartAndStopWheelSpin(false);
	}

    IEnumerator Load(bool isSJ = false)
    {
        float i = 0;
        while (i < 1)
        {
            i += UnityEngine.Random.Range(0, fEndRandomValue);  Mathf.MoveTowards(i, 1, Time.deltaTime * speed);
            if (i > 1)
                i = 1;
            fillBar.fillAmount = i;
            star.anchoredPosition = new Vector2(i * 1243, star.anchoredPosition.y); // 1243 is width of fill bar
            yield return null;
        }

        yield return new WaitForSeconds(.25f);
		fillBar.fillAmount = 1;
        if(bIsWebLoaded && !isSJ)
              UIController.instance.TransitionTo(PageType.GAMEPLAY);
    }

    public void Close()
    {
        Debug.Log("Close clicked... " + Time.time);
        StopAllCoroutines();

        gLoadingPanel.SetActive(false);
        lobbyPanel.SetActive(true);

        fillBar.fillAmount = 0;
        bIsWebLoaded = false;

        if(web != null)
           web.Close();
    }

    public override void YesClicked()
    {
        base.YesClicked();
        Close();
        base.Exit();
    }

// date and time 
    public void SetDateTime()
    {
     dateText.text = "";
     timeText.text = "";
        DateTime theTime = DateTime.Now;
        //  string day  = System.DateTime.Today.DayOfWeek.ToString();
        string date = theTime.Day + "-" + theTime.Month + "-" + theTime.Year;
        string newTime = DateTime.Now.ToShortTimeString();
       dateText.text = date;
      timeText.text = newTime;
    }

}
