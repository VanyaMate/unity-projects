using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WG.Point
{
    public class PointRoads : MonoBehaviour
    {
        private Dictionary<PointManager, Transform> _roads = new Dictionary<PointManager, Transform>();

        public void MakeRoadTo(Vector3 point)
        {
            GameObject road = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter));
            MeshFilter meshFilter = road.GetComponent<MeshFilter>();
            meshFilter.mesh = this._CreateRoadMesh(transform.position, point);
        }

        public void RemoveRoadTo(PointManager point)
        {

        }

        private Mesh _CreateRoadMesh(Vector3 start, Vector3 finish)
        {
            Mesh mesh           = new Mesh();

            float distance      = Vector3.Distance(start, finish);
            Vector3 direction   = (finish - start).normalized;
            int triangleHeight  = (int)Mathf.Floor(distance * 2); // .5f height

            Vector3[] vertices  = new Vector3[triangleHeight * 2];
            Vector2[] uv        = new Vector2[triangleHeight * 2];
            int[] triangles     = new int[triangleHeight * 3];

            int j               = 0;

            for (int i = 0; i < triangleHeight; i += 4)
            {
                vertices[i + 0] = new Vector3(direction.x - .5f, 0, direction.z * i / 2);
                vertices[i + 1] = new Vector3(direction.x + .5f, 0, direction.z * i / 2);
                vertices[i + 2] = new Vector3(direction.x - .5f, 0, direction.z * i);
                vertices[i + 3] = new Vector3(direction.x + .5f, 0, direction.z * i);

                uv[i + 0] = vertices[i + 0];
                uv[i + 1] = vertices[i + 1];
                uv[i + 2] = vertices[i + 2];
                uv[i + 3] = vertices[i + 3];

                triangles[j + 0] = i + 0;
                triangles[j + 1] = i + 2;
                triangles[j + 2] = i + 1;
                triangles[j + 3] = i + 1;
                triangles[j + 4] = i + 2;
                triangles[j + 5] = i + 3;
                j += 6;
            }

            mesh.vertices   = vertices;
            mesh.uv         = uv;
            mesh.triangles  = triangles;

            return mesh;
        }
    }
}
