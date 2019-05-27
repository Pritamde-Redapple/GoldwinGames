using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class SlotStop : MonoBehaviour
    {

        int stopHere;
        public float deacceleration;
        public float terminalVelocity;

        public GameObject slotBase;
        public void SetEnabled(int targetCardId)
        {
            GetComponent<Collider>().enabled = true;
            stopHere = targetCardId;
            Debug.Log("Slot Stopping At: " + targetCardId);
        }

        public void OnTriggerEnter(Collider other)
        {
            other.GetComponentInParent<SlotBase>().velocity -= deacceleration;
            if ((other.GetComponentInParent<SlotBase>().velocity <= terminalVelocity) && (stopHere == other.GetComponent<SlotCard>().id) )
            {
                
                other.GetComponentInParent<SlotBase>().StartMotion(false);
                GetComponent<Collider>().enabled = false;
                other.transform.localPosition = Vector3.zero;
                // other.GetComponentInParent<SlotBase>().ResetSlotSystem();
                //Disabling rest three cards
                foreach (Transform  item in slotBase.transform)
                {
                    if(item !=  other.transform)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
               
                AudioManager.Main.PlayNewSound("SlotStop");
                
            }
        }

        public int GetStopCardID()
        {
            return stopHere;
        }
    }
}
