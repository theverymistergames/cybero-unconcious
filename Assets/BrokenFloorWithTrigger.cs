using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFloorWithTrigger : MonoBehaviour {
    public GameObject[] floors;
    
    // Start is called before the first frame update
    void Awake() {
        foreach (var floor in floors) {
            floor.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            foreach (var floor in floors) {
                floor.GetComponent<Rigidbody>().isKinematic = false;
            }

            gameObject.GetComponent<BoxCollider>().enabled = false;
            
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
