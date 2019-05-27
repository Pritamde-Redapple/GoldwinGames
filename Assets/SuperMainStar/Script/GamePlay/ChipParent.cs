using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipParent : MonoBehaviour {

	private void Start()
	{
		transform.GetChild(0).GetComponent<Chip>().OnClick(false);
	}

}
