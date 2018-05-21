﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAI : MonoBehaviour {

    [SerializeField] ParticleSystem zappyParticles;

    EnemyAI ai;

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.CompareTag("Seeker"))
        {
            ai = other.gameObject.GetComponent<EnemyAI>();
            ai.IsFrozen = true;
            zappyParticles.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (ai != null && other.gameObject == ai.gameObject)
        {
            if(!ai.IsFrozen)
                ai.IsFrozen = true;

            zappyParticles.transform.LookAt(ai.gameObject.transform.position + Vector3.up *2);
        }

       
    }

    private void OnTriggerExit(Collider other)
    {
        
        zappyParticles.Stop();
        ai = null;
    }




}
