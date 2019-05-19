using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Level")
        {
            Destroy(gameObject);
        }
    }
}
