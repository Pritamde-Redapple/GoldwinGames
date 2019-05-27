using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public float timeLeft = 60f;
	public Text timerText;
    public GameObject blankPanel;
    private bool playAnimOnce;

	private bool canStart = false;

    private Color cDefaultCol;
    public Color cWarningColor;

	private void OnEnable()
	{
        cDefaultCol = timerText.color;
	}

	// Update is called once per frame
	void Update () {
		if(canStart && timeLeft > 0)
		{
			timeLeft -= Time.deltaTime;
			string min = ((int) timeLeft/60).ToString("00");
			string sec = (timeLeft % 60).ToString("00");
			timerText.text = min + ":" + sec;

            if (timeLeft <= 10 && timeLeft > 06)
            {
                timerText.color = cWarningColor;
            }

            if(timeLeft <= 06 && !playAnimOnce){
                timerText.color = Color.red;
                playAnimOnce = true;
                BlockAllButton();
                GamePlay.instance.SendGameData();

            }
            if (timeLeft <= 0)
			{
				canStart = false;
                GamePlay.instance.StartSpinning();
			}
		}
			
	}// END OF UPDATE FUNCTION

	public void StartTimer(float totalTime){
		timeLeft = totalTime;
		canStart = true;
        playAnimOnce = false;
        blankPanel.SetActive(false);
        timerText.color = cDefaultCol;
	}

    public void StopTimer()
    {
        canStart = false;
        timerText.text = "00:00";
        timerText.color = Color.red;
    }

    public void BlockAllButton()
    {
        blankPanel.SetActive(true);
    }

}// END OF Timer CLASS
