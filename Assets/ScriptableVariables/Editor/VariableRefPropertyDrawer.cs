﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(IntegerReference))]
[CustomPropertyDrawer(typeof(FloatReference))]
[CustomPropertyDrawer(typeof(BoolReference))]
[CustomPropertyDrawer(typeof(Vector3Reference))]
[CustomPropertyDrawer(typeof(ColorReference))]
public class VariableRefPropertyDrawer : PropertyDrawer
{
    /// <summary>
    /// Options to display in the popup to select constant or variable.
    /// </summary>
    private readonly string[] popupOptions =
        { "Use Variable", "Use Constant" };

    /// <summary> Cached style to use to draw the popup button. </summary>
    private GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (popupStyle == null)
        {
            popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
            popupStyle.imagePosition = ImagePosition.ImageOnly;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        SerializedProperty useVariable = property.FindPropertyRelative("useVariable");
        SerializedProperty constantValue = property.FindPropertyRelative("constant");
        SerializedProperty variable = property.FindPropertyRelative("variable");

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(buttonRect, useVariable.boolValue ? 0 : 1, popupOptions, popupStyle);

        useVariable.boolValue = result == 0;

        //var checkBoxRect = new Rect(position.x, position.y, 30, position.height);
        EditorGUI.PropertyField(position, useVariable.boolValue ? variable : constantValue, GUIContent.none);
        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
