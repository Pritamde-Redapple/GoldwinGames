using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperJoker
{
    public class SlotCard : MonoBehaviour
    {

        //  public float moveTo;
        public int id;
        float lastTarget;
        float lastTime;

        bool start;
        float velocity;

        SlotBase slotBase;

        private void Start()
        {
            slotBase = GetComponentInParent<SlotBase>();
        }
        public void LerpTo(float target, float inTime)
        {
            transform.DOLocalMoveY(transform.localPosition.y + target, inTime, false); //move by one card       
        }

        public void StartState(bool state)
        {
            start = state;
        }

        public void SetVelocity(float newVel)
        {
            velocity = newVel;
        }

        //private void FixedUpdate()
        //{
        //    if (start)
        //         transform.Translate(Vector3.up * slotBase.velocity);
        //        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (slotBase.velocity * Time.fixedDeltaTime), 0);
        //        if (transform.localPosition.y >= 1.32f)
        //        transform.localPosition = new Vector3(transform.localPosition.x, -slotBase.resetCardPosition, 0);
        //}

        public Vector3 GetEndPoint()
        {
            return transform.GetChild(0).localPosition;
        }


        public SlotBase GetSlotBase()
        {
            return slotBase;
        }


    }
}
