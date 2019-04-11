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
        /// <summary>
        /// Starts a block from fungus flowchart
        /// </summary>
        /// <returns> True if a block is activated, False otherwise </returns>
        public bool InteractWith() {
            return ExecuteBlock();
        }
    }
}
