using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabParent : MonoBehaviour {

    private void OnEnable()
    {
		transform.GetChild(0).GetComponent<Tab>().OnClick(false);
    }

    public void EnableATab(int _index)
    {
		transform.GetChild(_index).GetComponent<Tab>().OnClick(false);
    }
}
