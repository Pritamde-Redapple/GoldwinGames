using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace SuperJoker {
    public class ButtonManager : MonoBehaviour {

        public static ButtonManager instance;
        public Button clearButton;
        public Button doubleButton;
        public Button repeatButton;
        public Button removeButton;

        public int removeModifier;
        public Sprite normalState;
        public Sprite pressedState;
        public CardSelector cardSelector;
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            clearButton.onClick .AddListener(()=> OnClearCicked()  );
            doubleButton.onClick.AddListener(()=> OnDoubleClicked());
            repeatButton.onClick.AddListener(()=> OnRepeatClicked());
            removeButton.onClick.AddListener(()=> OnRemoveClicked());
          //  repeatButton.interactable = true;
            removeModifier = 1;
            // ToggleClearDoubleRepeat(true);          
        }

        public void ToggleClearDoubleRepeat(bool state)
        {
            clearButton .interactable = state;
            doubleButton.interactable = state;
            repeatButton.interactable = state;
            removeButton.interactable = state;
        }

        public void ToggleClearDouble(bool state)
        {
            clearButton.interactable = state;
            doubleButton.interactable = state;
        }

        void OnClearCicked()
        {
            AudioManager.Main.PlayNewSound("OtherButton");
            CardDeck.instance.ClearAll();
           // cardSelector.DeselectAllGroups();
            UIManager.inst.SetPlayAmount();
            removeModifier = 1;
        }

        void OnDoubleClicked()
        {
            AudioManager.Main.PlayNewSound("OtherButton");
            CardDeck.instance.DoubleAll();
            UIManager.inst.SetPlayAmount();
          //  cardSelector.DoubleAll();
        }

        void OnRepeatClicked()
        {
            AudioManager.Main.PlayNewSound("OtherButton");
            Debug.Log("On Repeat Clicked");
            UserData lastRoundData = DataCollector.instance.GetLastUserData();
            if (lastRoundData != null)
            {
                CardDeck.instance.RepeatCardsSelection(lastRoundData.GetCardData());             
            }
        }

        public void OnRemoveClicked()
        {
            removeModifier *= -1;
            SetRemoveButtonSprite();
        }

        void SetRemoveButtonSprite()
        {
            if (removeModifier < 0)
                removeButton.GetComponent<Image>().sprite = pressedState;
            else
                removeButton.GetComponent<Image>().sprite = normalState;
        }


        public void ResetRemoveModifier()
        {
            removeModifier = 1;
            SetRemoveButtonSprite();
        }
        //public void EnableRepeat(bool state)
        //{
        //    // repeatButton.interactable = state;
        //    UserData newUserData = DataCollector.instance.GetLastUserData();
        //    if(newUserData != null)
        //    {
        //        repeatButton.interactable = state;
        //    }
        //}

        public void ToggleRepeatRemove(bool hideRepeat = false)
        {
            repeatButton.gameObject.SetActive(hideRepeat);

            removeButton.gameObject.SetActive(!hideRepeat);
        }

        public void SetInteractiveStates(bool isZero)
        {
            if(isZero)
            {
                UserData lastRoundData = DataCollector.instance.GetLastUserData();
                if (lastRoundData.cardData.Length == 0)
                {
                    repeatButton.interactable = false;
                }
                else
                {
                    repeatButton.interactable = true;
                }

                doubleButton.interactable = false;
                clearButton.interactable  = false;
            }
            else
            {
                repeatButton.interactable = false;
                doubleButton.interactable = true;
                clearButton.interactable  = true;
                removeButton.interactable = true;
            }
            SetRemoveButtonSprite();
        }
    }
}
