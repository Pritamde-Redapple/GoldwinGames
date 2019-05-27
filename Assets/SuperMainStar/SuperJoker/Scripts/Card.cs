
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
namespace SuperJoker
{
    public class Card : InputDetection
    {

        public int id;
        public int suiteID;
        int myValue = 0;       
        Image playImage;
        Image chipImage;
       
        Text   chipText;
        Text playText;
        CardDeck cardDeck;
        Stack betValues;
        SuperJokerConstants.CardState cardState;
   

        private void Start()
        {          
            betValues = new Stack();
            cardDeck  = GetComponentInParent<CardDeck>();           
            playImage = transform.GetChild(1).GetComponent<Image>();
            chipImage = transform.GetChild(2).GetComponent<Image>();
            chipText  = transform.GetChild(3).GetComponent<Text>();
            playText  = transform.GetChild(4).GetComponent<Text>();
            SetChipAndValue(false);
            SetPlayText(true);
            cardState = SuperJokerConstants.CardState.DESELECTED;
           
        }

        //public void TrySelecting(int targetID, int targetSuiteID)
        //{
        //    if (targetID == 0)
        //    {
        //        if (targetSuiteID == suiteID)
        //            IsSelected();
        //    }
        //    else if (targetSuiteID == 0)
        //    {
        //        if (targetID == id)
        //            IsSelected();
        //    }

        //}

        //public void TryDeSelecting(int targetID, int targetSuiteID)
        //{
        //    if (targetID == 0)
        //    {
        //        if (targetSuiteID == suiteID)
        //        {
        //            playImage.sprite = cardDeck.GetPlayImageNormal();
        //            if(myValue == 0)
        //                IsCleared();
        //        }                   
        //    }
        //    else if (targetSuiteID == 0)
        //    {
        //        if (targetID == id)
        //        {
        //            playImage.sprite = cardDeck.GetPlayImageNormal();
        //            if(myValue == 0)
        //                IsCleared();
        //        }                   
        //    }
        //}

       


        public void IsSelected()
        {
            playImage.sprite = cardDeck.GetPlayImageSelected();
            cardState = SuperJokerConstants.CardState.SELECTED;
        }

        public SuperJokerConstants.CardState GetCardState()
        {
            return cardState;
        }
        public void Clicked(int modifier, bool ignoreStack = false)
        {
         //   Debug.Log("Card Clicked");
            SetChipAndValue(true);
            SetPlayText(false);
            if (modifier > 0)
            {
                if (ignoreStack == false)
                {
             //       Debug.Log("Pushing Clicked");
                    betValues.Push(CardDeck.instance.GetChipSelected());                    
                }
                else
                {
                   
                }
            }
            else
            {
                if(ignoreStack)
                {

                }
                else
                {                   
                    int amountToDeduct = (int)betValues.Pop();
                    UpdateValueChip(amountToDeduct * modifier);
                    UIManager.inst.SetPlayAmount();
                    return;
                }
               
            }

            UpdateValueChip(CardDeck.instance.GetChipSelected()* modifier);
            UIManager.inst.SetPlayAmount();
        }

        public void TryAsRepeatCard(CardData newCardData)
        {
           if (id == newCardData.cardID && suiteID == newCardData.cardSuite)
                {                    
                    RepeatSelect(newCardData.cardBetValue);               
                }
            
        }

        void RepeatSelect(int betValue)
        {
            if ((myValue + betValue) > SuperJokerConstants.MaxBetAmount)
                return;
            if (DataCollector.instance.CheckEnoughCash(betValue))
            {
                SetChipAndValue(true);
                SetPlayText(false);
                betValues.Push(betValue);
                UpdateValueChip(betValue);
                UIManager.inst.SetPlayAmount();
            }
        }

        public void IsCleared()
        {
            DataCollector.instance.AddCashBack(myValue);           
            SetChipAndValue(false);
            SetPlayText(true);
            myValue = 0;            
        }
        //for virtual amount
        public void IsDeselected()
        {
            playImage.sprite = cardDeck.GetPlayImageNormal();
            cardState = SuperJokerConstants.CardState.DESELECTED;
        }

        public void SetChipAndValue(bool state)
        {
            chipImage.gameObject.SetActive(state);
            chipText.gameObject.SetActive(state);
        }

        public void SetPlayText(bool state)
        {
            playText.gameObject.SetActive(state);
        }

        public void UpdateValueChip(int value)
        {
            myValue += value;
            chipImage.sprite = CardDeck.instance.GetChipForValueRange(Mathf.Abs
                (myValue));
            chipText.text = "" + Mathf.Abs(myValue);
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

        public void DoubleUp()
        {
            if (myValue != 0)
            {
                if ((myValue*2) > SuperJokerConstants.MaxBetAmount)
                    return;
                if (DataCollector.instance.CheckEnoughCash(myValue))
                {
                    betValues.Push(myValue);
                    UpdateValueChip(myValue);
                }
            }
        }

        public override void LeftClick(bool ignoreStack = false)
        {
           // Debug.Log("On LefT cLICK");
            if (ButtonManager.instance.removeModifier < 0)
            {
            //    Debug.Log("On rIGHT cLICK");
                RightClick();
                return;
            }

                AudioManager.Main.PlayNewSound("CardSelect");
            if ((myValue + CardDeck.instance.GetChipSelected()) > SuperJokerConstants.MaxBetAmount)
            {
           //     Debug.Log("Returning");
                return;
            }
            if (DataCollector.instance.CheckEnoughCash(CardDeck.instance.GetChipSelected()))
            {
           //     Debug.Log("Has Cash");
                Clicked(1 * ButtonManager.instance.removeModifier, ignoreStack);                                 
            }
        }

        public void OnMultiRightClick(int amountToDeduct)
        {
            AudioManager.Main.PlayNewSound("CardDeselect");
            SetChipAndValue(true);
            SetPlayText(false);
            UpdateValueChip(-amountToDeduct);
        }

        public override void RightClick(bool ignoreStack = false)
        {
            AudioManager.Main.PlayNewSound("CardDeselect");
            if(ignoreStack)
            {
                if (myValue > 0)
                {
                    Clicked(-1, ignoreStack);
                }
                
            }

            else if (myValue>0 && betValues.Count != 0 )
            {
                if(myValue < (int)betValues.Peek())
                {
                    DataCollector.instance.AddCashBack(myValue);
                }
                else
                {
                    DataCollector.instance.AddCashBack((int)betValues.Peek());
                }

                Clicked(-1, ignoreStack);
            }           
        }

        public CardData GetCardData()
        {
            CardData cardData = new CardData(id, suiteID, myValue);
            return cardData;
        }

        public void SetAsWinningCard(CardData cardData)
        {
            betValues.Clear();
            if(cardData.cardID == id && cardData.cardSuite == suiteID)
            {
                CardDeck.instance.SetWinningGlow(this);
            }
        }

        public CardData GetSingleCardValue()
        {
            CardData cardData = new CardData(id, suiteID, myValue);
            return cardData;
        }      

       
    }
}
