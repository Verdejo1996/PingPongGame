using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCourtGenerator : MonoBehaviour
{
    public ObjectPool cubePrefab;  // Prefab del cubo (debe tener un collider y Rigidbody)
    public int rows = 10;          // Número de filas (puedes ajustarlo)
    public int columns = 10;       // Número de columnas
    public float spacing = 0.1f;   // Espaciado entre los cubos (ajusta según sea necesario)
    public Vector3 tableOffset;

    void Start()
    {
        GenerateTable();
    }

    void GenerateTable()
    {
        // Itera a través de las filas y columnas
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(i, 0, j) + tableOffset;
                GameObject cube = cubePrefab.GetCube();  // Obtener cubo del Object Pool
                cube.transform.position = position;     // Establecer la posición
                cube.transform.SetParent(transform);    // Organizar los cubos bajo el TableManager
            }
        }
    }
}
