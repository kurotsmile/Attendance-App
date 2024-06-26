using System;
using System.Collections;
using System.Collections.Generic;
using Carrot;
using SimpleFileBrowser;
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

    private int index_cur = -1;
    private List<Box_item> list_box;

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
            box_item.On_Load();
            box_item.txt_name.text=i.ToString();
            box_obj.transform.SetParent(tr_all_item);
            box_obj.transform.localPosition=Vector3.zero;
            box_obj.transform.localScale=new Vector3(1f,1f,1f);
            box_obj.transform.localRotation=Quaternion.identity;

            if (PlayerPrefs.GetString(this.id_table +"_pi_" + i + "_timer") != "")
            {
                string timestampString = PlayerPrefs.GetString(this.id_table+"_pi_" + i + "_timer");

                if (long.TryParse(timestampString, out long timestampMilliseconds))
                {
                    DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestampMilliseconds).DateTime;
                    box_item.Set_Timer(dateTime);
                    box_item.Set_Tip("Ready");
                }
            }

            if (PlayerPrefs.GetString(this.id_table+"_app_id_" + i, "") != "")
            {
                box_item.img_icon_extension.gameObject.SetActive(true);
            }

            box_item.Set_Act_Click(()=>this.Show_menu_info_by_index(index));
            this.list_box.Add(box_item);
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

    private void Show_menu_info_by_index(int index){
        this.index_cur = index;
        this.Un_select_all_box();
        this.list_box[this.index_cur-1].On_Select();
        app.carrot.play_sound_click();
        this.Panel_menu_info.SetActive(true);
        this.app.carrot.clear_contain(tr_all_btn_info);

        Carrot_Button_Item btn_play_timer = this.Add_btn_info();
        btn_play_timer.set_icon(app.carrot.game.icon_play_music_game);
        btn_play_timer.set_label("Play Timer");
        btn_play_timer.set_act_click(() => this.Act_player_timer_cur());

        Carrot_Button_Item btn_edit_timer = this.Add_btn_info();
        btn_edit_timer.set_icon(app.carrot.sp_icon_restore);
        btn_edit_timer.set_label("Set Timer");
        btn_edit_timer.set_act_click(() => this.Act_set_timer_cur());

        string s_id_app = "";
        if (PlayerPrefs.GetString(this.id_table+"_app_id_" + index_cur) != "") s_id_app = PlayerPrefs.GetString(this.id_table+"_app_id_" + index_cur);
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

        Carrot_Button_Item btn_edit_link=this.Add_btn_info();
        btn_edit_link.set_icon(this.app.carrot.user.icon_user_edit);
        btn_edit_link.set_label("Edit App");
        btn_edit_link.set_act_click(()=>this.Act_Edit_app_open());

        Carrot_Button_Item btn_item = this.Add_btn_info();
        btn_item.set_label("Close");
        btn_item.set_act_click(() => this.Btn_close_menu_info());
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
        this.list_box[this.index_cur].Un_select();
        this.Panel_menu_info.SetActive(false);
    }

    public void Act_import(){
        app.file.Set_filter(Carrot_File_Data.JsonData);
        app.file.Open_file(Act_import_done);
    }

    private void Act_import_done(string[] s_path){
        string s_data=FileBrowserHelpers.ReadTextFromFile(s_path[0]);
        IDictionary obj_data = (IDictionary) Json.Deserialize(s_data);
        foreach (DictionaryEntry entry in obj_data)
        {
            string key = (string)entry.Key;
            var value = entry.Value;
            PlayerPrefs.SetString(key, value.ToString());
        }
        app.carrot.Show_msg("Improt Data","Impport data success!\n"+s_path[0]);
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
        if (PlayerPrefs.GetString(this.id_table+"_app_id_" + index_cur) != "") s_id_app = PlayerPrefs.GetString(this.id_table + "_app_id_" + index_cur);
        if (this.box_input != null) this.box_input.close();
        box_input = this.app.carrot.Show_input("Id App Open ("+this.index_cur+")", "Enter the name of the application package you want to open", s_id_app);
        box_input.set_act_done(this.Act_edit_app_oepn_done);
    }

    private void Act_edit_app_oepn_done(string s_val)
    {
        if (this.box_input != null) this.box_input.close();
        PlayerPrefs.SetString(this.id_table+"_app_id_" + this.index_cur, s_val);
        this.Create_table();
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
        PlayerPrefs.SetString(this.id_table + "_pi_" + this.index_cur + "_timer", this.ConvertToUnixTimestampMilliseconds(timer_nex).ToString());
        this.Create_table();
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
        PlayerPrefs.SetString(this.id_table + "_pi_" + this.index_cur + "_timer",this.ConvertToUnixTimestampMilliseconds(DateTime.Now.AddDays(1)).ToString());
        this.Create_table();
    }

    private void Act_set_timer_cur()
    {
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_title("Set Timer ("+this.index_cur+")");
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
            PlayerPrefs.SetString(this.id_table + "_pi_" + this.index_cur + "_timer", this.ConvertToUnixTimestampMilliseconds(dateTime).ToString());
            this.Create_table();
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
}
