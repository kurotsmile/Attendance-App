using Carrot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sort : MonoBehaviour
{
    [Header("Obj Main")]
    public Apps app;

    private Carrot_Box box;

    public void Show_Menu_Sort()
    {
        app.manager_Box.get_list_box().Sort((x, y) => x.remainingTime.CompareTo(y.remainingTime));
        List<Box_item> list = app.manager_Box.get_list_box();

        for(int i=0;i<list.Count; i++)
        {
            list[i].gameObject.transform.SetSiblingIndex(i);
        }
    }
}
