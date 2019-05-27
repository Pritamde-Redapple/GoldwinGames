using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class UIController : MonoBehaviour {

    public static UIController instance;

    public int iPointBalance;

    private Dictionary<UIPage.PageType, UIPage> dicAllPage = new Dictionary<UIPage.PageType, UIPage>();
    public UIPage currentPage;
    public GameObject messagePopUp;

    public List<WinningData> sAllResult;
    public Texture2D cursor;

	private string sPathToLoadScreen;
    private void Awake()
    {
        instance = this;

		#if UNITY_ANDROID
		sPathToLoadScreen = "Android/Screen/";

		#else 
		sPathToLoadScreen = "Screen/"+Constant.ScreenRatio + "/";
#endif

      
    }

    private void OnEnable()
    {
        TransitionTo(UIPage.PageType.LOGIN);
    }

    public void Addpage(UIPage _uiPage, bool _bHideAtStart)
    {
      /*  dicAllPage.Add(_uiPage.currentPageType, _uiPage);
        if (_bHideAtStart)
            _uiPage.OnEnter();
        else
            _uiPage.OnExit();
            */
    }

    public void TransitionTo(UIPage.PageType _eTo)
    {
        if (currentPage != null){
            currentPage.OnExit();
        }

	

		GameObject _obj = Resources.Load<GameObject>(sPathToLoadScreen + _eTo.ToString());
        GameObject _loadedObject = Instantiate(_obj, this.transform);

        /*  if(!dicAllPage.ContainsKey(_eTo))
          {
              return;  
          }

          currentPage = dicAllPage[_eTo];
          currentPage.OnEnter();*/


        currentPage = _loadedObject.GetComponent<UIPage>();
        currentPage.OnEnter();
    }

    public void NoInterNetPopUp()
    {
        GameObject _obj = Resources.Load<GameObject>(sPathToLoadScreen+"NoInternetPopup" );
        Instantiate(_obj, this.transform);
    }
    public void GameStopPopUp()
    {
        GameObject _obj = Resources.Load<GameObject>("GameStop");
        Instantiate(_obj, this.transform);
    }

    public void MessagePopUp()
    {
        if (messagePopUp == null)
        {
            GameObject _obj = Resources.Load<GameObject>(sPathToLoadScreen + "MessagePopup");
            messagePopUp = Instantiate(_obj, this.transform);
        }
       
    }


    public void GetResultForGame(string _sResult)
    {
        Debug.Log(_sResult);
        sAllResult = new List<WinningData>();
        sAllResult.Clear();

        JSONNode _jsonNode = JSON.Parse(_sResult);

        int count = _jsonNode["result"]["winning_details"].Count;

        for (int i = 0; i < count; i++)
        {
            WinningData data = new WinningData(_jsonNode["result"]["winning_details"][i]["winning_number"].Value,
                                              _jsonNode["result"]["winning_details"][i]["multiplier"].Value);
            sAllResult.Add(data);
        }
    }
}

public class WinningData
{
    public string number;
    public string multiplier;

    public WinningData(string number, string multiplier)
    {
        this.number = number;
        this.multiplier = multiplier;
    }
}
