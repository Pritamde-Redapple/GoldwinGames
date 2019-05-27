using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace SuperJoker
{
    public class WheelBase : MonoBehaviour
    {
        public string wheelBase;
        public float velocity;
        public float acceleration;
        public float accelerationDuration;
        bool start;

        public Wheel wheel;
        public WheelStop wheelStop;        
        public int targetCardID;       

        public UnityAction WheelStart;
        public UnityAction WheelStop;
        float resetVelocity;

        public Vector3[] wheelAngles;

        public AudioSource audioSource;

        private void Start()
        {              
            resetVelocity = velocity;
        }
        public void StartWheelMotion()
        {
            WheelStarted();
            wheel.SetStart(true);
            audioSource.Play();
        }       

      

        public void StopWheel(int targetID)
        {
            wheelStop.SetEnabled(true, targetID);
            targetCardID = targetID;
        }

        void WheelStoppped()
        {
            wheel.SetStart(false);
            velocity = resetVelocity;
            if (WheelStop != null)
                WheelStop.Invoke();
            ManualSetWheel(targetCardID);
        }

        void WheelStarted()
        {
            if (WheelStart != null)
                WheelStart.Invoke();
        }

        public void ManualSetWheel(int targetId)
        {
            Debug.Log("TagetID Manual Stop"+ targetId);
            wheel.transform.rotation = Quaternion.Euler(wheelAngles[targetId]);
        }
    }
}
