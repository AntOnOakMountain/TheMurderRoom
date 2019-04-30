using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour{

    public static Journal journal;
    public LinkedList<JournalEntry> entries;

    public GameObject journalUI;
    public GameObject textPrefab;
    public GameObject menuDialog;
    public GameObject sayDialog;


    void Start() {
        journal = this;
        entries = new LinkedList<JournalEntry>();
        ToggleJournal(false);
    }

    public void ToggleJournal(bool status) {
        for (int i = 0; i < journalUI.transform.parent.parent.parent.childCount; i++){
            journalUI.transform.parent.parent.parent.GetChild(i).gameObject.SetActive(status);
        }
    }

    public void Add(JournalEntry entry) {
        entries.AddLast(entry);
        GameObject textObject;
        if (journalUI.transform.childCount - 2 > entries.Count)
        {
            textObject = journalUI.transform.GetChild(entries.Count - 1).gameObject;
            textObject.SetActive(true);
        }
        else {
            textObject = Instantiate(textPrefab, journalUI.transform);
            RectTransform transform = textObject.GetComponent<RectTransform>();
            sayDialog.transform.SetAsLastSibling();
            menuDialog.transform.SetAsLastSibling();
        }
        textObject.GetComponent<Text>().text = entry.Text;
        textObject.GetComponent<Text>().alignment = entry.Character == "You" ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
        journalUI.transform.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);

    }

    public void Clear() {
        for (int i = 0; i < journalUI.transform.childCount - 2; i++) {
            journalUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        entries.Clear();
    }
}
