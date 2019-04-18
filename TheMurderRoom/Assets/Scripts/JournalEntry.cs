using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "JournalEntry", menuName = "JournalEntry", order = 1)]
public class JournalEntry : ScriptableObject {

    [HideInInspector]
    public bool isVisible = false;

    [SerializeField]
    public string id;
    [SerializeField]
    public Text text;
}
