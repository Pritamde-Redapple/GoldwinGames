using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperJoker
{
    public class CardDeck : MonoBehaviour
    {
        public static CardDeck instance;
        Card[] deckCards;
        Sprite playNormal;
        Sprite playSelected;

        public Sprite[] chipImages;

        int chipSelected;

        CardSelectingButton[] cardSelectingButtons;

        public Transform glowBackground;

        private void Awake()
        {
            instance = this;
        }
        // Use this for initialization
        void Start()
        {
            cardSelectingButtons = FindObjectsOfType<CardSelectingButton>();
            deckCards    = GetComponentsInChildren<Card>();
            playNormal   = Resources.Load<Sprite>("SJ_Resources/play_normal");
            playSelected = Resources.Load<Sprite>("SJ_Resources/play_selected");
            chipImages   = Resources.LoadAll<Sprite>("SJ_Resources/ChipImages");
            chipSelected = 5;
            DataCollector.instance.SetCards(deckCards);
            DataCollector.instance.SetCardCollectingButtons(cardSelectingButtons);

            // SetCardClick();           
        }

        void ShowWinner()
        {
            ShowWinningCard(2, 2);
        }

        void SetCardClick()
        {
            foreach (Card item in deckCards)
            {
                item.GetComponent<Button>().onClick.AddListener(()=> OnCardClick(item));
            }
        }

        void OnCardClick(Card newCard)
        {
            newCard.Clicked(1);
        }

        public Sprite GetChipImage(int value)
        {
            for (int i = 0; i < chipImages.Length; i++)
            {
                if(chipImages[i].name == value.ToString())
                {
                    return chipImages[i];
                }
            }

            return chipImages[0];
        }

        public Sprite GetChipForValueRange(int value)
        {
            if (value == 5)
                return GetChipImage(5);
            else if (value >= 10 && value < 20)
                return GetChipImage(10);
            else if (value >= 20 && value < 50)
                return GetChipImage(20);
            else if (value >= 50 && value < 100)
                return GetChipImage(50);
            else if (value >= 100 && value < 500)
                return GetChipImage(100);
            else if (value >= 500)
                return GetChipImage(500);
            else
                return GetChipImage(10);
        }

        public Sprite GetPlayImageSelected()
        {
            return playSelected;            
        }

        public Sprite GetPlayImageNormal()
        {
            return playNormal;
        }

        public void SetChipSelected(int value)
        {
            chipSelected = value;
        }

        public int GetChipSelected()
        {
            return chipSelected;
        }

        //public void SelectCards(int cardValue, int suiteValue)
        //{
        //    foreach (Card item in deckCards)
        //    {
        //        //item.TrySelecting(cardValue, suiteValue);
        //    }
        //}

        //public void DeSelectCards(int cardValue, int suiteValue)
        //{
        //    foreach (Card item in deckCards)
        //    {
        //      //  item.TryDeSelecting(cardValue, suiteValue);
        //    }
        //}

        public void ClearAll()
        {
            foreach (Card item in deckCards)
            {
                item.IsCleared();
            }

            //foreach (CardSelectingButton item in cardSelectingButtons)
            //{
            //    item.IsDeselected();
            //}
        }

        public void DoubleAll()
        {
            //foreach (CardSelectingButton item in cardSelectingButtons)
            //{
            //    item.DoubleUp();
            //}

            foreach (Card item in deckCards)
            {
                item.DoubleUp();
            }            
        }

        public void RepeatCardsSelection(CardData[] cardData)
        {
            foreach (CardData item in cardData)
            {                
               gameObject.BroadcastMessage("TryAsRepeatCard", item, SendMessageOptions.RequireReceiver);
            }
        }

        public void RepeatSelectingButtons()//SuiteData[] suiteData)
        {
            //foreach (SuiteData item in suiteData)
            //{
            //    cardSelectingButtons[0].transform.parent.BroadcastMessage("TryAsRepeatSuite", item, SendMessageOptions.RequireReceiver);
            //}
        }

        public void ShowWinningCard(int cardValue, int cardSuite)
        {
            CardData cardData = new CardData(cardValue, cardSuite, 0);
            gameObject.BroadcastMessage("SetAsWinningCard", cardData, SendMessageOptions.RequireReceiver);
        }

        public void SetWinningGlow(Card card)
        {
            glowBackground.gameObject.SetActive(true);
            glowBackground.transform.position = card.transform.position;
        }

        public void DisableWinningCard()
        {
            glowBackground.gameObject.SetActive(false);
        }
    }
}
