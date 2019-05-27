using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class SpinWheel : MonoBehaviour
{
    public enum Wheel
    {
        Outer,
        Middle,
        Inner
    }
    public Wheel wheelType;
    public enum RotationDirection { AntiClockWise, ClockWise, NONE };
    public RotationDirection rotationDirection;

    public List<int> prize;
    public List<AnimationCurve> animationCurves;

    private bool spinning;
    private float anglePerItem;
    public int randomTime;
    public int itemNumber;
    float multiplicationFactor = 1;

    public GameObject gNumberBlock;
    public GameObject gJewel;

    public SpinWheel previousWheel;
    public SpinWheel nextWheel;



    public float speed;
    public float speedToNumner = 20f;
    public int number;
    public int noOfTimeloop = 4;

    float _fTargetAngle;

    private AudioSource audio;

    Sequence run;

    void Start()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;

        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RotateWheelInfinitely();
        }
        if (Input.GetKeyDown(KeyCode.S) && wheelType == Wheel.Outer)
        {
            MoveToSpecificNumber(number);
        }

    }

    public void RotateWheelInfinitely()
    {
        Debug.Log("Current : " + transform.eulerAngles);

        //  Debug.Log(wheelType + " Start loop");
        if (gNumberBlock != null)
        {
            gNumberBlock.SetActive(false);
        }

        if (wheelType == Wheel.Inner && gJewel != null)
        {
            gJewel.SetActive(false);
        }

       // run.Complete();
        run.Kill();
        Vector3 endValue = Vector3.zero;
        if (rotationDirection == RotationDirection.ClockWise)
        {
            //endValue = new Vector3(0, 0, -(360 + Mathf.Abs(transform.rotation.eulerAngles.z)));
            endValue = new Vector3(0, 0, -360);
        }

        else if(rotationDirection == RotationDirection.AntiClockWise)
        {
            endValue = new Vector3(0, 0, 360);
        }
        run = DOTween.Sequence();
        Tween rot = transform.DORotate(endValue, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetRelative();
        run.Append(rot).SetLoops(-1);
    }

    public void MoveToSpecificNumber(int num)
    {
      //  run.Complete();
        run.Kill();
        run = DOTween.Sequence();
        Vector3 endValue = new Vector3(0, 0, 36);
        for (int i = 0; i < prize.Count; i++)
        {
            if (prize[i] == num)
            {
                Debug.Log("Go to index : " + i + "Number: "+ num);
                endValue = new Vector3(0, 0, (36 * i));
                break;
            }
        }

        if (rotationDirection == RotationDirection.AntiClockWise)
        {         
            endValue.z = 360 + endValue.z;
        }        
        else
        {
            float temp = transform.eulerAngles.z - endValue.z;
            if (temp < 0)
            {
                endValue.z -= 360;
            }
        }


        //Debug.Log(wheelType+"Z value : " + endValue);

        float time = Mathf.Abs((endValue.z - transform.localEulerAngles.z)) / 100f;
        time = Mathf.Clamp(time, 1, 2); // 6
        Debug.Log( "Current : " + transform.eulerAngles + " End Value: " + endValue + " time " + time);
       
            Tween rot = transform.DORotate(endValue , time, RotateMode.FastBeyond360)
      .SetEase(Ease.Linear)
      .OnComplete(() => { OnCompleteMovement(); });
            run.Append(rot);       

    }

    public void StopWheel(string num)
    {
        if (run != null)
        {
            run.Complete();
            run.Kill();
        }
        Vector3 endValue = new Vector3(0, 0, 36);
        for (int i = 0; i < prize.Count; i++)
        {
            if (prize[i].ToString() == num)
            {
                endValue = new Vector3(0, 0, 36 * i);
                break;
            }
        }

        if (rotationDirection == RotationDirection.AntiClockWise)
        {
            endValue.z = 360 + endValue.z;
        }

        transform.eulerAngles = endValue;
    }
    int counter = 0;

    public void StopRotateAnim()
    {
        MoveToSpecificNumber(number);
    }
    Sequence mySequence;
    Tween myTween;
    
    public void Go(float _fTargetAngle)
    {
        Vector3 endValue = transform.localEulerAngles;
        endValue.z = _fTargetAngle;

        float time = itemNumber;
        if (rotationDirection == RotationDirection.ClockWise)
            time = 10 - itemNumber;
        transform.DORotate(endValue, time * 0.5f, RotateMode.FastBeyond360)
                 .SetEase(Ease.OutBack)
                 .OnComplete(() => { OnCompleteMovement(); })
                 ;
    }

    IEnumerator GoToNumber(float _fTargetAngle)
    {

        float angle = 0;

        float _TargetSpeed = speed;

        if (rotationDirection == RotationDirection.ClockWise)
        {
            _TargetSpeed /= (10 - itemNumber);
        }
        else
        {
            _TargetSpeed /= (itemNumber);
        }

        //Debug.Log("_fTargetAngle ...................... " + _fTargetAngle + " _TargetSpeed ........" +_TargetSpeed);
        float _speed = speed;

        while (angle < _fTargetAngle)
        {
            transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, angle);
            angle += (multiplicationFactor * Time.deltaTime * _speed);
            //_speed /= (itemNumber);

            _speed = Mathf.MoveTowards(_speed, _TargetSpeed, Time.deltaTime * speedToNumner * (itemNumber + 1));
            //  Debug.Log(angle + " speed " +_speed);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, _fTargetAngle);
        //   Debug.Log("Complete " + Time.time);

        OnCompleteMovement();

    }

    void OnCompleteMovement()
    {
       // Debug.Log("Complete :" + wheelType);
       if (nextWheel)
            nextWheel.StopRotateAnim();
       
        audio.Play();
        if (gNumberBlock != null)
        {
            gNumberBlock.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
            gNumberBlock.SetActive(true);
        }
        if (wheelType == Wheel.Inner)
        {
            SoundController.instance.StartAndStopWheelSpin(false);
            gJewel.SetActive(true);
            Debug.Log("show result");
            GamePlay.instance.ShowResultBlock();
        }
    }
    public void FindNumberInArrayAndSetAngle(int _ivalue)
    {
        for (int i = 0; i < prize.Count; i++)
        {
            if (prize[i] == _ivalue)
            {
                Debug.Log("For number :" + _ivalue);
                itemNumber = i;
                float _angle = 36 * itemNumber;

                if (rotationDirection == RotationDirection.ClockWise)
                {
                    _angle = 360 + _angle;
                }
                transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, _angle);
                break;
            }
        }
    }

}