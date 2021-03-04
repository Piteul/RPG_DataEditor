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

    string jsonRepresentation;

    public Character()
    {
        name = "No Name";
        SaveToString();
    }

    public void GetCopy(Character ch)
    {
        this.name = ch.name;
        this.characterClass = ch.characterClass;
        HP = ch.HP;
        MP = ch.MP;
        this.attack = ch.attack;
        this.defence = ch.defence;
        this.inventory = ch.inventory;
        this.spells = ch.spells;

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

    /// <summary>
    /// Check & remove items that no longer exist and that would have been deleted directly in folder
    /// </summary>
    public void CheckInventory()
    {
        if (this.inventory != null)
        {
            this.inventory.RemoveAll(item => !item);
        }
    }

    /// <summary>
    /// Check & remove spells that no longer exist and that would have been deleted directly in folder
    /// </summary>
    public void CheckSpells()
    {
        if (this.spells != null)
        {
            this.spells.RemoveAll(item => !item);
        }

    }

}
