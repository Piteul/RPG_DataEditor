using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject
{
    public enum Type { Attack, Defence, Recovery}
    public int id { get { return this.GetInstanceID(); } }
    public new string name;
    public string description;
    public Type spellType;

    string jsonRepresentation;

    public Spell()
    {
        name = "No Name";
        SaveToString();
    }

    public string SaveToString()
    {
        jsonRepresentation = JsonUtility.ToJson(this);
        return jsonRepresentation;
    }

    public Spell Copy()
    {
        return (Spell)this.MemberwiseClone();
    }

}
