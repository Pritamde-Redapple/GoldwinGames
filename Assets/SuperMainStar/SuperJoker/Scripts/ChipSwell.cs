using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperJoker
{

    public class ChipSwell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.parent.BroadcastMessage("Swell", this.gameObject, SendMessageOptions.RequireReceiver);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.parent.BroadcastMessage("Reset", this.gameObject, SendMessageOptions.RequireReceiver);
        }

    }

}
