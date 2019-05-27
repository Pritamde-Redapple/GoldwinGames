using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Block : InputDetection {

    public enum BlockType
    {
        SINGLE,
        DOUBLE,
        TRIPLE
    }

    public enum BlockState
    {
        NORMAL,
        CLICKED
    }
    public BlockType blockType;
    public BlockState blockState;
    public int iBetAmount;

    public GameObject gNormalButton;
    public GameObject gClickedButton;

    public Text tNormalStateNumber;
    public Text tClickedStateNumber;
    public Text tBetAmount;

   public Sprite sHighliteSprite;
   public Sprite sPressedSprite;
    public Sprite sResultState;
    public Sprite sWinSprite;

    public KeyCode alphaKeyCode;
    public KeyCode keyBoardKeyCode;


    Image image;
    Sprite sDefaultSprite;
   

    #region All Delegates

    public delegate void OnSelectBlock(Transform transform);
    public static OnSelectBlock onSelectBlock;

    public delegate void OnDeSelectBlock(Block block);
    public static OnDeSelectBlock onDeSelectBlock;

    public delegate void OnAddBlock(Transform transform);
    public static OnAddBlock onAddBlock;

    public delegate void OnSelectTripleBlock(Block _block);
    public static OnSelectTripleBlock onSelectTripleBlock;

    #endregion
    private void Awake()
	{
        image = transform.GetChild(0).GetComponent<Image>();
        sDefaultSprite = image.sprite;
	}


	private void OnEnable()
	{
        GamePlay.onResultSuccess += OnDecideResult;
	}

	private void OnDisable()
	{
        GamePlay.onResultSuccess -= OnDecideResult;
	}
    private void Update()
    {
        if(Input.GetKeyDown(alphaKeyCode) || Input.GetKeyDown(keyBoardKeyCode))
        {
            LeftClick();
        }
       
    }


    void ResetData()
    {
        iBetAmount = 0;
    }

    public void UpdateText(string number)
    {
        tNormalStateNumber.text = number;
        tClickedStateNumber.text = number;

        iBetAmount = 0;
        image.sprite = sDefaultSprite;
        blockState = BlockState.NORMAL;

        gNormalButton.SetActive(true);
        gClickedButton.SetActive(false);
    }

    public void OnAddingBlock()
    {
        if(onAddBlock != null)
        {
            onAddBlock(this.transform);
        }
    }

    public void OnSelectSuccess(int _iBetAmount)
    {
        
        // set button to clicked one
        iBetAmount = _iBetAmount;
        image.sprite = sPressedSprite;
        blockState = BlockState.CLICKED;

        gNormalButton.SetActive(false);
        gClickedButton.SetActive(true);

        tBetAmount.text = iBetAmount.ToString();
       

        if (blockType == BlockType.TRIPLE && onSelectTripleBlock != null)
        {
            onSelectTripleBlock(this);
        }
    }

	public override void LeftClick()
	{
        base.LeftClick();
		
        if (onSelectBlock != null)
        {
            onSelectBlock(this.transform);
        }
	}

	public override void RightClick()
	{
        base.RightClick();
	
        if (onDeSelectBlock != null)
        {
            onDeSelectBlock(this);
        }
	}

    public void OnDeselectSuccess(int _gameAmount)
    {
        iBetAmount = _gameAmount;
      

        if (_gameAmount == 0)
        {
            image.sprite = sDefaultSprite;
            blockState = BlockState.NORMAL;

            gNormalButton.SetActive(true);
            gClickedButton.SetActive(false);
        }
        else
        {
            OnSelectSuccess(_gameAmount);
        }


       
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(blockState == BlockState.NORMAL)
        {
            image.sprite = sHighliteSprite;
        }
        else
        {
            GamePlay.instance.EnablePopupForBlock(this.transform);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (blockState == BlockState.NORMAL)
        {
            image.sprite = sDefaultSprite;
        }

        GamePlay.instance.DisablePopUp();
    }


    public void OnDecideResult(string _sNum)
    {
        if((blockType == BlockType.TRIPLE && (_sNum == tNormalStateNumber.text))
           || (blockType == BlockType.DOUBLE && (_sNum.Substring(1,2) == tNormalStateNumber.text))
           || (blockType == BlockType.SINGLE && (_sNum.Substring(2, 1) == tNormalStateNumber.text))) 
       
            {
               GamePlay.instance.lstResultBlock.Add(this);
                if (blockState == BlockState.NORMAL)
                {
                    image.sprite = sResultState;
                }
                else
                {
                    image.sprite = sWinSprite;

                    gNormalButton.SetActive(true);
                    gClickedButton.SetActive(false);
#if UNITY_ANDROID
                GamePlay.instance.ShowWinAmount(blockType, _sNum, iBetAmount);
#endif
                }
            } 
    }


}
