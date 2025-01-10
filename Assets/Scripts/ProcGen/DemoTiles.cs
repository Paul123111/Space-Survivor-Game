using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTiles : MonoBehaviour
{
    [SerializeField] ProceduralGeneration proceduralGeneration;

    // Start is called before the first frame update
    void Start()
    {
        proceduralGeneration.GenerateFromImage();
    }
}
