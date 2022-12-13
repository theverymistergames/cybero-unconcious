using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OneTimeTrigger : MonoBehaviour {
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;
    public UnityEvent uevent;
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;

        if (objectsToDisable.Length > 0) {
            foreach (var go in objectsToDisable) {
                go.SetActive(false);
            }
        }


        if (objectsToEnable.Length > 0) {
            foreach (var go in objectsToEnable) {
                go.SetActive(true);
            }
        }

        uevent?.Invoke();

        gameObject.SetActive(false);
    }
}
