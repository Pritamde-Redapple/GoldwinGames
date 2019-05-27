using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker {
    public class WheelBaseAnimator : MonoBehaviour {

        WheelBase wheelBase;
        Animator animator;


        private void Awake()
        {
            wheelBase = GetComponent<WheelBase>();
            animator = GetComponent<Animator>();
            Subscribe();
        }

        void Subscribe()
        {
            wheelBase.WheelStart += PlayAnimation;
            wheelBase.WheelStop  += StopAnimation;            
        }

        void PlayAnimation()
        {            
            animator.SetBool("blink", true);
        }

        void StopAnimation()
        {
            animator.SetBool("blink", false);           
        }

        private void OnDestroy()
        {
            wheelBase.WheelStart -= PlayAnimation;
            wheelBase.WheelStop  -= StopAnimation;
        }
    }
}
