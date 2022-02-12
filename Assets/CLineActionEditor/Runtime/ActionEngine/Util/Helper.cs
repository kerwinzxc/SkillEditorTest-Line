/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Util\Helper.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : 
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2019-3-17      SuperCLine           Created
|
+-----------------------------------------------------------------------------*/

namespace SuperCLine.ActionEngine
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using UnityEngine;
    using System.Collections.Generic;

    public static partial class Helper
    {
        public static readonly float FLT_MAX = float.MaxValue;
        public static readonly float FLT_MIN = float.MinValue;
        public static readonly float FLT_EPSILON = 0.0001f;
        public static readonly float FLT_COS5 = 0.99619469809174553229501040247389f;
        public static readonly float FLT_COS10 = 0.98480775301220805936674302458952f;
        public static readonly float FLT_COS15 = 0.9659258262890682867497431997289f;
        public static readonly float FLT_COS30 = 0.86602540378443864676372317075294f;
        public static readonly float FLT_COS45 = 0.70710678118654752440084436210485f;
        public static readonly float FLT_COS60 = 0.5f;

        public static readonly Vector2 Vec2Zero = Vector2.zero;
        public static readonly Vector3 Vec3Zero = Vector3.zero;

        public static readonly float S2MS = 1000f;
        public static readonly float MS2S = 1f / S2MS;

        public static readonly string Letter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static Transform Find(Transform tf, string name, bool includeInactive = false)
        {
            Transform[] childTF = tf.GetComponentsInChildren<Transform>(includeInactive);

            for (int i = 0; i < childTF.Length; ++i)
            {
                if (childTF[i].name == name)
                    return childTF[i];
            }

            return null;
        }

        public static GameObject AddChildScale(GameObject child, GameObject parent)
        {
            child.transform.parent = parent.transform;
            return child;
        }

        public static Transform AddChildScale(Transform child, Transform parent)
        {
            child.parent = parent;
            return child;
        }

        public static GameObject AddChild(GameObject child, GameObject parent)
        {
            Vector3 localScale = child.transform.localScale;
            Vector3 localPositon = child.transform.localPosition;
            Quaternion localRotation = child.transform.localRotation;

            //child.transform.parent = parent.transform;
            child.transform.SetParent(parent.transform);

            child.transform.localScale = localScale;
            child.transform.localPosition = localPositon;
            child.transform.localRotation = localRotation;

            return child;
        }

        public static Transform AddChild(Transform child, Transform parent)
        {
            Vector3 localScale = child.localScale;
            Vector3 localPosition = child.localPosition;
            Quaternion localRotation = child.localRotation;

            child.parent = parent;

            child.localScale = localScale;
            child.localPosition = localPosition;
            child.localRotation = localRotation;

            return child;
        }

        // math
        public static double Clamp(double value, double min, double max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static int Clamp(int value, int min, int max)
        {
            return value < min ? min : (value > max ? max : value);
        }

        public static Quaternion LookRotation(Vector3 dir)
        {
            if (dir.Equals(Vector3.zero))
            {
                return Quaternion.identity;
            }
            else
            {
                return Quaternion.LookRotation(dir);
            }
        }

        public static void Rotate(ref float x, ref float z, float angle, bool degree)
        {
            float rad = degree ? angle * Mathf.Deg2Rad : angle;
            float num1 = Mathf.Sin(rad);
            float num2 = Mathf.Cos(rad);
            float num3 = num1 * z + num2 * x;
            float num4 = num2 * z - num1 * x;
            x = num3;
            z = num4;
        }

        public static float DistanceSqr(Vector3 a, Vector3 b, bool xoz = false)
        {
            Vector3 dir = b - a;

            dir.y = (xoz ? 0 : dir.y);

            return dir.sqrMagnitude;
        }

        public static float DistanceSqr(float a)
        {
            return a * a;
        }

        public static float Distance(Vector3 a, Vector3 b, bool xoz = false)
        {
            Vector3 dir = b - a;

            dir.y = (xoz ? 0 : dir.y);

            return dir.magnitude;
        }

        public static float Distance(Unit u1, Unit u2)
        {
            return Vector3.Distance(u1.Position, u2.Position) - u1.Radius - u2.Radius;
        }

        public static float DistanceClamp(Unit u1, Unit u2)
        {
            float dist = Vector3.Distance(u1.Position, u2.Position) - u1.Radius - u2.Radius;
            return Mathf.Clamp(dist, 0f, dist);
        }

        public static Vector3 ConvertNavMeshPoint(Vector3 original)
        {
            Vector3 pos = Vector3.zero;
            bool isFind = false;
            float area = 2;
            UnityEngine.AI.NavMeshHit hit;
            while (!isFind)
            {
                if (UnityEngine.AI.NavMesh.SamplePosition(original, out hit, area, UnityEngine.AI.NavMesh.AllAreas))
                {
                    pos = hit.position;
                    isFind = true;
                }
                else
                {
                    area *= 2;
                }
            }
            return pos;
        }

        // file
        public static bool FileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static void CreateDirectory(string filePath, bool del = false)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            else
            {
                if (del)
                {
                    Directory.Delete(filePath);
                    Directory.CreateDirectory(filePath);
                }
            }
        }

        public static string GetFileMD5(string filePath)
        {
            if (!FileExist(filePath))
                return string.Empty;

            using (Stream stream = File.OpenRead(filePath))
            {
                MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider();
                byte[] md5Buffer = crypto.ComputeHash(stream);

                stream.Close();

                string md5 = BitConverter.ToString(md5Buffer);
                return md5.Replace("-", "").ToUpper();
            }
        }

        public static long GetFileSize(string filePath)
        {
            long size = 0;
            if (FileExist(filePath))
            {
                using (Stream stream = File.OpenRead(filePath))
                {
                    size = stream.Length;
                    stream.Close();
                }
            }

            return size;
        }

        public static string ReadText(string filePath)
        {
            if (FileExist(filePath))
                return File.ReadAllText(filePath);

            return string.Empty;
        }

        public static void WriteText(string filePath, string text)
        {
            CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, text);
        }

        public static byte[] ReadBytes(string filePath)
        {
            if (FileExist(filePath))
                return File.ReadAllBytes(filePath);

            return null;
        }

        public static void WriteBytes(string filePath, byte[] data)
        {
            CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllBytes(filePath, data);
        }

        public static void CopyFile(string srcPath, string destPath)
        {
            if (!FileExist(srcPath))
                return;

            CreateDirectory(Path.GetDirectoryName(destPath));
            File.Copy(srcPath, destPath, true);
        }

        public static void MoveFile(string srcPath, string destPath)
        {
            if (srcPath == destPath)
                return;

            if (!FileExist(srcPath))
                return;

            DeleteFile(destPath);

            CreateDirectory(Path.GetDirectoryName(destPath));
            File.Move(srcPath, destPath);
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void CopyDirectory(string srcPath, string destPath)
        {
            if (!Directory.Exists(srcPath))
                return;

            CreateDirectory(destPath, true);

            string[] dirs = Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories);
            for (int i=0; i<dirs.Length; ++i)
            {
                string name = dirs[i].Substring(srcPath.Length);
                CreateDirectory(destPath + name, true);
            }
            for (int i=0; i<dirs.Length; ++i)
            {
                string name = files[i].Substring(srcPath.Length);
                File.Copy(files[i], destPath + name, true);
            }
        }

        public static void MoveDirectory(string srcPath, string destPath)
        {
            if (srcPath == destPath)
                return;

            if (!Directory.Exists(srcPath))
                return;

            CreateDirectory(destPath, true);

            string[] dirs = Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories);
            string[] files = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirs.Length; ++i)
            {
                string name = dirs[i].Substring(srcPath.Length);
                CreateDirectory(destPath + name, true);
            }
            for (int i = 0; i < dirs.Length; ++i)
            {
                string name = files[i].Substring(srcPath.Length);
                File.Move(files[i], destPath + name);
            }

            Directory.Delete(srcPath, true);
        }

        public static string NormalizePath(string path)
        {
            return path.Replace('\\', '/');
        }

        public static string NormalizeDirectory(string path)
        {
            path = NormalizePath(path);
            path.TrimEnd(new char[] { '/' });

            return path;
        }

        // coding
        public static int GetKeyNumber()
        {
            return GameConfig.CodingKey.ToCharArray()[0];
        }

        public static byte[] Coding(byte[] bytes, int offset = 0)
        {
            if (null == bytes || bytes.Length == 0)
            {
                return bytes;
            }

            int length = bytes.Length;
            int code = length % 43 + GetKeyNumber();
            for (int i = offset; i < length; i += 2)
            {
                bytes[i] ^= (byte)(i % 100 + code);
            }
            return bytes;
        }

        public static string BytesToString(byte[] bytes)
        {
            if (null == bytes || bytes.Length == 0)
                return string.Empty;

            char[] chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                chars[i] = (char)bytes[i];
            }

            return new string(chars);
        }

        public static byte[] StringToBytes(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            byte[] bytes = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = (byte)s[i];
            }

            return bytes;
        }

        public static string BytesToUtf8String(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static byte[] Utf8StringToBytes(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        public static Dictionary<string, string> ParseConfig(string content)
        {
            Dictionary<string, string> cfg = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(content))
            {
                string[] lines = content.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; ++i)
                {
                    string[] kv = lines[i].Split(new string[] { "=" }, 2, StringSplitOptions.None);
                    if (kv.Length == 2)
                    {
                        cfg[kv[0].Trim()] = kv[1].Trim();
                    }
                }
            }

            return cfg;
        }

        // format second time
        public static string FormatTime(float time)
        {
            int min = (int)Mathf.Floor(time / 60f);
            int second = (int)Mathf.Repeat(time, 60f);
            int milisecond = (int)Mathf.Repeat(time * 1000f, 1000f);

            StringBuilder sb = new StringBuilder();

            if (min < 10) sb.Append("0");
            sb.AppendFormat("{0}", min);
            sb.Append(":");

            if (second < 10) sb.Append("0");
            sb.AppendFormat("{0}", second);
            sb.Append(":");

            if (milisecond < 10) sb.Append("00");
            else if (milisecond < 100) sb.Append("0");
            sb.AppendFormat("{0}", milisecond);

            return sb.ToString();
        }


        // search
        public static Unit[] Search(Vector3 pos, float radius, int layerMask, Transform[] exclude, ECampType selfCampType, bool closed, Vector3 direction, float fanAngle = -1, bool degree = true)
        {
            float fSqrDist = Helper.FLT_MAX;
            List<Unit> finder = new List<Unit>();
            if (radius > 0)
            {
                Collider[] colliders = Physics.OverlapSphere(pos, radius, layerMask);
                for (int i = 0; i < colliders.Length; ++i)
                {
                    bool bExclude = false;
                    if (exclude != null)
                    {
                        for (int k = 0; k < exclude.Length; ++k)
                        {
                            if (colliders[i].transform == exclude[k])
                            {
                                bExclude = true;
                                break;
                            }
                        }
                    }

                    if (bExclude) continue;

                    Vector3 vDist = colliders[i].transform.position - pos;
                    vDist.y = 0;
                    float rad = (degree ? fanAngle * Mathf.Deg2Rad : fanAngle);
                    if ((direction != Vector3.zero) && (fanAngle > 0) && Vector3.Dot(vDist.normalized, direction.normalized) < Mathf.Cos(rad * 0.5f))
                        continue;

                    UUnit uunit = colliders[i].transform.GetComponent<UUnit>();
                    ECampType targetCamp = uunit.Owner.Camp;
                    if (targetCamp != selfCampType && !uunit.Owner.IsDead)
                    {
                        if (closed)
                        {
                            if (vDist.sqrMagnitude < fSqrDist)
                            {
                                fSqrDist = vDist.sqrMagnitude;

                                finder.Clear();
                                finder.Add(uunit.Owner);
                            }
                        }
                        else if (!finder.Contains(uunit.Owner))
                        {
                            finder.Add(uunit.Owner);
                        }
                    }
                }
            }
            return finder.ToArray();
        }

        public static Collider[] Search(Vector3 pos, float radius, int layerMask, Transform[] exclude, bool closed, Vector3 direction, float fanAngle = -1, bool degree = true)
        {
            float fSqrDist = Helper.FLT_MAX;
            List<Collider> finder = new List<Collider>();
            if (radius > 0)
            {
                Collider[] colliders = Physics.OverlapSphere(pos, radius, layerMask);
                for (int i = 0; i < colliders.Length; ++i)
                {
                    bool bExclude = false;
                    for (int k = 0; k < exclude.Length; ++k)
                    {
                        if (colliders[i].transform == exclude[k])
                        {
                            bExclude = true;
                            break;
                        }
                    }
                    if (bExclude) continue;

                    Vector3 vDist = colliders[i].transform.position - pos;
                    vDist.y = 0;
                    float rad = (degree ? fanAngle * Mathf.Deg2Rad : fanAngle);
                    if ((direction != Vector3.zero) && (fanAngle > 0) && Vector3.Dot(vDist.normalized, direction.normalized) < Mathf.Cos(rad * 0.5f))
                        continue;

                    if (closed)
                    {
                        if (vDist.sqrMagnitude < fSqrDist)
                        {
                            fSqrDist = vDist.sqrMagnitude;

                            finder.Clear();
                            finder.Add(colliders[i]);
                        }
                    }
                    else
                    {
                        finder.Add(colliders[i]);
                    }

                }
            }
            return finder.ToArray();
        }

        public static string RandomString(int n)
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i<n; ++i)
            {
                int index = UnityEngine.Random.Range(0, Letter.Length);
                sb.Append(Letter[index]);
            }

            return sb.ToString();
        }

        public static string NonceStr(int n = 32)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:d19}{1}", System.DateTime.Now.Ticks, RandomString(n - 19));

            return sb.ToString();
        }

    }


}
