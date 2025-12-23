using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject cricketBall;
    public Transform rightPos, leftPos;
    public Image swingBtn, spinBtn;
    bool isOnRight = true;
    public bool swingMode = true;

    float swingAmount;
    float spinAmount;
    float maxSwing;
    float maxSpin;
    float sliderValue;

    public ImgSlider slider;
    // Start is called before the first frame update
    void Start()
    {
        swingBtn.color = Color.green;
        spinBtn.color = new Color(1f, 1f, 1f, 0f);
        cricketBall.GetComponent<CricketBallSpin>().resetPoint = rightPos;
        cricketBall.GetComponent<CricketBallSwing>().resetPoint = rightPos;
        maxSpin = cricketBall.GetComponent<CricketBallSpin>().bounceDeflectionAngle;
        maxSwing = cricketBall.GetComponent<CricketBallSwing>().swingForce;
    }

    public void SwitchSide()
    {
        if(isOnRight)
        {
            cricketBall.transform.position = leftPos.position;
            isOnRight = false;
            cricketBall.GetComponent<CricketBallSpin>().resetPoint = leftPos;
            cricketBall.GetComponent<CricketBallSwing>().resetPoint = leftPos;
        }
        else
        {
            cricketBall.transform.position = rightPos.position;
            isOnRight = true;
            cricketBall.GetComponent<CricketBallSpin>().resetPoint = rightPos;
            cricketBall.GetComponent<CricketBallSwing>().resetPoint = rightPos;
        }
    }

    public void BallThrow()
    {
        AdjustPower();
        if (swingMode)
        {
            cricketBall.GetComponent<CricketBallSwing>().ThrowBall();
        }
        else
        {
            cricketBall.GetComponent<CricketBallSpin>().ThrowBall();
        }
    }

    public void ToggleMode(bool swing)
    {
        swingMode = swing;
        if (swingMode)
        {
            swingBtn.color = Color.green;
            spinBtn.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            swingBtn.color = new Color(1f, 1f, 1f, 0f);
            spinBtn.color = Color.green;
        }
    }

    public void AdjustPower()
    {
        sliderValue = slider.storedValue;
        if (swingMode)
        {
            swingAmount = slider.GetValue() * maxSwing;
            cricketBall.GetComponent<CricketBallSwing>().swingForce = swingAmount;
        }
        else
        {
            spinAmount = slider.GetValue() * maxSpin;
            cricketBall.GetComponent<CricketBallSpin>().bounceDeflectionAngle = spinAmount;
        }
        Debug.Log(slider.GetValue());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
