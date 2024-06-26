using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type_Sort_Box {timer_a,timer_z,index_a,index_z}
public class Sort : MonoBehaviour
{
    [Header("Obj Main")]
    public Apps app;

    private Carrot_Box box;
    private Type_Sort_Box type_sort = Type_Sort_Box.index_a;

    public void Show_Menu_Sort()
    {
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_sort);
        box.set_title("Sort");

        Carrot_Box_Item item_sort_timer_a=box.create_item("sort_timer_a");
        item_sort_timer_a.set_icon(app.sp_icon_sort);
        item_sort_timer_a.set_title("Sort Timer a->z");
        item_sort_timer_a.set_tip("Sort in ascending chronological order");
        item_sort_timer_a.set_act(() => Sort_timer_a_z());
        if (type_sort == Type_Sort_Box.timer_a) this.Add_btn_check(item_sort_timer_a);

        Carrot_Box_Item item_sort_timer_z = box.create_item("sort_timer_z");
        item_sort_timer_z.set_icon(app.sp_icon_sort);
        item_sort_timer_z.set_title("Sort Timer z->a");
        item_sort_timer_z.set_tip("Sort in descending chronological order");
        item_sort_timer_z.set_act(() => Sort_timer_z_a());
        if (type_sort == Type_Sort_Box.timer_z) this.Add_btn_check(item_sort_timer_z);

        Carrot_Box_Item item_sort_index_a = box.create_item("sort_index_a");
        item_sort_index_a.set_icon(app.sp_icon_sort);
        item_sort_index_a.set_title("Sort Index a->z");
        item_sort_index_a.set_tip("Sort in ascending chronological order");
        item_sort_index_a.set_act(() => Sort_index_a_z());
        if (type_sort == Type_Sort_Box.index_a) this.Add_btn_check(item_sort_index_a);

        Carrot_Box_Item item_sort_index_z = box.create_item("sort_index_a");
        item_sort_index_z.set_icon(app.sp_icon_sort);
        item_sort_index_z.set_title("Sort Index z->a");
        item_sort_index_z.set_tip("Sort in descending chronological order");
        item_sort_index_z.set_act(() => Sort_index_z_a());
        if (type_sort == Type_Sort_Box.index_z) this.Add_btn_check(item_sort_index_z);
    }

    private void Add_btn_check(Carrot_Box_Item item)
    {
        Carrot_Box_Btn_Item btn_check = item.create_item();
        btn_check.set_icon_color(Color.white);
        btn_check.set_color(app.carrot.color_highlight);
        btn_check.set_icon(app.carrot.icon_carrot_done);
        Destroy(btn_check.GetComponent<Button>());
    }

    private void Sort_timer_a_z()
    {
        this.type_sort = Type_Sort_Box.timer_a;
        if (box != null) box.close();
        app.carrot.play_sound_click();
        app.manager_Box.get_list_box().Sort((x, y) => x.remainingTime.CompareTo(y.remainingTime));
        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    private void Sort_timer_z_a()
    {
        this.type_sort = Type_Sort_Box.timer_z;
        if (box != null) box.close();
        app.carrot.play_sound_click();
        app.manager_Box.get_list_box().Sort((x, y) => y.remainingTime.CompareTo(x.remainingTime));
        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    private void Sort_index_a_z()
    {
        this.type_sort = Type_Sort_Box.index_a;
        if (box != null) box.close();
        app.carrot.play_sound_click();
        app.manager_Box.get_list_box().Sort((x, y) => x.index.CompareTo(y.index));
        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    private void Sort_index_z_a()
    {
        this.type_sort = Type_Sort_Box.index_z;
        if (box != null) box.close();
        app.carrot.play_sound_click();
        app.manager_Box.get_list_box().Sort((x, y) => y.index.CompareTo(x.index));
        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }
}
