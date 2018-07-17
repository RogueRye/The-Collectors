using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PcPickup : MonoBehaviour {

    // Use this for initialization


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OfflineGameManager.singleton.messageDisplay.text = "Press 'F' to collect...";
            if (Input.GetKeyDown(KeyCode.F))
            {
                OfflineGameManager.singleton.messageDisplay.text = "";
                other.GetComponent<HandlePickups>().PickupItem();
                Debug.Log("item picked up");

                Destroy(gameObject);
            }
        }
    }
}
