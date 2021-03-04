using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// The Data Editor Tool allows you to create and manage different types of data, and to save them in ScriptableObjects.
/// This class manages the creation and management of this tool.
/// </summary>
public class DataEditorTool : EditorWindow
{
    Color defaultGUIColor;
    static string mainFolderPath = "Assets/Resources/GameData";

    //Contains the respective assets created
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
    //Use to easily find the selected grid element ID
    List<int> gridID = new List<int>();
    //Use to display the grid elements
    List<string> gridName = new List<string>();

    bool isInEditMode = false;

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

        //Title / Description
        GUILayout.Space(10);
        GUILayout.Label("Data Editor Tool", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.Label("Create and Manage your game data.", EditorStyles.helpBox);
        GUILayout.Space(10);

        //Generate Buttons
        GUILayout.BeginHorizontal();

        GUI.enabled = !isInEditMode;
        AddButton();

        GUI.enabled = !isInEditMode && (dataSelectedIndex >= 0);
        EditButton();
        RemoveButton();
        DuplicateButton();
        RefreshButton();

        GUILayout.EndHorizontal();

        GUI.enabled = !isInEditMode;

        //Generate Toolbar
        GUILayout.Space(5);
        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
        GUILayout.Space(10);

        GUI.enabled = true;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (isInEditMode)
        {
            GUILayout.Space(20);
            DisplayEditMode();
        }
        else
        {
            GUILayout.Label("Elements : ");
            GUILayout.Space(5);
            DisplayDataGrid();
        }

        if (GUI.changed)
        {
            LoadGridStrings();
        }
    }

    #region Buttons

    /// <summary>
    /// Create and add an asset.
    /// </summary>
    void AddButton()
    {
        GUI.color = Color.green;
        if (GUILayout.Button("Add"))
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
                AssetDatabase.CreateAsset(data, mainFolderPath + "/" + dataCategory + "/" + data.id.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                dataSelectedIndex = data.id;
            }
        }
    }

    /// <summary>
    /// Triggers the edit mode of an asset.
    /// </summary>
    void EditButton()
    {
        GUI.color = Color.yellow;
        if (GUILayout.Button("Edit"))
        {
            isInEditMode = true;
        }
    }

    /// <summary>
    /// Remove a asset.
    /// </summary>
    void RemoveButton()
    {

        GUI.color = Color.red;
        if (GUILayout.Button("Remove"))
        {
            dataCategory = (DataCategory)toolbarIndex;

            switch (dataCategory)
            {
                case DataCategory.Characters:
                    break;
                case DataCategory.Items:
                    //Remove the item from all inventories
                    Item itemToRemove;
                    if (dataSelectedIndex >= 0 && itemDict.TryGetValue(gridID[dataSelectedIndex], out itemToRemove))
                    {
                        foreach (Character character in characterDict.Values)
                            if (character.inventory.Contains(itemToRemove))
                            {
                                character.inventory.Remove(itemToRemove);
                            }
                    }
                    break;
                case DataCategory.Spells:
                    //Remove the spell from all spells lists
                    Spell spellToRemove;
                    if (dataSelectedIndex >= 0 && spellDict.TryGetValue(gridID[dataSelectedIndex], out spellToRemove))
                    {
                        foreach (Character character in characterDict.Values)
                            if (character.spells.Contains(spellToRemove))
                            {
                                character.spells.Remove(spellToRemove);
                            }
                    }
                    break;
                default:
                    break;
            }

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(gridID[dataSelectedIndex]));
            dataSelectedIndex = -1;
        }
    }

    /// <summary>
    /// Create and save a duplicate of an asset.
    /// </summary>
    void DuplicateButton()
    {
        GUI.color = Color.cyan;
        if (GUILayout.Button("Duplicate"))
        {
            dataCategory = (DataCategory)toolbarIndex;
            dynamic data = null;
            switch (dataCategory)
            {
                case DataCategory.Characters:
                    Character characterToDuplicate;
                    if (dataSelectedIndex >= 0 && characterDict.TryGetValue(gridID[dataSelectedIndex], out characterToDuplicate))
                    {
                        data = ScriptableObject.CreateInstance<Character>();
                        data.GetCopy(characterToDuplicate);
                    }
                    break;
                case DataCategory.Items:
                    Item itemToDuplicate;
                    if (dataSelectedIndex >= 0 && itemDict.TryGetValue(gridID[dataSelectedIndex], out itemToDuplicate))
                    {
                        data = ScriptableObject.CreateInstance<Item>();
                        data.GetCopy(itemToDuplicate);
                    }
                    break;
                case DataCategory.Spells:
                    Spell spellToDuplicate;
                    if (dataSelectedIndex >= 0 && spellDict.TryGetValue(gridID[dataSelectedIndex], out spellToDuplicate))
                    {
                        data = ScriptableObject.CreateInstance<Spell>();
                        data.GetCopy(spellToDuplicate);
                    }
                    break;
                default:
                    break;
            }

            if (data != null)
            {
                //if name end by : name(I) where I is a int, increment I to to distinguish duplication
                if (data.name[data.name.Length - 3].Equals('(') && data.name[data.name.Length - 1].Equals(')'))
                {
                    int index = (int)Char.GetNumericValue(data.name[data.name.Length - 2]);
                    index++;
                    char[] ch = data.name.ToCharArray();
                    ch[data.name.Length - 2] = Convert.ToChar(index.ToString());
                    data.name = new string(ch);
                }
                else
                {
                    data.name += "(1)";
                }

                AssetDatabase.CreateAsset(data, mainFolderPath + "/" + dataCategory + "/" + data.name + ".asset");
                AssetDatabase.SaveAssets();
            }
            dataSelectedIndex = -1;
        }
    }

    /// <summary>
    /// Refreshes the actually existing assets and updates the grid display.
    /// </summary>
    void RefreshButton()
    {
        GUI.color = defaultGUIColor;
        if (GUILayout.Button("Refresh"))
        {
            LoadDataList();
            LoadGridStrings();
        }
    }

    #endregion

    #region Display Panel

    /// <summary>
    /// Displays the edit window of the asset.
    /// </summary>
    void DisplayEditMode()
    {
        dataCategory = (DataCategory)toolbarIndex;

        switch (dataCategory)
        {
            case DataCategory.Characters:
                Character characterToEdit;
                if (gridID.Count > 0 && dataSelectedIndex >= 0 && characterDict.TryGetValue(gridID[dataSelectedIndex], out characterToEdit))
                {
                    Editor.CreateEditor(characterToEdit).OnInspectorGUI();
                }
                break;
            case DataCategory.Items:
                Item itemToEdit;
                if (gridID.Count > 0 && dataSelectedIndex >= 0 && itemDict.TryGetValue(gridID[dataSelectedIndex], out itemToEdit))
                {
                    Editor.CreateEditor(itemToEdit).OnInspectorGUI();
                }
                break;
            case DataCategory.Spells:
                Spell spellToEdit;
                if (gridID.Count > 0 && dataSelectedIndex >= 0 && spellDict.TryGetValue(gridID[dataSelectedIndex], out spellToEdit))
                {
                    Editor.CreateEditor(spellToEdit).OnInspectorGUI();
                }
                break;
            default:
                break;
        }

        GUILayout.Space(20);
        if (GUILayout.Button("Return"))
        {
            LoadDataList();
            isInEditMode = false;
            GUI.FocusControl(null);
        }
    }

    /// <summary>
    /// Displays available assets as a grid.
    /// </summary>
    void DisplayDataGrid()
    {
        if (gridName?.Count > 0)
        {
            GUILayout.BeginVertical("Box");
            dataSelectedIndex = GUILayout.SelectionGrid(dataSelectedIndex, gridName.ToArray(), 2);
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.Space(10);
            GUILayout.Label("Empty");
            dataSelectedIndex = -1;
        }
    }

    #endregion

    #region Load Data

    void ClearGrid()
    {
        if (gridID != null && gridName != null)
        {
            gridID.Clear();
            gridName.Clear();
        }
    }

    /// <summary>
    /// Loads and updates the necessary lists for the grid.
    /// </summary>
    void LoadGridStrings()
    {
        dataCategory = (DataCategory)toolbarIndex;

        ClearGrid();

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

    /// <summary>
    /// Loads and updates the lists containing the assets of respective categories.
    /// </summary>
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

                        character.CheckInventory();
                        character.CheckSpells();

                        //Rename the asset file if it does not have the generic name
                        if (character.name != "No Name")
                        {
                            AssetDatabase.RenameAsset(assetPath, character.name);
                        }
                    }
                    break;
                case DataCategory.Items:
                    guidAssets = AssetDatabase.FindAssets("t:Item", new[] { folderPath });
                    foreach (string asset in guidAssets)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                        Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
                        itemDict[item.id] = item;

                        if (item.name != "No Name")
                        {
                            AssetDatabase.RenameAsset(assetPath, item.name);
                        }
                    }
                    break;
                case DataCategory.Spells:

                    guidAssets = AssetDatabase.FindAssets("t:Spell", new[] { folderPath });
                    foreach (string asset in guidAssets)
                    {
                        var assetPath = AssetDatabase.GUIDToAssetPath(asset);
                        Spell spell = AssetDatabase.LoadAssetAtPath<Spell>(assetPath);
                        spellDict[spell.id] = spell;

                        if (spell.name != "No Name")
                        {
                            AssetDatabase.RenameAsset(assetPath, spell.name);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    #endregion

    void Debug_printGrid()
    {
        for (int i = 0; i < gridID.Count; i++)
        {
            Debug.Log("Index: " + i + ", ID : " + gridID[i]);
        }

        Debug.Log("IsEqual : " + (gridID.Count == gridName.Count));
        //Debug.Log("characterCount : " + characterDict.Count);
        //Debug.Log("itemCount : " + itemDict.Count);
        //Debug.Log("spellCount : " + spellDict.Count);
    }
}
