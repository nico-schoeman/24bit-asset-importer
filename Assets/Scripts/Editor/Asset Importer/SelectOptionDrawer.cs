// <copyright file="SelectOptionDrawer.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Custom property drawer for SelectOptionAttribute
    /// </summary>
    [CustomPropertyDrawer(typeof(SelectOptionAttribute))]
    public class SelectOptionDrawer : PropertyDrawer
    {
        /// <summary>
        /// Draw the property inside the given rectangle
        /// </summary>
        /// <param name="position">Rectangle to draw in</param>
        /// <param name="property">The property this PropertyDrawer is for</param>
        /// <param name="label">GUI label for the property</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // get the attribute
            SelectOptionAttribute selectOptions = attribute as SelectOptionAttribute;

            // Display the popup with the options defined in the attribute
            int index = System.Array.IndexOf(selectOptions.Options, property.intValue);

            string[] options = selectOptions.Options.Select(option => { return option.ToString(); }).ToArray();

            index = EditorGUI.Popup(position, index, options);

            // set the property int value. since this is only used for ints in this case
            if (property.propertyType == SerializedPropertyType.Integer)
            {
                property.intValue = (int)selectOptions.Options[index];
            }
            else
            {
                // throw a exception if its not a implemented property type
                throw new System.NotImplementedException();
            }
        }
    }
}
#endif