using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject
{
    public enum Type { Attack, Defence, Recovery}
    public int id { get { return this.GetInstanceID(); } }
    public string spellName;
    public string description;
    public Type spellType;
    





}
