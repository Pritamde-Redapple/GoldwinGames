using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBox : MonoBehaviour {
    public Transform tResultParent;

	private void OnEnable()
	{
        UpdateResult();
    }

    public void UpdateResult()
    {
        int count = tResultParent.childCount;
        for (int i = 0; i < count; i++)
        {
            if (UIController.instance.sAllResult.Count > i)
                tResultParent.GetChild(i).GetComponent<Result>().SetParameter(UIController.instance.sAllResult[i]);
        }
    }
	
}
