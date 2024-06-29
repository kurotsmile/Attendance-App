using Carrot;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine;

public class Backup : MonoBehaviour
{
    [Header("Obj Main")]
    public Apps app;

    private Carrot_Box box;
    private IDictionary data_export;

    public void Show_menu_backup()
    {
        app.carrot.play_sound_click();
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_backup);
        box.set_title("Backup");

        Carrot_Box_Item item_import=box.create_item("item_import");
        item_import.set_icon(app.sp_icon_import);
        item_import.set_title("Import");
        item_import.set_tip("Import data from json data file");
        item_import.set_act(() =>Act_import());

        Carrot_Box_Item item_export = box.create_item("item_export");
        item_export.set_icon(app.sp_icon_export);
        item_export.set_title("Export");
        item_export.set_tip("Export data as json data");
        item_export.set_act(() => Act_Export());
    }

    public void Act_import()
    {
        app.file.Set_filter(Carrot_File_Data.JsonData);
        app.file.Open_file(Act_import_done);
    }

    private void Act_import_done(string[] s_path)
    {
        string s_data = FileBrowserHelpers.ReadTextFromFile(s_path[0]);
        IDictionary obj_data = (IDictionary)Json.Deserialize(s_data);
        foreach (DictionaryEntry entry in obj_data)
        {
            string key = (string)entry.Key;
            var value = entry.Value;
            PlayerPrefs.SetString(key, value.ToString());
        }
        app.carrot.Show_msg("Improt Data", "Impport data success!\n" + s_path[0]);
        this.app.manager_Box.Create_table();
    }

    private void Act_Export()
    {
        app.carrot.play_sound_click();
        data_export = (IDictionary) Json.Deserialize("{}");
        data_export["list_table"] = Json.Serialize(app.manager_Box.Get_list_table());
        IList list_table = app.manager_Box.Get_list_table();
        foreach(var t in list_table)
        {
            var s_name_table = t.ToString();
            int leng_box= int.Parse(PlayerPrefs.GetString(s_name_table + "_lenth_app", "500"));
            for(int i = 0; i < leng_box; i++)
            {
                string s_timer = PlayerPrefs.GetString(s_name_table + "_pi_" + i + "_timer","");
                if (s_timer!= "") data_export[s_name_table + "_pi_" + i + "_timer"] = s_timer;

                string s_app = PlayerPrefs.GetString(s_name_table + "_app_id_" + i, "");
                if (s_app != "") data_export[s_name_table + "_app_id_" + i] = s_app;

                string s_pin = PlayerPrefs.GetString(s_name_table + "_pi_" + i, "");
                if (s_pin != "") data_export[s_name_table + "_pi_" + i] = s_pin;

                string s_people = PlayerPrefs.GetString(s_name_table + "_p_" + i, "");
                if (s_people != "") data_export[s_name_table + "_p_" + i] = s_people;

                string s_note = PlayerPrefs.GetString(s_name_table + "_note_" + i, "");
                if (s_note != "") data_export[s_name_table + "_note_" + i] = s_note;

                string s_count = PlayerPrefs.GetString(s_name_table + "_count_" + i, "");
                if (s_count != "") data_export[s_name_table + "_count_" + i] = s_count;

                string s_link = PlayerPrefs.GetString(s_name_table + "_link_" + i, "");
                if (s_link != "") data_export[s_name_table + "_link_" + i] = s_link;
            }
            
        }
        app.file.Set_filter(Carrot_File_Data.JsonData);
        app.file.Save_file(this.Act_export_done);
    }

    private void Act_export_done(string[] s_path)
    {
        FileBrowserHelpers.AppendTextToFile(s_path[0], Json.Serialize(this.data_export));
        app.carrot.Show_msg("Export", "Export success!\n" + s_path[0], Msg_Icon.Success);
    }
}
