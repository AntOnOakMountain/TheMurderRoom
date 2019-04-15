using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SequentialActivities", menuName = "Activities/SequentialActivities", order = 1)]
public class SequentialActivities : Activity {

    [SerializeField]
    private Activity[] activities;
    private int currentIndex = 0; 


    public override void Update() {
        if(currentIndex < activities.Length) {
            activities[currentIndex].Update();

            if (activities[currentIndex].IsDone()) {
                activities[currentIndex].End();
                currentIndex++;
            }
        }
    }

    public override void FixedUpdate() {
        if (currentIndex < activities.Length) {
            activities[currentIndex].FixedUpdate();

            if (activities[currentIndex].IsDone()) {
                activities[currentIndex].End();
                currentIndex++;
            }
        }
    }

    public override bool IsDone() {
        return currentIndex == activities.Length;
    }
}
