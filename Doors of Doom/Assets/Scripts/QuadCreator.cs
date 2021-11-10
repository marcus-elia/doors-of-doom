using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is based on https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html

public class QuadCreator : MonoBehaviour
{
    private float width_;
    private float height_;
    private float u1_, v1_, u2_, v2_;
    private Material mat_;

    public void SetParameters(float width, float height, float u1, float v1, float u2, float v2, Material mat)
    {
        width_ = width;
        height_ = height;
        u1_ = u1; v1_ = v1; u2_ = u2; v2_ = v2;
        mat_ = mat;
    }

    public void CreateMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mat_;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(width_, 0, 0),
            new Vector3(0, height_, 0),
            new Vector3(width_, height_, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(u1_, v1_),
            new Vector2(u2_, v1_),
            new Vector2(u1_, v2_),
            new Vector2(u2_, v2_)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
}
