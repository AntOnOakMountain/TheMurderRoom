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

    public Sprite partnerImage, selfImage;


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
            sayDialog.transform.SetAsLastSibling();
            menuDialog.transform.SetAsLastSibling();
        }
        Text textComponent;
        if (textObject.GetComponent<Text>() != null)
            textComponent = textObject.GetComponent<Text>();
        else
            textComponent = textObject.GetComponentInChildren<Text>();
        textComponent.text = entry.Text;
        RectTransform transform = textObject.transform.GetChild(0).GetComponent<RectTransform>();
        if (entry.Character == "You")
        {
            transform.pivot = new Vector2(1, 1);
            transform.anchorMin = new Vector2(1, 1);
            transform.anchorMax = new Vector2(1, 1);
            transform.localPosition = new Vector3(640,0,0);
            textComponent.alignment = TextAnchor.UpperRight;
            textObject.GetComponentInChildren<Image>().sprite = selfImage;
            textObject.GetComponentInChildren<HorizontalLayoutGroup>().padding = new RectOffset(0, 50, 0,0);
        }
        else {
            transform.pivot = new Vector2(0, 1);
            transform.anchorMin = new Vector2(0, 1);
            transform.anchorMax = new Vector2(0, 1);
            transform.localPosition = Vector3.zero;
            textComponent.alignment = TextAnchor.UpperLeft;
            textObject.GetComponentInChildren<Image>().sprite = partnerImage;
            textObject.GetComponentInChildren<HorizontalLayoutGroup>().padding = new RectOffset(50, 0, 0,0);
        }
        StartCoroutine(AfterAppend(textObject.GetComponent<LayoutElement>(), textComponent.GetComponent<RectTransform>()));
    }

    private IEnumerator AfterAppend(LayoutElement layout, RectTransform transform) {
        yield return null;
        layout.preferredHeight = transform.rect.height * 0.59f;
        journalUI.transform.parent.parent.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }

    public void Clear() {
        for (int i = 0; i < journalUI.transform.childCount - 2; i++) {
            Destroy(journalUI.transform.GetChild(i).gameObject);
            //journalUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        entries.Clear();
    }
}
