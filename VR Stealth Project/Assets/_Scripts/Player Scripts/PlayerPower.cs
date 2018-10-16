using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class PlayerPower : MonoBehaviour {

    public GameObject flashlight;
    public Slider batteryUI;
    public float decayRate = 1;
    public float rechargeRate = 0.5f;
    public float batteryLife = 10;

    float currentBattery;
    

    public UnityEvent onGripDown;
    public UnityEvent onTouchpadUp;
    public UnityEvent onMouseDown;
    Hand hand;

    private void Start()
    {
        currentBattery = batteryLife;
        flashlight.SetActive(false);
        if(batteryUI != null)
            batteryUI.maxValue = batteryLife;
        
        if(Player.instance != null)
            hand = Player.instance.GetHand((int)Hand.HandType.Right);
    }

    // Update is called once per frame
    void Update () {


        if (hand.controller != null)
        {
            if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
            {
                onGripDown.Invoke();
            }

            if (hand.controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                onTouchpadUp.Invoke();
            }

        }
        if (Input.GetButtonDown("Fire1"))
        {
            onMouseDown.Invoke();
        }

        if (flashlight.activeInHierarchy)
        {
            currentBattery = Mathf.Clamp(currentBattery -= (Time.deltaTime * decayRate), 0, batteryLife);

            if (currentBattery <= 0)
            {
                flashlight.SetActive(false);
                currentBattery = 0;
            }
            hand.controller.TriggerHapticPulse(300);
        }
        else
        {
            currentBattery = Mathf.Clamp(currentBattery += (Time.deltaTime * rechargeRate), 0, batteryLife);
        }

        if (batteryUI != null)
            batteryUI.value = currentBattery;

	}

    public void ToggleLight()
    {
        flashlight.SetActive(!flashlight.activeInHierarchy);
    }

}
