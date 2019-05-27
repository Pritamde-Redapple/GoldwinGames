using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{

    public Text tTriple;
    public Text tDouble;
    public Text tSingle;

    public string num;


    public string Triple;
    public string Double;
    public string Single;

    private GameObject multiplier;

    public void SetParameter(WinningData data)
    {
        string _iNum = num = int.Parse(data.number).ToString("000");
       // number = num = string.Format("{0:000}", number);


         Triple = _iNum;
        _iNum = _iNum.Remove(0, 1);
         Double = _iNum;
        _iNum =  _iNum.Remove(0, 1);
         Single = _iNum;

        tTriple.text = Triple;
        tDouble.text = Double;
        tSingle.text = Single;

        if(multiplier == null)
        {
            multiplier = transform.GetChild(3).gameObject;
        }

        if(data.multiplier == "1")
        {
            multiplier.SetActive(false);
        }
        else
        {
            multiplier.SetActive(true);
            if (data.multiplier == "10")
            {
                multiplier.transform.GetChild(0).GetComponent<Text>().text = "B";
            }
            else
               multiplier.transform.GetChild(0).GetComponent<Text>().text = data.multiplier + "X";
        }
    }
}
