namespace SuperCLine.ActionEngine
{
    using System;
    using System.Collections.Generic;

    public static class Utility
    {
        /// <summary>
        /// 类型相关的实用函数。
        /// </summary>
        public static class Type
        {
            private static readonly string[] RuntimeAssemblyNames =
            {
                "SuperCLine.ActionEngine",
                "Assembly-CSharp",
            };

            private static readonly string[] RuntimeOrEditorAssemblyNames =
            {
                "SuperCLine.ActionEngine",
                "Assembly-CSharp",
                "SuperCLine.ActionEngine.Editor",
                "Assembly-CSharp-Editor",
            };

            /// <summary>
            /// 在运行时程序集中获取指定基类的所有子类的名称。
            /// </summary>
            /// <param name="typeBase">基类类型。</param>
            /// <returns>指定基类的所有子类的名称。</returns>
            public static string[] GetRuntimeTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, RuntimeAssemblyNames);
            }

            /// <summary>
            /// 在运行时或编辑器程序集中获取指定基类的所有子类的名称。
            /// </summary>
            /// <param name="typeBase">基类类型。</param>
            /// <returns>指定基类的所有子类的名称。</returns>
            public static string[] GetRuntimeOrEditorTypeNames(System.Type typeBase)
            {
                return GetTypeNames(typeBase, RuntimeOrEditorAssemblyNames);
            }

            private static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
            {
                List<string> typeNames = new List<string>();
                foreach (string assemblyName in assemblyNames)
                {
                    System.Reflection.Assembly assembly = null;
                    try
                    {
                        assembly = System.Reflection.Assembly.Load(assemblyName);
                    }
                    catch
                    {
                        continue;
                    }

                    if (assembly == null)
                    {
                        continue;
                    }

                    System.Type[] types = assembly.GetTypes();
                    foreach (System.Type type in types)
                    {
                        if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                        {
                            typeNames.Add(type.FullName);
                        }
                    }
                }

                typeNames.Sort();
                return typeNames.ToArray();
            }
        }

        public static class Assembly
        {
            private static readonly System.Reflection.Assembly[] s_Assemblies = (System.Reflection.Assembly[]) null;
            private static readonly Dictionary<string, System.Type> s_CachedTypes = new Dictionary<string, System.Type>((IEqualityComparer<string>) StringComparer.Ordinal);

            static Assembly() => Utility.Assembly.s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();

            /// <summary>获取已加载的程序集。</summary>
            /// <returns>已加载的程序集。</returns>
            public static System.Reflection.Assembly[] GetAssemblies() => Utility.Assembly.s_Assemblies;

            /// <summary>获取已加载的程序集中的所有类型。</summary>
            /// <returns>已加载的程序集中的所有类型。</returns>
            public static System.Type[] GetTypes()
            {
                List<System.Type> typeList = new List<System.Type>();
                foreach (System.Reflection.Assembly assembly in Utility.Assembly.s_Assemblies)
                    typeList.AddRange((IEnumerable<System.Type>) assembly.GetTypes());
                return typeList.ToArray();
            }

            /// <summary>获取已加载的程序集中的所有类型。</summary>
            /// <param name="results">已加载的程序集中的所有类型。</param>
            public static void GetTypes(List<System.Type> results)
            {
                if (results == null)
                {
                    UnityEngine.Debug.LogError("Results is invalid.");
                    return;
                }

                results.Clear();
                foreach (System.Reflection.Assembly assembly in Utility.Assembly.s_Assemblies)
                    results.AddRange((IEnumerable<System.Type>) assembly.GetTypes());
            }

            /// <summary>获取已加载的程序集中的指定类型。</summary>
            /// <param name="typeName">要获取的类型名。</param>
            /// <returns>已加载的程序集中的指定类型。</returns>
            public static System.Type GetType(string typeName)
            {
                if (string.IsNullOrEmpty(typeName))
                {
                    UnityEngine.Debug.LogError("Type name is invalid.");
                    return null;
                }

                System.Type type1 = (System.Type) null;
                if (Utility.Assembly.s_CachedTypes.TryGetValue(typeName, out type1))
                    return type1;
                System.Type type2 = System.Type.GetType(typeName);
                if (type2 != null)
                {
                    Utility.Assembly.s_CachedTypes.Add(typeName, type2);
                    return type2;
                }

                foreach (System.Reflection.Assembly assembly in Utility.Assembly.s_Assemblies)
                {
                    System.Type type3 = System.Type.GetType($"{typeName}, {assembly.FullName}");
                    if (type3 != null)
                    {
                        Utility.Assembly.s_CachedTypes.Add(typeName, type3);
                        return type3;
                    }
                }

                return (System.Type) null;
            }
        }
    }
}