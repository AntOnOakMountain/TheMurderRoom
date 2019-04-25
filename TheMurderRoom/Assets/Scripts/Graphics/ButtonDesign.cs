using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDesign {

    public Sprite baseSprite;
    public Sprite highlightedSprite;
    public Sprite pressedSprite;
    public Sprite disabledSprite;

    public void Apply(Button button) {

        Image image = button.GetComponent<Image>();
        image.sprite = baseSprite;

        SpriteState ss = new SpriteState();
        ss.highlightedSprite = highlightedSprite;
        ss.pressedSprite = pressedSprite;
        ss.disabledSprite = disabledSprite;
        button.spriteState = ss;
    }
}
