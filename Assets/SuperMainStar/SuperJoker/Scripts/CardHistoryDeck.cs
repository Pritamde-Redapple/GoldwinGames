using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class CardHistoryDeck : MonoBehaviour
    {
        HistoryCard[] deckCards;
        private void Start()
        {
            deckCards = GetComponentsInChildren<HistoryCard>();            
        }

        public void SetDeckBase(List<CardHistoryData> historyCards)
        {
            for (int i = 0; i < historyCards.Count; i++)
            {
                if (deckCards[i] != null)
                {
                    deckCards[i].SetCardImage((int)historyCards[i].cardValue, (int)historyCards[i].suiteValue, (int)historyCards[i].mulitiplier, historyCards[i].isSuperBumper);
                }
                else
                    break;
            }
        }

        public void PushNewCard()
        {
          
        }

    }
}
