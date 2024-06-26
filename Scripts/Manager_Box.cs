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

    private Carrot_Box box;
    private Carrot_Window_Input box_input;
    private IList list_table;

    private List<Box_item> list_box;
    private Box_item box_cur;

    public void On_load(){

        if (PlayerPrefs.GetString("sel_table") != "") this.id_table = PlayerPrefs.GetString("sel_table");

        this.length_box=int.Parse(PlayerPrefs.GetString(this.id_table+"_lenth_app","500"));
        if (PlayerPrefs.GetString("list_table","") != "")
        {
            string s_list_table = PlayerPrefs.GetString("list_table");
            list_table = (IList)Json.Deserialize(s_list_table);
        }
        else
        {
            list_table = (IList) Json.Deserialize("[\"work\",\"shcools\"]");
        }
    } 

    public void Create_table(){
        this.list_box = new List<Box_item>();
        this.Panel_menu_info.SetActive(false);
        app.carrot.clear_contain(tr_all_item);
        
        for(int i=1;i<=length_box;i++){
            var index=i;
            GameObject box_obj=Instantiate(box_prefab);
            Box_item box_item=box_obj.GetComponent<Box_item>();
            box_item.On_Reset();
            box_item.txt_name.text=i.ToString();
            box_obj.transform.SetParent(tr_all_item);
            box_obj.transform.localPosition=Vector3.zero;
            box_obj.transform.localScale=new Vector3(1f,1f,1f);
            box_obj.transform.localRotation=Quaternion.identity;
            box_item.index = i;
            box_item.index_list = i;
            box_item.Set_Act_Click(()=>this.Show_menu_info_by_data(box_item));
            this.list_box.Add(box_item);
        }

        this.Load_meta_data_all_box();
    }

    private void Load_meta_data_all_box()
    {
        for(int i = 0; i < list_box.Count; i++)
        {
            this.list_box[i].On_Reset();
            this.list_box[i].index_list = i;
            if (PlayerPrefs.GetString(this.id_table + "_pi_" + this.list_box[i].index + "_timer") != "")
            {
                string timestampString = PlayerPrefs.GetString(this.id_table + "_pi_" + this.list_box[i].index + "_timer");

                if (long.TryParse(timestampString, out long timestampMilliseconds))
                {
                    DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampMilliseconds).DateTime;
                    this.list_box[i].Set_Timer(dateTime);
                    this.list_box[i].Set_Tip("Ready");
                }
            }

            if (PlayerPrefs.GetString(this.id_table + "_app_id_" + this.list_box[i].index, "") != "")
            {
                this.list_box[i].img_icon_extension.gameObject.SetActive(true);
            }
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
        PlayerPrefs.SetString(this.id_table + "_lenth_app", s_val);
        this.app.carrot.Show_msg("Change the number of elements successfully!\n("+s_val+" Box)");
        box_input.close();
        this.Create_table();
    }

    private void Show_menu_info_by_data(Box_item box_item){
        this.box_cur = box_item;
        this.Un_select_all_box();
        this.list_box[box_item.index_list].On_Select();
        app.carrot.play_sound_click();
        this.Panel_menu_info.SetActive(true);
        this.app.carrot.clear_contain(tr_all_btn_info);

        Carrot_Button_Item btn_play_timer = this.Add_btn_info();
        btn_play_timer.set_icon(app.carrot.game.icon_play_music_game);
        btn_play_timer.set_label("Play Timer");
        btn_play_timer.set_act_click(() => this.Act_player_timer_cur());

        string s_id_app = "";
        if (PlayerPrefs.GetString(this.id_table+"_app_id_" + box_cur.index) != "") s_id_app = PlayerPrefs.GetString(this.id_table+"_app_id_" + box_cur.index);
        if (s_id_app != "")
        {
            Carrot_Button_Item btn_open_app = this.Add_btn_info();
            btn_open_app.set_icon(app.carrot.icon_carrot_link);
            btn_open_app.set_label("Open App");
            btn_open_app.set_act_click(() => this.Act_open_link_app(s_id_app));

            Carrot_Button_Item btn_open_app_timer = this.Add_btn_info();
            btn_open_app_timer.set_icon(app.sp_icon_rocket);
            btn_open_app_timer.set_label("Open App + Timer");
            btn_open_app_timer.set_act_click(() => this.Act_open_link_app_and_act_timer(s_id_app));
        }

        Carrot_Button_Item btn_more = this.Add_btn_info();
        btn_more.set_icon(this.app.carrot.icon_carrot_advanced);
        btn_more.set_label("More");
        btn_more.set_act_click(() => this.Show_more_menu());

        Carrot_Button_Item btn_item = this.Add_btn_info();
        btn_item.set_label("Close");
        btn_item.set_act_click(() => this.Btn_close_menu_info());
    }

    private void Show_more_menu()
    {
        app.carrot.play_sound_click();
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_icon(app.carrot.icon_carrot_advanced);
        box.set_title("Menu ("+this.box_cur.index+")");

        Carrot_Box_Item item_index = box.create_item("item_index");
        item_index.set_icon(app.carrot.icon_carrot_app);
        item_index.set_title("Index");
        item_index.set_tip("Order of elements in the table (" + this.box_cur.index + ")");

        string s_id_app = PlayerPrefs.GetString(this.id_table + "_app_id_" + this.box_cur.index, "");
        if (s_id_app != "")
        {
            Carrot_Box_Item item_open_app_timer = box.create_item("item_open_app_and_timer");
            item_open_app_timer.set_icon(app.sp_icon_rocket);
            item_open_app_timer.set_title("Open App + Timer");
            item_open_app_timer.set_tip(s_id_app);
            item_open_app_timer.set_act(() => this.Act_open_link_app_and_act_timer(s_id_app));

            Carrot_Box_Item item_app_link = box.create_item("item_app_link");
            item_app_link.set_icon(app.carrot.icon_carrot_link);
            item_app_link.set_title("App open package id");
            item_app_link.set_tip(s_id_app);
            item_app_link.set_act(() => OpenApp_by_bundleId(s_id_app));

            Carrot_Box_Item item_edit_app = box.create_item("item_edit_app");
            item_edit_app.set_icon(this.app.carrot.user.icon_user_edit);
            item_edit_app.set_title("Edit App");
            item_edit_app.set_tip("Set up app opening by bundle id");
            item_edit_app.set_act(() => this.Act_Edit_app_open());

            Carrot_Box_Item item_del_app = box.create_item("item_del_app");
            item_del_app.set_icon(app.carrot.sp_icon_del_data);
            item_del_app.set_title("Delete App");
            item_del_app.set_tip("Delete the app for this object");
            item_del_app.set_act(() => Act_del_app_cur());
        }
        else
        {
            Carrot_Box_Item item_add_app = box.create_item("item_add_app");
            item_add_app.set_icon(this.app.carrot.icon_carrot_add);
            item_add_app.set_title("Add App");
            item_add_app.set_tip("Set up app opening by bundle id");
            item_add_app.set_act(() => this.Act_Edit_app_open());
        }

        if (this.box_cur.txt_tip.text != "")
        {
            Carrot_Box_Item item_timer = box.create_item("item_timer");
            item_timer.set_icon(app.sp_icon_timer);
            item_timer.set_title("Timer");
            item_timer.set_tip(this.box_cur.txt_tip.text);
            item_timer.set_act(() => Act_player_timer_cur());
        }



        Carrot_Box_Item item_edit_timer = box.create_item("item_edit_timer");
        item_edit_timer.set_icon(app.carrot.sp_icon_restore);
        item_edit_timer.set_title("Set Timer");
        item_edit_timer.set_tip("Set time and change timer");
        item_edit_timer.set_act(() => this.Act_set_timer_cur());

        Carrot_Box_Item item_del_timer = box.create_item("item_del_timer");
        item_del_timer.set_icon(app.carrot.sp_icon_del_data);
        item_del_timer.set_title("Delete Timer");
        item_del_timer.set_tip("Delete the timer for this object");
        item_del_timer.set_act(()=>Act_del_timer_cur());

        Carrot_Box_Item item_close = box.create_item("item_close");
        item_close.set_icon(app.carrot.icon_carrot_cancel);
        item_close.set_title("Close");
        item_close.set_tip("Close menus and options");
        item_close.set_act(() => this.Btn_close_menu_info());
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
        this.box_cur.Un_select();
        this.Panel_menu_info.SetActive(false);
        if (box != null) box.close();
    }

    public void Show_List_Table()
    {
        if (this.box != null) this.box.close();
        this.box = app.carrot.Create_Box();
        this.box.set_icon(app.carrot.icon_carrot_all_category);
        this.box.set_title("List Table");
        Carrot_Box_Btn_Item btn_add=this.box.create_btn_menu_header(this.app.carrot.icon_carrot_add);
        btn_add.set_act(() => Act_box_add_table());

        for (int i = 0; i < this.list_table.Count; i++)
        {
            var id_table = this.list_table[i].ToString();
            Carrot_Box_Item item_m = this.box.create_item("item_table_" + i);
            item_m.set_icon(app.carrot.icon_carrot_app);
            item_m.set_title(this.list_table[i].ToString());
            item_m.set_tip("Table " + (i + 1));
            item_m.set_act(() => Act_sel_table(id_table));

            if (id_table == this.id_table)
            {
                Carrot_Box_Btn_Item btn_check=item_m.create_item();
                btn_check.set_icon(app.carrot.icon_carrot_done);
                btn_check.set_icon_color(Color.white);
                btn_check.set_color(app.carrot.color_highlight);
            }
        }
    }

    private void Act_sel_table(string id_table)
    {
        app.carrot.play_sound_click();
        PlayerPrefs.SetString("sel_table", id_table);
        this.id_table = id_table;
        this.Create_table();
        if (box != null) box.close();
    }

    private void Act_box_add_table()
    {
        if (this.box_input != null) this.box_input.close();
        box_input = this.app.carrot.Show_input("New Tabel", "Enter new worktable name");
        box_input.set_act_done(this.Act_add_table_done);
    }

    private void Act_add_table_done(string s_val)
    {
        this.list_table.Add(s_val);
        PlayerPrefs.SetString("list_table", Json.Serialize(this.list_table));
    }

    private void Act_Edit_app_open(){
        string s_id_app = "";
        if (PlayerPrefs.GetString(this.id_table+"_app_id_" + box_cur.index) != "") s_id_app = PlayerPrefs.GetString(this.id_table + "_app_id_" + box_cur.index);
        if (this.box_input != null) this.box_input.close();
        if (this.box != null) this.box.close();
        box_input = this.app.carrot.Show_input("Id App Open ("+this.box_cur.index+")", "Enter the name of the application package you want to open", s_id_app);
        box_input.set_act_done(this.Act_edit_app_oepn_done);
    }

    private void Act_edit_app_oepn_done(string s_val)
    {
        if (this.box_input != null) this.box_input.close();
        PlayerPrefs.SetString(this.id_table+"_app_id_" + this.box_cur.index, s_val);
        this.Load_meta_data_all_box();
    }

    private void Act_open_link_app(string id_app)
    {
        app.carrot.play_sound_click();
        this.OpenApp_by_bundleId(id_app);
    }

    private void Act_open_link_app_and_act_timer(string id_app)
    {
        app.carrot.play_sound_click();
        DateTime timer_nex = DateTime.Now.AddDays(1);
        timer_nex.AddMinutes(1);
        PlayerPrefs.SetString(this.id_table + "_pi_" + this.box_cur.index + "_timer", this.ConvertToUnixTimestampMilliseconds(timer_nex).ToString());
        this.Load_meta_data_all_box();
        this.OpenApp_by_bundleId(id_app);
    }

    private void OpenApp_by_bundleId(string bundleId)
    {
#if UNITY_ANDROID
        bool fail = false;
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            fail = true;
        }

        if (fail)
        {
            if (this.app.carrot.store_public == Store.Google_Play) Application.OpenURL("https://play.google.com/store/apps/details?id=" + bundleId);
            if (this.app.carrot.store_public == Store.Microsoft_Store) Application.OpenURL("ms-windows-store:navigate?appid=" + bundleId);
            if (this.app.carrot.store_public == Store.Amazon_app_store) Application.OpenURL("amzn://apps/android?p=" + bundleId);
        }
        else
        {
            ca.Call("startActivity", launchIntent);
        }

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
#endif
    }

    private void Act_player_timer_cur()
    {
        app.carrot.play_sound_click();
        PlayerPrefs.SetString(this.id_table + "_pi_" + this.box_cur.index + "_timer",this.ConvertToUnixTimestampMilliseconds(DateTime.Now.AddDays(1)).ToString());
        this.Load_meta_data_all_box();
    }

    private void Act_del_timer_cur()
    {
        this.box_cur.Act_stop_timer();
        app.carrot.play_sound_click();
        PlayerPrefs.DeleteKey(this.id_table + "_pi_" + this.box_cur.index + "_timer");
        this.Load_meta_data_all_box();
        if (box != null) box.close();
    }

    private void Act_del_app_cur()
    {
        app.carrot.play_sound_click();
        PlayerPrefs.DeleteKey(this.id_table + "_app_id_" + this.box_cur.index);
        this.Load_meta_data_all_box();
        if (box != null) box.close();
        this.Panel_menu_info.SetActive(false);
    }

    private void Act_set_timer_cur()
    {
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_title("Set Timer ("+this.box_cur.index+")");
        box.set_icon(app.sp_icon_timer);

        Carrot_Box_Item item_hours=box.create_item();
        item_hours.set_icon(app.carrot.icon_carrot_database);
        item_hours.set_title("Hour");
        item_hours.set_tip("Set hours");
        item_hours.set_type(Box_Item_Type.box_number_input);

        Carrot_Box_Item item_minutes = box.create_item();
        item_minutes.set_icon(app.carrot.icon_carrot_database);
        item_minutes.set_title("Minute");
        item_minutes.set_tip("Set minutes");
        item_minutes.set_type(Box_Item_Type.box_number_input);

        Carrot_Box_Btn_Panel panl=box.create_panel_btn();
        Carrot_Button_Item btn_done= panl.create_btn("btn_done");
        btn_done.set_icon_white(app.carrot.icon_carrot_done);
        btn_done.set_label_color(Color.white);
        btn_done.set_bk_color(app.carrot.color_highlight);
        btn_done.set_label("Done");
        btn_done.set_act_click(() =>
        {
            int h=int.Parse(item_hours.get_val());
            int m= int.Parse(item_minutes.get_val());
            DateTime dateTime = DateTime.Now.AddHours(h).AddMinutes(m);
            PlayerPrefs.SetString(this.id_table + "_pi_" + this.box_cur.index + "_timer", this.ConvertToUnixTimestampMilliseconds(dateTime).ToString());
            this.Load_meta_data_all_box();
            app.carrot.play_sound_click();
            box.close();
        });

        Carrot_Button_Item btn_cancel = panl.create_btn("btn_close");
        btn_cancel.set_icon_white(app.carrot.icon_carrot_cancel);
        btn_cancel.set_label_color(Color.white);
        btn_cancel.set_bk_color(app.carrot.color_highlight);
        btn_cancel.set_label("Cancel");
        btn_cancel.set_act_click(() =>
        {
            app.carrot.play_sound_click();
            box.close();
        });
    }

    long ConvertToUnixTimestampMilliseconds(DateTime dateTime)
    {
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
        return dateTimeOffset.ToUnixTimeMilliseconds();
    }

    private void Un_select_all_box()
    {
        for(int i = 0; i < this.list_box.Count; i++)
        {
            this.list_box[i].Un_select();
        }
    }

    public string Get_id_table_cur()
    {
        return this.id_table;
    }

    public List<Box_item> get_list_box()
    {
        return this.list_box;
    }

    public IList Get_list_table()
    {
        return this.list_table;
    }
}
