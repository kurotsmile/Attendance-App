using Carrot;
using UnityEngine;

public class Apps : MonoBehaviour
{
    [Header("Main Obj")]
    public Carrot.Carrot carrot;
    public Manager_Box manager_Box;
    public Carrot_File file;
    public Pin pin;

    [Header("Asset Icon")]
    public Sprite sp_icon_length;
    public Sprite sp_icon_timer;
    public Sprite sp_icon_rocket;
    public Sprite sp_icon_backup;
    public Sprite sp_icon_import;
    public Sprite sp_icon_export;
    public Sprite sp_icon_sort;
    public Sprite sp_icon_pin;
    public Sprite sp_icon_erase;
    public Sprite sp_icon_tager;
    public Sprite sp_icon_note;
    public Sprite sp_icon_columer;
    public Sprite sp_icon_count;
    public Sprite sp_icon_minus;

    [Header("Sound")]
    public AudioSource sound_bk;

    void Start()
    {
        carrot.Load_Carrot(Check_Exit_app);
        carrot.game.load_bk_music(this.sound_bk);

        if (carrot.os_app == OS.Window)
            file.type = Carrot_File_Type.StandaloneFileBrowser;
        else
            file.type = Carrot_File_Type.SimpleFileBrowser;

        manager_Box.On_load();
        manager_Box.Create_table();
        pin.On_load();
    }

    private void Check_Exit_app(){

    }

    public void Show_setting(){
        Carrot_Box box_setting=carrot.Create_Setting();

        Carrot_Box_Item item_columer = box_setting.create_item_of_top();
        item_columer.set_icon(sp_icon_columer);
        item_columer.set_title("Number of columns in the table");
        item_columer.set_tip("Number of columns (" + manager_Box.Get_num_columer() + ")");
        item_columer.set_act(this.manager_Box.Show_Change_Columer_Box);

        Carrot_Box_Item item_length=box_setting.create_item_of_top();
        item_length.set_icon(sp_icon_length);
        item_length.set_title("Set the number of object elements");
        item_length.set_tip("Change the number of objects ("+manager_Box.Get_length_box()+")");
        item_length.set_act(this.manager_Box.Show_Change_Length_Box);
        box_setting.update_color_table_row();
    }

    public void Show_List_Table(){
        carrot.play_sound_click();
        manager_Box.Show_List_Table();
    }

    public void Show_List_Pin()
    {
        carrot.play_sound_click();
        pin.Show_list_pin();
    }
}
