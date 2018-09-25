using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_hook : MonoBehaviour {

    EnemyAI owner;

    private void Start()
    {
        owner = GetComponentInParent<EnemyAI>();
    }

    public void Step()
    {
        owner.Step();
    }

}
