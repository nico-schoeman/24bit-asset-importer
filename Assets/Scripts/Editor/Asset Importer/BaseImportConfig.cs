using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

#if (UNITY_EDITOR)
namespace EditorTools
{
    public class BaseImportConfig : ScriptableObject
    {
        // Add a right-click context menu to run the retroactive process on/in the select folders (can also select a asset)
        [MenuItem("Assets/Reimport Assets in selected folders")]
        public static void Reimport()
        {
            AssetImporter.Run();
        }
    }

    /// <summary>
    /// Attribute to define a array of selectable system.object
    /// </summary>
    public class SelectOptionAttribute : PropertyAttribute
    {
        public object[] options;

        public SelectOptionAttribute(object[] options)
        {
            this.options = options;
        }
    }

    /// <summary>
    /// Custom property drawer for SelectOptionAttribute
    /// </summary>
    [CustomPropertyDrawer(typeof(SelectOptionAttribute))]
    public class SelectOptionDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // get the attribute
            SelectOptionAttribute selectOptions = attribute as SelectOptionAttribute;

            // Display the popup with the options defined in the attribute
            int index = EditorGUI.Popup(position, System.Array.IndexOf(selectOptions.options, property.intValue), selectOptions.options.Select(option => { return option.ToString(); }).ToArray());

            // set the property int value. since this is only used for ints in this case
            if (property.propertyType == SerializedPropertyType.Integer)
                property.intValue = (int)selectOptions.options[index];
            else
                throw new System.NotImplementedException(); // throw a exception if its not a implemented property type
        }
    }
}
#endif