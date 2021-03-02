using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public enum Type { Weapon, Armour, Consumable}
    public int id { get { return this.GetInstanceID(); } }
    public string itemName;
    public string description;
    public Type itemType;
    





}
