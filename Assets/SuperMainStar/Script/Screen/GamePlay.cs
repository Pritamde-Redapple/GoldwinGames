using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GWebUtility;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlay : UIPage
{
    public static GamePlay instance;

    public RectTransform clickPopUp;
    public GameObject gWinPanel;
    public GameObject gWinData;
	public Text tWinAmount;
    public Text tWinAmountInWinPanel;

    public ResultBox resultBox;
    private List<BlockData> lstAllBlockData;
    private List<BlockData> lstBlockDataForRepeat;

    public List<Block> lstResultBlock;
    public List<Chip> lstAllChip;
    public Chip lastSelectedChip;

    public int currentSelectedChip;
    public Text tPointBalance;
    public Text tPlayValue;
    public Text tUserName;


    public Transform tripleButtonParent;
    public List<Tab> lstAllTab;

    public Button bClear;
    public Button bDoubleUp;
    public Button bRepeat;
	public Button bRemove;

    private bool isRemoveClicked;

    public GameObject gInfoPanel;

    public Text messageBoxText ;
    public Animation aMessageBoxOverlay;

    private int iSelectedTabIndex;
    private double iPointBalance;


    public Tab currentSelectedTab;
    public RandomPickBlock currentSelectedDoubleRandomBlock;
    public RandomPickBlock currentSelectedTripleRandomBlock;

    public TabParent tabParent;
    private double playValue;
    public double IPointBalance
    {
        get
        {
            return iPointBalance;
        }

        set
        {
            iPointBalance = value;
            tPointBalance.text = value.ToString();
            playValue = (firstPointBalance - IPointBalance);
            tPlayValue.text = playValue.ToString();

           // Debug.Log("play value : " + playValue);
        }
    }

    public bool IsRemoveClicked
    {
        get
        {
            return isRemoveClicked;
        }

        set
        {
            isRemoveClicked = value;
            if(!value && lastSelectedChip != null)
            {
                lastSelectedChip.EnableDisable(true);
            }
        }
    }

    private string sResultNumber = "000";
    private string singleWinAmount;
    private string doubleWinAmount;
    private string tripleWinAmount;

    public List<SpinWheel> lstAllWheel;
    public Text tResultNumber;
    public Text multiplierText;
    public ParticleSystem bumperEffect;
    public ParticleSystem coinEffectForLessWin;

    public Text singleWinValueText;
    public Text doubleWinValueText;
    public Text tripleWinValueText;

    public delegate void OnClearClicked();
    public static OnClearClicked onClearClicked;


    private double firstPointBalance;

    #region Android Version
    public Sprite sNormalBGForPlay;
    public Sprite sSelectedBGForPlay;
    public GameObject gNotEnoughCoin;
    #endregion

    #region All Delegates

    public delegate void OnResultSuccess(string number);
    public static OnResultSuccess onResultSuccess;



    #endregion

    private float delayBetweenResultCallback;
    private Timer timerScript;

    private void Awake()
    {
        instance = this;
        timerScript = GetComponent<Timer>();
    }
    private void Start()
    {
        UIController.instance.Addpage(this, false);

        IPointBalance = firstPointBalance = Constant.PointBalance; 
    }

	
	private void OnEnable()
    {
        lstAllBlockData = new List<BlockData>();
        lstAllBlockData.Clear();
        bClear.interactable = false;
        bDoubleUp.interactable = false;

        Block.onSelectBlock += OnSelectBlock;
        Block.onDeSelectBlock += OnDeSelectBlock;
        Block.onAddBlock += OnSelectFromRowColoum;


        RandomPickBlock.onSelectRandomPickBlock += RemovePreviousSelectedBlock;

        if (UIController.instance.sAllResult.Count > 0)
        {
            sResultNumber = int.Parse(UIController.instance.sAllResult[0].number).ToString("000");

            for (int i = 0; i < lstAllWheel.Count; i++)
            {
                lstAllWheel[i].FindNumberInArrayAndSetAngle(int.Parse(sResultNumber[i].ToString()));
            }
        }
    }

    private void OnDisable()
    {
        Block.onSelectBlock -= OnSelectBlock;
        Block.onDeSelectBlock -= OnDeSelectBlock;
        Block.onAddBlock -= OnSelectFromRowColoum;

        RandomPickBlock.onSelectRandomPickBlock -= RemovePreviousSelectedBlock;
        SoundController.instance.PlayAndStopWinSound(false);
    }
    public override void OnEnter()
	{
        base.OnEnter();
        float timeValue = 90 - Constant.TimerForGame;
        UpdateTimer(timeValue);
        ShowMessage( "Place your chip");
		if(tUserName != null)
            tUserName.text = Constant.Name.ToUpper();
        gWinData.SetActive(false);
       

        if (singleWinValueText != null)
        {
            singleWinValueText.text = singleWinAmount = "0";
            doubleWinValueText.text = doubleWinAmount = "0";
            tripleWinValueText.text = tripleWinAmount = "0";
        }

        ResetPlayValue();
    }

    public void  UpdateTimer(float timeValue)
    {
        Debug.Log("Updating Timer :"+ timeValue);
        if (timeValue <= 0)
        {
            Debug.Log("Timer complete");
            timerScript.StopTimer();
            timerScript.BlockAllButton();
            if (Constant.GameStatus == ErrorCode.RUNNING)
            {
                StartSpinning();
            }
            else
            {
                UIController.instance.MessagePopUp();
                StopAllWheel();
            }
        }
        else
        {
            timerScript.StartTimer(timeValue);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

   
  public void ResetAllData()
    {
        if(UIController.instance.messagePopUp != null)
        {
            Destroy(UIController.instance.messagePopUp);
        }
        lstBlockDataForRepeat = new List<BlockData>();
        lstBlockDataForRepeat.Clear();

        for (int i = 0; i < lstAllBlockData.Count; i++)
        {
            BlockData _blockData = new BlockData(lstAllBlockData[i].block, lstAllBlockData[i].game_number, lstAllBlockData[i].game_amount);
            lstBlockDataForRepeat.Add(_blockData);
        }
#if UNITY_ANDROID
       // if (lstBlockDataForRepeat.Count > 0)
        {
            bRepeat.gameObject.SetActive((lstBlockDataForRepeat.Count > 0));
            bDoubleUp.gameObject.SetActive(!(lstBlockDataForRepeat.Count > 0));
        }
        bRemove.interactable = false;
#endif
        ClearAllBlock();
        for (int i = 0; i < lstResultBlock.Count; i++)
        {
            lstResultBlock[i].OnDeselectSuccess(0);
        }
           bRepeat.interactable = ((lstBlockDataForRepeat.Count > 0));
     /*  if(lstBlockDataForRepeat.Count > 0)
       else
            bRepeat.interactable = false;*/
        gWinData.SetActive(false);
      
        ShowMessage("Place your chip");


        ResetPlayValue();

        if (singleWinValueText != null)
        {
            singleWinValueText.text = singleWinAmount = "0";
            doubleWinValueText.text = doubleWinAmount = "0";
            tripleWinValueText.text = tripleWinAmount = "0";
        }

        DisableWinPanel();
        Debug.Log("Reset All Data");
        playValue = 0;
        timerScript.StartTimer(90);
    }

    void ResetPlayValue()
    {
        for (int i = 0; i < allBlockDetails.Count; i++)
        {
            allBlockDetails[i].tPlayValue.text = "PLAY : 0";
            allBlockDetails[i].tWinValue.text = "WIN : 0";

            allBlockDetails[i].tPlayBG.sprite = sNormalBGForPlay;
            allBlockDetails[i].tWinBG.sprite = sNormalBGForPlay;
        }
    }
	void OnSelectBlock(Transform _trans)
    {
        if (timerScript.timeLeft > 6f)
        {
            if (IsRemoveClicked)
            {
                OnDeSelectBlock(_trans.GetComponent<Block>());
                if (lstAllBlockData.Count == 0)
                    IsRemoveClicked = false;
            }
            else
            {
                SoundController.instance.PlayAudio(SoundController.ClipType.BLOCK);
                Block _block = _trans.GetComponent<Block>();
                int _amount = Constant.GetLimitedBetAmount(_block.blockType, currentSelectedChip);
                if (CheckBalance(_amount))
                {

                    if (Constant.GetMaximumBetAmount(_block.blockType) >= (_block.iBetAmount + _amount))
                    {
                        IPointBalance -= _amount;
                        AddDataToList(_trans, _amount);
                        EnablePopupForBlock(_trans);

                    }
                    else
                    {
                        EnablePopUpForInsufficient(_trans, _trans.GetComponent<Block>().blockType, PopUp.ERROR.Maximum_Bet);
                        ShowMessage("Maximum Bet Limit Exceed");
                    }
                }
                else
                {
                    EnablePopUpForInsufficient(_trans, _trans.GetComponent<Block>().blockType, PopUp.ERROR.Insufficient_Balance);
                    ShowMessage("Insufficient Balance");
                }
            }

        }

    }


    void OnDeSelectBlock(Block _block)
    {
        SoundController.instance.PlayAudio(SoundController.ClipType.BUTTON);
#if !UNITY_ANDROID
        clickPopUp.gameObject.SetActive(false);
#endif
        BlockData _blockData = lstAllBlockData.Find((BlockData obj) => (obj.game_number == _block.tNormalStateNumber.text));
        if (_blockData != null)
        {

            AddToBalance(_blockData.allGameAmount.Last());
            bool _isLast = _blockData.RemoveLastAmount();
            _block.OnDeselectSuccess(_blockData.game_amount);
            if(_isLast)
            {
                RemoveBlockData(_blockData, _block.blockType, false);
            }
            else
            {
                EnablePopupForBlock(_block.transform);
#if UNITY_ANDROID
                int playValue = GetPlayValueFromBlock(_block.blockType);
               // Debug.Log("After Remove : " + playValue);
                allBlockDetails[(int)_block.blockType].tPlayValue.text = "PLAY : " + playValue;
                if(playValue == 0)
                    allBlockDetails[(int)_block.blockType].tPlayBG.sprite = sNormalBGForPlay;
                else
                    allBlockDetails[(int)_block.blockType].tPlayBG.sprite = sSelectedBGForPlay;
#endif
            }

            // RemoveBlockData(_blockData);
        }

        if (lstAllBlockData.Count == 0)
            IsRemoveClicked = false;

    }

    public void OnSelectFromRowColoum(Transform _trans)
    {
        Block _block = _trans.GetComponent<Block>();
        int _amount = Constant.GetLimitedBetAmount(_block.blockType, currentSelectedChip);

        if (CheckBalance(_amount) && Constant.GetMaximumBetAmount(_block.blockType) >= (_block.iBetAmount + _amount))
        {
            IPointBalance -= _amount;
            AddDataToList(_trans, _amount);
        }
        else
        {
            //Debug.Log("Insufficient Balance");
        }
    }


    public void AddDataToList(Transform _trans, int _iExtraAmount)
    {
        Block _block = _trans.GetComponent<Block>();
        BlockData _blockData = lstAllBlockData.Find((BlockData obj) => (obj.game_number ==  _block.tNormalStateNumber.text));
        if (_blockData != null)
        {
            _blockData.game_amount += _iExtraAmount;
            _blockData.allGameAmount.Add(_iExtraAmount);
        }
        else
        {
			_blockData = new BlockData(_block,  _block.tNormalStateNumber.text, _iExtraAmount);
            lstAllBlockData.Add(_blockData);
            bClear.interactable = true;
            bDoubleUp.interactable = true;
#if UNITY_ANDROID
            bRemove.interactable = true;
#endif
        }

#if !UNITY_ANDROID
        clickPopUp.GetComponent<PopUp>().SetBlockData(_block.blockType, _block.tNormalStateNumber.text, _blockData.game_amount);
#else

        Block.BlockType type = _trans.GetComponent<Block>().blockType;
        allBlockDetails[(int)type].tPlayValue.text = "PLAY : " + GetPlayValueFromBlock(type);
        allBlockDetails[(int)_block.blockType].tPlayBG.sprite = sSelectedBGForPlay;

#endif
      
        _trans.GetComponent<Block>().OnSelectSuccess(_blockData.game_amount);
        bRepeat.interactable = false;
#if UNITY_ANDROID
        bRepeat.gameObject.SetActive(false);
#endif
        bDoubleUp.gameObject.SetActive(true);
    }

    public void AddDataToListForDoubleUp(BlockData _blockData)
    {
            int _iExtraAmount = _blockData.game_amount;
            _blockData.game_amount += _iExtraAmount;
            _blockData.allGameAmount.Add(_iExtraAmount);

        if((currentSelectedTab.name[0] == _blockData.game_number[0] && _blockData.block.blockType == Block.BlockType.TRIPLE)
            || (_blockData.block.blockType == Block.BlockType.DOUBLE)
             || (_blockData.block.blockType == Block.BlockType.SINGLE))
            _blockData.block.OnSelectSuccess(_blockData.game_amount);


#if UNITY_ANDROID
        Block.BlockType type = _blockData.block.blockType;
        allBlockDetails[(int)type].tPlayValue.text = "PLAY : " + GetPlayValueFromBlock(type);
        allBlockDetails[(int)_blockData.block.blockType].tPlayBG.sprite = sSelectedBGForPlay;
#endif

    }

    public void AddDataToListForRepeat(BlockData _perviousBlockData, int _iExtraAmount)
    {
        BlockData _blockData = lstAllBlockData.Find((BlockData obj) => (obj == _perviousBlockData));
        if (_blockData != null)
        {
            _blockData.game_amount += _iExtraAmount;
            _blockData.allGameAmount.Add(_iExtraAmount);
        }
        else
        {
            _blockData = new BlockData(_perviousBlockData.block, _perviousBlockData.game_number, _iExtraAmount);
            lstAllBlockData.Add(_blockData);
            bClear.interactable = true;
            bDoubleUp.interactable = true;
#if UNITY_ANDROID
            bRemove.interactable = true;
#endif
        }
        if ((currentSelectedTab.name[0] == _blockData.game_number[0] && _blockData.block.blockType == Block.BlockType.TRIPLE)
            || (_blockData.block.blockType == Block.BlockType.DOUBLE)
             || (_blockData.block.blockType == Block.BlockType.SINGLE))
            _blockData.block.OnSelectSuccess(_blockData.game_amount);

        if (_blockData.block.blockType == Block.BlockType.TRIPLE)
        {
            int _index = int.Parse(_blockData.game_number[0].ToString());
            lstAllTab[_index].AddData(_blockData);
            lstAllTab[_index].Select();
        }

#if UNITY_ANDROID
        Block.BlockType type = _blockData.block.blockType;
        allBlockDetails[(int)type].tPlayValue.text = "PLAY : " + GetPlayValueFromBlock(type);
        allBlockDetails[(int)_blockData.block.blockType].tPlayBG.sprite = sSelectedBGForPlay;
#endif
        bRepeat.interactable = false;

    }

    public List<BlockData> GetAllBlockDataForTab(string name)
    {
        List<BlockData> _allBlockData = new List<BlockData>();
        _allBlockData = lstAllBlockData.FindAll((BlockData obj) => (obj.game_number[0] == name[0] && obj.block.blockType == Block.BlockType.TRIPLE));
        return _allBlockData;
    }
    void RemovePreviousSelectedBlock(Block.BlockType _blockType)
    {
        List<BlockData> _blockData = lstAllBlockData.FindAll((BlockData obj) => (obj.block.blockType == _blockType));
        for (int i = 0; i < _blockData.Count; i++)
        {
            _blockData[i].block.OnDeselectSuccess(0);
            RemoveBlockData(_blockData[i], _blockType);
        }
    }

    public void RemovePreviousSelectedTripleBlock(string _sTabNumber)
    {
        List<BlockData> _blockData = lstAllBlockData.FindAll((BlockData obj) => ((obj.block.blockType == Block.BlockType.TRIPLE) && obj.game_number[0] == _sTabNumber[0]));

        for (int i = 0; i < _blockData.Count; i++)
        {
            _blockData[i].block.OnDeselectSuccess(0);
            RemoveBlockData(_blockData[i], Block.BlockType.TRIPLE);
        }
    }

    int GetPlayValueFromBlock(Block.BlockType _blockType)
    {
        int _iPlayValue = 0;
        List<BlockData> _blockData = lstAllBlockData.FindAll((BlockData obj) => (obj.block.blockType == _blockType));
        for (int i = 0; i < _blockData.Count; i++)
        {
            _iPlayValue += _blockData[i].game_amount;
        }

        return _iPlayValue;
    }


    void RemoveBlockData(BlockData _blockData, Block.BlockType _blockType, bool _deductbalance = true)
    {
        if(_deductbalance)
             AddToBalance(_blockData.game_amount);
        lstAllBlockData.Remove(_blockData);
        if(lstAllBlockData.Count == 0)
        {
            bClear.interactable = false;
            bDoubleUp.interactable = false;
            bRepeat.interactable |= (lstBlockDataForRepeat != null && lstBlockDataForRepeat.Count > 0);

            #if UNITY_ANDROID
            if(bRepeat.interactable)
            {
                bRepeat.gameObject.SetActive(true);
                bDoubleUp.gameObject.SetActive(false);
            }

            #endif
        }

#if UNITY_ANDROID
        int playValue = GetPlayValueFromBlock(_blockType);
        allBlockDetails[(int)_blockType].tPlayValue.text = "PLAY : " + playValue;

        if (playValue == 0)
            allBlockDetails[(int)_blockType].tPlayBG.sprite = sNormalBGForPlay;
        else
            allBlockDetails[(int)_blockType].tPlayBG.sprite = sSelectedBGForPlay;
#endif
    }


    void SetPositionOfPopUp(Transform _trans)
    {
        clickPopUp.gameObject.SetActive(true);

        clickPopUp.transform.SetParent(_trans);
        clickPopUp.transform.localScale = Vector3.one;
        clickPopUp.anchoredPosition = Vector3.one;
        clickPopUp.transform.SetParent(this.transform.GetChild(0));

    }

    public void EnablePopupForBlock(Transform _trans)
    {
#if !UNITY_ANDROID
        SetPositionOfPopUp(_trans);

        Block _block = _trans.GetComponent<Block>();
        clickPopUp.GetComponent<PopUp>().SetBlockData(_block.blockType, _block.tNormalStateNumber.text, _block.iBetAmount);
#endif
    }


    public void EnablePopUpForRowColoum(Transform _trans, string _betAmount)
    {
#if !UNITY_ANDROID
        SetPositionOfPopUp(_trans);
        clickPopUp.GetComponent<PopUp>().SetBlockDataForRow(_trans.GetComponent<RowColumPickBlock>().blockType, _betAmount);
#endif
    }


    public void EnablePopUpForInsufficient(Transform _trans, Block.BlockType _blockType, PopUp.ERROR _error)
    {
#if !UNITY_ANDROID
        SetPositionOfPopUp(_trans);
        clickPopUp.GetComponent<PopUp>().ShowInsufficientPopUp(_blockType, _error);
#else
        // Show Insufficient popup
        gNotEnoughCoin.SetActive(true);
        string _msg = "";
        if(_error == PopUp.ERROR.Insufficient_Balance)
        {
            _msg = "Not enough points please top up your account or contact adminstrator";
        }
        else
        {
            _msg = "Maximum bet limit exceed";
        }

        gNotEnoughCoin.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _msg;

#endif
    }

    public void DisablePopUp()
    {
#if !UNITY_ANDROID
        clickPopUp.gameObject.SetActive(false);
#endif
    }

    public void DeSelectAllChip()
    {
        for (int i = 0; i < lstAllChip.Count; i++)
        {
            lstAllChip[i].EnableDisable(false);
        }
    }

    public void SelectChip(Chip chip, string chipName)
    {
        lastSelectedChip = chip;
        currentSelectedChip = int.Parse(chipName);
        IsRemoveClicked = false;

#if UNITY_ANDROID
        if (lstAllBlockData.Count > 0)
        {
           bRemove.interactable = true;
        }
#endif
       
    }

    public void DeSelectAllTab()
    {
        for (int i = 0; i < lstAllTab.Count; i++)
        {
            lstAllTab[i].DeSelect();
        }
    }


    public bool CheckBalance(float _iAmount)
    {
        if (IPointBalance - (_iAmount) >= 0)
        {
            return true;
        }
        return false;

    }



    public void AddToBalance(int _iBalance)
    {
        IPointBalance += _iBalance;
    }

    public float startSpinningTime = 0;
    public void StartSpinning()
    {
        startSpinningTime = Time.time;
        Debug.Log("Start Spinning");
        tResultNumber.text = "";
        multiplierText.text = "";
		SoundController.instance.StartAndStopWheelSpin (true);
        for (int i = 0; i < lstAllWheel.Count; i++)
        {
            // lstAllWheel[i].FindNumberInArray( int.Parse(sResultNumber[i].ToString()));
            lstAllWheel[i].RotateWheelInfinitely();
        }

        CloseInfo();
    }


   public void ShowMessage(string _sMessage)
    {
        messageBoxText.text = _sMessage;
        aMessageBoxOverlay.Play();
    }



    public void ShowResultBlock()
    {
        Debug.Log("Win Amount : " + Constant.WinAmount);
       if (Constant.WinAmount.Length > 0 && double.Parse(Constant.WinAmount) > 0)
        {
            gWinData.SetActive(true);
            tWinAmount.text = Constant.WinAmount;
            tWinAmountInWinPanel.text = Constant.WinAmount;
            Invoke("EnableWinPanel", 2f);

           
        }
        if (playValue != 0)
        {
            GameHistoryElement element = Instantiate(gameHistoryElement, gameHistory);
            element.SetParameter(Constant.CurrentGameSession, (firstPointBalance - IPointBalance), double.Parse(Constant.WinAmount));

            totalPlayValue += playValue;
            totalWinValue += double.Parse(Constant.WinAmount);

            txtTotalPlay.text = totalPlayValue.ToString();
            txtTotalWin.text = totalWinValue.ToString();
        }



        resultBox.UpdateResult();

        lstResultBlock = new List<Block>();
        lstResultBlock.Clear();
       // Debug.Log("Result : " + Constant.ResultNumber);
        int _iIndex = int.Parse(Constant.ResultNumber.Substring(0, 1));
        tabParent.EnableATab(_iIndex);
        tResultNumber.text = Constant.ResultNumber;
        if (UIController.instance.sAllResult[0].multiplier != "1")
        {
            if (UIController.instance.sAllResult[0].multiplier == "10")
            {
                multiplierText.text = "B";
                if(gWinData.activeInHierarchy)
                     bumperEffect.Play(); 
            }
            else
                multiplierText.text = UIController.instance.sAllResult[0].multiplier + "X";
        }

        if (double.Parse(Constant.WinAmount) >= 1000 && (UIController.instance.sAllResult[0].multiplier != "10"))
              coinEffectForLessWin.Play();
        if (onResultSuccess != null)
        {
            onResultSuccess(Constant.ResultNumber);
        }
       // Invoke("ResetAllData", 10f);

    }
	void EnableWinPanel()
	{
		gWinPanel.SetActive(true);
        SoundController.instance.PlayAndStopWinSound(true);
		//Invoke("DisableWinPanel", 8f);
	}
    void DisableWinPanel()
    {
        gWinPanel.SetActive(false);
        tPlayValue.text = "0";
        firstPointBalance = Constant.PointBalance;
        IPointBalance = Constant.PointBalance;
    }
#region Block Content For Android
	
    public float speed = 100;

    public bool isOpen = false;
    private bool isMoving = false;
    private int previousIndex;
    private int afterIndex;
    private int currentIndex;

    public List<BlockDetails> allBlockDetails;
    public Sprite sOpenArrow;
    public Sprite sCloseArrow;
    float fAdditionalAmount;

    public void BlockPanelClicked(int index)
	{
        if (!isMoving)
        {
            afterIndex = -1;
            if (isOpen && index != previousIndex)
            {
                afterIndex = index;
                Move(previousIndex);
            }
            else
            {
                previousIndex = index;
                Move(index);
            }

         
        }
	}

    private void Move(int i)
    {
        currentIndex = i;
        float endPosX = 0;
        RectTransform _rectTransform = null;

        _rectTransform = allBlockDetails[i].rectTransform;
        if (!isOpen)
        {
            endPosX = allBlockDetails[i].fOpenPos;
            allBlockDetails[i].arrow.sprite = sOpenArrow;
        }
        else
        {
            endPosX = allBlockDetails[i].fClosePos;
            allBlockDetails[i].arrow.sprite = sCloseArrow;
        }
        isOpen = !isOpen;

        fAdditionalAmount = allBlockDetails[i].fAdditionValueForPanel;
        StartCoroutine(MovePanel(_rectTransform, endPosX, allBlockDetails[i].fAdditionValueForPanel));
    }

    IEnumerator MovePanel(RectTransform _rectTransform, float endPosX, float fAdditionalAmount)
	{
        isMoving = true;
        Vector2 pos = allBlockDetails[currentIndex].rBlockPanel.anchoredPosition;
        if (isOpen)
        {
            pos.x += fAdditionalAmount;
        }

        allBlockDetails[currentIndex].rBlockPanel.anchoredPosition = pos;

         Vector3 endPos = _rectTransform.anchoredPosition;
		endPos.x = endPosX;

        yield return null;
        _rectTransform.DOAnchorPosX(endPosX, 0.35f).SetEase(Ease.OutQuad).OnComplete(() => { OnCompleteMove(); });
    }

    void OnCompleteMove()
    {
         Vector3  pos = allBlockDetails[currentIndex].rBlockPanel.anchoredPosition;
        if (!isOpen)
            pos.x -= fAdditionalAmount;
        allBlockDetails[currentIndex].rBlockPanel.anchoredPosition = pos;

        isMoving = false;

        if (afterIndex != -1)
        {
            previousIndex = afterIndex;
            Move(afterIndex);
            afterIndex = -1;
        }
    }

  public void ShowWinAmount(Block.BlockType _blockType,string _number, int _iAmount)
    {
        string winValue = "";
        if(_blockType == Block.BlockType.SINGLE)
        {
            winValue = singleWinAmount;
            if (singleWinValueText != null)
                singleWinValueText.text = singleWinAmount;
        }
        else if (_blockType == Block.BlockType.DOUBLE)
        {
            winValue = doubleWinAmount;
            if (doubleWinValueText != null)
                doubleWinValueText.text = doubleWinAmount;
        }
        else if (_blockType == Block.BlockType.TRIPLE)
        {
            winValue = tripleWinAmount;
            if (tripleWinValueText != null)
                tripleWinValueText.text = tripleWinAmount;
        }
        allBlockDetails[(int)_blockType].tWinValue.text = "WIN :" + winValue; // Constant.GetWinValue(_blockType, _number, _iAmount)
        allBlockDetails[(int)_blockType].tWinBG.sprite = sSelectedBGForPlay;
    }
#endregion
#region UI Button Function

    public void Close()
    {
        UIController.instance.TransitionTo(PageType.LOBBY);
    }

    public void ClearClicked()
    {
		SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
        for (int i = 0; i < lstAllBlockData.Count; i++)
        {
            AddToBalance(lstAllBlockData[i].game_amount);
            lstAllBlockData[i].block.OnDeselectSuccess(0);
        }

        lstAllBlockData.Clear();
        bClear.interactable = false;
        bDoubleUp.interactable = false;
#if UNITY_ANDROID
        bRemove.interactable = false;
        if (lstBlockDataForRepeat!= null && lstBlockDataForRepeat.Count > 0)
        {
            bRepeat.gameObject.SetActive(true);
            bDoubleUp.gameObject.SetActive(false);
        }
#endif

        if (onClearClicked != null)
            onClearClicked();

        bRepeat.interactable |= (lstBlockDataForRepeat != null && lstBlockDataForRepeat.Count > 0);

        ResetPlayValue();
    }


    void ClearAllBlock()
    {
        for (int i = 0; i < lstAllBlockData.Count; i++)
        {
            lstAllBlockData[i].block.OnDeselectSuccess(0);
        }

        lstAllBlockData.Clear();
        bClear.interactable = false;
        bDoubleUp.interactable = false;

        if (onClearClicked != null)
            onClearClicked();

        bRepeat.interactable |= (lstBlockDataForRepeat != null && lstBlockDataForRepeat.Count > 0);
    }

    public void DoubleUpClicked()
    {
		SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
        for (int i = 0; i < lstAllBlockData.Count; i++)
        {
            if( Constant.IsEligibleForDoubleUp(lstAllBlockData[i].block.blockType, lstAllBlockData[i].game_amount))
            {
                if (CheckBalance(lstAllBlockData[i].game_amount))
                {
                    IPointBalance -= (lstAllBlockData[i].game_amount);
                    AddDataToListForDoubleUp(lstAllBlockData[i]);
                }

              /*  int _amount = Constant.GetLimitedBetAmount(lstAllBlockData[i].block.blockType, lstAllBlockData[i].game_amount);
                Debug.Log(lstAllBlockData[i].block.name + " : " + _amount + " : Game Amount " + lstAllBlockData[i].game_amount + " Remaining balance :  " + IPointBalance);
                if (CheckBalance(_amount))
                {
                    if (Constant.GetMaximumBetAmount(lstAllBlockData[i].block.blockType) >= (lstAllBlockData[i].block.iBetAmount + _amount))
                    {
                        IPointBalance -= (_amount);
                        //   AddDataToList(lstAllBlockData[i].block.transform, lstAllBlockData[i].game_amount);
                        AddDataToListForDoubleUp(lstAllBlockData[i]);
                    }
                }*/
            }
            
        }
    }
    

    public void RepeatClicked()
    {
        Debug.Log("firstPointBalance" + firstPointBalance + " IPointBalance " + IPointBalance);
		SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
        for (int i = 0; i < lstBlockDataForRepeat.Count; i++)
        {
            int _iAmount = lstBlockDataForRepeat[i].game_amount;
            Transform _trans = lstBlockDataForRepeat[i].block.transform;
            if (IPointBalance - (_iAmount) >= 0)
            {
                IPointBalance -= _iAmount;
                AddDataToListForRepeat(lstBlockDataForRepeat[i], _iAmount);
            }
            else
            {
                ShowMessage("Not enough points and top up your account");
            }
        }
#if UNITY_ANDROID
        bRepeat.gameObject.SetActive(false);
        bDoubleUp.gameObject.SetActive(true);
#else
        bRepeat.interactable = false;
#endif
    }

    public void InfoClicked()
    {
        if(clickPopUp != null)
          clickPopUp.gameObject.SetActive(false);
		SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
        gInfoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        SoundController.instance.PlayAudio(SoundController.ClipType.BUTTON);
        gInfoPanel.SetActive(false);
    }

	public void RemoveClicked()
	{
		SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
		IsRemoveClicked = true;
		bRemove.interactable = false;

        DeSelectAllChip();
	}

    public override void YesClicked()
    {
        base.YesClicked();
        StopAllCoroutines();
        UIController.instance.TransitionTo(PageType.LOBBY);
    }
#endregion

#region Info Section

    public List<Image> selectedTabImage;
    public List<GameObject> allTabContent;

    public double totalPlayValue;
    public double totalWinValue;

    public Text txtTotalPlay;
    public Text txtTotalWin;

    public Transform gameHistory;
    public GameHistoryElement gameHistoryElement;

    public void TabInInfoClicked(int index)
    {
        SoundController.instance.PlayAudio(SoundController.ClipType.BUTTON);
        for (int i = 0; i < 3; i++)
        {
            selectedTabImage[i].enabled = false;
            allTabContent[i].SetActive(false);
        }

        selectedTabImage[index].enabled = true;
        allTabContent[index].SetActive(true);
    }

#endregion

#region All API

    public void SendGameData()
    {
        delayBetweenResultCallback = Time.time;
      //  Debug.Log(timerScript.timeLeft + " send ");
      if(clickPopUp != null)
        clickPopUp.gameObject.SetActive(false);
#if UNITY_ANDROID
        // To close Panel if it is opened
        if (isOpen)
        {
            if (isMoving)
            {
               // Debug.Log("is moving true");
                isMoving = false;
            }
            BlockPanelClicked(previousIndex);
        }

        gNotEnoughCoin.SetActive(false);
#endif
        Debug.LogError(" Data submit time " + Time.time + " :::: " + HrtzzJsonUtilityHelper.ToJson(lstAllBlockData.ToArray(), Constant.CurrentGameSession, Constant.CurrentDeviceType));
        GetComponent<AudioSource>().Play();
        ShowMessage("No More Play");
        Web.Create()
           .SetUrl(Constant.AddGameDataURL, Web.RequestType.POST, Web.ResponseType.TEXT)
             .AddPostData(HrtzzJsonUtilityHelper.ToJson(lstAllBlockData.ToArray(), Constant.CurrentGameSession, Constant.CurrentDeviceType))
             .AddHeader("Content-Type", "application/json")
		     .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
                    .SetOnSuccessDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Success " + _response.GetText());
                        Debug.Log("Data submit success " + Time.time);
                        JSONNode _jsonNode = JSON.Parse(_response.GetText());
                        if (_jsonNode["status"].Value == "1")
                        {
                         
                        }
                        else
                        {
                               UIController.instance.NoInterNetPopUp();
                        }

                        _web.Close();
                    })
                    .SetOnFailureDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Found Error " + _response.GetError());
                        UIController.instance.NoInterNetPopUp();
                        _web.Close();
                    })
                    .Connect();

  
    }

   public void GetGameResult()
   {
        float fetchingData = Time.time;
        Debug.Log("Fetching Data: "+ Time.time);
        Debug.Log("AccessToken : " + Constant.CurrentAccessToken + " GameSession : " + Constant.CurrentGameSession);
        Web.Create()
           .SetUrl(Constant.GameResultURL, Web.RequestType.POST, Web.ResponseType.TEXT)
           .AddField(Constant.GameSession, Constant.CurrentGameSession)
           .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
                  .SetOnSuccessDelegate((Web _web, Response _response) =>
                  {
                    
                      Debug.Log("Time diferrence: " + (Time.time - fetchingData));
                      Debug.Log("Success " + _response.GetText());
                      Debug.Log("Get result : " + Time.time);
                      JSONNode _jsonNode = JSON.Parse(_response.GetText());
                      if (_jsonNode["status"].Value == "1")
                      {
                          if (_jsonNode["result"]["wining_number"].Value != null)
						   sResultNumber = Constant.ResultNumber = int.Parse(  _jsonNode["result"]["wining_number"].Value).ToString("000");
                           Constant.WinAmount = _jsonNode["result"]["win_amount"].Value;

                            singleWinAmount = _jsonNode["result"]["result_win_singles_return_amount"].Value;
                            doubleWinAmount = _jsonNode["result"]["result_win_doubles_return_amount"].Value;
                            tripleWinAmount = _jsonNode["result"]["result_win_triples_return_amount"].Value;

                          
                            singleWinValueText.text = singleWinAmount;
                            doubleWinValueText.text = doubleWinAmount;
                            tripleWinValueText.text = tripleWinAmount;
                         

                          UIController.instance.GetResultForGame(_response.GetText());
                           Constant.PointBalance = double.Parse( _jsonNode["result"]["remaining_balance"].Value);

                          for (int i = 0; i < lstAllWheel.Count; i++)
                          {
                              lstAllWheel[i].number = ( int.Parse(sResultNumber[i].ToString()));
                          }
                          MoveToSpecificNumberInWheel();
                          Debug.Log((Time.time - delayBetweenResultCallback)); 
                          //if ((Time.time - delayBetweenResultCallback) > 9)
                          //{
                          //    MoveToSpecificNumberInWheel();
                          //}
                          //else
                          //{
                          //    Invoke("MoveToSpecificNumberInWheel", 5f);
                          //}
                      }
                      else
                      {
                          UIController.instance.NoInterNetPopUp();
                      }

                      _web.Close();
                  })
                  .SetOnFailureDelegate((Web _web, Response _response) =>
                  {
                      Debug.Log("Found Error " + _response.GetError());
                      UIController.instance.NoInterNetPopUp();
                      _web.Close();
                  })
                  .Connect();


    }

#endregion

    void MoveToSpecificNumberInWheel()
    {
        lstAllWheel[0].MoveToSpecificNumber(lstAllWheel[0].number);
    }

    public void StopAllWheel()
    {
        for (int i = 0; i < 3; i++)
        {
            lstAllWheel[i].StopWheel(sResultNumber[i].ToString());
        }
        SoundController.instance.StopWheelSound();
    }
}

[System.Serializable]
public class BlockData
{
    [System.NonSerialized] public Block block;  //[System.NonSerialized] 
    public string game_number;
    public int game_amount;
    [System.NonSerialized]  public List<int> allGameAmount; 

    public BlockData(Block block, string number, int game_amount)
    {
        this.block = block;
        this.game_number = number;
        this.game_amount = game_amount;
        allGameAmount = new List<int>();
        allGameAmount.Add(game_amount);
    }

    public bool RemoveLastAmount()
    {
        bool isLast = false;

        this.game_amount -= allGameAmount[allGameAmount.Count - 1];
        allGameAmount.RemoveAt(allGameAmount.Count - 1);

        if (!(allGameAmount.Count > 0))
        {
            isLast = true;
        }
        return isLast;
    }
}

[System.Serializable]
public class BlockDetails
{
    public string name;
    public float fOpenPos;
    public float fClosePos;
    public RectTransform rectTransform;
    public RectTransform rBlockPanel;
    public float fAdditionValueForPanel;
    public Image arrow;
    public Text tPlayValue;
    public Text tWinValue;

    public Image tPlayBG;
    public Image tWinBG;
}
