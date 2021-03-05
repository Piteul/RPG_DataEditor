# RPG_DataEditor

## Use the Data Editor Tool

You can access the tool in the Unity Header, under the "Tools" category. Quite Simple.

The **Data Editor Tool** allows you to view and manage assets relating to classes : Character, Item and Spell.
It allows you to create, delete, duplicate and edit assets. These are located in the _"Assets/Resources/GameData"_ folder and then in their respective sub-folder.

The manipulable elements are **Scriptable Objects**, saved in .asset file. In addition, they contain all of their data in **JSON** form.

Although there is a semi-automatic data refresh, do not hesitate to use the Refresh button if necessary.

Finally, you find a set of prebuilt assets _to play with_.

![RPGDataEditorTool1](Images\RPGDataEditorTool1.PNG "List Elements") ![RPGDataEditorTool2](Images\RPGDataEditorTool2.PNG "Edit Mode")

## How to Play

At game launch, you will be invited to use the prompt system.
Do not hesitate to type _"Help"_ or _"/help"_ to display the available commands.

Here is the list of available commands :

- ShowAllCharacters | Show All Characters | ShowCharacters | Show Characters :
  Show All Characters available.

- ShowAllItems | Show All Items | ShowItems | Show Items :
  Show All Items available.

- ShowAllSpells | Show All Spells | ShowSpells | Show Spells :
  Show All Spells available.

- ShowCharacter characterIndex | Show Character characterIndex / Where characterIndex is a integer :
  Show a specific Character.

- ShowItem itemIndex | Show Item citemIndex / Where itemIndex is a integer :
  Show a specific Item.

- ShowSpell spellIndex | Show Spell spellIndex / Where spellIndex is a integer :
  Show a specific Spell.

- AddItem characterIndex itemIndex | Add Item characterIndex itemIndex / Where indexes are integers :
  Add an Item to a Character.

- RemoveItem characterIndex itemIndex | Remove Item characterIndex itemIndex / Where indexes are integers :
  Remove an Item to a Character.

- AddSpell characterIndex spellIndex | Add Spell characterIndex spellIndex / Where indexes are integers :
  Add an Spell to a Character.

- RemoveSpell characterIndex spellIndex | Remove Spell characterIndex spellIndex / Where indexes are integers :
  Remove an Spell to a Character.

  ![RPGDataEditorGame1](Images\RPGDataEditorGame1.PNG "Screenplay 1") ![RPGDataEditorGame2](Images\RPGDataEditorGame2.PNG "Screenplay 2")

## Time Spent

**Part 1 : Data Editor Tool** | 5h30
**Part 2 : GUI and Prompt System** | 3h30
**Documentation and set of prebuilt assets** | 0h30

**Total : 9h30**
