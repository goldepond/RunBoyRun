using System;
using UnityEngine;

internal class Ract
{
    private float v1;
    private float v2;
    private float v3;
    private float v4;

    public Ract(float v1, float v2, float v3, float v4)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
        this.v4 = v4;
    }

    public static implicit operator Camera(Ract v)
    {
        throw new NotImplementedException();
    }
}