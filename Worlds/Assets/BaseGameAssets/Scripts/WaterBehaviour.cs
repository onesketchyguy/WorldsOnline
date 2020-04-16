using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World.Effects
{
    public class WaterBehaviour : MonoBehaviour
    {
        public new MeshFilter renderer;
        public new MeshCollider collider;
        private Mesh mesh;

        private Vector3[] vertices;
        private int[] triangles;

        [Header("Generation")]
        [Range(0.1f, 10)]
        public float resolution = 0.5f;

        [Range(1, 100)]
        public int xSize = 20;

        [Range(1, 100)]
        public int zSize = 20;

        [Range(0, .9f)]
        public float perlinNoiseScale = 0.3f;

        [Range(0, 2f)]
        public float noiseWeight = 1.5f;

        public float scrollSpeed = 1;

        private Vector2 scroll;

        public bool autoUpdate = false;

        private void Start()
        {
            StartCoroutine(GenerateWaves());
        }

        private void OnValidate()
        {
            if (autoUpdate)
                CreateShape();
        }

        private void VerifyMesh()
        {
            renderer.mesh = renderer.sharedMesh = mesh = new Mesh();
            if (collider != null) collider.sharedMesh = mesh;
        }

        private void CreateShape()
        {
            VerifyMesh();

            vertices = new Vector3[(xSize + 1) * (zSize + 1)];
            var halfX = (xSize * 0.5f);
            var halfZ = (zSize * 0.5f);
            for (int z = 0, i = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    var noiseLayer1 = Mathf.PerlinNoise((x + scroll.x) * perlinNoiseScale, (z + scroll.y) * perlinNoiseScale) * noiseWeight;
                    var noiseLayer2 = Mathf.PerlinNoise((z + scroll.y) * perlinNoiseScale, (x + scroll.x) * perlinNoiseScale) * noiseWeight;

                    var y = (-1 * resolution) + ((noiseLayer1 + noiseLayer2) * resolution);
                    vertices[i] = new Vector3(x - halfX, y, z - halfZ) * resolution;
                    i++;
                }
            }

            CalculateTris();

            UpdateMesh();
        }

        private void CalculateTris()
        {
            triangles = new int[xSize * zSize * 6];

            for (int z = 0, vert = 0, tris = 0; z < zSize; z++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    triangles[tris] = vert;
                    triangles[tris + 1] = vert + xSize + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + xSize + 1;
                    triangles[tris + 5] = vert + xSize + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }

        private void UpdateMesh()
        {
            if (collider != null)
                collider.enabled = false;
            mesh.Clear();
            mesh.SetVertices(vertices);
            mesh.triangles = (triangles);

            mesh.RecalculateNormals();
            if (collider != null)
                collider.enabled = true;
        }

        private IEnumerator GenerateWaves()
        {
            while (true)
            {
                yield return null;
                CreateShape();

                var s = Random.value * scrollSpeed * Time.deltaTime;

                scroll += Vector2.one * s;

                yield return null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(xSize * resolution, 1, zSize * resolution));
        }
    }
}