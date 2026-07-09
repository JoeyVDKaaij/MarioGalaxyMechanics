using UnityEditor;

[CustomEditor(typeof(GravityApplier))]
public class GravityApplierEditor : Editor
{
    private SerializedProperty _atmosphereType;
    private SerializedProperty _reverseGravityDirection;
    private SerializedProperty _gravityDir;
    private SerializedProperty _useCustomCenterPoint;
    private SerializedProperty _transformCenterPoint;
    private SerializedProperty _vector3CenterPoint;
    private SerializedProperty _gravityStrength;
    private SerializedProperty _changeGravityDirOnExit;
    private SerializedProperty _gravityDirOnExit;
    
    private void OnEnable()
    {
        _atmosphereType = serializedObject.FindProperty("atmosphereType");
        _reverseGravityDirection = serializedObject.FindProperty("reverseGravityDirection");
        _gravityDir = serializedObject.FindProperty("gravityDir");
        _useCustomCenterPoint = serializedObject.FindProperty("useCustomCenterPoint");
        _transformCenterPoint = serializedObject.FindProperty("transformCenterPoint");
        _vector3CenterPoint = serializedObject.FindProperty("vector3CenterPoint");
        _gravityStrength = serializedObject.FindProperty("gravityStrength");
        _changeGravityDirOnExit = serializedObject.FindProperty("changeGravityDirOnExit");
        _gravityDirOnExit = serializedObject.FindProperty("gravityDirOnExit");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(_atmosphereType);
        EditorGUILayout.PropertyField(_reverseGravityDirection);

        switch (_atmosphereType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(_gravityDir);
                break;
            
            case 1:
                EditorGUILayout.PropertyField(_useCustomCenterPoint);
                if (_useCustomCenterPoint.boolValue)
                    EditorGUILayout.PropertyField(_vector3CenterPoint);
                else 
                    EditorGUILayout.PropertyField(_transformCenterPoint);
                
                EditorGUILayout.PropertyField(_gravityStrength);
                break;
        }
        
        EditorGUILayout.PropertyField(_changeGravityDirOnExit);
        if (_changeGravityDirOnExit.boolValue)
            EditorGUILayout.PropertyField(_gravityDirOnExit);
        
        serializedObject.ApplyModifiedProperties();
    }
}