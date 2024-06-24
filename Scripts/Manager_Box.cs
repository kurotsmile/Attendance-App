using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Box : MonoBehaviour
{
    [Header("Main Obj")]
    public Apps app;

    [Header("Manager Box Obj")]
    public GameObject box_prefab;
    public Transform tr_all_item;
    private int length_box=500;

    public void Create_table(){
        app.carrot.clear_contain(tr_all_item);
        
        for(int i=1;i<=length_box;i++){
            GameObject box_obj=Instantiate(box_prefab);
            box_obj.GetComponent<Box_item>().txt_name.text=i.ToString();
            box_obj.transform.SetParent(tr_all_item);
            box_obj.transform.localPosition=Vector3.zero;
            box_obj.transform.localScale=new Vector3(1f,1f,1f);
            box_obj.transform.localRotation=Quaternion.identity;
        }
    }
}
