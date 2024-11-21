using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateCustomMesh : MonoBehaviour
{

    Vector3[] vertices = new Vector3[8];
    Vector2[] uv = new Vector2[4];
    int[] triangles = new int[24];

    GameObject meshObject;
    Mesh mesh;

    [SerializeField] Tilemap walls;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMeshData();

        mesh = new Mesh();
        mesh.name = "Custom Cube Mesh";

        meshObject = new GameObject("MeshObject", typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider));

        meshObject.GetComponent<MeshFilter>().mesh = mesh;
        meshObject.GetComponent<MeshCollider>().sharedMesh = mesh;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    void GenerateMeshData() {
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 2, 0);
        vertices[2] = new Vector3(2, 2, 0);
        vertices[3] = new Vector3(2, 0, 0);
        vertices[4] = new Vector3(0, 0, 2);
        vertices[5] = new Vector3(0, 2, 2);
        vertices[6] = new Vector3(2, 2, 2);
        vertices[7] = new Vector3(2, 0, 2);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        triangles[6] = 4;
        triangles[7] = 5;
        triangles[8] = 6;

        triangles[9] = 4;
        triangles[10] = 6;
        triangles[11] = 7;

        triangles[12] = 4;
        triangles[13] = 5;
        triangles[14] = 0;

        triangles[15] = 5;
        triangles[16] = 1;
        triangles[17] = 0;

        triangles[18] = 3;
        triangles[19] = 6;
        triangles[20] = 7;

        triangles[21] = 3;
        triangles[22] = 2;
        triangles[23] = 6;
    }

    void FindTiles() {
        //walls.G
    }
}
