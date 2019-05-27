using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHistoryElement : MonoBehaviour {

    public Text txtSNo;
    public Text txtHandId;
    public Text txtPlayValue;
    public Text txtWinValue;

    public void SetParameter(string handId, double playValue, double winValue)
    {
        txtHandId.text = handId;
        txtPlayValue.text = playValue.ToString();
        txtWinValue.text = winValue.ToString();

        txtSNo.text = transform.GetSiblingIndex().ToString();
    }

}
