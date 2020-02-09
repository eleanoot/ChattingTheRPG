using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneItemManager : MonoBehaviour
{
    public static SceneItemManager sceneItems = null;

    [System.Serializable]
    public class ItemCondition
    {
        public string sceneName;
        public string loadIfThisVarTrue;
    }

    [System.Serializable]
    public class LoadCondition
    {
        public string loadIfThisVarTrue;
        public bool varStatus;
    }
    

    
    public ItemCondition[] loadWith;
    public Dictionary<string, bool> loadConditionStatuses = new Dictionary<string, bool>(); // this needs to be saved

    private void Awake()
    {
        sceneItems = this;
        foreach (ItemCondition ic in loadWith)
        {
            loadConditionStatuses.Add(ic.loadIfThisVarTrue, true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemCondition ic in loadWith)
        {
            if (loadConditionStatuses[ic.loadIfThisVarTrue])
                SceneManager.LoadScene(ic.sceneName, LoadSceneMode.Additive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


