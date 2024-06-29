using Carrot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type_Sort_Box {timer_a,timer_z,index_a,index_z,amount_a,amount_z}
public class Sort : MonoBehaviour
{
    [Header("Obj Main")]
    public Apps app;

    private Carrot_Box box;
    private Type_Sort_Box type_sort = Type_Sort_Box.index_a;

    public void Show_Menu_Sort()
    {
        app.carrot.play_sound_click();
        if (box != null) box.close();
        box = app.carrot.Create_Box();
        box.set_icon(app.sp_icon_sort);
        box.set_title("Sort");

        Carrot_Box_Item item_sort_timer_a=box.create_item("sort_timer_a");
        item_sort_timer_a.set_icon(app.sp_icon_sort_asc);
        item_sort_timer_a.set_title("Sort Timer a->z");
        item_sort_timer_a.set_tip("Sort in ascending chronological order");
        item_sort_timer_a.set_act(() => Sort_timer(true));
        if (type_sort == Type_Sort_Box.timer_a) this.Add_btn_check(item_sort_timer_a);

        Carrot_Box_Item item_sort_timer_z = box.create_item("sort_timer_z");
        item_sort_timer_z.set_icon(app.sp_icon_sort_desc);
        item_sort_timer_z.set_title("Sort Timer z->a");
        item_sort_timer_z.set_tip("Sort in descending chronological order");
        item_sort_timer_z.set_act(() => Sort_timer(false));
        if (type_sort == Type_Sort_Box.timer_z) this.Add_btn_check(item_sort_timer_z);

        Carrot_Box_Item item_sort_index_a = box.create_item("sort_index_a");
        item_sort_index_a.set_icon(app.sp_icon_sort_asc);
        item_sort_index_a.set_title("Sort Index a->z");
        item_sort_index_a.set_tip("Sort in ascending chronological order");
        item_sort_index_a.set_act(() => Sort_index(true));
        if (type_sort == Type_Sort_Box.index_a) this.Add_btn_check(item_sort_index_a);

        Carrot_Box_Item item_sort_index_z = box.create_item("sort_index_a");
        item_sort_index_z.set_icon(app.sp_icon_sort_desc);
        item_sort_index_z.set_title("Sort Index z->a");
        item_sort_index_z.set_tip("Sort in descending chronological order");
        item_sort_index_z.set_act(() => Sort_index(false));
        if (type_sort == Type_Sort_Box.index_z) this.Add_btn_check(item_sort_index_z);

        Carrot_Box_Item sort_count_a = box.create_item("sort_count_a");
        sort_count_a.set_icon(app.sp_icon_sort_asc);
        sort_count_a.set_title("Sort Amount z->a");
        sort_count_a.set_tip("Sort in ascending chronological order");
        sort_count_a.set_act(() => this.Sort_amount(true));
        if (type_sort == Type_Sort_Box.amount_a) this.Add_btn_check(sort_count_a);

        Carrot_Box_Item sort_count_z = box.create_item("sort_count_z");
        sort_count_z.set_icon(app.sp_icon_sort_desc);
        sort_count_z.set_title("Sort Amount a->z");
        sort_count_z.set_tip("Sort in ascending chronological order");
        sort_count_z.set_act(() => this.Sort_amount(false));
        if (type_sort == Type_Sort_Box.amount_z) this.Add_btn_check(sort_count_z);
    }

    private void Add_btn_check(Carrot_Box_Item item)
    {
        Carrot_Box_Btn_Item btn_check = item.create_item();
        btn_check.set_icon_color(Color.white);
        btn_check.set_color(app.carrot.color_highlight);
        btn_check.set_icon(app.carrot.icon_carrot_done);
        Destroy(btn_check.GetComponent<Button>());
    }

    private void Sort_timer(bool is_asc)
    {
        if (box != null) box.close();
        app.carrot.play_sound_click();
        if (is_asc)
        {
            this.type_sort = Type_Sort_Box.timer_a;
            app.manager_Box.get_list_box().Sort((x, y) => x.remainingTime.CompareTo(y.remainingTime));
        }
        else
        {
            this.type_sort = Type_Sort_Box.timer_z;
            app.manager_Box.get_list_box().Sort((x, y) => y.remainingTime.CompareTo(x.remainingTime));
        }

        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    private void Sort_index(bool is_asc)
    {
        if (box != null) box.close();
        app.carrot.play_sound_click();

        if (is_asc)
        {
            this.type_sort = Type_Sort_Box.index_a;
            app.manager_Box.get_list_box().Sort((x, y) => x.index.CompareTo(y.index));
        }
        else
        {
            this.type_sort = Type_Sort_Box.index_z;
            app.manager_Box.get_list_box().Sort((x, y) => y.index.CompareTo(x.index));
        }

        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }

    private void Sort_amount(bool is_asc)
    {
        if (box != null) box.close();
        app.carrot.play_sound_click();
        if (is_asc)
        {
            this.type_sort = Type_Sort_Box.amount_a;
            app.manager_Box.get_list_box().Sort((x, y) => x.amount.CompareTo(y.amount));
        }
        else
        {
            this.type_sort = Type_Sort_Box.amount_z;
            app.manager_Box.get_list_box().Sort((x, y) => y.amount.CompareTo(x.amount));
        }

        List<Box_item> list = app.manager_Box.get_list_box();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].index_list = i;
            list[i].txt_name.text = list[i].index+" ("+list[i].amount+")";
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }
}
