using System.Collections.Generic;
using UnityEngine;

public class GumdropBehaviour : MonoBehaviour
{
    [SerializeField] private string objectID;  // ID for this object, could be any unique string or GUID
    [SerializeField] public string tag;

    [SerializeField] private List<GameObject> touchingObjects = new List<GameObject>(); // List to store objects this one is touching

    public List<GameObject> connectingObjects = new List<GameObject>(); // List to store objects this one is connected to

    [SerializeField] private List<GameObject> splatPrefabs = new List<GameObject>();

    private void Start()
    {
        // remame object to objectID + random number
        gameObject.name = objectID + "_" + tag + "_" + Random.Range(0, 99999);

        // fill splat prefabs list with all splat prefabs in the folder
        foreach (GameObject splat in Resources.LoadAll<GameObject>("SplatPrefabs"))
        {
            splatPrefabs.Add(splat);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Ensure you only add objects with the same ID and valid tag
        if (other.CompareTag(tag) && other.gameObject.GetComponent<GumdropBehaviour>().objectID == objectID)
        {
            AddObjectToList(other.gameObject);

            // get the other objects list and add any objects from their list that are not already in this list into this list
            foreach (GameObject obj in other.gameObject.GetComponent<GumdropBehaviour>().touchingObjects)
            {
                if (!connectingObjects.Contains(obj))
                {
                    connectingObjects.Add(obj);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When objects stop touching, remove them from the list
        if (other.CompareTag(tag) && other.gameObject.GetComponent<GumdropBehaviour>().objectID == objectID)
        {
            RemoveObjectFromList(other.gameObject);
        }
    }

    void AddObjectToList(GameObject obj)
    {
        // Add this object to the list if it's not already in the list
        if (!touchingObjects.Contains(obj))
        {
            touchingObjects.Add(obj);
        }

        // Add this object to the connecting objects list if it's not already in the list
        if (!connectingObjects.Contains(obj))
        {
            connectingObjects.Add(obj);
        }
    }

    void RemoveObjectFromList(GameObject obj)
    {
        // Remove the object from the list if it exists
        if (touchingObjects.Contains(obj))
        {
            touchingObjects.Remove(obj);
        }

        // Remove the object from the connecting objects list if it exists
        if (connectingObjects.Contains(obj))
        {
            connectingObjects.Clear();
        }
    }

    void CheckForConnectedObjects()
    {
        // Now you can check if there are 2 or more objects in this list
        if (connectingObjects.Count >= 3) // We are in the list, so we need to check for 3 or more.
        {
            Debug.Log("3 or more objects with the same ID are touching!");
            // Handle the logic for when 3 or more objects are touching
        }
    }

    void Update()
    {
        CheckForConnectedObjects();
    }

    private void OnDestroy()
    {
        // spawn random 1-4 splat prefabs, with random rotation
        foreach (GameObject splat in splatPrefabs)
        {
            var newSplat = Instantiate(splat, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));

            newSplat.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color + new Color(0.3f, 0.3f, 0.3f, 0);

            // newSplat.GetComponent<SpriteRenderer>().color = new Color(newSplat.GetComponent<SpriteRenderer>().color.r, newSplat.GetComponent<SpriteRenderer>().color.g, newSplat.GetComponent<SpriteRenderer>().color.b, 0.5f);

            newSplat.GetComponent<SpriteRenderer>().sortingOrder = -1;

            var size = Random.Range(5, 20);
            newSplat.transform.localScale = new Vector3(size,size,size);
        }
    }
}