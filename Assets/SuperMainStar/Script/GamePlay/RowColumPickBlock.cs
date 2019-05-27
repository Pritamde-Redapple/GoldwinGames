using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RowColumPickBlock : InputDetection {

    public enum RowColumType
    {
        Row,
        Coloum
    }

    public RowColumType rowColumType;
    public Block.BlockType blockType;
    public Transform tButtonParent;
    public List<Block> lstAllBlock;
  
	private void OnValidate()
	{
        OnClick();
	}


	public void OnClick()
    {
       
        lstAllBlock = new List<Block>();
        lstAllBlock.Clear();
      //  Debug.Log(gameObject.name);
        int _iBlockIndex = int.Parse(gameObject.name);

        if(rowColumType == RowColumType.Row)
        {
            for (int i = 0; i < 10; i++)
            {
                int _index = (_iBlockIndex * 10) + i;
                lstAllBlock.Add(tButtonParent.GetChild(_index).GetComponent<Block>());

            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                int _index = (_iBlockIndex + (i * 10));
                lstAllBlock.Add(tButtonParent.GetChild(_index).GetComponent<Block>());
            }

        }

    }


	public override void LeftClick()
	{
        SoundController.instance.PlayAudio(SoundController.ClipType.CHIP);
        base.LeftClick();

        if (!GamePlay.instance.IsRemoveClicked)
        {
            for (int i = 0; i < lstAllBlock.Count; i++)
            {
                lstAllBlock[i].OnAddingBlock();
            }
            int _amount = Constant.GetLimitedBetAmount(blockType, GamePlay.instance.currentSelectedChip);
            if (GamePlay.instance.CheckBalance(_amount))
            {
                CalculateTotalPlay();               
            }
            else
            {
                GamePlay.instance.EnablePopUpForInsufficient(this.transform, blockType, PopUp.ERROR.Insufficient_Balance);
            }
        }
        else
        {
            RightClick();
        }
      
    }

	public override void RightClick()
	{
        SoundController.instance.PlayAudio(SoundController.ClipType.CHIP);
        base.RightClick();
        for (int i = 0; i < lstAllBlock.Count; i++)
        {
            lstAllBlock[i].RightClick();
        }

        CalculateTotalPlay();
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
        base.OnPointerEnter(eventData);

        CalculateTotalPlay();
    }

	public override void OnPointerExit(PointerEventData eventData)
	{
        base.OnPointerExit(eventData);
        GamePlay.instance.DisablePopUp();
	}


    void CalculateTotalPlay()
    {
        // Enable popup
        int _iBetAmount = 0;
        for (int i = 0; i < lstAllBlock.Count; i++)
        {
            _iBetAmount += lstAllBlock[i].iBetAmount;
        }

      //  Debug.Log("Total Pay " + _iBetAmount);
        GamePlay.instance.EnablePopUpForRowColoum(this.transform, _iBetAmount.ToString());
    }
}
