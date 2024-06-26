using Carrot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [Header("Main Obj")]
    public Apps app;
    public Color32[] pin_color;

    private int index_pin_cur = 0;
    private Carrot_Box box;

    public void Show_list_pin()
    {
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_pin);
        box.set_title("List Pin");

        for (int i = 0; i < pin_color.Length; i++)
        {
            Carrot_Box_Item item_pin = box.create_item("item_pin_" + i);
            item_pin.set_icon(app.sp_icon_pin);
            item_pin.img_icon.color = pin_color[i];
            item_pin.set_title("Pin " + (i + 1));
            item_pin.set_tip("Pin for box");
        }
    }
}
