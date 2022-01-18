using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        GameObject obj=item.GetItemObject();
        GameObject ItemObj=Instantiate(obj,position,Quaternion.identity);
        ItemWorld ItemScr=ItemObj.GetComponent<ItemWorld>();
        ItemScr.SetItem(item);
        return ItemScr;
    }

    private Item item;

    public void SetItem(Item item)
    {
        this.item=item;
    }
    public Item GetItem()
    {
        return item;
    }
    public void DestroyMySelf()
    {
        Destroy(this.gameObject);
    }
}
