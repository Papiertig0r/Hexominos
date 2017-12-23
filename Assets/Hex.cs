using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] prefabs = new GameObject[5];

    public float scale;
    public float distance;
    public float size;

    public enum FieldType
    {
        STAR,
        EARTH,
        WIND,
        WATER,
        FIRE
    }

    public FieldType[] fieldType = new FieldType[6];

    // Use this for initialization
    void Start ()
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

        Vector3[] vertices = new Vector3[6];
        for(int i = 0; i < 6; i++)
        {
            float rot = 60 * i + 30;
            rot *= Mathf.PI / 180;
            vertices[i] = new Vector3(Mathf.Cos(rot), Mathf.Sin(rot), 0) * size;
        }

        meshFilter.mesh.vertices = vertices;

        int[] triangles = new int[12];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 5;

        triangles[3] = 5;
        triangles[4] = 1;
        triangles[5] = 2;

        triangles[6] = 5;
        triangles[7] = 2;
        triangles[8] = 4;

        triangles[9] = 4;
        triangles[10] = 2;
        triangles[11] = 3;

        meshFilter.mesh.triangles = triangles;

        Vector3[] normals = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            normals[i] = -Vector3.forward;
        }
        meshFilter.mesh.normals = normals;

        Vector2[] uv = new Vector2[6];

        uv[0] = new Vector2(0.5f, 0);
        uv[1] = new Vector2(0, 0.25f);
        uv[2] = new Vector2(0, 0.75f);
        uv[3] = new Vector2(0.5f, 1);
        uv[4] = new Vector2(1, 0.75f);
        uv[5] = new Vector2(1, 0.25f);

        meshFilter.mesh.uv = uv;

        MeshCollider collider = GetComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.mesh;
    }

    //! \todo change name
    public void CreateFields()
    {
        float rotZ;
        for (int i = 0; i < 6; i++)
        {
            GameObject field = Instantiate<GameObject>(prefabs[(int)fieldType[i]]);
            field.transform.SetParent(this.transform);
            field.transform.localScale = new Vector3(scale, scale, scale);
            rotZ = 60 + i * 60;
            field.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotZ + 90));
            field.transform.position = new Vector3(Mathf.Cos(rotZ * Mathf.PI / 180), Mathf.Sin(rotZ * Mathf.PI / 180), 0) * distance;
        }
    }

    public bool CheckValidity(int direction, FieldType type)
    {
        Debug.Log(type + " and " + fieldType[direction]);
        if(type == FieldType.STAR || (fieldType[direction] == type || fieldType[direction] == FieldType.STAR))
        {
            Debug.Log("Matches!");
            return true;
        }

        Debug.Log("Doesn't match!");
        return false;
    }

    public bool CheckNeighbors()
    {
        int isPopulated = 0;
        int isValid = 0;
        Vector3 cubePosition = HexMath.AxialToCube(HexMath.PixelToHex(transform.position));

        RaycastHit hit;
        int direction;
        for(int i = 0; i < 6; i++)
        {
            if(Physics.Raycast(HexMath.HexToPixel(HexMath.CubeToAxial(cubePosition + HexMath.direction[i])) + Vector3.forward, Vector3.back, out hit))
            {
                Hex hex = hit.collider.GetComponent<Hex>();
                if(hex != null)
                {
                    isPopulated++;

                    direction = i + 3;
                    if(direction > 5)
                    {
                        direction -= 6;
                    }

                    if(hex.CheckValidity(direction, fieldType[i]))
                    {
                        isValid++;
                    }
                }
            }
        }

        return (isPopulated != 0) && (isPopulated == isValid);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.lastPointer = GameManager.pointer;
        GameManager.pointer = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.pointer = GameManager.lastPointer;
        GameManager.lastPointer = null;
    }

    public void BeginDrag()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.enabled = false;
    }

    public void EndDrag()
    {
        MeshCollider collider = GetComponent<MeshCollider>();
        collider.enabled = true;
    }
}
