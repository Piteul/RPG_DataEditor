﻿using System.Collections;
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

    public void GetCopy(Item it)
    {
        this.name = it.name;
        this.description = it.description;
        this.itemType = it.itemType;
        SaveToString();
    }

    /// <summary>
    /// Update and return the entity's definition in Json format
    /// </summary>
    /// <returns></returns>
    public string SaveToString()
    {
        jsonRepresentation = JsonUtility.ToJson(this);
        return jsonRepresentation;
    }
}
