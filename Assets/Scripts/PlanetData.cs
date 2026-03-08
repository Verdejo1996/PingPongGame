using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPlaneta", menuName = "Datos/Planeta")]
public class PlanetData : ScriptableObject
{
    public string nombre;
    public string nombreWwise;
    [TextArea] public string descripcion;
    public string escenaDestino;
    public PaddleDefinition rewardPaddle;
    public bool isAvailable;
    public bool isTutorialPlanet;
}
