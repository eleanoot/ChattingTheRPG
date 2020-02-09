using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
[System.Serializable]
public class Save 
{
    public string currentScene = "";
    public List<string> currentInventory = new List<string>();
    public float playerX = 0;
    public float playerY = 0;
    public List<ExampleVariableStorage.SaveVariable> dialogueVars = new List<ExampleVariableStorage.SaveVariable>();
    public IEnumerable<string> nodesVisited;

    public List<SceneItemManager.LoadCondition> sceneVars = new List<SceneItemManager.LoadCondition>();

}
