using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Journal : MonoBehaviour{

    public static Journal journal;
    public LinkedList<JournalEntry> entries;

    public GameObject journalUI;
    public GameObject textPrefab;


    void Start() {
        journal = this;
        entries = new LinkedList<JournalEntry>();
        journalUI.transform.parent.parent.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetButtonDown("OpenJournal")) {
            journalUI.transform.parent.parent.gameObject.SetActive(!journalUI.transform.parent.parent.gameObject.activeSelf);
        }
    }

    public void Add(JournalEntry entry) {
        entries.AddLast(entry);
        GameObject textObject = Instantiate(textPrefab, journalUI.transform);
        RectTransform transform = textObject.GetComponent<RectTransform>();
        transform.Translate(new Vector3(0, 0 - (entries.Count * 145), 0));
        textObject.GetComponent<Text>().text = entry.Text;
        journalUI.transform.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    public void Clear() {
        entries.Clear();
    }
}
