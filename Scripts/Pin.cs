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
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_pin);
        box.set_title("List Pin");

        for (int i = 0; i < pin_color.Length; i++)
        {
            var index_pin = i;
            Carrot_Box_Item item_pin = box.create_item("item_pin_" + i);
            if(i==0)
                item_pin.set_icon(app.sp_icon_erase);
            else
                item_pin.set_icon(app.sp_icon_pin);
            item_pin.img_icon.color = pin_color[i]; 
            item_pin.set_title("Pin " + (i + 1));
            item_pin.set_tip("Pin for box");
            item_pin.set_act(() => Set_Pin_for_box(index_pin));
        }
    }

    public void Set_Pin_for_box(int index_pin)
    {
        index_pin_cur = index_pin;
        this.img_icon_pin_menu.sprite=this.Get_icon_sp_by_index(index_pin);
        if (box != null) box.close();
    }

    public Color32 Get_color_pin_cur()
    {
        if (index_pin_cur == 0)
            return Color.white;
        else
            return this.pin_color[index_pin_cur];
    }

    public Color32 Get_color_pin(int index)
    {
        return this.pin_color[index];
    }

    public int Get_index_cur()
    {
        return this.index_pin_cur;
    }

    public Sprite Get_icon_sp_by_index(int index)
    {
        if (index == 0)
            return app.sp_icon_erase;
        else
            return app.sp_icon_pin;
    }

    public Sprite Get_icon_sp_by_cur()
    {
        if (index_pin_cur == 0)
            return app.sp_icon_erase;
        else
            return app.sp_icon_pin;
    }
}
