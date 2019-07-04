using socket.io;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;
using System.Linq;
using SuperJoker;
public class SocketController : MonoBehaviour
{
    private Socket socket;
    public static SocketController Instance;
    public float startingRotationTimer = 0;
    bool isAddedUser;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ConnectSocket();
    }

    public void ConnectSocket()
    {
       
        socket = Socket.Connect("http://13.234.42.202:3000/");//http://18.219.52.107:3006/");//"http://35.154.209.176:3006/");  //http://142.4.15.147:3020   //http://18.219.52.107:3006(new)
        socket.OnReconnect += OnConnectSocket;
        socket.On("user_connected", UserConnectedCallback);
        socket.On("connected_room", ConnectedRoomCallback);
        socket.On("session_creation", SessionCreationCallback);
        socket.On("gameresultprepare", ResultReadyCallback);
        socket.On("game_status_ack", GameStatusAcknowldgement);
        socket.On("game_closing_event", GameOnOffStatus);

        //Super Joker
        socket.On("super_joker_session_creation", OnSJSessionCreated);
        socket.On("super_joker_result_prepare", OnResultPrepared);
        socket.On("joker_game_status_ack", GameStatusUpdate);

    }

   

    private void OnDisable()
    {
        socket.OnReconnect -= OnConnectSocket;
    }

    void OnConnectSocket(int attemptTime)
    {
        Debug.Log("On reconnect : " + attemptTime);
        isAddedUser = false;
        AddUser();
        GameStatus();
    }

    public void ReconnectUser()
    {
        isAddedUser = false;
        AddUser();
    }
    private void Update()
    {
        NetWorkConnectionError();
    }

    void NetWorkConnectionError()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Invoke("NetworkErrorSceneTransition", 3.0f);
        }
    }

    void NetworkErrorSceneTransition()
    {
        SceneManager.LoadScene("GameSelection");
    }

    #region On Functions

    public void UserConnectedCallback(string _data)
    {
#if _D_I
        Debug.Log("Event: User Connected > " + _data);
#endif
        isAddedUser = true;
    }

    public void ConnectedRoomCallback(string _data)
    {
#if _D_I
        Debug.Log("Event: Connected Room > " + _data);
#endif
        JSONNode node = JSONNode.Parse(_data);
        if (node["status"].Value == ErrorCode.SUCCESS_CODE)
        {

            Debug.Log("WaitingPanelShown");
           
        }
    }

    public void SessionCreationCallback(string _data)
    {
#if _D_I
        Debug.Log("Event: Session Creation > " + _data  );
        Debug.Log("Current Time > " + Time.time);
#endif
        Debug.Log("Event: Session Creation > " + _data);
        JSONNode node = JSONNode.Parse(_data);
        if (UIController.instance != null && UIController.instance.currentPage.currentPageType == UIPage.PageType.GAMEPLAY)
        {
            Constant.CurrentGameSession = node["result"]["game_session"].Value;

            GamePlay.instance.ResetAllData();
        }

    }

    public void ResultReadyCallback(string _data)
    {
        if(GamePlay.instance)
            Debug.LogError(Time.time - GamePlay.instance.startSpinningTime);
#if _D_I
        Debug.Log("Event: Result ready > " + _data);
        Debug.Log("Result Ready Time " + Time.time);
#endif
        JSONNode node = JSONNode.Parse(_data);
        if (UIController.instance != null && UIController.instance.currentPage.currentPageType == UIPage.PageType.GAMEPLAY)
        {
            if(gotGameResumeAPIStatus)
               GamePlay.instance.GetGameResult();
        }

    }

    public void GameStatusAcknowldgement(string _data)
    {
#if _D_I
        Debug.Log("Event: Game Status > " + _data);
#endif
        gotGameResumeAPIStatus = true;
        JSONNode node = JSONNode.Parse(_data);
        Debug.Log("On Game Resume: "+ _data);
        if (UIController.instance != null)
            Debug.Log(UIController.instance.currentPage.currentPageType);
        if (UIController.instance != null && UIController.instance.currentPage.currentPageType == UIPage.PageType.GAMEPLAY)
        {
            Constant.GameStatus = node["result"]["game_staus"].Value;
            if (Constant.GameStatus == ErrorCode.RUNNING)
            {
                Constant.CurrentGameSession = node["result"]["game_session"].Value;
                Constant.TimerForGame = float.Parse(node["result"]["pending_time"].Value ) ;
            }
            else
            {
                Constant.CurrentGameSession = node["result"]["old_session"].Value;
                Constant.TimerForGame = 0;
            }

            GamePlay.instance.UpdateTimer(Constant.TimerForGame);

            Debug.Log("Game session " + Constant.CurrentGameSession);

        }

    }

    public void GameOnOffStatus(string _data)
    {
#if _D_I
        Debug.Log("Event: Game ON/OFF Status > " + _data);
#endif

        JSONNode node = JSONNode.Parse(_data);
        
        if (node["result"]["game_on_off"].Value == ErrorCode.OFF)
        {
            UIController.instance.GameStopPopUp();
        }

    }



    #endregion

    #region Emit Functions
    #region AddUser
    public void AddUser()
    {
        if (!isAddedUser)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["access_token"] = Constant.CurrentAccessToken;
            socket.EmitJson("adduser", new JSONObject(data).ToString());

        }
    }
    #endregion

    #region Game Status
    public void GameStatus()
    {
        gotGameResumeAPIStatus = false;
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["access_token"] = Constant.CurrentAccessToken;
        data["game_session"] = Constant.CurrentGameSession;
        socket.EmitJson("game_status_req", new JSONObject(data).ToString());
        Debug.Log("Game Status : current Game Session " + Constant.CurrentGameSession);        
    }
    #endregion

    #endregion

    bool gotGameResumeAPIStatus = true;
    private void OnApplicationPause(bool pause)
    {
        if(!pause && socket != null)
        {
            Debug.Log("Resume Game");

            if (UIManager.inst != null)
                SendForSuperJokerStatus();
            else
                 GameStatus();

        }
        else if(pause)
        {
            Debug.Log("Pause Game");
        }
    }


    #region SuperJoker

   public void SendForSuperJokerStatus()
   {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["access_token"] = Constant.CurrentAccessToken;
        data["game_session"] = Constant.CurrentGameSession;
        socket.EmitJson("joker_game_status_req", new JSONObject(data).ToString());
    }
    bool isSessionCreated;
    void OnSJSessionCreated(string _data)
    {
        if(LevelManager.inst != null)
        {
            JSONNode _jsonNode = JSON.Parse(_data);
            if (_jsonNode["status"].Value == "1")
            {
                Constant.CurrentGameSession = _jsonNode["result"]["game_session"].Value;
            }
                
            Debug.Log(System.DateTime.Now+ "New Session: " + _data);
            TimerController.inst.StartTimer(60);     //resets and starts the timer
         //   LevelManager.inst.SetInputBlocker(false); //disables the input blocker
            LevelManager.inst.ResetAllCardValues();  //deselects all cards
            LevelManager.inst.SetText(true); // makes play text visible again
            CardDeck.instance.DisableWinningCard(); // disables the glow of the winning card

            if(TimerController.inst.TimeUp == null)
                TimerController.inst.TimeUp += LevelManager.inst.ToggleInputBlocker; //subscribes to time up again
            if (TimerController.inst.WheelSpinTime == null)
                TimerController.inst.WheelSpinTime += LevelManager.inst.StartSpinning; // subscribes to start spinning
           TimerController.inst.TimeUp += DataCollector.instance.FetchAllCardsAndSuites; // subscribes to getting all card data

            UIManager.inst.ShowWaitForNextSession(false);  //disables the pop up that says wait for next session
                                                           // ButtonManager.instance.ToggleClearDoubleRepeat(true); //enables all three buttons : clear, double, repeat
         //   UIManager.inst.SetPlayAmount();
            UIManager.inst.ShowWinPopup(false); //disables the winpopup if any from the previous round
            LevelManager.inst.SetSelectionWheelLights(false);
            //LevelManager.inst.SetCenterWheelTexts(false);
            //LevelManager.inst.SetMultiplierAnimator(false);
            // UIManager.inst.SetWinAmount(0);
            // LevelManager.inst.ResetVeloctiy();
            SuperJokerConstants.PointBalance = Constant.PointBalance;
        }
       
    }

    void OnResultPrepared(string _data)
    {
       // Debug.Log("Result Ready: " + System.DateTime.Now +"__"+ _data);        
        Invoke("GetResultWithDelay",4);
      //  Invoke("SetStopEffects", 8);
    }

    void GetResultWithDelay()
    {
        if (LevelManager.inst != null)
            LevelManager.inst.GetResult();
    }

    void SetStopEffects()
    {
        if (LevelManager.inst != null)
        {
            LevelManager.inst.SetCenterWheelAnimation(false);
            LevelManager.inst.SetCenterWheelTexts(true);
        }
    }

    void GameStatusUpdate(string data)
    {
        //        {
        //            "status":"1",
        //"result":{
        //                "game_session":"1546252669999",
        //"curenttime":"2018-12-31 16:08:48",
        //"pending_time":1,
        //"game_staus":"running"
        //},
        //"message":"game details"
        //}  

#if UNITY_ANDROID
        Debug.Log("Game Resume Data: " + data);
        JSONNode node = JSONNode.Parse(data);       

        if(node["result"]["game_staus"].ToString().Contains( "running")) 
        {
            Constant.CurrentGameSession = node["result"]["game_session"].Value;
            int timePending = node["result"]["pending_time"].AsInt;
            TimerController.inst.UpdateTime(timePending);
        }
        else
        {
            Constant.CurrentGameSession = node["result"]["old_session"].Value;
            Constant.TimerForGame = 0;
            TimerController.inst.StopTimer();
            ButtonManager.instance.ToggleClearDoubleRepeat(false);
            LevelManager.inst.SetInputBlocker(true);
            LevelManager.inst.SetText(false);
            UIManager.inst.ShowWaitForNextSession(true);
        }

#endif

        //   if (data["result"][""])
        //gotGameResumeAPIStatus = true;
        //JSONNode node = JSONNode.Parse(_data);
        //if (UIController.instance != null)
        //    Debug.Log(UIController.instance.currentPage.currentPageType);
        //if (UIController.instance != null && UIController.instance.currentPage.currentPageType == UIPage.PageType.GAMEPLAY)
        //{
        //    Constant.GameStatus = node["result"]["game_staus"].Value;
        //    if (Constant.GameStatus == ErrorCode.RUNNING)
        //    {
        //        Constant.CurrentGameSession = node["result"]["game_session"].Value;
        //        Constant.TimerForGame = float.Parse(node["result"]["pending_time"].Value) - 30;
        //    }
        //    else
        //    {
        //        Constant.CurrentGameSession = node["result"]["old_session"].Value;
        //        Constant.TimerForGame = 0;
        //    }

        //    GamePlay.instance.UpdateTimer(Constant.TimerForGame);

        //    Debug.Log("Game session " + Constant.CurrentGameSession);

        //}
    }

#endregion
}

