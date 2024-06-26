using Carrot;
using UnityEngine;

public class Apps : MonoBehaviour
{
    [Header("Main Obj")]
    public Carrot.Carrot carrot;
    public Manager_Box manager_Box;
    public Carrot_File file;

    [Header("Asset Icon")]
    public Sprite sp_icon_length;
    public Sprite sp_icon_timer;
    public Sprite sp_icon_rocket;

    void Start()
    {
        carrot.Load_Carrot(Check_Exit_app); 
        manager_Box.On_load();
        manager_Box.Create_table();
    }

    private void Check_Exit_app(){

    }

    public void Show_setting(){
        Carrot_Box box_setting=carrot.Create_Setting();
        Carrot_Box_Item item_length=box_setting.create_item_of_top();
        item_length.set_icon(sp_icon_length);
        item_length.set_title("Set the number of object elements");
        item_length.set_tip("Change the number of objects");
        item_length.set_act(this.manager_Box.Show_Change_Length_Box);
        box_setting.update_color_table_row();
    }

    public void Show_List_Table(){
        carrot.play_sound_click();
        manager_Box.Show_List_Table();
    }

    public void Btn_Export(){
        carrot.play_sound_click();
    }

    public void Btn_Import(){
        carrot.play_sound_click();
        manager_Box.Act_import();
    }
}
