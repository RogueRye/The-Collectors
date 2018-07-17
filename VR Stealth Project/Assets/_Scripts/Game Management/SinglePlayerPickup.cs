using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SinglePlayerPickup : MonoBehaviour {

    HandlePickups player;
    Throwable throwable;

    public void Start()
    {

        player = Player.instance.gameObject.GetComponent<HandlePickups>();
        throwable = GetComponent<Throwable>();

        throwable.onDetachFromHand.AddListener(() => PickMeUp());
        
    }

    public void PickMeUp()
    {
        player.PickupItem();
        Debug.Log("item picked up");

        Destroy(gameObject);
    }

}
