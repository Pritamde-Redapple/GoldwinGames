using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperJoker
{
    public class HistoryStrip : MonoBehaviour
    {
        public Text slno;
        public Text handId;
        public Text playAmount;
        public Text winAmount;
        public Text multiplier;

        public float blinkRate = 1.5f;
        string tempMultiText;
        bool toggleText;
        public void SetMultiplier(int multi, bool isSB = false)
        {
            if(multi == 1)
            {
                multiplier.text = "-";
            }

            else if(multi == 10)
            {
                multiplier.text = "B";
            }

            else
            {
                multiplier.text = "" + multi + "x";
            }

            if(isSB)
            {
                if (multiplier.text == "-")
                {
                    multiplier.text = "SB";
                }
                else
                {
                    multiplier.text += " " + "SB";
                }
                
               // tempMultiText = multiplier.text;
               // InvokeRepeating("BlinkText", blinkRate, blinkRate);
            }
        }

        
        void BlinkText()
        {
            toggleText = !toggleText;
            if(toggleText)
            {
                multiplier.text = "SB";
            }
            else
            {
                multiplier.text = tempMultiText;
            }
        }
    }
}
