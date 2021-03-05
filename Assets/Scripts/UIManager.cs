using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("InputField")]
    public InputField inputField;

    [Header("Help Panel")]
    public GameObject helpPanel;
    public Text helpText;

    [Header("Show Element Panel")]
    public GameObject showElementPanel;
    public Image showElementImage;
    public Text showElementName;
    public Text showElementDesc;

    [Header("Show All Elements Panel")]
    public GameObject showAllElementsPanel;
    public GameObject element;

    [Header("Message")]
    public GameObject message;
    public int displayTime = 2;

    List<Character> characters = new List<Character>();
    List<Item> items = new List<Item>();
    List<Spell> spells = new List<Spell>();
    string mainFolderPath = "Assets/Resources/GameData";

    enum DataCategory { Characters, Items, Spells }

    // Start is called before the first frame update
    void Start()
    {
        LoadDataList();
        showAllElementsPanel.SetActive(false);
        showElementPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    void DisplayMessage(string text)
    {
        StartCoroutine(DisplayMessageCoroutine(text));
    }
    IEnumerator DisplayMessageCoroutine(string text)
    {
        message.GetComponent<Text>().text = text;
        message.SetActive(true);

        yield return new WaitForSeconds(displayTime);

        message.SetActive(false);
        message.GetComponent<Text>().text = "";
    }

    public void InputHandler()
    {
        //Check if the last char of input is a digit
        //Which mean it's a specific action
        if (char.IsDigit(inputField.text[inputField.text.Length - 1]))
        {
            string subInput = inputField.text.Substring(0, inputField.text.Length - 1);

            if (char.IsDigit(subInput[subInput.Length - 2]))
            {
                int index1 = int.Parse(subInput[subInput.Length - 2].ToString());
                int index2 = int.Parse(inputField.text[inputField.text.Length - 1].ToString());
                subInput = inputField.text.Substring(0, subInput.Length - 2);

                switch (subInput)
                {
                    case "AddItem ":
                    case "AddItem":
                    case "Add Item ":
                        if ((index1 < 0 || index1 > characters.Count - 1) || (index2 < 0 || index2 > items.Count - 1))
                        {
                            DisplayMessage("One of the Indexes is out of range");
                            return;
                        }
                        characters[index1].inventory.Add(items[index2]);
                        break;

                    case "RemoveItem ":
                    case "RemoveItem":
                    case "Remove Item ":
                        if ((index1 < 0 || index1 > characters.Count - 1) || (index2 < 0 || index2 > items.Count - 1))
                        {
                            DisplayMessage("One of the Indexes is out of range");
                            return;
                        }
                        characters[index1].inventory.Remove(items[index2]);
                        break;

                    case "AddSpell ":
                    case "AddSpell":
                    case "Add Spell ":
                        if ((index1 < 0 || index1 > characters.Count - 1) || (index2 < 0 || index2 > spells.Count - 1))
                        {
                            DisplayMessage("One of the Indexes is out of range");
                            return;
                        }
                        characters[index1].spells.Add(spells[index2]);
                        break;

                    case "RemoveSpell ":
                    case "RemoveSpell":
                    case "Remove Spell ":
                        if ((index1 < 0 || index1 > characters.Count - 1) || (index2 < 0 || index2 > spells.Count - 1))
                        {
                            DisplayMessage("One of the Indexes is out of range");
                            return;
                        }
                        characters[index1].spells.Remove(spells[index2]);
                        break;

                    default:
                        print("What are you writing about ?");
                        DisplayMessage("What are you writing about ?");
                        break;
                }

            }
            else
            {
                int index = int.Parse(inputField.text[inputField.text.Length - 1].ToString());

                switch (subInput)
                {
                    case "ShowCharacter ":
                    case "ShowCharacter":
                    case "Show Character ":
                        ShowElement(DataCategory.Characters, index);
                        break;

                    case "ShowItem ":
                    case "ShowItem":
                    case "Show Item ":
                        ShowElement(DataCategory.Items, index);
                        break;

                    case "ShowSpell ":
                    case "ShowSpell":
                    case "Show Spell ":
                        ShowElement(DataCategory.Spells, index);
                        break;

                    default:
                        print("What are you writing about ?");
                        DisplayMessage("What are you writing about ?");
                        break;
                }
            }
        }
        else
        {
            switch (inputField.text)
            {
                case "/help":
                case "help":
                case "Help":
                    DisplayHelp();
                    break;

                case "ShowAllCharacters":
                case "Show All Characters":
                case "ShowCharacters":
                case "Show Characters":
                    ShowAllElements(DataCategory.Characters);
                    break;

                case "ShowAllItems":
                case "Show All Items":
                case "ShowItems":
                case "Show Items":
                    ShowAllElements(DataCategory.Items);
                    break;

                case "ShowAllSpells":
                case "Show All Spells":
                case "ShowSpells":
                case "Show Spells":
                    ShowAllElements(DataCategory.Spells);
                    break;

                default:
                    print("What are you writing about ?");
                    DisplayMessage("What are you writing about ?");
                    break;
            }
        }
    }

    private void DisplayHelp()
    {
        helpText.text = "ShowAllCharacters | Show All Characters | ShowCharacters | Show Characters :" + "\n" +
        "Show All Characters available." + "\n" +
        "\n" +
        "ShowAllItems | Show All Items | ShowItems | Show Items :" + "\n" +
        "Show All Items available." + "\n" +
        "\n" +
        "ShowAllSpells | Show All Spells | ShowSpells | Show Spells :" + "\n" +
        "Show All Spells available." + "\n" +
        "\n" +
        "ShowCharacter characterIndex | Show Character characterIndex / Where characterIndex is a integer :" + "\n" +
        "Show a specific Character." + "\n" +
        "\n" +
        "ShowItem itemIndex | Show Item citemIndex / Where itemIndex is a integer :" + "\n" +
        "Show a specific Item." + "\n" +
        "\n" +
        "ShowSpell spellIndex | Show Spell spellIndex / Where spellIndex is a integer :" + "\n" +
        "Show a specific Spell." + "\n" +
        "\n" +
        "AddItem characterIndex itemIndex | Add Item characterIndex itemIndex / Where indexes are integers :" + "\n" +
        "Add an Item to a Character." + "\n" +
        "\n" +
        "RemoveItem characterIndex itemIndex | Remove Item characterIndex itemIndex / Where indexes are integers :" + "\n" +
        "Remove an Item to a Character." + "\n" +
        "\n" +
        "AddSpell characterIndex spellIndex | Add Spell characterIndex spellIndex / Where indexes are integers :" + "\n" +
        "Add an Spell to a Character." + "\n" +
        "\n" +
        "RemoveSpell characterIndex spellIndex | Remove Spell characterIndex spellIndex / Where indexes are integers :" + "\n" +
        "Remove an Spell to a Character." + "\n" +
        "\n";

        helpPanel.SetActive(true);
    }

    void ShowAllElements(DataCategory dataCategory)
    {
        showElementPanel.SetActive(false);
        helpPanel.SetActive(false);
        showAllElementsPanel.SetActive(true);
        GameObject newElement;

        //Clean the Panel
        foreach (Transform child in showAllElementsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        switch (dataCategory)
        {
            case DataCategory.Characters:
                foreach (Character character in characters)
                {
                    newElement = Instantiate(element, showAllElementsPanel.transform);
                    newElement.transform.GetChild(0).GetComponent<Image>().sprite = character.sprite;
                    newElement.transform.GetChild(1).GetComponent<Text>().text = character.name;
                    newElement.transform.GetChild(2).GetComponent<Text>().text = characters.IndexOf(character).ToString();
                }
                break;
            case DataCategory.Items:
                foreach (Item item in items)
                {
                    newElement = Instantiate(element, showAllElementsPanel.transform);
                    newElement.transform.GetChild(0).GetComponent<Image>().sprite = item.sprite;
                    newElement.transform.GetChild(1).GetComponent<Text>().text = item.name;
                    newElement.transform.GetChild(2).GetComponent<Text>().text = items.IndexOf(item).ToString();
                }
                break;
            case DataCategory.Spells:
                foreach (Spell spell in spells)
                {
                    newElement = Instantiate(element, showAllElementsPanel.transform);
                    newElement.transform.GetChild(1).GetComponent<Text>().text = spell.name;
                    newElement.transform.GetChild(2).GetComponent<Text>().text = spells.IndexOf(spell).ToString();
                }
                break;
            default:
                break;
        }
    }

    void ShowElement(DataCategory dataCategory, int index)
    {
        showAllElementsPanel.SetActive(false);
        helpPanel.SetActive(false);
        showElementPanel.SetActive(true);

        //Clean the Panel
        showElementImage.sprite = null;
        showElementName.text = "";
        showElementDesc.text = "";

        switch (dataCategory)
        {
            case DataCategory.Characters:
                if (index < 0 || index > characters.Count - 1)
                {
                    //print(characters.Count);
                    DisplayMessage("Index out of range");
                    return;
                }
                Character character = characters[index];
                showElementImage.sprite = character.sprite;
                showElementName.text = character.name;
                showElementDesc.text = character.ToString();
                break;
            case DataCategory.Items:
                if (index < 0 || index > items.Count - 1)
                {
                    DisplayMessage("Index out of range");
                    return;
                }
                Item item = items[index];
                showElementImage.sprite = item.sprite;
                showElementName.text = item.name;
                showElementDesc.text = item.ToString();
                break;
            case DataCategory.Spells:
                if (index < 0 || index > spells.Count - 1)
                {
                    DisplayMessage("Index out of range");
                    return;
                }
                Spell spell = spells[index];
                showElementName.text = spell.name;
                showElementDesc.text = spell.ToString();
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

        characters.Clear();
        items.Clear();
        spells.Clear();

        string[] guidAssets;

        //Get all guid of assets contains in folder path
        guidAssets = AssetDatabase.FindAssets("t:Character", new[] { (mainFolderPath + "/Characters") });
        foreach (string asset in guidAssets)
        {
            //Get path of th asset
            var assetPath = AssetDatabase.GUIDToAssetPath(asset);
            //Get class from asset
            Character character = AssetDatabase.LoadAssetAtPath<Character>(assetPath);
            Character newCharacter = ScriptableObject.CreateInstance<Character>();
            newCharacter = character.DeepCopy();
            characters.Add(newCharacter);
        }

        guidAssets = AssetDatabase.FindAssets("t:Item", new[] { (mainFolderPath + "/Items") });
        foreach (string asset in guidAssets)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(asset);
            Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);
            items.Add(item);
        }

        guidAssets = AssetDatabase.FindAssets("t:Spell", new[] { (mainFolderPath + "/Spells") });
        foreach (string asset in guidAssets)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(asset);
            Spell spell = AssetDatabase.LoadAssetAtPath<Spell>(assetPath);
            spells.Add(spell);
        }
    }

    #region Singleton

    static UIManager instance;

    public static UIManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("A singleton can only be instantiated once!");
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion
}
