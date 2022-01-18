using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public enum ItemType{Sword,HealthPotion,ManaPotion};
    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.Sword:        return ItemAsset.Instance.swordSprite;
            case ItemType.HealthPotion: return ItemAsset.Instance.healthPotion;
        }
    }
    public GameObject GetItemObject()
    {
        switch(itemType)
        {
            default:
            case ItemType.Sword:        return ItemAsset.Instance.swordObj;
            case ItemType.HealthPotion: return ItemAsset.Instance.healthPotionObj;
        }
    }
    public bool CanStackable()
    {
        switch(itemType)
        {
            default:
            case ItemType.Sword:    
            return false;
            case ItemType.HealthPotion:
            case ItemType.ManaPotion:
            return true;
        }
    }
}
