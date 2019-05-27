using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RandomPickBlock : InputDetection {
    
   

    public Block.BlockType pickBlockType;
    public Transform tButtonParent;

    public Sprite sHighliteSprite;
    private Sprite sDefaultSprite;

    Image image;


    #region All Delegates

    public delegate void OnSelectRandomPickBlock(Block.BlockType blockType);
    public static OnSelectRandomPickBlock onSelectRandomPickBlock;

    #endregion
  

    private void Start()
    {
        image = transform.GetComponent<Image>();
        sDefaultSprite = image.sprite;
    }

    public void SetNormalSprite()
    {
        image.sprite = sDefaultSprite;
    }

	public override void LeftClick()
	{
        base.LeftClick();
        SoundController.instance.PlayAudio(SoundController.ClipType.RANDOM_BUTTON);
        RemovePreviousSelectedBlock();
        transform.parent.GetComponent<RandomClickParent>().DeselectAllButton();
        image.sprite = sHighliteSprite;

        if(pickBlockType == Block.BlockType.DOUBLE)
        {
            GamePlay.instance.currentSelectedDoubleRandomBlock = this;
        }
        else
        {
            GamePlay.instance.currentSelectedTripleRandomBlock = this;
        }

        List<int> numbers = new List<int>();
        numbers.Clear();
        int _iRandomPickBlockIndex = int.Parse(gameObject.name);
        for (int i = 0; i < 100; i++)
        {
            numbers.Add(i);
        }

        int[] randomNumbers = new int[_iRandomPickBlockIndex];

        for (int i = 0; i < randomNumbers.Length; i++)
        {
            int thisNumber = Random.Range(0, numbers.Count);
            randomNumbers[i] = numbers[thisNumber];
            numbers.RemoveAt(thisNumber);
            if(pickBlockType == Block.BlockType.TRIPLE)
            {
                tButtonParent = GamePlay.instance.currentSelectedTab.buttonPanel;
            }
            tButtonParent.GetChild(randomNumbers[i]).GetComponent<Block>().OnAddingBlock();
        }



    }

	public override void RightClick()
	{
        base.RightClick();
	}


    void RemovePreviousSelectedBlock()
    {
        

        if(pickBlockType == Block.BlockType.TRIPLE)
        {
            GamePlay.instance.currentSelectedTab.OnRandomPickClicked();
        }
        else if (onSelectRandomPickBlock != null)
        {
            onSelectRandomPickBlock(pickBlockType);
        }
    }

	public override void OnPointerEnter(PointerEventData eventData)
	{
        image.sprite = sHighliteSprite;
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
        if ((pickBlockType == Block.BlockType.DOUBLE && GamePlay.instance.currentSelectedDoubleRandomBlock != this) ||
            (pickBlockType == Block.BlockType.TRIPLE && GamePlay.instance.currentSelectedTripleRandomBlock != this))
        {
            image.sprite = sDefaultSprite;
        }
	}
}

