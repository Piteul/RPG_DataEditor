using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class DataEditorTool : EditorWindow
{
    Color defaultGUIColor;
    Vector2 scrollPosition;
    static string mainFolderPath = "Assets/Resources/GameData";

    Dictionary<int, Character> characterDict = new Dictionary<int, Character>();
    Dictionary<int, Item> itemDict = new Dictionary<int, Item>();
    Dictionary<int, Spell> spellDict = new Dictionary<int, Spell>();

    enum DataCategory { Characters, Items, Spells }
    DataCategory dataCategory;

    //Toolbar
    int toolbarIndex = 0;
    static string[] toolbarStrings = (string[])Enum.GetNames(typeof(DataCategory));

    //Grid
    static int dataSelectedIndex = 0;
    List<int> gridID;
    List<string> gridName;


    [MenuItem("Tools/Data Editor Tool")]
    static void Init()
    {

        DataEditorTool window = (DataEditorTool)GetWindow(typeof(DataEditorTool));
        window.Show();

        //Checks if the folders exist, otherwise creates them
        if (!AssetDatabase.IsValidFolder(mainFolderPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "GameData");
        }

        foreach (string dataCategoryString in toolbarStrings)
        {
            if (!AssetDatabase.IsValidFolder(mainFolderPath + "/" + dataCategoryString))
            {
                AssetDatabase.CreateFolder(mainFolderPath, dataCategoryString);
            }
        }

    }

    private void OnEnable()
    {
        LoadDataList();
        LoadGridStrings();
    }

    private void OnProjectChange()
    {
        LoadDataList();
        LoadGridStrings();
    }

    private void OnGUI()
    {
        defaultGUIColor = GUI.color;

        GUILayout.Space(10);
        GUILayout.Label("Data Editor Tool", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.Label("Create and Manage your game data.", EditorStyles.helpBox);
        GUILayout.Space(10);

        //Generate Buttons
        GUILayout.BeginHorizontal();

        GUI.color = Color.green;
        if (GUILayout.Button("Add"))
        {
            //string dataCategory = toolbarStrings[toolbarIndex];
            dataCategory = (DataCategory)toolbarIndex;
            dynamic data = null;

            switch (dataCategory)
            {
                case DataCategory.Characters:
                    data = ScriptableObject.CreateInstance<Character>();
                    break;
                case DataCategory.Items:
                    data = ScriptableObject.CreateInstance<Item>();
                    break;
                case DataCategory.Spells:
                    data = ScriptableObject.CreateInstance<Spell>();
                    break;
                default:
                    break;
            }

            if (data != null)
            {
                AssetDatabase.CreateAsset(data, mainFolderPath + "/" + dataCategory + "/" + data.id.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                dataSelectedIndex = data.id;
            }
        }

        GUI.enabled = (dataSelectedIndex >= 0);
        GUI.color = Color.red;
        if (GUILayout.Button("Remove"))
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(gridID[dataSelectedIndex]));
            dataSelectedIndex = -1;
        }

        GUI.color = Color.cyan;
        if (GUILayout.Button("Duplicate"))
        {
            dataCategory = (DataCategory)toolbarIndex;
            dynamic data = null;

            switch (dataCategory)
            {
                case DataCategory.Characters:
                    data = ScriptableObject.CreateInstance<Character>();
                    break;
                case DataCategory.Items:
                    data = ScriptableObject.CreateInstance<Item>();
                    break;
                case DataCategory.Spells:
                    data = ScriptableObject.CreateInstance<Spell>();
                    break;
                default:
                    break;
            }

            if (data != null)
            {
                //TODO : Copy()
                AssetDatabase.CreateAsset(data, mainFolderPath + "/" + dataCategory + "/" + data.id.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                dataSelectedIndex = data.id;
            }
        }
        GUI.enabled = true;

        GUI.color = defaultGUIColor;
        if (GUILayout.Button("Refresh"))
        {
            LoadDataList();
            LoadGridStrings();
            //Debug.Log("characterCount : " + characterDict.Count);
            //Debug.Log("itemCount : " + itemDict.Count);
            //Debug.Log("spellCount : " + spellDict.Count);
        }


        GUILayout.EndHorizontal();

        GUI.color = defaultGUIColor;

        //Generate Toolbar
        GUILayout.Space(5);
        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);

        GUILayout.Space(10);
        GUILayout.Label("Elements : ");

        DisplayDataList();

        if (GUI.changed)
        {
            LoadGridStrings();
        }
    }

    void DisplayDataList()
    {
        if (gridName?.Count > 0)
        {
            GUILayout.BeginVertical("Box");
            dataSelectedIndex = GUILayout.SelectionGrid(dataSelectedIndex, gridName.ToArray(), 2);

            //if (GUILayout.Button("Start"))
            //{
            //    Debug.Log("You chose " + gridName[dataSelectedIndex] + ", ID : " + gridID[dataSelectedIndex]);
            //}
            GUILayout.EndVertical();
        }
    }

    void LoadGridStrings()
    {
        dataCategory = (DataCategory)toolbarIndex;
        gridID.Clear();
        gridName.Clear();

        switch (dataCategory)
        {
            case DataCategory.Characters:

                foreach (Character character in characterDict.Values)
                {
                    gridID.Add(character.id);
                    gridName.Add(character.name);
                }
                break;
            case DataCategory.Items:
                foreach (Item item in itemDict.Values)
                {
                    gridID.Add(item.id);
                    gridName.Add(item.name);
                }
                break;
            case DataCategory.Spells:
                foreach (Spell spell in spellDict.Values)
                {
                    gridID.Add(spell.id);
                    gridName.Add(spell.name);
                }
                break;
            default:
                break;
        }
    }


    void LoadDataList()
    {

        characterDict.Clear();
        itemDict.Clear();
        spellDict.Clear();

        string folderPath;
        string[] guidAssets;

        foreach (string categoryName in Enum.GetNames(typeof(DataCategory)))
        {
            folderPath = mainFolderPath + "/" + categoryName.ToString();
            dataCategory = (DataCategory)Enum.Parse(typeof(DataCategory), categoryName);

            switch (dataCategory)
            {
                case DataCategory.Characters:
                    //Get all guid of assets contains in folder path
                    guidAssets = AssetDatabase.FindAssets("t:Character", new[] { folderPath });
                    foreach (string asset in guidAssets)
                    {
                        //Get path of th asset
                        var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                        //Get class from asset
                        Character character = AssetDatabase.LoadAssetAtPath<Character>(assetPath);
                        characterDict[character.id] = character;
                    }
                    break;
                case DataCategory.Items:
                    guidAssets = AssetDatabase.FindAssets("t:Item", new[] { folderPath });
                    foreach (string asset in guidAssets)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                        Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                        itemDict[item.id] = item;
                    }
                    break;
                case DataCategory.Spells:
                    guidAssets = AssetDatabase.FindAssets("t:Spell", new[] { folderPath });
                    foreach (string asset in guidAssets)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                        Spell spell = AssetDatabase.LoadAssetAtPath<Spell>(assetPath);
                        spellDict[spell.id] = spell;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    //void LoadDataListOld()
    //{

    //    characterDict.Clear();
    //    itemDict.Clear();
    //    spellDict.Clear();
    //    string folderPath;
    //    Type classType;
    //    foreach (string categoryName in Enum.GetNames(typeof(DataCategory)))
    //    {
    //        folderPath = mainFolderPath + "/" + categoryName.ToString();
    //        Debug.Log(folderPath);
    //        classType = Type.GetType(categoryName);
    //        Debug.Log(classType);

    //        //Get all guid of assets contains in folder path
    //        string[] guidAssets = AssetDatabase.FindAssets("t:" + categoryName, new[] { folderPath });
    //        foreach (string asset in guidAssets)
    //        {
    //            //Get path of th asset
    //            var assetPath = AssetDatabase.GUIDToAssetPath(asset);
    //            //Get class from asset
    //            var character = AssetDatabase.LoadAssetAtPath(assetPath, classType);


    //            characterList.Add(character);
    //        }

    //        Debug.Log(characterList.Count);
    //    }   

}
