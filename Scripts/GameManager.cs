using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject mainMenu;
    private Inventory inventory;
    private Player player;
    private ExampleVariableStorage dialogueVars;

    private AsyncOperation asyncLoadLevel;
    private bool loadingGame = false;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<Player>();
        dialogueVars = FindObjectOfType<ExampleVariableStorage>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public bool SaveGame()
    {
        // Create a save instance with all data for the current session saved into it
        Save save = CreateSaveGameObject();

        // Pass path for the Save to be saved to with .save file extension
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        
        return true;
    }

    public void LoadGame()
    {
        // Check that the save file actually exists- only one save file ever at a time here
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            inventory.EmptyInventory();
            dialogueVars.ResetToDefaults();

            mainMenu.SetActive(false);

            // Provide binary formatter a stream of bytes to read and create a save object from
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // Convert save game information into actual game state 
            loadingGame = true;
            //SceneManager.LoadScene(save.currentScene);
            StartCoroutine(LoadScene(save));

            for (int i = 0; i < save.currentInventory.Count; i++)
            {
                //inventory.AddItem(save.currentInventory[i]);
                inventory.LoadItem(save.currentInventory[i]);
            }

            

            //player.transform.position = new Vector3(save.playerX, save.playerY);

            //for (int i = 0; i < save.dialogueVars.Count; i++)
            //{
            //    object value;
            //    ExampleVariableStorage.SaveVariable variable = save.dialogueVars[i];

            //    switch (variable.type)
            //    {
            //        case Yarn.Value.Type.Number:
            //            value = variable.valueAsFloat;
            //            break;

            //        case Yarn.Value.Type.String:
            //            value = variable.valueAsString;
            //            break;

            //        case Yarn.Value.Type.Bool:
            //            value = variable.valueAsBool;
            //            break;

            //        case Yarn.Value.Type.Null:
            //            value = null;
            //            break;

            //        default:
            //            throw new System.ArgumentOutOfRangeException();

            //    }

            //    var v = new Yarn.Value(value);

            //    dialogueVars.SetValue(variable.name, v);
            //}

            //FindObjectOfType<DialogueRunner>().dialogue.visitedNodes = save.nodesVisited;

            

            
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    IEnumerator LoadScene(Save save)
    {
        asyncLoadLevel = SceneManager.LoadSceneAsync(save.currentScene);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
    }

    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        //foreach (IInventoryItem item in inventory.GetItems())
        foreach (string item in inventory.GetItems())
        {
            save.currentInventory.Add(item);
        }

        foreach (KeyValuePair<string, Yarn.Value> entry in dialogueVars.GetAllVariables())
        {
            ExampleVariableStorage.SaveVariable var = new ExampleVariableStorage.SaveVariable();
            var.name = entry.Key;
            var.type = entry.Value.type;

            var.valueAsBool = entry.Value.AsBool;
            var.valueAsFloat = entry.Value.AsNumber;
            var.valueAsString = entry.Value.AsString;

            save.dialogueVars.Add(var);

        }

        foreach (KeyValuePair<string, bool> condition in SceneItemManager.sceneItems.loadConditionStatuses)
        {
            SceneItemManager.LoadCondition var = new SceneItemManager.LoadCondition();
            var.loadIfThisVarTrue = condition.Key;
            var.varStatus = condition.Value;

            save.sceneVars.Add(var);
        }

        save.currentScene = SceneManager.GetActiveScene().name;

        save.playerX = player.transform.position.x;
        save.playerY = player.transform.position.y;

        save.nodesVisited = FindObjectOfType<DialogueRunner>().dialogue.visitedNodes;

        return save;
    }

    // Update is called once per frame
    void Update()
    {
        // Don't allow menuing for now if in dialogue. 
        if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Main menu is not shown, so show it
            if (!mainMenu.activeInHierarchy)
            {
                mainMenu.SetActive(true);
            }
            // Main menu is showing and is the only menu
            else if (mainMenu.activeInHierarchy && mainMenu.GetComponent<MainMenu>().currentMenu == mainMenu)
            {
                mainMenu.SetActive(false);
            }
            // Main menu is showing and is not the current menu
            else 
            {
                mainMenu.GetComponent<MainMenu>().DisableSubmenu();
                
            }

            


        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (loadingGame)
        {
            // Provide binary formatter a stream of bytes to read and create a save object from
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            foreach (var condition in save.sceneVars)
            {
                SceneItemManager.sceneItems.loadConditionStatuses[condition.loadIfThisVarTrue] = condition.varStatus;
            }

            player = FindObjectOfType<Player>();
            player.transform.position = new Vector3(save.playerX, save.playerY);

            dialogueVars = FindObjectOfType<ExampleVariableStorage>();
            for (int i = 0; i < save.dialogueVars.Count; i++)
            {
                object value;
                ExampleVariableStorage.SaveVariable variable = save.dialogueVars[i];

                switch (variable.type)
                {
                    case Yarn.Value.Type.Number:
                        value = variable.valueAsFloat;
                        break;

                    case Yarn.Value.Type.String:
                        value = variable.valueAsString;
                        break;

                    case Yarn.Value.Type.Bool:
                        value = variable.valueAsBool;
                        break;

                    case Yarn.Value.Type.Null:
                        value = null;
                        break;

                    default:
                        throw new System.ArgumentOutOfRangeException();

                }

                var v = new Yarn.Value(value);

                dialogueVars.SetValue(variable.name, v);
            }

            

            FindObjectOfType<DialogueRunner>().dialogue.visitedNodes = save.nodesVisited;

            loadingGame = false;
        }
        
    }
}
