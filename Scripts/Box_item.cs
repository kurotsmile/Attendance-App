using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Box_item : MonoBehaviour
{
    public int index = -1;
    public int index_list = -1;
    public Text txt_name;
    public Text txt_tip;
    public Image img_icon_extension;

    private UnityAction act;
    private bool isCountingDown = false;

    private DateTime targetTime;
    public TimeSpan remainingTime;

    public void On_Reset()
    {
        this.txt_tip.gameObject.SetActive(false);
        this.img_icon_extension.gameObject.SetActive(false);
        this.GetComponent<Animator>().Play("box_item_nomal");
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

            remainingTime = targetTime - DateTime.UtcNow;

            if (remainingTime.TotalSeconds > 0)
            {
                //this.txt_tip.text = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D2}",remainingTime.Days, remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                this.txt_tip.text = string.Format("{0:D2}:{1:D2}:{2:D2}", remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);              
            }
            else
            {
                this.txt_tip.text = "Time's up!";
                this.GetComponent<Animator>().enabled = true;
                this.GetComponent<Animator>().Play("Box_items");
            }

        }
    }

    public void On_Select()
    {
        this.GetComponent<Animator>().Play("box_item_nomal");
        this.GetComponent<Animator>().enabled = false;
        this.txt_name.color = Color.red;
    }

    public void Un_select()
    {
        this.txt_name.color = Color.black;
        this.GetComponent<Animator>().enabled = true;
    }

    public void ResetCountdown(TimeSpan newCountdownTime)
    {
        remainingTime = newCountdownTime;
        isCountingDown = true;
    }
}
