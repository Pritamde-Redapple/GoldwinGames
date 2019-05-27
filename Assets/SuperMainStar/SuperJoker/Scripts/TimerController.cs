using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace SuperJoker
{
    public class TimerController : MonoBehaviour
    {
        public static TimerController inst;
        public float timeLeft = 60f;
        public Text timerText;
        private bool canStart = false;

        public Color cDefaultCol;
        public Color cWarningColor;

        public UnityAction TimeUp;
        public UnityAction WheelSpinTime;
        private void Awake()
        {
            inst = this;
        }
        // Use this for initialization
        void Start()
        {           
            timeLeft = timeLeft - Constant.TimerForGame;
            if(timeLeft>0)
                StartTimer(timeLeft);
        }

        // Update is called once per frame
        void Update()
        {
            if (canStart && timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                string min = ((int)timeLeft / 60).ToString("00");
                string sec = (timeLeft % 60).ToString("00");
                timerText.text = min + ":" + sec;

                if (timeLeft <= 10 && timeLeft > 06)
                {
                    timerText.color = cWarningColor;
                }

                if (timeLeft <= 06 )
                {
                    timerText.color = Color.red;
                    if (TimeUp != null)
                    {
                        TimeUp.Invoke();

                    }
                   
                }
                if (timeLeft <= 0)
                {
                    canStart = false;
                    if (WheelSpinTime != null)
                    {
                        WheelSpinTime.Invoke();
                    }
                }
            }

        }

        public void StartTimer(float totalTime)
        {
           
            timeLeft = totalTime;
            canStart = true;           
            timerText.color = cDefaultCol;
            UIManager.inst.SetWinAmount(0);
            UIManager.inst.SetPlayAmount();
            LevelManager.inst.SetInputBlocker(false);
        }

        public void UpdateTime(int newTime)
        {
            timeLeft = newTime;
        }

        public void StopTimer()
        {
            Debug.Log("Timer Stopped");
            canStart = false;
            timerText.text = "00:00";
            timerText.color = Color.red;
        }
    }
}
