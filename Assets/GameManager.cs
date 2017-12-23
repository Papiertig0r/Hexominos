using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Hex hexPrefab;

    public static Hex pointer = null;
    public static Hex lastPointer = null;

    // Use this for initialization
    void Start ()
    {
        InstantiateHex();

        pointer = InstantiateHex();
        pointer.BeginDrag();
    }

    // Update is called once per frame
    void Update ()
    {
        if (pointer != null)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position = HexMath.HexToPixel(HexMath.PixelToHex(position));
            pointer.transform.position = position;
            if(Input.GetMouseButtonDown(0) && pointer.CheckNeighbors())
            {
                pointer.EndDrag();
                pointer = null;

                pointer = InstantiateHex();
                pointer.BeginDrag();
            }
        }
    }

    public Hex InstantiateHex()
    {
        Hex hex = Instantiate<Hex>(hexPrefab);

        for (int i = 0; i < 6; i++)
        {
            hex.fieldType[i] = (Hex.FieldType)Random.Range(0, 5);
        }

        hex.transform.SetParent(this.transform);
        hex.CreateFields();

        return hex;
    }
}
