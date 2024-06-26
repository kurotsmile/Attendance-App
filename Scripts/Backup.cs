using Carrot;
using UnityEngine;

public class Backup : MonoBehaviour
{
    [Header("Obj Main")]
    public Apps app;

    private Carrot_Box box;

    public void Show_menu_backup()
    {
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_backup);
        box.set_title("Backup");

        Carrot_Box_Item item_import=box.create_item("item_import");
        item_import.set_icon(app.sp_icon_import);
        item_import.set_title("Import");
        item_import.set_tip("Import data from json data file");
        item_import.set_act(() => app.manager_Box.Act_import());

        Carrot_Box_Item item_export = box.create_item("item_export");
        item_export.set_icon(app.sp_icon_export);
        item_export.set_title("Export");
        item_export.set_tip("Export data as json data");

    }
}
