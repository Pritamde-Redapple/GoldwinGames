using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class Wheel : MonoBehaviour
    {

        bool start;
        WheelBase wheelBase;

        private void Start()
        {
            wheelBase = GetComponentInParent<WheelBase>();
        }
        public void SetStart(bool state)
        {
            start = state;
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (start)
                transform.Rotate(Vector3.forward * wheelBase.velocity * Time.deltaTime);
        }
    }
}
