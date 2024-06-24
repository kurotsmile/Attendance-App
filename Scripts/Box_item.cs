using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Box_item : MonoBehaviour
{
    public Text txt_name;
    private UnityAction act;
    public void On_Click(){
        Debug.Log("Click");
        act?.Invoke();
    }

    public void Set_Act_Click(UnityAction act){
        this.act=act;
    }
}
