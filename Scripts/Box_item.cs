using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Box_item : MonoBehaviour
{
    public Text txt_name;
    public Text txt_tip;

    private UnityAction act;
    private float remainingTime;
    private bool isCountingDown = false;

    private DateTime targetTime;

    public void On_Load()
    {
        this.txt_tip.gameObject.SetActive(false);
    }

    public void On_Click(){
        Debug.Log("Click");
        act?.Invoke();
    }

    public void Set_Act_Click(UnityAction act){
        this.act=act;
    }

    public void Set_Tip(string s_tip)
    {
        this.txt_tip.text = s_tip;
        this.txt_tip.gameObject.SetActive(true);
    }

    public void Set_Timer(DateTime timer)
    {
        this.isCountingDown = true;
        targetTime = timer;
    }

    void Update()
    {
        if (isCountingDown)
        {

            TimeSpan remainingTime = targetTime - DateTime.Now;

            if (remainingTime.TotalSeconds > 0)
            {
                //this.txt_tip.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",remainingTime.Days, remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                this.txt_tip.text = string.Format("{0:D2}:{1:D2}:{2:D2}", remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
            }
            else
            {
                this.txt_tip.text = "Time's up!";
            }

        }
    }


    public void ResetCountdown(TimeSpan newCountdownTime)
    {
        remainingTime = (float)newCountdownTime.TotalSeconds;
        isCountingDown = true;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }
}
