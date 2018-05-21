using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour {

    public Transform target;
    public EnemyAI owner;
    public void Init(EnemyAI ai)
    {
        owner = ai;
        target = owner.target;
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.transform == target)
        {
            owner.anim.Play("Kill");
            owner.agent.enabled = false;
            OfflineGameManager.singleton.AssignWinner(owner.gameObject);
        }      
    }

}
