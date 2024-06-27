using Carrot;
using UnityEngine;
using UnityEngine.UI;

public class Pin : MonoBehaviour
{
    [Header("Main Obj")]
    public Apps app;
    public Color32[] pin_color;

    [Header("Ui Obj")]
    public Image img_icon_pin_menu;
    private int index_pin_cur = 1;
    private Carrot_Box box; 

    public void Show_list_pin()
    {
        app.carrot.play_sound_click();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_pin);
        box.set_title("List Pin");

        for (int i = 1; i < pin_color.Length; i++)
        {
            var index_pin = i;
            Carrot_Box_Item item_pin = box.create_item("item_pin_" + i);
            item_pin.set_icon(app.sp_icon_pin);
            item_pin.img_icon.color = pin_color[i]; 
            item_pin.set_title("Pin " +i);
            item_pin.set_tip("Pin for box");
            item_pin.set_act(() => Set_Pin_for_box(index_pin));

            if (i == this.index_pin_cur)
            {
                Carrot_Box_Btn_Item btn_sel=item_pin.create_item();
                btn_sel.set_icon(this.app.carrot.icon_carrot_done);
                btn_sel.set_color(app.carrot.color_highlight);
                Destroy(btn_sel.GetComponent<Button>());
            }
        }
    }

    public void Set_Pin_for_box(int index_pin)
    {
        index_pin_cur = index_pin;
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


}
