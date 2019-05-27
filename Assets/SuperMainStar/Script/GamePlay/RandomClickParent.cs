using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClickParent : MonoBehaviour {

    public  void DeselectAllButton()
    {
        int count = transform.childCount;
        for (int i = 0; i < count; i++)
        {
            transform.GetChild(i).GetComponent<RandomPickBlock>().SetNormalSprite();
        }
    }
}
