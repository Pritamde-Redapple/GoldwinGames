using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperJoker
{
    public class CardSelector : MonoBehaviour
    {

        public Button[] cardSelectingButtons;

        public GameObject[] playTexts;

        private void Start()
        {            
            playTexts = GameObject.FindGameObjectsWithTag("playtext");           
        }

        void TimeUp()
        {
            ToggleAllTexts(false);
        }      

        public void ToggleAllTexts(bool state)
        {
            foreach (GameObject item in playTexts)
            {
                item.SetActive(state);
            }
        }

        public void DeselectAllGroups()
        {
            foreach (Button item in cardSelectingButtons)
            {
                item.GetComponent<GroupCardsSelector>().ClearAll();
            }
        }

        public void DoubleAll()
        {
            foreach (Button item in cardSelectingButtons)
            {
                item.GetComponent<GroupCardsSelector>().Clean();
            }
        }
           
    }
}
