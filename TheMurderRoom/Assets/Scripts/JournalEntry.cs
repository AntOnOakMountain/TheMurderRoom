using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntry {


    private string character, text;

    
    public JournalEntry(string character, string text) {
        this.Character = character;
        this.Text = text;
    }

    public string Character
    {
        get
        {
            return character;
        }

        set
        {
            character = value;
        }
    }

    public string Text
    {
        get
        {
            return text;
        }

        set
        {
            text = value;
        }
    }
}
