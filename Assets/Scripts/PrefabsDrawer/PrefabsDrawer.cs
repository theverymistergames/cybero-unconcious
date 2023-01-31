using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Attach this to a game object and all instantiated prefabs will be children to this game object
public class PrefabsDrawer : MonoBehaviour
{
    //The object we want to add
    public GameObject[] prefabs;

    //Whats the radius of the circle we will add objects inside of?
    public float radius = 5f;

    //How many GOs will we add each time we press a button?
    public int objectsCount = 5;

    public bool randomizeRotation;
    public bool withoutCollider;

    //Should we add or remove objects within the circle
    public enum Actions { AddObjects, RemoveObjects }

    public Actions action;

    //Add a prefab that we instantiated in the editor script
    public void AddPrefab(GameObject prefab, Vector3 center)
    {
        //Get a random position within a circle in 2d space
        var positionInCircle = Random.insideUnitCircle * radius;

        //But we are in 3d, so make it 3d and move it to where the center is
        var position = new Vector3(positionInCircle.x, withoutCollider ? 0.001f : 0f, positionInCircle.y) + center;

        prefab.transform.position = position;

        if (randomizeRotation) {
            var angles = prefab.transform.eulerAngles;
            prefab.transform.eulerAngles = new Vector3(angles.x, Random.value * 360, angles.z);
        }

        prefab.transform.parent = transform;
    }
    
    //Returns all objects within the circle
    public List<GameObject> GetAllObjectsWithinCircle(Vector3 center) {
        //Get an array with all children to this transform
        var children = GetAllChildren();

        //All objects within the circle
        var childrenWithinCircle = new List<GameObject>();

        foreach (var child in children) {
            //If this child is within the circle
            if (Vector3.SqrMagnitude(child.transform.position - center) < radius * radius) {
                //DestroyImmediate(child);
                childrenWithinCircle.Add(child);
            }
        }

        return childrenWithinCircle;
    }

    //Get an array with all children to this GO
    public GameObject[] GetAllChildren() {
        //This array will hold all children
        var children = new GameObject[transform.childCount];
        var childCount = 0;
        
        foreach (Transform child in transform) {
            children[childCount] = child.gameObject;
            childCount += 1;
        }

        return children;
    }
}