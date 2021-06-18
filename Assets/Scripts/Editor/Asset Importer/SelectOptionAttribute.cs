// <copyright file="SelectOptionAttribute.cs" company="Nico Schoeman">
// Copyright (c) Nico Schoeman. All rights reserved.
// </copyright>

#if (UNITY_EDITOR)
namespace EditorTools.AssetImporter
{
    using UnityEngine;

    /// <summary>
    /// Attribute to define a array of selectable system.object
    /// </summary>
    public class SelectOptionAttribute : PropertyAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectOptionAttribute" /> class.
        /// </summary>
        /// <param name="options">The options to use by the property drawer</param>
        public SelectOptionAttribute(object[] options)
        {
            this.Options = options;
        }

        /// <summary>
        /// Gets or sets the options for this attribute
        /// </summary>
        public object[] Options { get; set; }
    }
}
#endif