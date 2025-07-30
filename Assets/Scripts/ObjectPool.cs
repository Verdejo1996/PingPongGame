using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject cubePrefab;       // Prefab del cubo
    public int poolSize = 50;           // Tamaño inicial del pool
    private Queue<GameObject> cubePool = new();  // Cola de cubos disponibles
    private Dictionary<GameObject, Vector3> cubePositions = new Dictionary<GameObject, Vector3>(); // Diccionario para almacenar las posiciones originales de los cubos


    void Start()
    {
/*        // Llenamos el pool con cubos al inicio
        for (int i = 0; i < poolSize; i++)
        {
            GameObject cube = Instantiate(cubePrefab);
            cube.SetActive(false);  // Desactivamos el cubo al inicio
            cubePool.Enqueue(cube);
        }*/
    }

    private void Update()
    {
        if (!Game_Controller.Instance.playing)
        {
            ResetTable();
        }
    }

    // Obtener un cubo del pool
    public GameObject GetCube(Vector3 position)
    {
        GameObject cube;
        if (cubePool.Count > 0)
        {
            cube = cubePool.Dequeue();
            cube.SetActive(true);

            // Resetear el Rigidbody cuando se reactiva el cubo
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            rb.isKinematic = true;  // Aseguramos que isKinematic sea true al reactivar el cubo

            cubePositions[cube] = position; // Guardamos la posición de este cubo en el diccionario
            //return cube;
        }
        else
        {
            // Si no hay cubos disponibles, creamos uno nuevo
            cube = Instantiate(cubePrefab);
            cubePositions[cube] = position; // Guardamos la posición de este nuevo cubo
            //cubePool.Enqueue(cube);
            //return cube;
        }
        cube.transform.position = position;
        return cube;
    }

    // Devolver un cubo al pool
    public void ReturnCube(GameObject cube)
    {
        cube.SetActive(false);  // Desactivamos el cubo

        
        if (!cubePositions.ContainsKey(cube))
        {
            cubePositions[cube] = cube.transform.position; // Guardamos la posición si no estaba ya en el diccionario
        }

        // Resetear Rigidbody al devolverlo al pool
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        rb.isKinematic = true;  // Aseguramos que isKinematic sea true al desactivarlo

        cubePool.Enqueue(cube);  // Lo agregamos de nuevo al pool
    }

    public void ResetTable()
    {
        foreach (GameObject cube in cubePool)
        {
            if (!cube.activeSelf)
            {
                cube.SetActive(true);

                // Recuperamos la posición original del diccionario
                if (cubePositions.TryGetValue(cube, out Vector3 originalPosition))
                {
                    cube.transform.position = originalPosition;  // Restaura la posición original
                }

                Rigidbody rb = cube.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
        }
    }
}
