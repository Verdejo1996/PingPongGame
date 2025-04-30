using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Base_PowerUp : ScriptableObject
{
    public string powerUpName;
    public Sprite icon;

    public abstract void Activate(Player_Controller player);
}
