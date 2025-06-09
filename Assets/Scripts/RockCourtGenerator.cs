using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCourtGenerator : MonoBehaviour
{
    public GameObject rockPrefab;
    public int width = 10;
    public int length = 10; // Cambiamos de 20 a 10
    public float spacing = 1.0f;
    public Transform courtParent;

    private List<GameObject> tiles = new List<GameObject>();
    private float yOffset = -0.31f; // Altura fija

    void Start()
    {
        GenerateCourt();
    }

    public void GenerateCourt()
    {
        ClearExistingTiles();

        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int z = -length / 2; z < length / 2; z++)
            {
                Vector3 pos = new Vector3(x * spacing, yOffset, z * spacing);
                GameObject tile = Instantiate(rockPrefab, pos, Quaternion.identity, courtParent);
                tile.name = $"Rock_{x}_{z}";
                tiles.Add(tile);
            }
        }
    }

    public void BreakRandomTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, tiles.Count);
            GameObject tile = tiles[index];

            if (!tile.GetComponent<Rigidbody>())
                tile.AddComponent<Rigidbody>(); // Agrega gravedad
        }
    }

    public void ResetCourt()
    {
        foreach (GameObject tile in tiles)
        {
            if (tile.TryGetComponent<Rigidbody>(out Rigidbody rb))
                Destroy(rb); // Elimina físicas

            tile.transform.position = new Vector3(tile.transform.position.x, yOffset, tile.transform.position.z);
        }
    }

    private void ClearExistingTiles()
    {
        foreach (var tile in tiles)
        {
            if (tile != null)
                DestroyImmediate(tile);
        }

        tiles.Clear();
    }
}
