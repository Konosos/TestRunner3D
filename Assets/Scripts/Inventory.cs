using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> itemList;
    
    public Inventory()
    {
        itemList=new List<Item>();
        AddItemToList(new Item{itemType=Item.ItemType.Sword, amount=1});
        AddItemToList(new Item{itemType=Item.ItemType.HealthPotion, amount=1});
        
    }
    public void AddItemToList(Item item)
    {
        if(item.CanStackable())
        {
            bool inBag=false;
            foreach(Item itemInven in itemList)
            {
                if(itemInven.itemType==item.itemType)
                {
                    itemInven.amount+=item.amount;
                    inBag=true;
                }
            }
            if(!inBag)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
    }
    public void RemoveItem(Item item)
    {
        item.amount-=1;
        if(item.amount<=0)
        {
            itemList.Remove(item);
        }
    }
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
