using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using System;
namespace SuperJoker
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager inst;
        public Button closeSupkerJoker;
        public Button closeApplication;
        public Button minimizeApplication;
        public GameObject lobbyManager;

        public Texture2D mouseCursor;

        public Transform slotMachine;
        public Transform innerWheel;
        public Transform outerWheel;
        public Transform glowEffectObject;

        public Vector3 slotMachinePos;
        public Vector3 slotScale;
        public Vector3 wheelPosition;

        public Transform slotMachinePosRef;
        public Transform centerWheelPosRef;
        public GameObject infoPanel;
        #region Minimize Game 
        [DllImportAttribute("user32.dll")]
        public extern static bool ShowWindow(IntPtr hwnd, int nCmdShow);
        //public static boolean{} ShowWindow(IntPtr hwnd, int nCmdShow);
        [DllImportAttribute("user32.dll")]
        public extern static IntPtr GetForegroundWindow();
        [DllImportAttribute("user32.dll")]
        public extern static IntPtr GetActiveWindow();

        public Image InputBlockerObj;

        
        public WheelBase wheelO;
        public WheelBase wheelI;
        public SlotBase slotLeft;
        public SlotBase slotRight;

      
       

        CardSelector cardSelector;
        CardHistoryDeck cardHistoryDeck;
        [Header("Center Wheel Animation Object")]
        public GameObject cardFlashAnimation;
        public HistoryCard centerWheelCardAndSuite;
        public GameObject wheelLights;
        
        public Text multiplierText;

        bool stopUpdate;
        Animator wheelGlowAnimator;

        public GameObject quitPanel;

        public void Minimize()
        {
            //Minimize the window
            ShowWindow(GetActiveWindow(), 2);
        }
        #endregion
        private void Awake()
        {
            inst = this;
        }

        private void Start()
        {
            cardSelector = FindObjectOfType<CardSelector>();
            cardHistoryDeck = FindObjectOfType<CardHistoryDeck>();
            stopUpdate = false;
            wheelGlowAnimator = wheelLights.GetComponentInChildren<Animator>();
            wheelI.WheelStop += SetStopEffects;
        }

        private void OnEnable()
        {
            closeSupkerJoker.onClick.AddListener(() => BackToLobby());
            closeApplication.onClick.AddListener(() => CloseApp());
            minimizeApplication.onClick.AddListener(() => Minimize());
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            TimerController.inst.TimeUp += ToggleInputBlocker;
            TimerController.inst.WheelSpinTime += StartSpinning;
            Invoke("StopUpdate", 2);
        }

        private void OnDestroy()
        {
            wheelI.WheelStop -= SetStopEffects;
        }

        void StopUpdate()
        {
            stopUpdate = true;
        }

        public void BackToLobby()
        {
            Instantiate(lobbyManager);
            if(Constant.ScreenRatio == "16X9")
                SceneManager.LoadScene(1);
            else if(Constant.ScreenRatio == "16X10")
                SceneManager.LoadScene(2);

        }

        public void CloseApp()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        public void MinimizeApp()
        {
            
        }

        public void ResizeGame()
        {
            float _fRatio = Screen.width * 1.0f / Screen.height * 1.0f;
            Debug.Log(_fRatio + " " + _fRatio.ToString("F"));

            switch (_fRatio.ToString("F"))
            {

                case "1.60": // 16:10
                    slotMachine.transform.localScale = slotScale;
                    slotMachine.transform.position = slotMachinePos;
                    innerWheel.position = wheelPosition;
                    outerWheel.position = wheelPosition;                  
                    break;

                default:
                    Constant.ScreenRatio = "16X9";
                    break;
            }
        }

        void Update()
        {
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape) && quitPanel != null)
            {
                if (quitPanel.activeSelf)
                    quitPanel.SetActive(false);
                else
                    quitPanel.SetActive(true);
            }
#endif

            if (stopUpdate)
                return;
           slotMachine.position = slotMachinePosRef.position;
           innerWheel.position  = centerWheelPosRef.position;
           outerWheel.position  = centerWheelPosRef.position;         
            if (Constant.ScreenRatio == "16X10")
                 glowEffectObject.GetComponent<RectTransform>().localPosition = new Vector3(3, 15, 0);
        }

        bool isBlocked = false;
        public void ToggleInputBlocker()
        {
            TimerController.inst.TimeUp -= ToggleInputBlocker;
            isBlocked = !isBlocked;
            SetInputBlocker(true);
            SetText(false);
            ButtonManager.instance.ToggleClearDoubleRepeat(false);
        }

        public void SetInputBlocker(bool state = true)
        {
            InputBlockerObj.enabled = state;
            SetText(false);
        }

        public void StartSpinning()
        {
            infoPanel.SetActive(false);
            wheelO.StartWheelMotion();
            wheelI.StartWheelMotion();
            slotLeft.StartMotion(true);
            slotRight.StartMotion(true);
            SetCenterWheelAnimation(true);
            SetCenterWheelTexts(false);
        }

        public void GetResult()
        {
            DataCollector.instance.GetResult();
        }

        public void StopSlot(int cardID, int cardSuite)
        {
            slotLeft.StopMotionAtCard(cardID);
            slotRight.StopMotionAtCard(cardSuite);
        }
        Vector2 newCardData;
        public void StopWheel(int cardId, int cardSuite)
        {
            if (wheelO.velocity == 0)
                return;
            wheelO.StopWheel(cardId);
            wheelI.StopWheel(cardSuite);
            newCardData = new Vector2(cardId, cardSuite);
        }

        public void ResetAllCardValues()
        {
            if (cardSelector == null)
                cardSelector = FindObjectOfType<CardSelector>();
            cardSelector.BroadcastMessage("IsCleared", false ,SendMessageOptions.DontRequireReceiver);
        }

        public void SetText(bool state)
        {
            if (cardSelector == null)
                cardSelector = FindObjectOfType<CardSelector>();
            cardSelector.ToggleAllTexts(state);
        }

        public void SetCardHistoryDeck(List<CardHistoryData> historyDeck)
        {
            if (cardHistoryDeck == null)
                cardHistoryDeck = FindObjectOfType<CardHistoryDeck>();
            cardHistoryDeck.SetDeckBase(historyDeck);
        }

        public void SetCenterWheelAnimation(bool state)
        {
            cardFlashAnimation.SetActive(state);
            if(!state)
            {
                centerWheelCardAndSuite.SetCardImage((int)newCardData.x, (int)newCardData.y);
                SetSelectionWheelLights(true);
            }
            else
            {
                SetSelectionWheelLights(false);
                wheelLights.SetActive(false);
                multiplierText.gameObject.SetActive(false);
            }
        }
        //glow object
        public void SetSelectionWheelLights(bool state)
        {
            if (state)
            {
                wheelLights.SetActive(state);
                wheelGlowAnimator.SetBool("blink", state);
             //   SetCenterWheelTexts(state);
            }
            else
            {
                wheelGlowAnimator.SetBool("blink", state);
           //     SetCenterWheelTexts(state);
            }           
        }
        //the history card in the center
        public void SetCenterWheelTexts(bool state)
        {
            centerWheelCardAndSuite.gameObject.SetActive(state);            
        }

        int xValue = 0;
        //the 2x,3x text
        public void SetMultiplierText(int newValue)
        {
            xValue = newValue;
            Debug.Log("New Multiplier Amount: "+ newValue);

            if (newValue == 1)
            {
                multiplierText.text = "";
            }
            else if(newValue == 10)
            {                
                multiplierText.text = "B";
            }
            else
            {
                multiplierText.text = newValue + "X";
            }
        }

        public bool IsSuperBumper()
        {
            if (xValue == 10)
                return true;
            else
                return false;
        }

        void SetStopEffects()
        {
            SetCenterWheelAnimation(false);
            SetCenterWheelTexts(true);
            CheckMultiplierStateOnStop();
        }

        public void CheckMultiplierStateOnStop()
        {
            if(xValue == 1)
            {
                multiplierText.gameObject.SetActive(false);
            }
            else if(IsWinner())
            {
                Debug.Log("Is Winner");
                SetMultiplierAnimator(true, true);
            }
            else
            {
                Debug.Log("Didn't Winner");
                SetMultiplierAnimator(true, false);
            }
        }

        public void SetMultiplierAnimator(bool state, bool animState)
        {
           multiplierText.gameObject.SetActive(state);
           multiplierText.gameObject.GetComponent<Animator>().SetBool("pop", animState);
        }

        public void SetWheelResults(int outerWheelTargetID, int innerWheelTargetID)
        {
            wheelO.ManualSetWheel(outerWheelTargetID);
            wheelI.ManualSetWheel(innerWheelTargetID);
        }

        public void SetSlotResults(int id, int suite)
        {
            slotLeft.ManualSetWheel(id);
            slotRight.ManualSetWheel(suite);
        }

        public bool IsWinner()
        {
            if(( Constant.PointBalance - SuperJokerConstants.PointBalance) > 0)
            {
                return true;
            }

            return false;
        }
    }

      
}
