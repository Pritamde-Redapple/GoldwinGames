using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class ChipEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 0.2f);
    }
}
