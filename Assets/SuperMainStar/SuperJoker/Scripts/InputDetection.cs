using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SuperJoker
{
    public class InputDetection : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                LeftClick();
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                RightClick();
            }
        }

        public virtual void LeftClick(bool ignoreStack = false)
        {
            
        }

        public virtual void RightClick(bool ignoreStack = false)
        {
            
        }
    }


}
