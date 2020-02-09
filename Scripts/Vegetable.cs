using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

namespace Yarn.Unity.Example
{
    public class Vegetable : IInventoryItem
    {
        public AudioClip pickupSfx;

        public string Name
        {
            get
            {
                return "Vegetable";
            }
        }

        public override void OnPickup()
        {
            base.OnPickup();
            FindObjectOfType<DialogueRunner>().GetComponent<ExampleVariableStorage>().SetValue("$taken_veg", new Yarn.Value(true));
            SceneItemManager.sceneItems.loadConditionStatuses[loadConditionVar] = false;
            SoundManager.soundManager.PlaySingleSfx(pickupSfx);
        }


    }
}
