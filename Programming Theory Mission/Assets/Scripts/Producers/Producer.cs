using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all producer types.
public abstract class Producer : MonoBehaviour
{
    // The types of resource producers.
    [SerializeField]
    protected List<GameObject> resourcePrefabs;

    // The amount of resources we've produced.
    private int totalProduced = 0;

    // Number of seconds until a new resource is created.
    protected abstract int productionRate { get; }

    // The number of resources that can be produced before going dormant.
    protected abstract int productionLimit { get; }

    // Number of seconds to lay dormant after hitting the production limit.
    protected abstract int dormantPeriod { get; }


    void Start()
    {
        StartCoroutine(productionLoop());
    }

    IEnumerator productionLoop()
    {
        while (true)
        {
            if (totalProduced == productionLimit)
            {
                // Going dormant
                yield return new WaitForSeconds(dormantPeriod);
                totalProduced = 0;
            }
            else
            {
                // Still producing
                yield return new WaitForSeconds(productionRate);
                var newFood = produceResource();
                totalProduced++;
            }
        }
    }

    // Instantiate a resource object and return it.
    private GameObject produceResource()
    {
        int index = (resourcePrefabs.Count == 1) ? 0 : Random.Range(0, resourcePrefabs.Count);
        GameObject newResource = Instantiate(resourcePrefabs[index]);

        // Position the resource randomly within the producer tile making sure it's not hidden by the ground.
        MeshRenderer mesh = newResource.GetComponent<MeshRenderer>();
        var offset = new Vector3(Random.Range(-4f, 4f), mesh.bounds.extents.y / 2, Random.Range(-4f, 4f));
        newResource.transform.position = gameObject.transform.position + offset;

        GameObject resources = GameObject.Find("Resources");
        newResource.transform.parent = resources.transform;

        return newResource;
    }
}
