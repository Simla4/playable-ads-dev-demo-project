using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;

public class PaintableObject : MonoSingleton<PaintableObject>
{
    [SerializeField] private P3dPaintableTexture paintableTexture;

    public P3dPaintableTexture PaintableTexture => paintableTexture;
}
