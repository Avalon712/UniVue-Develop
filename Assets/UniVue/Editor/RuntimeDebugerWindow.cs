﻿using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UniVue.Evt;
using UniVue.Evt.Evts;
using UniVue.Model;
using UniVue.Utils;
using UniVue.ViewModel;
using UniVue.ViewModel.Models;

namespace UniVue.Editor
{
    internal sealed class RuntimeDebugerWindow : EditorWindow
    {
        #region 字段
        private GUIContent _refresh_icon;

        private Dictionary<string, List<UIBundle>> _views;
        private List<UIEvent> _events;
        private List<EventCall> _calls;
        private List<AutowireInfo> _autowires;
        private List<IEntityMapper> _mappers;

        private DebugContent _debugContent;
        private IBindableModel _currentDrawModel;
        private List<PropertyRecorder> _values;
        private List<IBindableModel> _models;
        private Dictionary<object, bool> _foldouts;
        private bool _rebuildModels = true;

        private Vector2 _pos;
        private Vector2 _pos2;
        private Vector2 _pos3;

        private const int TIME = 100;
        private int _timer = TIME;
        #endregion

        [MenuItem("UniVue/RuntimeDebuger")]
        public static void OpenEditorWindow()
        {
            var window = GetWindow<RuntimeDebugerWindow>("UniVue运行时调试器");
            
            window.position = new Rect(320, 240, 930, 500);
            window._values = new List<PropertyRecorder>();
            window._models = new List<IBindableModel>();
            window._foldouts = new Dictionary<object, bool>();
            window._refresh_icon = EditorGUIUtility.IconContent("d_Refresh");

            EditorApplication.playModeStateChanged += (mode) =>
            {
                if (mode == PlayModeStateChange.ExitingPlayMode)
                {
                    window._models.Clear();
                    window._values.Clear();
                    window._timer = TIME;
                    window._rebuildModels = true;
                    window._views = null;
                    window._currentDrawModel = null;
                }
            };
            window.Show();
        }

        private void GetRuntimeData()
        {
            if(_timer >= TIME)
            {
                _timer = 0;

                VMTable table = Vue.Updater.Table;
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                _views = table.GetType().GetField("_views", flags).GetValue(table) as Dictionary<string, List<UIBundle>>;

                EventManager eventManager = Vue.Event;
                _calls = eventManager.GetType().GetField("_calls", flags).GetValue(eventManager) as List<EventCall>;
                _autowires = eventManager.GetType().GetField("_autowires", flags).GetValue(eventManager) as List<AutowireInfo>;
                _mappers = eventManager.GetType().GetField("_mappers", flags).GetValue(eventManager) as List<IEntityMapper>;
                _events = eventManager.GetType().GetField("_events", flags).GetValue(eventManager) as List<UIEvent>;
            }
            _timer++;
        }

        private void OnGUI()
        {
            if (!Application.isPlaying || Vue.Router == null) return;
            GetRuntimeData();
            Draw_TopStates();
            Draw_TopToolbar();
            Draw_DebugContent();
        }

        private void Draw_TopStates()
        {
            string info = $"Views: {Vue.Router.ViewCount}     UIBundles: {Vue.Updater.Table.BundleCount}     PropertyUIs: {Vue.Updater.Table.PropertyUICount}     UpdateCache-Models: {Vue.Updater.Table.UpdateCacheModelCount}     Realtime-Models: {_models.Count}";
            GUILayout.Label(info); 
        }

        private void Draw_TopToolbar()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Debug - 模型绑定"))
            {
                _debugContent = DebugContent.Model;
            }
            if (GUILayout.Button("Debug - 事件绑定"))
            {
                _debugContent = DebugContent.Event;
            }
            GUILayout.EndHorizontal();
            Draw_Horizontal_Line(Color.black);
        }

        private void Draw_DebugContent()
        {
            switch (_debugContent)
            {
                case DebugContent.Model:
                    Draw_DebugContent_Model();
                    break;
                case DebugContent.Event:
                    Draw_DebugContent_Event();
                    break;
            }
        }

        #region 绘制模型绑定
        private void Draw_DebugContent_Model()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.Label($"Models [count={_models.Count}]");
            _pos = GUILayout.BeginScrollView(_pos, GUILayout.Width(200));
            Draw_Models(_views);
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            ////分割线
            GUILayout.Box(string.Empty, GUILayout.Width(5), GUILayout.ExpandHeight(true));

            if (_currentDrawModel != null)
            {
                GUILayout.BeginVertical(GUILayout.Width(320));
                GUILayout.Label($"Model: 【{GetModelName(_currentDrawModel) + "-" + _currentDrawModel.GetHashCode().ToString("X")}】");
                GUILayout.Label($"ModelType: 【{GetTypeFullName()}】");
                _pos2 = GUILayout.BeginScrollView(_pos2);
                Draw_Model_Properties();
                GUILayout.EndScrollView();
                GUILayout.EndVertical();

                ////分割线
                GUILayout.Box(string.Empty, GUILayout.Width(5), GUILayout.ExpandHeight(true));

                _pos3 = GUILayout.BeginScrollView(_pos3, GUILayout.MinWidth(380));
                Draw_Model_Bundles(_views);
                GUILayout.EndScrollView();
            }

            GUILayout.EndHorizontal();
        }

        private void Draw_Models(Dictionary<string, List<UIBundle>> views)
        {
            if (_rebuildModels)
            {
                _models.Clear();
                foreach (var bundles in views.Values)
                {
                    for (int i = 0; i < bundles.Count; i++)
                    {
                        if (bundles[i].active && !_models.Contains(bundles[i].Model))
                            _models.Add(bundles[i].Model);
                    }
                }
                _rebuildModels = false;
            }
            for (int i = 0; i < _models.Count; i++)
            {
                if (GUILayout.Button(GetModelName(_models[i]) + "-" + _models[i].GetHashCode().ToString("X")))
                {
                    _values.Clear();
                    _foldouts.Clear();
                    _rebuildModels = true;
                    _currentDrawModel = _models[i];
                }
            }
        }

        private void Draw_Model_Bundles(Dictionary<string, List<UIBundle>> views)
        {
            foreach (var viewName in views.Keys)
            {
                if (Draw_UIBundles(viewName, views[viewName]))
                {
                    Draw_Horizontal_Line(Color.white);
                    GUILayout.Space(5);
                }
            }
        }

        private void Draw_Model_Properties()
        {
            if(_values.Count == 0)
            {
                using (var it = GetProperties().GetEnumerator())
                {
                    while (it.MoveNext())
                    {
                        _values.Add(it.Current);
                    }
                }
            }
            for (int i = 0; i < _values.Count; i++)
            {
                PropertyRecorder record = _values[i];
                Draw_Horizontal_Line(Color.white);
                switch (record.BindType)
                {
                    case BindableType.Enum:
                        Draw_Property_Enum(record);
                        break;
                    case BindableType.Bool:
                        Draw_Property_Bool(record);
                        break;
                    case BindableType.Float:
                        Draw_Property_Float(record);
                        break;
                    case BindableType.Int:
                        Draw_Property_Int(record);
                        break;
                    case BindableType.String:
                        Draw_Property_String(record);
                        break;
                    case BindableType.Sprite:
                        Draw_Property_Sprite(record);
                        break;
                    case BindableType.ListEnum:
                        Draw_Property_ListEnum(record);
                        break;
                    case BindableType.ListBool:
                        Draw_Property_ListBool(record);
                        break;
                    case BindableType.ListFloat:
                        Draw_Property_ListFloat(record);
                        break;
                    case BindableType.ListInt:
                        Draw_Property_ListInt(record);
                        break;
                    case BindableType.ListString:
                        Draw_Property_ListString(record);
                        break;
                    case BindableType.ListSprite:
                        Draw_Property_ListSprite(record);
                        break;
                }
            }
            Draw_Horizontal_Line(Color.white);
        }

        #region 绘制属性

        private void Draw_Property_Int(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【Int】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            int propertyValue = (int)record.PropertyValue;
            record.PropertyValue = EditorGUILayout.IntField(propertyValue);
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_Float(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【Float】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            float propertyValue = (float)record.PropertyValue;
            record.PropertyValue = EditorGUILayout.FloatField(propertyValue);
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_Enum(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label($"PropertyType: 【{record.Property.PropertyType.Name}】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            object propertyValue = record.PropertyValue;
            if(!ReflectionUtil.HasFlags(record.Property.PropertyType))
                record.PropertyValue = EditorGUILayout.EnumPopup((Enum)propertyValue);
            else
                record.PropertyValue = EditorGUILayout.EnumFlagsField((Enum)propertyValue);
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_String(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【String】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            string propertyValue = record.PropertyValue as string;
            record.PropertyValue = EditorGUILayout.TextField(propertyValue); 
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_Bool(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【Bool】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            bool propertyValue = (bool)record.PropertyValue;
            record.PropertyValue = EditorGUILayout.Toggle(propertyValue); 
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_Sprite(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【Sprite】");
            GUILayout.BeginHorizontal();
            GUILayout.Label("PropertyValue: ");
            Sprite propertyValue = record.PropertyValue as Sprite;
            record.PropertyValue = EditorGUILayout.ObjectField(propertyValue, record.Property.PropertyType, true);
            if (GUILayout.Button(_refresh_icon))
            {
                record.PropertyValue = record.GetPropertyNewestValue();
            }
            if (GUILayout.Button("Apply"))
            {
                record.UpdateValue();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListInt(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【List<Int>】");
            List<int> propertyValue = record.PropertyValue as List<int>;
            if (propertyValue == null) return;
            for (int i = 0; i < propertyValue.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                propertyValue[i] = EditorGUILayout.IntField(propertyValue[i]);
                if (GUILayout.Button("Delete"))
                {
                    propertyValue.RemoveAt(i); 
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            int v = (int)record.Temp;
            record.Temp = EditorGUILayout.IntField(v);
            if (GUILayout.Button("Add"))
            {
                propertyValue.Add((int)record.Temp);
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyUIUpdate(record.PropertyName, propertyValue);
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListFloat(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【List<Float>】");
            List<float> propertyValue = record.PropertyValue as List<float>;
            if (propertyValue == null) return;
            for (int i = 0; i < propertyValue.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                propertyValue[i] = EditorGUILayout.FloatField(propertyValue[i]);
                if (GUILayout.Button("Delete"))
                {
                    propertyValue.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal(); 
            float v = (float)record.Temp;
            record.Temp = EditorGUILayout.FloatField(v);
            if (GUILayout.Button("Add"))
            {
                propertyValue.Add((float)record.Temp);
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyUIUpdate(record.PropertyName, propertyValue);
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListString(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【List<String>】");
            List<string> propertyValue = record.PropertyValue as List<string>;
            if (propertyValue == null) return;
            for (int i = 0; i < propertyValue.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                propertyValue[i] = EditorGUILayout.TextField(propertyValue[i]);
                if (GUILayout.Button("Delete"))
                {
                    propertyValue.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            string v = (string)record.Temp;
            record.Temp = EditorGUILayout.TextField(v);
            if (GUILayout.Button("Add"))
            {
                propertyValue.Add((string)record.Temp);
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyUIUpdate(record.PropertyName, propertyValue);
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListEnum(PropertyRecorder record)
        {
            Type enumType = record.Property.PropertyType.GetGenericArguments()[0];
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label($"PropertyType: 【List<{enumType.Name}>】");
            
            object propertyValue = record.PropertyValue;
            if (propertyValue == null) return;

            Type type = propertyValue.GetType();
            int count = (int)type.GetProperty("Count").GetValue(propertyValue);
            bool isFlagsEnum = ReflectionUtil.HasFlags(enumType);

            PropertyInfo accessor =  type.GetProperty("Item", BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < count; i++)
            {
                object[] args = new object[1] { i };
                object value = accessor.GetValue(propertyValue, args);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                if (!isFlagsEnum)
                    accessor.SetValue(propertyValue, EditorGUILayout.EnumPopup((Enum)value), args);
                else
                    accessor.SetValue(propertyValue, EditorGUILayout.EnumFlagsField((Enum)value), args);
                if (GUILayout.Button("Delete"))
                {
                    type.GetMethod("RemoveAt", BindingFlags.Instance | BindingFlags.Public).Invoke(propertyValue, new object[1] { i });
                    count = (int)type.GetProperty("Count").GetValue(propertyValue);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            if (!isFlagsEnum)
                record.Temp = EditorGUILayout.EnumPopup((Enum)record.Temp);
            else
                record.Temp = EditorGUILayout.EnumFlagsField((Enum)record.Temp);
            if (GUILayout.Button("Add"))
            {
                type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public).Invoke(propertyValue, new object[1] { record.Temp });
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyAll();
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListSprite(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【List<Sprite>】");
            List<Sprite> propertyValue = record.PropertyValue as List<Sprite>; 
            if (propertyValue == null) return;
            for (int i = 0; i < propertyValue.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                propertyValue[i] = EditorGUILayout.ObjectField(propertyValue[i], typeof(Sprite), true) as Sprite;
                if (GUILayout.Button("Delete"))
                {
                    propertyValue.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            record.Temp = EditorGUILayout.ObjectField(record.Temp as Sprite, typeof(Sprite), true) as Sprite;
            if (GUILayout.Button("Add"))
            {
                propertyValue.Add(record.Temp as Sprite);
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyUIUpdate(record.PropertyName, propertyValue);
            }
            GUILayout.EndHorizontal();
        }

        private void Draw_Property_ListBool(PropertyRecorder record)
        {
            GUILayout.Label($"PropertyName: 【{record.PropertyName}】");
            GUILayout.Label("PropertyType: 【List<Bool>】");
            List<bool> propertyValue = record.PropertyValue as List<bool>;
            if (propertyValue == null) return;
            for (int i = 0; i < propertyValue.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Index[{i}]");
                propertyValue[i] = EditorGUILayout.Toggle(propertyValue[i]);
                if (GUILayout.Button("Delete"))
                {
                    propertyValue.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            bool v = (bool)record.Temp;
            record.Temp = EditorGUILayout.Toggle(v);
            if (GUILayout.Button("Add"))
            {
                propertyValue.Add((bool)record.Temp);
            }
            if (GUILayout.Button("Update UI"))
            {
                _currentDrawModel.NotifyUIUpdate(record.PropertyName, propertyValue);
            }
            GUILayout.EndHorizontal();
        }

        #endregion

        private bool Draw_UIBundles(string viewName, List<UIBundle> bundles)
        {
            int count = 0;
            for (int i = 0; i < bundles.Count; i++)
            {
                if (bundles[i].active && ReferenceEquals(bundles[i].Model, _currentDrawModel))
                {
                    count++;
                }
            }
            if (count == 0) { return false; }

            _foldouts.TryAdd(viewName, false);
            _foldouts[viewName] = EditorGUILayout.Foldout(_foldouts[viewName], viewName);
            if (_foldouts[viewName])
            {
                for (int i = 0; i < bundles.Count; i++)
                {
                    if (bundles[i].active && ReferenceEquals(bundles[i].Model, _currentDrawModel))
                    {
                        count++;
                        Draw_UIBundle(bundles[i]);
                    }
                }
            }
            return true;
        }

        private void Draw_UIBundle(UIBundle bundle)
        {
            Draw_Horizontal_Line(Color.black);
            List<PropertyUI> propertyUIs = bundle.ProertyUIs;
            for (int i = 0; i < propertyUIs.Count; i++)
            {
                Draw_PropertyUI(propertyUIs[i]);
            }
            GUILayout.Space(5);
        }

        private void Draw_PropertyUI(PropertyUI propertyUI)
        {
            _foldouts.TryAdd(propertyUI, false);
            _foldouts[propertyUI] = EditorGUILayout.Foldout(_foldouts[propertyUI], $"{propertyUI.PropertyName} - {GetPropertyUITypeName(propertyUI)}");
            if (_foldouts[propertyUI])
            {
                using (var it = propertyUI.GetUI<Component>().GetEnumerator())
                {
                    while (it.MoveNext())
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField(it.Current, it.Current.GetType(), true);
                        GUILayout.Space(5);
                        object value = GetUIValue(it.Current);
                        if (value is Sprite sprite)
                            EditorGUILayout.ObjectField(sprite, sprite.GetType(), true);
                        else
                            GUILayout.Label(value as string);
                        GUILayout.EndHorizontal();
                    }
                }
            }
            Draw_Horizontal_Line(Color.black);
        }

        #endregion

        private void Draw_DebugContent_Event()
        {

        }

        #region 工具函数

        private object GetUIValue(Component current)
        {
            if (current is Toggle toggle)
                return toggle.isOn ? "√" : "×";
            else if (current is TMP_Text text)
                return text.text;
            else if (current is Image image)
                return image.sprite;
            else if (current is TMP_Dropdown dropdown)
                return dropdown.options[dropdown.value].text;
            else if (current is Slider slider)
                return slider.value.ToString();
            else if (current is TMP_InputField inputField)
                return inputField.text;

            return string.Empty;
        }

        private void Draw_Horizontal_Line(Color color)
        {
            GUILayout.Space(4);

            Rect rect = GUILayoutUtility.GetRect(10, 1, GUILayout.ExpandWidth(true));
            rect.height = 1;
            rect.xMin = 0;
            rect.xMax = EditorGUIUtility.currentViewWidth;
            EditorGUI.DrawRect(rect, color);
            GUILayout.Space(4);
        }

        private IEnumerable<PropertyRecorder> GetProperties()
        {
            Type modelType = _currentDrawModel.GetType();
            int _typeFlag;
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(AtomModel<>))
                _typeFlag = 1;
            else if (modelType == typeof(GroupModel))
                _typeFlag = 2;
            else
                _typeFlag = 3;

            switch (_typeFlag)
            {
                case 1:
                    {
                        PropertyInfo propertyInfo = modelType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
                        string propertyName = modelType.GetProperty("PropertyName", BindingFlags.Instance | BindingFlags.Public).GetValue(_currentDrawModel) as string;
                        BindableType bindType = ReflectionUtil.GetBindableType(propertyInfo.PropertyType);
                        if (bindType != BindableType.None)
                            yield return new PropertyRecorder(bindType,_currentDrawModel, propertyName, propertyInfo, propertyInfo.GetValue(_currentDrawModel));
                    }
                    break;
                case 2:
                    {
                        BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
                        List<INotifiableProperty> properties = modelType.GetField("_properties", flags).GetValue(_currentDrawModel) as List<INotifiableProperty>;

                        for (int i = 0; i < properties.Count; i++)
                        {
                            Type pType = properties[i].GetType();
                            string propertyName = pType.GetProperty("PropertyName", flags).GetValue(properties[i]) as string;
                            PropertyInfo propertyInfo = pType.GetProperty("Value", flags);
                            BindableType bindType = ReflectionUtil.GetBindableType(propertyInfo.PropertyType); 
                            if (bindType != BindableType.None)
                                yield return new PropertyRecorder(bindType, properties[i], propertyName, propertyInfo, propertyInfo.GetValue(properties[i]));
                        }
                    }
                    break;
                case 3:
                    {
                        PropertyInfo[] properties = modelType.GetProperties();
                        for (int i = 0; i < properties.Length; i++)
                        {
                            BindableType bindType = ReflectionUtil.GetBindableType(properties[i].PropertyType);
                            if (bindType != BindableType.None)
                                yield return new PropertyRecorder(bindType, _currentDrawModel, properties[i].Name, properties[i], properties[i].GetValue(_currentDrawModel));
                        }
                    }
                    break;
            }
        }

        private string GetTypeFullName()
        {
            Type modelType = _currentDrawModel.GetType();
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(AtomModel<>))
                return $"AtomModel<{modelType.GetGenericArguments()[0].Name}>";
            else
                return modelType.FullName;
        }

        private string GetPropertyUITypeName(PropertyUI propertyUI)
        {
            Type type = propertyUI.GetType();
            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericArguments()[0];
                return $"{type.GetGenericTypeDefinition().Name}<{genericType.Name}>";
            }
            else
                return type.Name;
        }

        private string GetModelName(IBindableModel model)
        {
            Type modelType = model.GetType();
            int _typeFlag;
            if (modelType.IsGenericType && modelType.GetGenericTypeDefinition() == typeof(AtomModel<>))
                _typeFlag = 1;
            else if (modelType == typeof(GroupModel))
                _typeFlag = 2;
            else
                _typeFlag = 3;

            if (_typeFlag < 3)
                return modelType.GetField("_modelName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(model) as string;
            else
                return modelType.Name;
        }

        #endregion
    }

    internal sealed class PropertyRecorder
    {
        public BindableType BindType { get; set; }

        private object Model { get; set; }

        public PropertyInfo Property { get; set; }

        public object PropertyValue { get; set; }

        public string PropertyName { get; set; }

        public object Temp { get; set; }

        public PropertyRecorder(BindableType bindType, object model, string propertyName, PropertyInfo property, object propertyValue)
        {
            Model = model;
            BindType = bindType;
            Property = property;
            PropertyValue = propertyValue;
            PropertyName = propertyName;
            InitTempValue();
        }

        private void InitTempValue()
        {
            switch (BindType)
            {
                case BindableType.Enum:
                case BindableType.Bool:
                case BindableType.Float:
                case BindableType.Int:
                case BindableType.String:
                case BindableType.Sprite:
                    Temp = PropertyValue;
                    break;
                case BindableType.ListEnum:
                    Type enumType = Property.PropertyType.GetGenericArguments()[0];
                    Temp = Enum.GetValues(enumType).GetValue(0);
                    break;
                case BindableType.ListBool:
                    Temp = false;
                    break;
                case BindableType.ListFloat:
                    Temp = 1f;
                    break;
                case BindableType.ListInt:
                    Temp = 1;
                    break;
                case BindableType.ListString:
                    Temp = string.Empty;
                    break;
                case BindableType.ListSprite:
                    break;

            }
        }

        public void UpdateValue()
        {
            Property.SetValue(Model, PropertyValue);
        }

        public object GetPropertyNewestValue()
        {
            return Property.GetValue(Model);
        }
    }

    internal enum DebugContent
    {
        None,
        Model,
        Event
    }
}
