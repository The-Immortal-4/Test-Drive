using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public GameObject[] pedestrianPrefab;
    public int pedestriansToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while(count < pedestriansToSpawn)
        {
            int ranPadestrain = Random.Range(0, pedestrianPrefab.Length);
            GameObject obj = Instantiate(pedestrianPrefab[ranPadestrain]);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            obj.transform.position = child.position;
            
            yield return new WaitForEndOfFrame();
            count++;
        }
    }
}
