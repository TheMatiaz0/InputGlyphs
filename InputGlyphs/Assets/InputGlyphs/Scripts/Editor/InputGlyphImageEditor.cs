#if INPUT_SYSTEM && ENABLE_INPUT_SYSTEM
using UnityEditor;

namespace InputGlyphs.Display.Editor
{
    [CustomEditor(typeof(InputGlyphImage)), CanEditMultipleObjects]
    public class InputGlyphImageEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var imageProperty = serializedObject.FindProperty(nameof(InputGlyphImage.Image));
            EditorGUILayout.PropertyField(imageProperty);

            var inputActionReferenceProperty = serializedObject.FindProperty(nameof(InputGlyphImage.InputActionReference));
            EditorGUILayout.PropertyField(inputActionReferenceProperty);

            var glyphsLayoutDataProperty = serializedObject.FindProperty(nameof(InputGlyphImage.GlyphsLayoutData));
            EditorGUILayout.PropertyField(glyphsLayoutDataProperty);

            var enableLayoutElementProperty = serializedObject.FindProperty(nameof(InputGlyphImage.EnableLayoutElement));
            EditorGUILayout.PropertyField(enableLayoutElementProperty);
            if (enableLayoutElementProperty.boolValue)
            {
                var layoutElementPriorityProperty = serializedObject.FindProperty(nameof(InputGlyphImage.LayoutElementPriority));
                EditorGUILayout.PropertyField(layoutElementPriorityProperty);

                var layoutElementSizeProperty = serializedObject.FindProperty(nameof(InputGlyphImage.LayoutElementSize));
                EditorGUILayout.PropertyField(layoutElementSizeProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
