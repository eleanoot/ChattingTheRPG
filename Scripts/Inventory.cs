using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Inventory : MonoBehaviour
{
    public static Inventory invInstance = null;
    private const int SLOTS = 9;

    //private List<IInventoryItem> mItems = new List<IInventoryItem>();
    private List<string> mItems = new List<string>();

    private void Awake()
    {
        if (invInstance == null)
            invInstance = this;
        else if (invInstance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(IInventoryItem item)
    {
        if (mItems.Count < SLOTS)
        {
           // mItems.Add(item);
            mItems.Add(item.name);
            item.OnPickup();
        }
    }

    public void LoadItem(string item)
    {
        mItems.Add(item);
    }

    [YarnCommand("giveitem")]
    public void RemoveItem(string itemToRemove)
    {
        //foreach (IInventoryItem i in mItems)
        foreach (string i in mItems)
        {
            //if (i.name == itemToRemove)
            if (i == itemToRemove)
            {
                mItems.Remove(i);
                return;
            }
        }
        Debug.Log(string.Format("Cannot find item {0} to remove!", itemToRemove));
    }

    public void EmptyInventory()
    {
        mItems.Clear();
    }

    //public List<IInventoryItem> GetItems()
    //{
    //    return mItems;
    //}

    public List<string> GetItems()
    {
        return mItems;
    }
}
