using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataEditorTool : EditorWindow
{
    Color defaultGUIColor;
    Vector2 scrollPosition;
    static string mainFolderPath = "Assets/Resources/GameData";

    int toolbarIndex = 0;
    static string[] toolbarStrings = { "Characters", "Items", "Spells" };

    static int dataSelected;

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

        foreach (string dataCategory in toolbarStrings)
        {
            if (!AssetDatabase.IsValidFolder(mainFolderPath + "/" + dataCategory))
            {
                AssetDatabase.CreateFolder(mainFolderPath, dataCategory);
            }
        }


        //if (!AssetDatabase.IsValidFolder(mainFolderPath + "/Items"))
        //{
        //    AssetDatabase.CreateFolder(mainFolderPath, "Items");
        //}

        //if (!AssetDatabase.IsValidFolder(mainFolderPath + "/Spells"))
        //{
        //    AssetDatabase.CreateFolder(mainFolderPath, "Spells");
        //}

    }

    private void OnGUI()
    {
        defaultGUIColor = GUI.color;

        GUILayout.Space(10);
        GUILayout.Label("Data Editor Tool", EditorStyles.boldLabel);
        GUILayout.Space(10);

        //Generate Buttons
        GUILayout.BeginHorizontal();

        GUI.color = Color.green;
        if (GUILayout.Button("Add"))
        {
            string dataCategory = toolbarStrings[toolbarIndex];
            dynamic data = null;

            switch (dataCategory)
            {
                case "Characters":
                    data = ScriptableObject.CreateInstance<Character>();
                    break;

                case "Items":
                    data = ScriptableObject.CreateInstance<Item>();
                    break;

                case "Spells":
                    data = ScriptableObject.CreateInstance<Spell>();
                    break;

                default:
                    break;
            }

            if (data != null)
            {
                AssetDatabase.CreateAsset(data, mainFolderPath + "/" + dataCategory + "/" + data.id.ToString() + ".asset");
                AssetDatabase.SaveAssets();
                dataSelected = data.id;
            }
        }

        GUI.color = Color.red;
        if (GUILayout.Button("Remove"))
        {


        }

        GUI.color = Color.cyan;
        if (GUILayout.Button("Duplicate"))
        {
        }
        //GUILayout.Space(5);


        GUI.color = defaultGUIColor;

        if (GUILayout.Button("Refresh"))
        {


        }

        GUI.enabled = true;

        GUILayout.EndHorizontal();

        GUI.color = defaultGUIColor;

        //Generate Toolbar
        GUILayout.Space(5);
        toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);

        GUILayout.Space(10);
        GUILayout.Label("Elements : ");

        //DisplayDataList();




    }

    void DisplayDataList()
    {
        //Display List

        GUILayout.BeginVertical();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);



        GUILayout.EndScrollView();




    }


}
