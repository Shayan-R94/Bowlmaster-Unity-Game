using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinCounter : MonoBehaviour {

    public Text standingDisplay;

    private GameManager gameManager;
    private bool ballOutOfPlay = false;
    private int lastStandingCount = -1;
    private float lastChangeTime;
    private int lastSettledCount = 10;

    // Use this for initialization
    void Start () {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    
    // Update is called once per frame
    void Update () {
        standingDisplay.text = CountStanding().ToString();

        if (ballOutOfPlay)
        {
            UpdateStandingCountAndSettle();
            standingDisplay.color = Color.red;
        }
    }

    public void Reset()
    {
        lastSettledCount = 10;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Ball")
        {
            ballOutOfPlay = true;
        }
    }

    void UpdateStandingCountAndSettle()
    {
        // Update the lastStandingCount and call PinsHaveSettled() when they have settled.
        int currentStanding = CountStanding();

        if (currentStanding != lastStandingCount)
        {
            lastChangeTime = Time.time;
            lastStandingCount = currentStanding;
            return;
        }

        // How long to wait to consider pins settled.
        float settledTime = 3f;

        if ((Time.time - lastChangeTime) > settledTime)
        {
            // If last change > 3s ago.
            PinsHaveSettled();
        }
    }

    void PinsHaveSettled()
    {
        int standing = CountStanding();
        int pinFall = lastSettledCount - standing;
        lastSettledCount = standing;
        gameManager.Bowl(pinFall);

        // Indicates pins have settled and ball not back in box.
        lastStandingCount = -1;
        ballOutOfPlay = false;
        standingDisplay.color = Color.green;
    }

    int CountStanding()
    {
        int standing = 0;

        foreach (Pin pin in GameObject.FindObjectsOfType<Pin>())
        {
            if (pin.IsStanding())
            {
                standing++;
            }
        }

        return standing;
    }
}
