using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IInventoryItem : MonoBehaviour
{
    string Name { get;  }
    public string loadConditionVar;
    public virtual void OnPickup()
    {
        gameObject.SetActive(false);
    }
}


