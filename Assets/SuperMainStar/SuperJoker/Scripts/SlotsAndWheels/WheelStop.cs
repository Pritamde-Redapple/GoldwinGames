using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperJoker
{
    public class WheelStop : MonoBehaviour
    {


        public int stopPointID;

        public float deacceleration;
        public float terminalVelocity;
        public  AudioSource audioSource;

        public void OnTriggerEnter(Collider other)
        {
            WheelBase newWheelBase = other.GetComponentInParent<WheelBase>();
            newWheelBase.velocity -= deacceleration;

            if ((Mathf.Abs(newWheelBase.velocity) <= Mathf.Abs(terminalVelocity)) && (stopPointID == other.GetComponent<WheelPoint>().id))
            {
               // newWheelBase.velocity = 0;
                GetComponent<Collider>().enabled = false;
                audioSource.Stop();
                Debug.Log("Wheel Stopped message sent");
                SendMessageUpwards("WheelStoppped", SendMessageOptions.RequireReceiver);               
                AudioManager.Main.PlayNewSound("WheelStop");
            }
        }

        public void SetStopPointID(int newID)
        {
            Debug.Log("Stopping At: "+ newID);
            stopPointID = newID;
        }

        public void SetEnabled(bool state, int stopID)
        {
            SetStopPointID(stopID);
            GetComponent<Collider>().enabled = true;
            Debug.Log("Setting collider enabled");
        }
    }
}
