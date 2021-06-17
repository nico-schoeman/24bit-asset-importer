using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class BaseImportConfig : ScriptableObject
{
    [MenuItem("Assets/Reimport Assets in selected folders")]
    public static void Reimport ()
    {
        AssetImporter.Run();
    }
}

public class SelectOptionAttribute : PropertyAttribute
{
    public object[] options;

    public SelectOptionAttribute(object[] options)
    {
        this.options = options;
    }
}

[CustomPropertyDrawer(typeof(SelectOptionAttribute))]
public class SelectOptionDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SelectOptionAttribute selectOptions = attribute as SelectOptionAttribute;

        Debug.Log($"{property.intValue} {System.Array.IndexOf(selectOptions.options, property)}");
        int index = EditorGUI.Popup(position, System.Array.IndexOf(selectOptions.options, property.intValue), selectOptions.options.Select(option => { return option.ToString(); }).ToArray());

        if (property.propertyType == SerializedPropertyType.Integer)
        property.intValue = (int)selectOptions.options[index];
    }
}