using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public enum Type { Weapon, Armour, Consumable}
    public int id { get { return this.GetInstanceID(); } }
    public new string name;
    public string description;
    public Type itemType;

    string jsonRepresentation;

    public Item()
    {
        name = "No Name";
        SaveToString();
    }

    public string SaveToString()
    {
        jsonRepresentation = JsonUtility.ToJson(this);
        return jsonRepresentation;
    }

    public Item Copy()
    {
        return (Item)this.MemberwiseClone();
    }

}
