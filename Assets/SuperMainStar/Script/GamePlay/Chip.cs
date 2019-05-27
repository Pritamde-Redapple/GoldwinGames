using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Chip : MonoBehaviour {

    Button button;
    GameObject gClickedImage;
	private Image thisImage;

	public Sprite sNormalState;
	public Sprite sSelectedState;

	private void OnValidate()
	{
		thisImage = GetComponent<Image> ();
        gClickedImage = transform.GetChild(0).gameObject;

	}
	private void Awake()
	{
        button = GetComponent<Button>();
		button.onClick.AddListener(delegate {
			OnClick(true);
		});
	}


    public void EnableDisable(bool _bValue)
    {
		#if UNITY_ANDROID
        if(thisImage == null)
            thisImage = GetComponent<Image>();

        if (_bValue)
			thisImage.sprite = sSelectedState;
		else
			thisImage.sprite = sNormalState;
		#else
		if(gClickedImage == null)
		gClickedImage = transform.GetChild(0).gameObject;
        gClickedImage.SetActive(_bValue);
		#endif
    }


	public void OnClick(bool _playSound = true)
    {
		if(_playSound)
	        SoundController.instance.PlayAudio (SoundController.ClipType.CHIP);
        GamePlay.instance.DeSelectAllChip();
        
        EnableDisable(true);

        GamePlay.instance.SelectChip(this, gameObject.name);
       /* GamePlay.instance.lastSelectedChip = this;

        GamePlay.instance.currentSelectedChip = int.Parse(gameObject.name);
        GamePlay.instance.IsRemoveClicked = false;*/
    }
}
