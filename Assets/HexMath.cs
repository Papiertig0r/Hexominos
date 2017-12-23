using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axial
{
    public float q;
    public float r;

    public Axial(float q, float r)
    {
        this.q = q;
        this.r = r;
    }

    public override string ToString()
    {
        return ("(" + q.ToString() + ", " + r.ToString() + ")");
    }
};

public static class HexMath
{
    public static Vector3[] direction =
    {
        new Vector3( 0, -1,  1),
        new Vector3(-1,  0,  1),
        new Vector3(-1,  1,  0),
        new Vector3( 0, -1, -1),
        new Vector3( 1,  0, -1),
        new Vector3( 1, -1,  0)
    };


    public static Axial CubeToAxial(Vector3 cube)
    {
        float q = (int)cube.x;
        float r = (int)cube.z;
        return new Axial(q, r);
    }

    public static Vector3 AxialToCube(Axial hex)
    {
        float x = hex.q;
        float z = hex.r;
        float y = -x - z;
        return new Vector3(x, y, z);
    }

    public static Vector3 HexToPixel(Axial hex)
    {
        float x = Mathf.Sqrt(3) * (hex.q + hex.r / 2f);
        float y = 3f / 2f * hex.r;
        return new Vector3(x, y, 0);
    }

    public static Axial PixelToHex(Vector3 pixel)
    {
        float q = (pixel.x * Mathf.Sqrt(3f) / 3f - pixel.y / 3f);
        float r = (pixel.y * 2f / 3f);
        return HexRound(new Axial(q, r));
    }

    public static Axial HexRound(Axial hex)
    {
        return CubeToAxial(CubeRound(AxialToCube(hex)));
    }

    public static Vector3 CubeRound(Vector3 cube)
    {
        float rx = Mathf.Round(cube.x);
        float ry = Mathf.Round(cube.y);
        float rz = Mathf.Round(cube.z);

        float xDiff = Mathf.Abs(rx - cube.x);
        float yDiff = Mathf.Abs(ry - cube.y);
        float zDiff = Mathf.Abs(rz - cube.z);

        if( xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if(yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3(rx, ry, rz);
    }
}
