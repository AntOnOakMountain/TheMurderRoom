using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobbing : ICameraEffect {


    [Header("HeadBobbing")]
    public float intensity = 0.1f;
    public float bobbingSpeed = 2f;
    [Tooltip("Speed to return to neutral when standing still.")]
    public float bobbingReturnSpeed = 0.2f;
    private float bobOscillate = 0;
    private float timer = 0;

    void Update() {
        // If standing still return to neutral position
        if (Mathf.Abs(Player.GetPlayer().speed) < Mathf.Epsilon) {
            float timeLeftUntilNeutral = timer % 0.5f;
            // if not back to neutral, go back to neutral
            if (!(timeLeftUntilNeutral < Mathf.Epsilon)) {
                if (timeLeftUntilNeutral < bobbingReturnSpeed) {
                    timer = 0;
                }
                else {
                    timer += bobbingReturnSpeed * Time.deltaTime;
                }
            }
        }
        else {
            timer += Time.deltaTime * bobbingSpeed * (Player.GetPlayer().speed / Player.GetPlayer().maxSpeed);
        }

        bobOscillate = Mathf.Sin(timer * (2 * Mathf.PI));

        positionOffset = new Vector3(0, bobOscillate * intensity, 0);
    }
}
