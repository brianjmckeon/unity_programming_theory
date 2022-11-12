using System.Collections.Generic;
using UnityEngine;

public class Farmyard : MonoBehaviour
{
    // The list of unique producer types we'll use in laying out the farmyard.
    [SerializeField]
    private List<GameObject> producerPrefabs = new List<GameObject>();

    // The fence prefab that'll be used for building the surrounding fence walls.
    [SerializeField]
    private GameObject fencePrefab;

    private MeshRenderer groundMesh;


    void Start()
    {
        groundMesh = GameObject.Find("Ground").GetComponent<MeshRenderer>();

        buildFarmyard();
        buildFence();
    }

    // Builds north, south, east, and west walls around the farmyard.
    // Adds all fence sections to the "Borders" object to keep the editor's hierarchy clean.
    void buildFence()
    {
        var borders = GameObject.Find("Borders");
        borders.transform.parent = gameObject.transform;

        buildNorthAndSouthFences(borders);
        buildEastAndWestFences(borders);
    }

    // Builds the north and south walls of the fence.
    // Adds new objects to parent.
    void buildNorthAndSouthFences(GameObject parent)
    {
        // We'll need to rotate the fence prefab to orient it correctly for each side of the yard.
        // So we use a reference object for this purpose and destroy it later.
        Quaternion quat = Quaternion.AngleAxis(90, Vector3.up);
        GameObject fenceRefObject = Instantiate(fencePrefab, Vector3.zero, quat);
        var fenceMesh = fenceRefObject.GetComponent<MeshRenderer>();

        float northStartX = groundMesh.bounds.min.x;
        float southStartX = groundMesh.bounds.max.x;
        float startZ = groundMesh.bounds.min.z - fenceMesh.bounds.extents.z / 6;

        int count = (int)(groundMesh.bounds.extents.z / fenceMesh.bounds.extents.z);

        for (int i = 0; i <= count; i++)
        {
            var northSection = Instantiate(
                fencePrefab,
                new Vector3(northStartX, 0, startZ + i * fenceMesh.bounds.extents.z * 2),
                quat);

            var southSection = Instantiate(
                fencePrefab,
                new Vector3(southStartX, 0, startZ + i * fenceMesh.bounds.extents.z * 2),
                quat);

            northSection.transform.parent = parent.transform;
            southSection.transform.parent = parent.transform;
        }

        Destroy(fenceRefObject);
    }

    // Builds the east and west walls of the fence.
    // Adds new objects to parent.
    void buildEastAndWestFences(GameObject parent)
    {
        // No rotation necessary; we can use the prefab as-is.
        Quaternion quat = fencePrefab.transform.rotation;
        GameObject fenceRefObject = Instantiate(fencePrefab, Vector3.zero, quat);
        var fenceMesh = fenceRefObject.GetComponent<MeshRenderer>();

        float northStartZ = groundMesh.bounds.min.z;
        float southStartZ = groundMesh.bounds.max.z;
        float startX = groundMesh.bounds.min.x + fenceMesh.bounds.extents.x * 1.7f;

        int count = (int)(groundMesh.bounds.extents.x / fenceMesh.bounds.extents.x);

        for (int i = 0; i <= count; i++)
        {
            var eastSection = Instantiate(
                fencePrefab,
                new Vector3(startX + i * fenceMesh.bounds.extents.x * 2, 0, southStartZ),
                quat);

            var westSection = Instantiate(
                fencePrefab,
                new Vector3(startX + i * fenceMesh.bounds.extents.x * 2, 0, northStartZ),
                quat);

            eastSection.transform.parent = parent.transform;
            westSection.transform.parent = parent.transform;
        }

        Destroy(fenceRefObject);
    }

    // Create random producer tiles and lay them out in a grid.
    // All created objects will be children of the "Producers" object.
    void buildFarmyard()
    {
        GameObject producers = GameObject.Find("Producers");

        // All producer prefabs should be the same size otherwise it will throw off the calculation below!
        // We grab the first one in the list to use as reference.
        var producerMesh = producerPrefabs[0].GetComponent<MeshRenderer>();

        // Determine how many rows and columns of producer prefabs we can fit within our bounds.
        int rows = (int)(groundMesh.bounds.extents.z / producerMesh.bounds.extents.z);
        int columns = (int)(groundMesh.bounds.extents.x / producerMesh.bounds.extents.x);

        float startX = groundMesh.bounds.min.x + producerMesh.bounds.extents.x;
        float startZ = groundMesh.bounds.min.z + producerMesh.bounds.extents.z;

        // Loop over rows and columns creating producer tiles.
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                // Calculate new position for this producer and instantiate it.
                var pos = new Vector3(
                    startX + c * producerMesh.bounds.extents.x * 2,
                    0,
                    startZ + r * producerMesh.bounds.extents.z * 2);

                // Choose one of our producer types at random.
                int i = Random.Range(0, producerPrefabs.Count);
                GameObject tile = Instantiate(producerPrefabs[i], pos, producerPrefabs[i].transform.rotation);
                tile.transform.parent = producers.transform;
            }
        }
    }
}
