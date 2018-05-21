using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HandlePickups : MonoBehaviour {


    public TMP_Text uiCounter;
    public Slider pickupSlider;
    public float resourceTotal = 5;

    float currentPickups;


	// Use this for initialization
	void Start () {
        currentPickups = 0;
        if (pickupSlider != null)
            pickupSlider.maxValue = resourceTotal;
    }
	
	// Update is called once per frame
	void Update () {
        if(uiCounter != null)
            uiCounter.text = string.Format("{0}", currentPickups);
        if (pickupSlider != null)
            pickupSlider.value = currentPickups;
    }

    public void PickupItem()
    {
        currentPickups++;
       
    }

    public void ActivateMachine()
    {
        if(currentPickups >= resourceTotal)
        {
            OfflineGameManager.singleton.AssignWinner(gameObject);
        }
        else
        {
            if(OfflineGameManager.singleton.messageDisplay != null)
            {

            }
        }
    }

}
