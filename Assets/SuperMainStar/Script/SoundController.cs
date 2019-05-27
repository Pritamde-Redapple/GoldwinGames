using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundController : MonoBehaviour {

	public static SoundController instance;

	public enum ClipType
	{
		BLOCK,
		TAB,
		CHIP,
		BUTTON,
        RANDOM_BUTTON
	}

	public AudioClip aBlockClip;
	public AudioClip aTabClip;
	public AudioClip aChipClip;
	public AudioClip aButtonClip;
    public AudioClip aRandomButtonClip;

    private AudioSource asForButtons;
	private AudioSource asForWheelSpin;	
	private AudioSource asForJewel;
    private AudioSource asForWin;

	void Awake()
	{
		instance = this;

        DontDestroyOnLoad(this.gameObject);
		asForButtons = GetComponents<AudioSource>()[0];
		asForWheelSpin = GetComponents<AudioSource>()[1];
		asForJewel = GetComponents<AudioSource>()[2];
        asForWin = GetComponents<AudioSource>()[3];
	}

	public void PlayAudio(ClipType _clip)
	{
		AudioClip _audioClip = null;
		switch (_clip) {
		case ClipType.BLOCK:
			_audioClip = aBlockClip;
			break;
		case ClipType.TAB:
			_audioClip = aTabClip;
			break;
		case ClipType.CHIP:
			_audioClip = aChipClip;
			break;
		case ClipType.BUTTON:
			_audioClip = aButtonClip;
			break;
        case ClipType.RANDOM_BUTTON:
            _audioClip = aRandomButtonClip;
            break;
        }

		asForButtons.clip = _audioClip;
		asForButtons.Play ();

		//Debug.Log ("Play sound " + _clip);
	}

	public void StartAndStopWheelSpin(bool _bPlay)
	{
		if (_bPlay)
			asForWheelSpin.Play ();
		else {
			asForWheelSpin.Stop ();
			Invoke ("PlayJewelSound", 0.5f);
		}
	}

    public void StopWheelSound()
    {
        asForWheelSpin.Stop();
    }
	public void PlayJewelSound()
	{
		asForJewel.Play ();
	}

    public void PlayAndStopWinSound(bool value)
    {
        if(value)
        {
            asForWin.Play();
        }
        else
        {
            asForWin.Stop();
        }
    }

}
