using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperJoker
{
    public class GroupCardsSelector : InputDetection
    {

        Text chipValue;
        Image chipImage;
        Text playText;
        int myValue;
        Stack betValues;
        public Card[] cardSet;
        public int id;       
        // Use this for initialization
        void Start()
        {           
            betValues = new Stack();
            myValue = 0;
            Text[] allTexts = GetComponentsInChildren<Text>();
            Image[] chipImages = GetComponentsInChildren<Image>();
            foreach (Image item in chipImages)
            {
                if (item.name == "ChipImage")
                    chipImage = item;
            }
            foreach (Text item in allTexts)
            {
                if (item.name == "ChipValue")
                    chipValue = item;
                if (item.name == "PlayText")
                    playText = item;
            }
            SetChipAndValue(false);
            SetPlayText(true);
        }
        

        public override void LeftClick(bool ignoreStack = false)
        {
            AudioManager.Main.PlayNewSound("RowSelect");            
            OnClicked(1);
        }

        public override void RightClick(bool ignoreStack = false)
        {
            AudioManager.Main.PlayNewSound("CardDeselect");
            OnClicked(-1);
        }

        void OnClicked(int modifier)
        {
            int mulitplier = 0; 
            if(modifier > 0)
            {
                foreach (Card item in cardSet)
                {
                    CardData cardData = item.GetCardData();
                    
                    if ((cardData.cardBetValue + CardDeck.instance.GetChipSelected()) <= SuperJokerConstants.MaxBetAmount )
                    {
                        mulitplier++;
                        item.LeftClick(false);
                      //  UpdateValueChip(CardDeck.instance.GetChipSelected());
                   
                    }
                }
                // betValues.Push(CardDeck.instance.GetChipSelected());
                UIManager.inst.SetPlayAmount();
            }

            else
            {                
                foreach (Card item in cardSet)
                {
                    //if (item.GetCardState() == SuperJokerConstants.CardState.SELECTED)
                    //{
                        
                        mulitplier++;
                    item.RightClick(false);
                        //if (item.GetVirtualValue() == 0)
                        //{
                        //    item.IsDeselected();
                        //}
                       // DataCollector.instance.AddCashBack((int)betValues.Peek());
                  //  }
                }
                UIManager.inst.SetPlayAmount();
                //   UpdateValueChip(-(int) betValues.Pop() * mulitplier);               
            }
           
        }

        public void DoubleUp()
        {
            //if (myValue == 0)
            //    return;
            //int amount = 0;
            foreach (Card item in cardSet)
            {

                item.DoubleUp();
                //if(item.GetCardState() == SuperJokerConstants.CardState.SELECTED)
                //{
                //    CardData cardData = item.GetCardData();
                //    amount = item.GetVirtualValue();
                //    if((cardData.cardBetValue + amount) <= SuperJokerConstants.MaxBetAmount)
                //    {
                //        if (DataCollector.instance.CheckEnoughCash(item.GetVirtualValue()))
                //        {
                //            item.AddVirtualValue(amount);
                //            UpdateValueChip(amount);
                //        }
                //    }
                //}
            }
            //if(amount >= 0)
            //    betValues.Push(amount);
        }

        public void SetChipAndValue(bool state)
        {
            chipImage.gameObject.SetActive(state);
            chipValue.gameObject.SetActive(state);
        }

        public void SetPlayText(bool state)
        {
            playText.gameObject.SetActive(state);
        }

        public void UpdateValueChip(int value)
        {
            
            myValue += value;
            chipImage.sprite = CardDeck.instance.GetChipForValueRange(Mathf.Abs(myValue));
            chipValue.text = "" + Mathf.Abs(myValue);
            TogglePlayAndChips(myValue);
        }

        void TogglePlayAndChips(int value)
        {
            if (value < 5)
            {
                SetChipAndValue(false);
                SetPlayText(true);
                myValue = 0;
            }
            else
            {
                SetPlayText(false);
                SetChipAndValue(true);
            }
        }

        public void ClearAll()
        {            
            foreach (Card item in cardSet)
            {
               
                    item.IsCleared();
                    //item.AddVirtualValue(-item.GetVirtualValue());
                   // item.IsDeselected();                    
               
            }
           // DataCollector.instance.AddCashBack(myValue);
         //   UpdateValueChip(-myValue);           
        }

        public void Clean()
        {
            UpdateValueChip(-myValue);
        }
    }
}
