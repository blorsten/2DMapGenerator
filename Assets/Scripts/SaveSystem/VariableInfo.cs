using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Struct used to save different types found by <see cref="MapDataSaver"/>
    /// Creator: MP
    /// </summary>
    public struct VariableInfo
    {
        private const BindingFlags VariablesBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public Type Type { get; private set; }
        public Dictionary<PropertyInfo, object> PropertiesInfos { get; private set; }
        public Dictionary<FieldInfo, object> FieldInfos { get; private set; }

        public VariableInfo(Type type) : this()
        {
            Type = type;
        }

        public void Load(MonoBehaviour monoBehaviour)
        {
            FieldInfos = Type.GetFields(VariablesBindingFlags)
                .Where(info => info.GetCustomAttributes(typeof(PersistentData), false).Any())
                .ToDictionary(info => info, info => info.GetValue(monoBehaviour));

            PropertiesInfos = Type.GetProperties(VariablesBindingFlags)
                .Where(info => info.GetCustomAttributes(typeof(PersistentData), false).Any())
                .ToDictionary(info => info, info => info.GetValue(monoBehaviour, null));
        }
    }
}