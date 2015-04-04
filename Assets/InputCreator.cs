#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

public class InputCreator : MonoBehaviour {

#if UNITY_EDITOR
	// Use this for initialization
	[ContextMenu("Do")]
	void Do()
	{
		for (int i=0; i < 4; i++)
		{
			AddAxis(new InputAxis() { name = "Horizontal"+i, sensitivity = 1f, type = AxisType.JoystickAxis, dead = .3f, axis = 1, joyNum = i+1, gravity = 1000 });
			AddAxis(new InputAxis() { name = "DHorizontal"+i, sensitivity = 1f, type = AxisType.JoystickAxis, dead = .1f, axis = 7, joyNum = i+1, gravity = 1000 });
			AddAxis(new InputAxis() { name = "Vertical"+i, sensitivity = 1f, type = AxisType.JoystickAxis, dead = .3f, axis = 2, joyNum = i+1, invert = true, gravity = 1000 });
			AddAxis(new InputAxis() { name = "DVertical"+i, sensitivity = 1f, type = AxisType.JoystickAxis, dead = .1f, axis = 8, joyNum = i+1, gravity = 1000 });
			AddAxis(new InputAxis() { name = "TopButton"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 3", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "LeftButton"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 0", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "RightButton"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 2", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "BottomButton"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 1", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "Start"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 9", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "Logo"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 12", joyNum = i+1 });
			AddAxis(new InputAxis() { name = "Pad"+i, sensitivity = 1f, type = AxisType.KeyOrMouseButton, positiveButton = "joystick " + (i+1) + " button 13", joyNum = i+1 });
		}
	}

	private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
	{
		SerializedProperty child = parent.Copy();
		child.Next(true);
		do
		{
			if (child.name == name) return child;
		}
		while (child.Next(false));
		return null;
	}

	private static bool AxisDefined(string axisName)
	{
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
		axesProperty.Next(true);
		axesProperty.Next(true);
		while (axesProperty.Next(false))
		{
			SerializedProperty axis = axesProperty.Copy();
			axis.Next(true);
			if (axis.stringValue == axisName) return true;
		}
		return false;
	}

	public enum AxisType
	{
		KeyOrMouseButton = 0,
		MouseMovement = 1,
		JoystickAxis = 2
	};
	
	public class InputAxis
	{
		public string name;
		public string descriptiveName;
		public string descriptiveNegativeName;
		public string negativeButton;
		public string positiveButton;
		public string altNegativeButton;
		public string altPositiveButton;
		
		public float gravity;
		public float dead;
		public float sensitivity;
		
		public bool snap = false;
		public bool invert = false;
		
		public AxisType type;
		
		public int axis;
		public int joyNum;
	}
	
	private static void AddAxis(InputAxis axis)
	{
		if (AxisDefined(axis.name)) return;
		
		SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
		SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
		
		axesProperty.arraySize++;
		serializedObject.ApplyModifiedProperties();
		
		SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
		
		GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
		GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
		GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
		GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
		GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
		GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
		GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
		GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
		GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
		GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
		GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
		GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
		GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
		GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
		GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;
		
		serializedObject.ApplyModifiedProperties();
	}
#endif

}
