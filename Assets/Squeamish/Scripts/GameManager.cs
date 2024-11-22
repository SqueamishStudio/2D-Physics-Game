using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Create list of objects
    [SerializeField] List<GameObject> objectPrefabs = new List<GameObject>();

    [SerializeField] List<GameObject> objectCreationPrefabs = new List<GameObject>(); 

    [SerializeField] List<GameObject> objects = new List<GameObject>();

    [SerializeField] Transform spawnTransform;

    [SerializeField] private int points = 0;

    [SerializeField] private TextMeshProUGUI pointsText;

    [SerializeField] private GameObject pipe;

    [SerializeField] private float rotAmount = 10;

    private float time = 5;

    private float creationTime = 0;

    private Vector3 slerpPos;

    // Start is called before the first frame update
    void Start()
    {
        // add the first 3 objects in object prefabs to the object creation prefabs
        objectCreationPrefabs.Add(objectPrefabs[0]);
        objectCreationPrefabs.Add(objectPrefabs[1]);
        objectCreationPrefabs.Add(objectPrefabs[2]);

        // then remove the first 3 objects from object prefabs

        objectPrefabs.RemoveAt(0);
        objectPrefabs.RemoveAt(0);
        objectPrefabs.RemoveAt(0);

        InvokeRepeating("AddPrefab", time * 10, time * 10);
    }

    // Update is called once per frame
    public void CreateObject()
    {
        if (creationTime > 1)
        {
            GameObject gameObject = Instantiate(objectCreationPrefabs[Random.Range(0, objectCreationPrefabs.Count)], spawnTransform.position, Quaternion.identity);
            objects.Add(gameObject);

            creationTime = 0;
        }

        return;
    }

    private void AddPrefab()
    {
        if (objectPrefabs.Count == 0)
        {
            return;
        }

        var toAdd = objectPrefabs[Random.Range(0, objectPrefabs.Count)];

        objectCreationPrefabs.Add(toAdd);
        objectPrefabs.Remove(toAdd);
    }

    void Update()
    {
        var destroyList = new List<GameObject>();

        // Check if any objects have 3 or more connections
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<GumdropBehaviour>().connectingObjects.Count > 2 && !destroyList.Contains(obj)) // If object has 3 or more connections and is not already in the destroy list
            {
                List<GameObject> connectedObjects = obj.GetComponent<GumdropBehaviour>().connectingObjects;

                // Add all connected objects to the destroy list if not already there
                foreach (GameObject obj2 in connectedObjects)
                {
                    if (!destroyList.Contains(obj2))
                    {
                        destroyList.Add(obj2);
                    }
                }

                // Award points based on the number of connected objects
                int pointsToAdd = connectedObjects.Count;

                // Bonus points for connections larger than 3
                if (pointsToAdd > 3)
                {
                    pointsToAdd += pointsToAdd - 3;  // Adds extra points for more than 3 connected objects
                }

                points += pointsToAdd * 10;

                // update the points on the UI
                pointsText.text = points.ToString();
            }
        }

        // Destroy all objects in the destroy list
        foreach (GameObject obj2 in destroyList)
        {
            objects.Remove(obj2);
            Destroy(obj2);
        }

        destroyList.Clear();

        // forever have the pipe rotate
        pipe.transform.Rotate(0, 0, rotAmount * Time.deltaTime);

        // add to the creation time
        creationTime += Time.deltaTime;
    }
}