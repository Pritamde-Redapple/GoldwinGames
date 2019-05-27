using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SuperJoker
{
    public class SlotBase : MonoBehaviour
    {

        public float velocity;
        public float acceleration;

        public float resetCardPosition;
        private GameObject slotItem;
        private SlotCard[] childCards;
        public SlotStop slotStop;
        float[] targetPoints;
        int target;
        float iterator = 0.66f;
        int loopNumber;
        bool start;
        

        public float maxCardPosition;        
        public float resetVelocity;
        //// Use this for initialization
        void Start()
        {
            childCards = GetComponentsInChildren<SlotCard>();
            targetPoints = new float[4] { 0.66f, 0, -0.66f, -1.32f };
            resetVelocity = velocity;            
        }
       
        void FixedUpdate()
        {
            if (!start)
                return;

            for (int i = 0; i < childCards.Length; i++)
            {              
                childCards[i].transform.Translate(Vector3.up * velocity * Time.deltaTime);
              //  childCards[i].transform.localPosition = new Vector3(childCards[i].transform.localPosition.x, + velocity,);
                if (childCards[i].transform.localPosition.y >= maxCardPosition)
                    childCards[i].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -resetCardPosition, 0);
            }
           

        }       

        public void StartMotion(bool state)
        {
            start = state;
            if (start)
            {
                velocity = resetVelocity;
                slotStop.gameObject.GetComponent<Collider>().enabled = false;

                foreach (SlotCard item in childCards)
                {
                    item.gameObject.SetActive(true);
                }
            }
        }    
      

        public void StopMotionAtCard(int stopAt)
        {
            slotStop.SetEnabled(stopAt);
        }

        public void ResetSlotSystem()
        {
            start = false;            
        }

        public void ResetCardPositions()
        {

        }

        public void ManualSetWheel(int targetId)
        {         

           switch (targetId)
            {
                case 0:
                    break;
                case 1:
                    childCards[0].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0, 0);
                    childCards[1].transform.localPosition = new Vector3(childCards[1].transform.localPosition.x, -0.66f, 0);
                    childCards[2].transform.localPosition = new Vector3(childCards[2].transform.localPosition.x, -1.32f, 0);
                    if(childCards.Length >3)
                        childCards[3].transform.localPosition = new Vector3(childCards[3].transform.localPosition.x, -1.98f, 0);
                    break;
                case 2:
                    childCards[0].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0.66f, 0);
                    childCards[1].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0, 0);
                    childCards[2].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -0.66f, 0);
                    if (childCards.Length > 3)
                        childCards[3].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -1.98f, 0);
                    break;
                case 3:
                    childCards[0].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -1.32f, 0);
                    childCards[1].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0.66f, 0);
                    childCards[2].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0, 0);
                    if (childCards.Length > 3)
                        childCards[3].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -0.66f, 0);
                    break;
                case 4:
                    childCards[0].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -0.66f, 0);
                    childCards[1].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -1.32f, 0);
                    childCards[2].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, -1.98f, 0);
                    if (childCards.Length > 3)
                        childCards[3].transform.localPosition = new Vector3(childCards[0].transform.localPosition.x, 0, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
