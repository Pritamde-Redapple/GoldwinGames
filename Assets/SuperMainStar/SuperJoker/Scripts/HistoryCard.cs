using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperJoker {
    public class HistoryCard : MonoBehaviour {

        Image cardImage;
        Image suiteImage;

        public Text multiplierText;
        public string superBumperText = "SB";

        public bool showMuliplier;
        public float toggleTime = 1.5f;

        private void Start()
        {
            cardImage = transform.GetChild(0).GetComponent<Image>();
            suiteImage = transform.GetChild(1).GetComponent<Image>();           
        }

        //public void SetCardImage(int card = 0, int suite = 0, int multi = 0)
        //{
        //    string itemCard = SuperJokerConstants.CARDNAMEPREFIX + card;
        //    string itemSuite = SuperJokerConstants.SUITENAMEPREFIX + suite;
        //    if (card == 0)
        //        return;
        //    cardImage.sprite = Resources.Load<Sprite>("SJ_Resources/" + itemCard);
        //    suiteImage.sprite = Resources.Load<Sprite>("SJ_Resources/" + itemSuite);
        //    if (showMuliplier && multi != 1)
        //    {
        //        if (multi == 10)
        //            multiplierText.text = "B";                   
        //        else
        //            multiplierText.text = multi + "X"; // show values
        //    }
        //    else if (multiplierText != null)
        //        multiplierText.text = "";
        //}
        string mt;
        public void SetCardImage(int card = 0, int suite = 0, int multi = 0, bool sb = false)
        {
            string itemCard = SuperJokerConstants.CARDNAMEPREFIX + card;
            string itemSuite = SuperJokerConstants.SUITENAMEPREFIX + suite;
            if (card == 0)
                return;
            cardImage.sprite = Resources.Load<Sprite>("SJ_Resources/" + itemCard);
            suiteImage.sprite = Resources.Load<Sprite>("SJ_Resources/" + itemSuite);
            if (showMuliplier && multi != 1)
            {
                if (multi == 10)
                    multiplierText.text = "B";
                else
                    multiplierText.text = multi + "X"; // show values
            }
            else if (multiplierText != null)
                multiplierText.text = "";
           
            if(sb && showMuliplier)
            {
                if(multiplierText.text == "")
                {
                    multiplierText.text = "SB";
                }
                else
                {
                    mt = multiplierText.text;
                    InvokeRepeating("ToggleMultiplier", toggleTime, toggleTime);
                }
               
            }
            else
            {
                CancelInvoke("ToggleMultiplier");
            }
        }
        bool showSuperBumber = false;
       
        void ToggleMultiplier()
        {
            showSuperBumber = !showSuperBumber;
          
            if (showSuperBumber)
            {
                multiplierText.text = "SB";
            }
            else
            {
                multiplierText.text = mt;
            }
        }
    }

   
}
