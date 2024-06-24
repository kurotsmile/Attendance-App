using System;
using System.Collections;
using System.Collections.Generic;
using Carrot;
using UnityEngine;

public class Manager_Box : MonoBehaviour
{
    [Header("Main Obj")]
    public Apps app;

    [Header("Manager Box Obj")]
    public GameObject box_prefab;
    public GameObject btn_info_prefab;
    public Transform tr_all_item;
    public Transform tr_all_btn_info;
    public GameObject Panel_menu_info;
    private int length_box=500;
    private string id_table="pi_work";
    private Carrot_Window_Input box_input;
    public void On_load(){
        this.length_box=PlayerPrefs.GetInt(this.id_table+"_lenth_app",500);
        this.Panel_menu_info.SetActive(false);
    }

    public void Create_table(){
        app.carrot.clear_contain(tr_all_item);
        
        for(int i=1;i<=length_box;i++){
            var index=i;
            GameObject box_obj=Instantiate(box_prefab);
            Box_item box_item=box_obj.GetComponent<Box_item>();
            box_item.txt_name.text=i.ToString();
            box_obj.transform.SetParent(tr_all_item);
            box_obj.transform.localPosition=Vector3.zero;
            box_obj.transform.localScale=new Vector3(1f,1f,1f);
            box_obj.transform.localRotation=Quaternion.identity;
            box_item.Set_Act_Click(()=>this.Show_menu_info_by_index(index));
        }
    }

    public int Get_Length_Box(){
        return this.length_box;
    }

    public void Show_Change_Length_Box(){
        box_input=this.app.carrot.Show_input("Length","Enter number table box",this.Get_Length_Box().ToString(),Window_Input_value_Type.input_field);
        box_input.set_act_done(this.Act_Done_Input);
    }

    private void Act_Done_Input(string s_val){
        this.app.carrot.play_sound_click();
        length_box=int.Parse(s_val);
        this.app.carrot.Show_msg(s_val);
        box_input.close();
        this.Create_table();
    }

    private void Show_menu_info_by_index(int index){
        app.carrot.play_sound_click();
        this.Panel_menu_info.SetActive(true);
        this.app.carrot.clear_contain(tr_all_btn_info);
        Carrot_Button_Item btn_item=this.Add_btn_info();
        btn_item.set_label("Close");
        btn_item.set_act_click(()=>this.Btn_close_menu_info());

        Carrot_Button_Item btn_edit_link=this.Add_btn_info();
        btn_edit_link.set_label("Edit App");
        btn_edit_link.set_act_click(()=>this.Btn_close_menu_info());
    }

    private Carrot_Button_Item Add_btn_info(){
        GameObject btn_obj=Instantiate(btn_info_prefab);
        btn_obj.transform.SetParent(tr_all_btn_info);
        btn_obj.transform.localPosition=Vector3.zero;
        btn_obj.transform.localScale=new Vector3(1f,1f,1f);
        btn_obj.transform.localRotation=Quaternion.identity;
        return btn_obj.GetComponent<Carrot_Button_Item>();
    }

    public void Btn_close_menu_info(){
        app.carrot.play_sound_click();
        this.Panel_menu_info.SetActive(false);
    }
}
