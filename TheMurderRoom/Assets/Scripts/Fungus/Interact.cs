using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;


namespace Fungus {
    /// <summary>
    /// The block will execute when player interacts with the Npc/Object.
    /// </summary>
    [EventHandlerInfo("MonoBehaviour",
                      "Interact",
                      "The block will execute when player interacts with the NPC/Object.")]
    [AddComponentMenu("")]
    public class Interact : EventHandler {
        public void InteractWith() { // Can't name it same as class (Interact) so InteractWith will have to do
            ExecuteBlock();
        }
    }
}
