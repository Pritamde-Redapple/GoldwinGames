using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Loading : UIPage {

    public Image fillBar;
    public RectTransform star;

    public float speed = 5f;

    public Ease easeType = Ease.InOutSine;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        fillBar.DOFillAmount(1, 5f).SetEase(easeType).OnUpdate(() => {

            star.anchoredPosition = new Vector2(fillBar.fillAmount * 1243, star.anchoredPosition.y);
        }).OnComplete(() => { CheckScreenRatioAndLoadScene(); });//UIController.instance.TransitionTo(PageType.LOGIN)
    }

    
    public override void OnEnter()
    {
        base.OnEnter();

        fillBar.DOFillAmount(1, 5f).SetEase(easeType).OnUpdate(() => {

            star.anchoredPosition = new Vector2(fillBar.fillAmount * 1243, star.anchoredPosition.y);
        }).OnComplete(() => { CheckScreenRatioAndLoadScene(); });//UIController.instance.TransitionTo(PageType.LOGIN)
    }

    void CheckScreenRatioAndLoadScene()
    {
        float _fRatio = Screen.width * 1.0f / Screen.height * 1.0f;
     //   Debug.Log(_fRatio + " " + _fRatio.ToString("F"));

        switch (_fRatio.ToString("F"))
        {
            case "1.78": // 16:9
                Constant.ScreenRatio = "16X9";
                break;
            case "1.60": // 16:10
                Constant.ScreenRatio = "16X10";
               // Constant.ScreenRatio = "16X9";
                break;
            case "1.25": // 5:4
                Constant.ScreenRatio = "16X9";
                // Constant.ScreenRatio = "5X4";
                break;
            case "1.34": // 4:3
                Constant.ScreenRatio = "16X9";
                // Constant.ScreenRatio = "4X3";
                break;
            case "1.50": // 3:2
                Constant.ScreenRatio = "16X9";
                // Constant.ScreenRatio = "3X2";
                break;
            default:
                Constant.ScreenRatio = "16X9";
                break;

        }

#if UNITY_ANDROID
         Constant.CurrentDeviceType = "mobile";
         SceneManager.LoadScene("Game_16X9");
#else
        Constant.CurrentDeviceType = "pc";
        SceneManager.LoadScene("Game_"+Constant.ScreenRatio);
#endif
    }

}
