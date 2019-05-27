using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlockPanel : MonoBehaviour {
    public List<BlockDetail> allBlockPanel;

    public float speed = 100;

    public bool isOpen = false;
    private bool isMoving = false;
    private int previousIndex;
    private int afterIndex;
    private int currentIndex;

    public Sprite sOpenArrow;
    public Sprite sCloseArrow;

    public void BlockPanelClicked(int index)
    {
        Debug.Log("index " + index);
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
        float panelPos = 0;
        RectTransform _rectTransform = null;

        _rectTransform = allBlockPanel[i].rectTransform;
        if (!isOpen)
        {
            endPosX = allBlockPanel[i].fOpenPos;
            panelPos = allBlockPanel[i].fOpenPosForPanel;
            allBlockPanel[i].arrow.sprite = sOpenArrow;
        }
        else
        {
            endPosX = allBlockPanel[i].fClosePos;
            panelPos = allBlockPanel[i].fClosePosForPanel;
            allBlockPanel[i].arrow.sprite = sCloseArrow;
        }
        isOpen = !isOpen;

        isMoving = true;

       
        allBlockPanel[i].rectTransform.DOAnchorPosX(endPosX, 1f).SetEase(Ease.OutQuad).OnComplete(() => { OnComplete(); });
        allBlockPanel[i].rBlockPanel.DOAnchorPosX(panelPos, 1f).SetEase(Ease.OutQuad);
        // StartCoroutine(MovePanel(_rectTransform, endPosX, allBlockDetails[i].fAdditionValueForPanel));
    }


    void OnComplete()
    {
        isMoving = false;

        if (afterIndex != -1)
        {
            previousIndex = afterIndex;
            Move(afterIndex);
            afterIndex = -1;
        }
    }
}

[System.Serializable]
public class BlockDetail
{
    public string name;
    public float fOpenPos;
    public float fClosePos;

    public float fOpenPosForPanel;
    public float fClosePosForPanel;

    public RectTransform rectTransform;
    public RectTransform rBlockPanel;
    public float fAdditionValueForPanel;
    public Image arrow;
    public Text tPlayValue;
    public Text tWinValue;

    public Image tPlayBG;
    public Image tWinBG;
}
