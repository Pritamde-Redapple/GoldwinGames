using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperJoker
{
    public class CardSelectingButton : InputDetection
    {
        public int id;
        public int cardSelects;
        public int suiteSelects;

        [Header("Number of cards to multiply")]
        public int multiplier;
        Text chipValue;
        Image chipImage;
        Text playText;
        int myValue;
        Stack betValues;
        public Card[] cardSet;

        private void Start()
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

        public void UpdateValueChip(int value)
        {
            myValue += value;
            chipImage.sprite = CardDeck.instance.GetChipForValueRange(Mathf.Abs(myValue));
            chipValue.text = ""+ Mathf.Abs(myValue);
            TogglePlayAndChips(myValue* multiplier);
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

        void TogglePlayAndChips(int value)
        {
            if(value < 5)
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

        public void IsDeselected(bool shouldAddValue = true)
        {
            if(shouldAddValue)
               DataCollector.instance.AddCashBack(myValue);
            UpdateValueChip(-myValue);            
        }

        public void DoubleUp()
        {
            if(myValue != 0)
            {
                if (myValue >= SuperJokerConstants.MaxBetAmount)
                    return;
                if (DataCollector.instance.CheckEnoughCash(myValue))
                    UpdateValueChip(myValue);
            }            
        }

        public override void LeftClick(bool ignoreStack = false)
        {
            foreach (Card item in cardSet)
            {
                CardData cardData = item.GetCardData();
                if(cardData.cardBetValue + CardDeck.instance.GetChipSelected() <= SuperJokerConstants.MaxBetAmount)
                {
                    multiplier++;
                }
            }



            AudioManager.Main.PlayNewSound("RowSelect");
            if (myValue >= SuperJokerConstants.MaxBetAmount)
                return;

            if (DataCollector.instance.CheckEnoughCash(CardDeck.instance.GetChipSelected()*multiplier))
            {
                OnClickedCardSelectingButton(1 * ButtonManager.instance.removeModifier);
            }
           
           
        }

        public override void RightClick(bool ignoreStack = false)
        {
            AudioManager.Main.PlayNewSound("CardDeselect");
            if (myValue > 0)
            {
                if(myValue < (int)betValues.Peek()*multiplier)
                {
                    DataCollector.instance.AddCashBack(myValue);
                }
                else
                {
                    DataCollector.instance.AddCashBack((int)betValues.Peek() * multiplier);
                }
                OnClickedCardSelectingButton(-1);
            }
        }


        void OnClickedCardSelectingButton(int modifier)
        {
            if (modifier == -1 && myValue == 0)
                return;
        //  CardDeck.instance.SelectCards(cardSelects,suiteSelects);
            if(modifier > 0)
            {
                betValues.Push(CardDeck.instance.GetChipSelected());
                UpdateValueChip(CardDeck.instance.GetChipSelected() * modifier * multiplier);
            }
            else
            {
                int newAmount = (int)betValues.Pop();
                UpdateValueChip(newAmount * modifier * multiplier);
            }
            if (myValue == 0)
            {
             //   CardDeck.instance.DeSelectCards(cardSelects, suiteSelects);
                transform.parent.BroadcastMessage("Refresh");
            }
        }

        //public SuiteData GetSelectingButtonData()
        //{
        //    SuiteData suiteData = new SuiteData(cardSelects, suiteSelects, myValue);
        //    return suiteData;
        //}

        public void TryAsRepeatSuite()//SuiteData newSuiteData)
        {
            //if (DataCollector.instance.CheckEnoughCash(newSuiteData.suiteBetValue))
            //{
            //    if (cardSelects == newSuiteData.cardID && suiteSelects == newSuiteData.suiteID)
            //        RepeatSelect(newSuiteData.suiteBetValue);
            //}
        }

        void RepeatSelect(int betValue)
        {
          //  CardDeck.instance.SelectCards(cardSelects, suiteSelects);
            UpdateValueChip(betValue);
        }

        public void Refresh()
        {
           // if(myValue != 0)
           //     CardDeck.instance.SelectCards(cardSelects, suiteSelects);
        }
    }
}
