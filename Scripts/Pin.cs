using Carrot;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

public class Pin : MonoBehaviour
{
    [Header("Main Obj")]
    public Apps app;
    public Color32[] pin_color;

    [Header("Ui Obj")]
    public Image img_icon_pin_menu;
    private int index_pin_cur = 1;
    private int index_pin_edit = 1;
    private Carrot_Box box;
    private Carrot_Window_Input box_input = null;
    private Carrot_Window_Msg msg = null;
    private int[] coun_pin;
    private bool is_show_pin = false;

    public void On_load()
    {
        coun_pin = new int[pin_color.Length];
        this.index_pin_cur = PlayerPrefs.GetInt("sel_pin", 1);
    }

    public void Show_list_pin()
    {
        this.list_pin();
    }

    public void list_pin(UnityAction<int> act_sel=null)
    {
        app.carrot.play_sound_click();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_pin);
        box.set_title("List Pin");

        this.Check_length_pin();

        if (act_sel == null)
        {
            Carrot_Box_Btn_Item btn_del_all = box.create_btn_menu_header(app.carrot.sp_icon_del_data);
            btn_del_all.set_act(() => Act_del_all_pin());
        }

        string s_name_table = app.manager_Box.Get_id_table_cur();
        for (int i = 1; i < pin_color.Length; i++)
        {
            var index_pin = i;
            Carrot_Box_Item item_pin = box.create_item("item_pin_" + i);
            item_pin.set_icon(app.sp_icon_pin);
            item_pin.img_icon.color = pin_color[i];

            string s_name_pin = PlayerPrefs.GetString(this.app.manager_Box.Get_id_table_cur() + "_pin_" + i, "");
            if(s_name_pin=="")
                item_pin.set_title("Pin " +i);
            else
                item_pin.set_title(s_name_pin);

            item_pin.set_tip("Number of pinned objects (" + coun_pin[i]+")");

            if (i == this.index_pin_cur)
            {
                Carrot_Box_Btn_Item btn_sel=item_pin.create_item();
                btn_sel.set_icon(this.app.carrot.icon_carrot_done);
                btn_sel.set_color(app.carrot.color_highlight);
                Destroy(btn_sel.GetComponent<Button>());
            }

            if (act_sel == null)
            {
                item_pin.set_act(() => Set_Pin_for_box(index_pin));

                Carrot_Box_Btn_Item btn_edit = item_pin.create_item();
                btn_edit.set_icon(this.app.carrot.user.icon_user_edit);
                btn_edit.set_color(app.carrot.color_highlight);
                btn_edit.set_act(() => Act_show_edit_pin(index_pin));

                Carrot_Box_Btn_Item btn_del = item_pin.create_item();
                btn_del.set_icon(this.app.sp_icon_erase);
                btn_del.set_color(app.carrot.color_highlight);
                btn_del.set_act(() =>
                {
                    this.Act_del_all_pin_by_type(index_pin);
                });

                Carrot_Box_Btn_Item btn_tag = item_pin.create_item();
                btn_tag.set_icon(this.app.sp_icon_tager);
                btn_tag.set_color(app.carrot.color_highlight);
                btn_tag.set_act(() => Act_show_pin_by_type(index_pin));
            }
            else
            {
                item_pin.set_act(() =>
                {
                    box.close();
                    act_sel(index_pin);
                });
            }
        }

        if (is_show_pin==true&&act_sel==null)
        {
            Carrot_Box_Item item_pin = box.create_item("item_pin_cancel");
            item_pin.set_icon(app.carrot.icon_carrot_cancel);
            item_pin.set_title("Remove pin filter");
            item_pin.set_tip("Return to the full list of all pins");
            item_pin.set_act(() => Act_cancel_filter_pin());
        }
    }

    private void Act_cancel_filter_pin()
    {
        app.carrot.play_sound_click();
        this.is_show_pin = false;
        if (box != null) box.close();
        this.app.manager_Box.Create_table();
    }

    private void Act_show_pin_by_type(int index_pin)
    {
        app.carrot.play_sound_click();
        if (coun_pin[index_pin] > 0)
        {
            string s_id_table = app.manager_Box.Get_id_table_cur();
            List<Box_item> list_table = app.manager_Box.get_list_box();
            foreach (var t in list_table)
            {
                if (PlayerPrefs.GetString(s_id_table + "_pi_" + t.index, "") == index_pin.ToString())
                {
                    t.gameObject.SetActive(true);
                }
                else
                {
                    t.gameObject.SetActive(false);
                }
            }
            if (box != null) box.close();
            this.is_show_pin = true;
        }
        else
        {
            app.carrot.Show_msg("Shown by pin type", "List is empty!",Msg_Icon.Alert);
        }

    }

    private void Act_del_all_pin_by_type(int index)
    {
        app.carrot.play_sound_click();
        this.msg = app.carrot.Show_msg("Delete All pin data ("+index+")", "Are you sure you want to delete all pinned ("+ index + ")data?", () =>
        {
            string s_id_table = app.manager_Box.Get_id_table_cur();
            for (int i = 0; i < app.manager_Box.Get_length_box(); i++)
            {
                if(PlayerPrefs.GetString(s_id_table + "_pi_" + i,"")==index.ToString()) PlayerPrefs.DeleteKey(s_id_table + "_pi_" + i);
            }

            if (this.msg != null) this.msg.close();

            app.carrot.delay_function(1f, () =>
            {
                app.carrot.Show_msg("Delete All pin data ("+index+")", "Delete All Pin ("+index+") success!", Msg_Icon.Success);
                if (box != null) box.close();
                app.manager_Box.Create_table();
            });
        });
    }

    private void Act_del_all_pin()
    {
        app.carrot.play_sound_click();
        this.msg=app.carrot.Show_msg("Delete All pin data", "Are you sure you want to delete all pinned data?", () =>
        {
            string s_id_table = app.manager_Box.Get_id_table_cur();
            for(int i=0;i<app.manager_Box.Get_length_box();i++) 
            {
                PlayerPrefs.DeleteKey(s_id_table + "_pi_" + i);
            }
            if (this.msg != null) this.msg.close();

            app.carrot.delay_function(1f, () =>
            {
                app.carrot.Show_msg("Delete All pin data", "Delete All Pin success!", Msg_Icon.Success);
                if (box != null) box.close();
                app.manager_Box.Create_table();
            });
        });
    }

    private void Check_length_pin()
    {
        string s_id_table = app.manager_Box.Get_id_table_cur();
        coun_pin = new int[pin_color.Length];

        for(int i = 0; i < app.manager_Box.Get_length_box(); i++)
        {
            if (PlayerPrefs.GetString(s_id_table + "_pi_" + i, "") != "")
            {
                int index_pin =int.Parse(PlayerPrefs.GetString(s_id_table + "_pi_" + i));
                this.coun_pin[index_pin]++;
            }
        }
    }

    public void Set_Pin_for_box(int index_pin)
    {
        PlayerPrefs.SetInt("sel_pin", index_pin);
        index_pin_cur = index_pin;
        app.manager_Box.Reset_act_bar_menu_info();
        if (box != null) box.close();
    }

    public Color32 Get_color_pin(int index)
    {
        return this.pin_color[index];
    }

    public Color32 Get_color_pin_cur()
    {
        return this.pin_color[index_pin_cur];
    }

    public int Get_index_cur()
    {
        return this.index_pin_cur;
    }

    private void Act_show_edit_pin(int index_pin)
    {
        string s_pin_name = PlayerPrefs.GetString(this.app.manager_Box.Get_id_table_cur()+"_pin_" + index_pin, "Pin " + index_pin);
        this.index_pin_edit = index_pin;
        if (this.box_input != null) this.box_input.close();
        box_input = this.app.carrot.Show_input("Name this pin ("+index_pin+")", "Enter a name for this pin group", s_pin_name);
        box_input.set_act_done(this.Act_done_edit_pin);
    }

    private void Act_done_edit_pin(string s_val)
    {
        PlayerPrefs.SetString(this.app.manager_Box.Get_id_table_cur() + "_pin_" +index_pin_edit, s_val);
        if (this.box_input != null) this.box_input.close();
        if (this.box != null) this.box.close();
        this.Show_list_pin();
    }
}
