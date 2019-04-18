using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus {
    /// <summary>
    /// The block will execute when the player opens the pad.
    /// </summary>
    [EventHandlerInfo("TheMurderRoom",
                      "Open Pad",
                      "The block will execute when the player opens the pad.")]
    [AddComponentMenu("")]
    public class OpenPad : EventHandler {
        /// <summary>
        /// Starts a block from fungus flowchart
        /// </summary>
        /// <returns> True if a block is activated, False otherwise </returns>
        public bool Open() {
            return ExecuteBlock();
        }
    }
}
