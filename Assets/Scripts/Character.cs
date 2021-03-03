using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : ScriptableObject
{
    public enum Class { Warrior, Mage, Dancer, Thief }

    public int id { get { return this.GetInstanceID(); } }
    public new string name;
    public Class characterClass;
    public int HP;
    public int MP;
    public int attack;
    public int defence;
    public List<Item> inventory;
    public List<Spell> spells;

    public string jsonRepresentation;

    public Character()
    {
        name = "No Name";
        SaveToString();
    }

    public string SaveToString()
    {
        jsonRepresentation = JsonUtility.ToJson(this);
        return jsonRepresentation;
    }

    public Character Copy()
    {
        return (Character)this.MemberwiseClone();
    }

}
