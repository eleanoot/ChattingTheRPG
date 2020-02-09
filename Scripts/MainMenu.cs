using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public MainMenu menuInstance = null;
    public GameObject[] subMenus;

    public Component[] buttons;

    public GameObject currentMenu;
    public GameObject previousMenu;

    private void Awake()
    {
        if (menuInstance == null)
            menuInstance = this;
        else if (menuInstance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        buttons = GetComponentsInChildren<Button>();
        currentMenu = this.gameObject;
        previousMenu = this.gameObject;
    }

    public void ShowInventory()
    {
        subMenus[0].SetActive(true);
        currentMenu = subMenus[0];
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }

        // Fill in the text.
        Inventory inventory = FindObjectOfType<Inventory>();
       // List<IInventoryItem> items = inventory.GetItems();
        List<string> items = inventory.GetItems();
        subMenus[0].GetComponentInChildren<Text>().text = "";
        //foreach (IInventoryItem i in items)
        foreach (string i in items)
        {
            if (i != null)
                //subMenus[0].GetComponentInChildren<Text>().text = subMenus[0].GetComponentInChildren<Text>().text + "- " + i.name + "\n";
                subMenus[0].GetComponentInChildren<Text>().text = subMenus[0].GetComponentInChildren<Text>().text + "- " + i + "\n";
            
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        if (GameManager.instance.SaveGame())
        {
            subMenus[1].SetActive(true);
            currentMenu = subMenus[1];
        }
    }


    // In the event of future other menus that have buttons, this will probably be easier with base Menu class containing buttons
    public void DisableSubmenu()
    {
        currentMenu.SetActive(false);
        currentMenu = previousMenu;
        foreach (Button b in buttons)
        {
            b.interactable = true;
        }
    }
    
}
