using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace SuperJoker
{
    public class ChipSelector : MonoBehaviour
    {      

        public Button[] chips;
        public Image glow;
        GameObject currentSelected;

      

        private void Start()
        {
            chips = GetComponentsInChildren<Button>();
            SetButtons();
            OnButtonClick(chips[0]);            
        }

        void SetButtons()
        {
            foreach (Button item in chips)
            {
                item.onClick.AddListener(()=> OnButtonClick(item));
            }            
        }

        public void OnButtonClick(Button clickedButton)
        {   
            #if UNITY_ANDROID
            ButtonManager.instance.ResetRemoveModifier();
            #endif
            AudioManager.Main.PlayNewSound("ChipSelect");
            clickedButton.transform.DOScale(new Vector3(SuperJokerConstants.selectedChipSize, SuperJokerConstants.selectedChipSize, SuperJokerConstants.selectedChipSize),0.2f);
            SetGlow(clickedButton);
            currentSelected = clickedButton.gameObject;
            foreach (Button item in chips)
            {
                if(item != clickedButton)
                {
                    item.transform.DOScale(Vector3.one,0.2f);              
                }
                else
                {
                    int value = System.Convert.ToInt16(item.name);
                    CardDeck.instance.SetChipSelected(value);
                }
            }
        }

        void SetGlow(Button target)
        {
            glow.transform.SetParent(target.transform);
            glow.transform.localPosition = Vector3.zero;
            glow.enabled = true;
            glow.transform.DOScale(new Vector3(SuperJokerConstants.normalChipSize, SuperJokerConstants.normalChipSize, SuperJokerConstants.normalChipSize), 0.2f);
        }

        public void Swell(GameObject go)
        {
            if (go == currentSelected)
                return;
            go.transform.DOScale(new Vector3(SuperJokerConstants.selectedChipSize, SuperJokerConstants.selectedChipSize, SuperJokerConstants.selectedChipSize), 0.2f);
        }

        public void Reset(GameObject go)
        {
            if (go == currentSelected)
                return;
            go.transform.DOScale(Vector3.one, 0.2f);
        }
    }
}
