using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Box_item : MonoBehaviour
{
    public int index = -1;
    public int index_list = -1;
    public string s_people_name;
    public int amount = 0;
    public Text txt_name;
    public Text txt_tip;
    public Image img_icon_link;
    public Image img_icon_user;
    public Image img_icon_note;
    public Image img_icon_pin;

    private UnityAction act;
    private bool isCountingDown = false;
    private bool isSelect = false;

    private DateTime targetTime;
    public TimeSpan remainingTime;


    public void On_Reset()
    {
        this.txt_tip.gameObject.SetActive(false);
        this.img_icon_link.gameObject.SetActive(false);
        this.img_icon_user.gameObject.SetActive(false);
        this.img_icon_note.gameObject.SetActive(false);
        this.img_icon_pin.gameObject.SetActive(false);
        this.GetComponent<Animator>().Play("box_item_nomal");
    }

    public void On_Click(){
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
                if(this.isSelect==false)this.GetComponent<Animator>().Play("Box_items");
            }

        }
    }

    public void On_Select()
    {
        this.isSelect = true;
        this.GetComponent<Animator>().Play("box_item_select");
        this.txt_name.color = Color.red;
    }

    public void Un_select()
    {
        this.isSelect = false;
        this.txt_name.color = Color.black;
        this.GetComponent<Animator>().Play("box_item_nomal");
    }

    public void Act_stop_timer()
    {
        this.txt_tip.gameObject.SetActive(false);
        this.txt_tip.text = "";
        this.GetComponent<Animator>().Play("box_item_nomal");
        isCountingDown = false;
    }

    public void ResetCountdown(TimeSpan newCountdownTime)
    {
        remainingTime = newCountdownTime;
        isCountingDown = true;
    }
}
