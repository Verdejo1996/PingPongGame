using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }
    public GameObject cubePrefab;       // Prefab del cubo
    public int poolSize = 100;           // Tamaño inicial del pool
    private Queue<GameObject> cubePool = new();  // Cola de cubos disponibles
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        // Llenamos el pool con cubos al inicio
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.SetActive(false);  // Desactivamos el cubo al inicio
            cubePool.Enqueue(cube);
        }
    }

    // Obtener un cubo del pool
    public GameObject GetCube()
    {
        if (cubePool.Count > 0)
        {
            GameObject cube = cubePool.Dequeue();
            cube.SetActive(true);
            return cube;
        }
        else
        {
            // Si no hay cubos disponibles, creamos uno nuevo
            GameObject cube = Instantiate(cubePrefab);
            return cube;
        }
    }

    // Devolver un cubo al pool
    public void ReturnCube(GameObject cube)
    {
        cube.SetActive(false);  // Desactivamos el cubo
        cubePool.Enqueue(cube);  // Lo agregamos de nuevo al pool
    }

    public void ResetTable()
    {
        foreach (GameObject cube in cubePool)
        {
            if (!cube.activeSelf)
            {
                cube.SetActive(true);
                Rigidbody rb = cube.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }
}
