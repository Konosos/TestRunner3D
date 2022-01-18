using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField]private GameObject slot;
    [SerializeField]private Transform panel;

    public void SetInventory(Inventory inventory)
    {
        this.inventory=inventory;
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        foreach(Transform child in panel)
        {
            Destroy(child.gameObject);
        }

        int x=0;
        int y=0;
        float itemSlotCellSize=55f;
        foreach(Item item in inventory.GetItemList())
        {
            RectTransform slots=Instantiate(slot as GameObject).GetComponent<RectTransform>();
            slots.gameObject.transform.SetParent(panel);
            slots.anchoredPosition=new Vector3(-80+x*itemSlotCellSize,75-y*itemSlotCellSize,0);
            slots.gameObject.GetComponent<SlotCell>().SetItem(item);

            slots.Find("Image").GetComponent<Image>().sprite=item.GetSprite();
            if(item.amount>1)
            {
                slots.Find("amount").GetComponent<Text>().text=item.amount.ToString();
            }
            else
            {
                slots.Find("amount").GetComponent<Text>().text="";
            }
            x++;
            if(x>3)
            {
                x=0;
                y++;
            }
        }
    }
}
