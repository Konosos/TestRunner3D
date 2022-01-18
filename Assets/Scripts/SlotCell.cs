using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotCell : MonoBehaviour
{
    private Item item;
    private PlayerMoving player;

    private void Start()
    {
        player=FindObjectOfType<PlayerMoving>();
    }

    public void SetItem(Item item)
    {
        this.item=item;
    }
    public void ClickButton()
    {
        switch(item.itemType)
        {
            default:
            case Item.ItemType.Sword:           
            player.UseNewSword(item);
            player.RemoveOneItem(item);
            break;
            case Item.ItemType.HealthPotion: 
            player.AddHP();   
            player.RemoveOneItem(item);
            break;
        }
    }
    public void CloseButton()
    {
        Debug.Log("Close Button");
    }
}
