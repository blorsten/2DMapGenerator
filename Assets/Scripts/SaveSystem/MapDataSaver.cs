using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Used on maps to save its childs persistent data.
    /// Creator: MP
    /// </summary>
    public class MapDataSaver
    {
        private List<VariableInfo> _savedTypes;
        private readonly List<VariableData<FieldInfo>> _savedFieldInfos;
        private readonly List<VariableData<PropertyInfo>> _savedPropertyInfos;
        private readonly List<Guid> _dirtyIds;

        public Map Map { get; set; }
        public Guid MapId { get; set; }
        public int MapSeed { get; set; }
        public MapBlueprint MapBlueprint { get; set; }

        public MapDataSaver(Map map)
        {
            Map = map;

            MapId = map.ID;
            MapSeed = map.Seed;
            MapBlueprint = map.MapBlueprint;

            _savedFieldInfos = new List<VariableData<FieldInfo>>();
            _savedPropertyInfos = new List<VariableData<PropertyInfo>>();
            _dirtyIds = new List<Guid>();
        }

        private void Initialize()
        {
            _savedTypes = new List<VariableInfo>();

            var components = Map.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour monoBehaviour in components)
            {
                Type monoType = monoBehaviour.GetType();

                if (_savedTypes.All(info => info.Type != monoType))
                {
                    VariableInfo newVariableInfo = new VariableInfo(monoType);
                    newVariableInfo.Load(monoBehaviour);
                    _savedTypes.Add(newVariableInfo);
                }
            }
        }

        public void LoadPersistentData()
        {
            if (_savedTypes == null)
                Initialize();

            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
            {
                if (child.Id == Guid.Empty)
                {
                    child.Initialize(Map.Random);

                    if (_dirtyIds.Contains(child.Id))
                        child.IsDirty = true;
                }

                LoadPersistentData(child);
            }
        }

        public void SavePersistentData()
        {
            _savedPropertyInfos.Clear();
            _savedFieldInfos.Clear();
            _dirtyIds.Clear();

            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
            {
                SavePersistentData(child);
            }
        }

        private void SavePersistentData(DataIdentity identity)
        {
            foreach (VariableInfo persistentDataClass in _savedTypes)
            {
                Component comp = identity.GetComponent(persistentDataClass.Type);

                foreach (KeyValuePair<FieldInfo, object> fieldInfo in persistentDataClass.FieldInfos)
                {
                    var data = fieldInfo.Key.GetValue(comp);

                    if (data != null && !data.Equals(fieldInfo.Value))
                    {
                        _savedFieldInfos.Add(
                            new VariableData<FieldInfo>(persistentDataClass.Type, fieldInfo.Key, identity.Id, data));

                        _dirtyIds.Add(identity.Id);
                        identity.IsDirty = true;
                    }
                }

                foreach (KeyValuePair<PropertyInfo, object> propertyInfo in persistentDataClass.PropertiesInfos)
                {
                    var data = propertyInfo.Key.GetValue(comp, null);

                    if (data != null && !data.Equals(propertyInfo.Value))
                    {
                        _savedPropertyInfos.Add(
                            new VariableData<PropertyInfo>(persistentDataClass.Type, propertyInfo.Key, identity.Id, data));

                        _dirtyIds.Add(identity.Id);
                        identity.IsDirty = true;
                    }
                }
            }
        }

        private void LoadPersistentData(DataIdentity identity)
        {
            if (!identity.IsDirty)
                return;

            foreach (VariableData<FieldInfo> fieldCabinet in _savedFieldInfos.Where(cabinet => cabinet.Id == identity.Id))
            {
                fieldCabinet.Info.SetValue(identity.GetComponent(fieldCabinet.Type), fieldCabinet.Data);
            }

            foreach (VariableData<PropertyInfo> propertyCabinet in _savedPropertyInfos.Where(cabinet => cabinet.Id == identity.Id))
            {
                propertyCabinet.Info.SetValue(identity.GetComponent(propertyCabinet.Type), propertyCabinet.Data, null);
            }

            identity.IsDirty = false;
        }
    }
}