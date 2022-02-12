/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  :\Assets\CLineActionEditor\ActionEngine\Framework\UObject\UDrawTool.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-24      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;

    public class UDrawTool : MonoBehaviour
    {

        private LineRenderer GetLineRenderer(Transform t)
        {
            LineRenderer lr = t.GetComponent<LineRenderer>();
            if (lr == null)
            {
                lr = t.gameObject.AddComponent<LineRenderer>();
            }
            lr.SetWidth(0.1f, 0.1f);

            return lr;
        }

        public void DrawLine(Transform t, Vector3 start, Vector3 end)
        {
            LineRenderer lr = GetLineRenderer(t);
            lr.SetVertexCount(2);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        public void DrawSector(Transform t, Vector3 center, float angle, float radius)
        {
            LineRenderer lr = GetLineRenderer(t);
            int pointAmount = 100;   
            float eachAngle = angle / pointAmount;
            Vector3 forward = t.forward;

            lr.SetVertexCount(pointAmount);
            lr.SetPosition(0, center);
            lr.SetPosition(pointAmount - 1, center);

            for (int i = 1; i < pointAmount - 1; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;
                lr.SetPosition(i, pos);
            }
        }

        public void DrawCircle(Transform t, Vector3 center, float radius)
        {
            LineRenderer lr = GetLineRenderer(t);
            int pointAmount = 100;   
            float eachAngle = 360f / pointAmount;
            Vector3 forward = t.forward;

            lr.SetVertexCount(pointAmount + 1);

            for (int i = 0; i <= pointAmount; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, eachAngle * i, 0f) * forward * radius + center;
                lr.SetPosition(i, pos);
            }
        }
 
        public void DrawRectangle(Transform t, Vector3 bottomMiddle, float length, float width)
        {
            LineRenderer lr = GetLineRenderer(t);
            lr.SetVertexCount(5);

            lr.SetPosition(0, bottomMiddle - t.right * (width / 2));
            lr.SetPosition(1, bottomMiddle - t.right * (width / 2) + t.forward * length);
            lr.SetPosition(2, bottomMiddle + t.right * (width / 2) + t.forward * length);
            lr.SetPosition(3, bottomMiddle + t.right * (width / 2));
            lr.SetPosition(4, bottomMiddle - t.right * (width / 2));
        }

        public void DrawRectangle2D(Transform t, float distance, float length, float width)
        {
            LineRenderer lr = GetLineRenderer(t);
            lr.SetVertexCount(5);

            if (MathTool.IsFacingRight(t))
            {
                Vector2 forwardMiddle = new Vector2(t.position.x + distance, t.position.y);
                lr.SetPosition(0, forwardMiddle + new Vector2(0, width / 2));
                lr.SetPosition(1, forwardMiddle + new Vector2(length, width / 2));
                lr.SetPosition(2, forwardMiddle + new Vector2(length, -width / 2));
                lr.SetPosition(3, forwardMiddle + new Vector2(0, -width / 2));
                lr.SetPosition(4, forwardMiddle + new Vector2(0, width / 2));
            }
            else
            {
                Vector2 forwardMiddle = new Vector2(t.position.x - distance, t.position.y);
                lr.SetPosition(0, forwardMiddle + new Vector2(0, width / 2));
                lr.SetPosition(1, forwardMiddle + new Vector2(-length, width / 2));
                lr.SetPosition(2, forwardMiddle + new Vector2(-length, -width / 2));
                lr.SetPosition(3, forwardMiddle + new Vector2(0, -width / 2));
                lr.SetPosition(4, forwardMiddle + new Vector2(0, width / 2));
            }
        }

        public GameObject go;
        public MeshFilter mf;
        public MeshRenderer mr;
        public Shader shader;
        private GameObject CreateMesh(List<Vector3> vertices)
        {
            int[] triangles;
            Mesh mesh = new Mesh();

            int triangleAmount = vertices.Count - 2;
            triangles = new int[3 * triangleAmount];
     
            for (int i = 0; i < triangleAmount; i++)
            {
                triangles[3 * i] = 0;    
                triangles[3 * i + 1] = i + 1;
                triangles[3 * i + 2] = i + 2;
            }

            if (go == null)
            {
                go = new GameObject("mesh");
                go.transform.position = new Vector3(0, 0.1f, 0);  
                mf = go.AddComponent<MeshFilter>();
                mr = go.AddComponent<MeshRenderer>();
                shader = Shader.Find("Unlit/Color");
                if (Application.isPlaying)
                    Destroy(go, 0.5f);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles;

            mf.mesh = mesh;
            if (!Application.isPlaying)
            {
                mr.sharedMaterial = new Material(shader);
                mr.sharedMaterial.color = Color.red;
            }
            else
            {
                mr.material.shader = shader;
                mr.material.color = Color.red;
            }

            return go;
        }
   
        public void DrawSectorSolid(Transform t, Vector3 center, float angle, float radius)
        {
            int pointAmount = 100;  
            float eachAngle = angle / pointAmount;
            Vector3 forward = t.forward;

            List<Vector3> vertices = new List<Vector3>();
            vertices.Add(center);

            for (int i = 1; i < pointAmount - 1; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, -angle / 2 + eachAngle * (i - 1), 0f) * forward * radius + center;
                vertices.Add(pos);
            }

            CreateMesh(vertices);
        }

        public void DrawCircleSolid(Transform t, Vector3 center, float radius)
        {
            int pointAmount = 100; 
            float eachAngle = 360f / pointAmount;
            Vector3 forward = t.forward;

            List<Vector3> vertices = new List<Vector3>();

            for (int i = 0; i <= pointAmount; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, eachAngle * i, 0f) * forward * radius + center;
                vertices.Add(pos);
            }

            CreateMesh(vertices);
        }

        public void DrawRectangleSolid(Transform t, Vector3 bottomMiddle, float length, float width)
        {
            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(bottomMiddle - t.right * (width / 2));
            vertices.Add(bottomMiddle - t.right * (width / 2) + t.forward * length);
            vertices.Add(bottomMiddle + t.right * (width / 2) + t.forward * length);
            vertices.Add(bottomMiddle + t.right * (width / 2));

            CreateMesh(vertices);
        }
  
        public void DrawRectangleSolid2D(Transform t, float distance, float length, float width)
        {
            List<Vector3> vertices = new List<Vector3>();

            if (MathTool.IsFacingRight(t))
            {
                Vector3 forwardMiddle = new Vector3(t.position.x + distance, t.position.y);
                vertices.Add(forwardMiddle + new Vector3(0, width / 2));
                vertices.Add(forwardMiddle + new Vector3(length, width / 2));
                vertices.Add(forwardMiddle + new Vector3(length, -width / 2));
                vertices.Add(forwardMiddle + new Vector3(0, -width / 2));
            }
            else
            { 
                Vector3 forwardMiddle = new Vector3(t.position.x - distance, t.position.y);
                vertices.Add(forwardMiddle + new Vector3(0, width / 2));
                vertices.Add(forwardMiddle + new Vector3(-length, width / 2));
                vertices.Add(forwardMiddle + new Vector3(-length, -width / 2));
                vertices.Add(forwardMiddle + new Vector3(0, -width / 2));
            }

            CreateMesh(vertices);
        }

        public class MathTool
        {

            public float piDivide180 = Mathf.PI / 180;

            public static bool IsFacingRight(Transform t)
            {
                if (t.localEulerAngles.y > 0) return false;
                else return true;
            }

            public static void FacingRight(Transform t)
            {
                t.localEulerAngles = new Vector3(0, 0, 0);
            }

            public static void FacingLeft(Transform t)
            {
                t.localEulerAngles = new Vector3(0, 180, 0);
            }

            public static Vector2 GetVector2(Vector3 a)
            {
                Vector2 posA = new Vector2(a.x, a.z);
                return posA;
            }

            public static float GetDistance(Transform a, Transform b)
            {
                Vector2 posA = GetVector2(a.position);
                Vector2 posB = GetVector2(b.position);
                return Vector2.Distance(posA, posB);
            }

            public static Vector2 GetDirection(Transform a, Transform b)
            {
                Vector2 posA = GetVector2(a.position);
                Vector2 posB = GetVector2(b.position);
                return posB - posA;
            }

        }
    }
}
