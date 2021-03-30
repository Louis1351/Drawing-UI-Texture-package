using LS.DrawTexture.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace LS.DrawTexture.EditorScript
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(DrawTextureUI))]
    public class DrawTextureUIEditor : Editor
    {
        private DrawTextureUI component = null;
        private SerializedProperty brushSize;
        private SerializedProperty opacity;
        private SerializedProperty brushColor;
        private SerializedProperty drawOnTransparency;
        private SerializedProperty gradientColor;
        private SerializedProperty type;
        private SerializedProperty resolution;

        private float brushSizeTemp = 0.0f;
        private float opacityTemp = 0.0f;
        private Color brushColorTemp = Color.clear;
        private bool drawOnTransparencyTemp = true;
        private BrushesWindow window = null;
        private bool foldoutGroup1 = false;
        private bool foldoutGroup2 = false;
        private bool foldoutGroup3 = false;
        private Texture2D[] textures = null;

        public Texture2D[] Textures { get => textures; set => textures = value; }
        public DrawTextureUI Component { get => component; set => component = value; }

        void OnAwake()
        {
            Initialize();
        }

        void OnEnable()
        {
            Initialize();
        }

        public override void OnInspectorGUI()
        {
            UpdateComponent();

            serializedObject.Update();

            foldoutGroup1 = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutGroup1, "Brush Settings", EditorStyles.foldoutHeader);
            if (foldoutGroup1)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Width(100.0f));
                EditorGUILayout.BeginVertical();

                if (GUILayout.Button(textures[component.IdTexture], GUILayout.Width(100.0f), GUILayout.Height(100.0f)))
                {
                    if (window == null)
                    {
                        window = ScriptableObject.CreateInstance<BrushesWindow>();
                        window.Init(this);
                    }
                }

                GUIStyle style = new GUIStyle();
                style.alignment = TextAnchor.MiddleCenter;
                style.normal.textColor = Color.white;
                EditorGUILayout.LabelField(textures[component.IdTexture].name, style, GUILayout.Width(100.0f), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(brushSize);
                EditorGUILayout.PropertyField(opacity);
                
                if (type.enumValueIndex == (int)DrawTextureUI.BrushType.none
               || type.enumValueIndex == (int)DrawTextureUI.BrushType.additive
               || type.enumValueIndex == (int)DrawTextureUI.BrushType.multiply)
                {
                    EditorGUILayout.PropertyField(brushColor);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            foldoutGroup2 = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutGroup2, "Brush Mode", EditorStyles.foldoutHeader);
            if (foldoutGroup2)
            {
                EditorGUILayout.PropertyField(type);

                EditorGUILayout.PropertyField(drawOnTransparency);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();


            foldoutGroup3 = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutGroup3, "Texture Settings", EditorStyles.foldoutHeader);
            if (foldoutGroup3)
            {
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Clear Texture"))
                    {
                        component.Clear();
                    }
                    if (GUILayout.Button("Save Texture"))
                    {
                        SaveTextureWindow popup = ScriptableObject.CreateInstance<SaveTextureWindow>();
                        popup.Init(this);
                    }
                }
                if (!Application.isPlaying)
                {
                    EditorGUILayout.PropertyField(resolution);
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            serializedObject.ApplyModifiedProperties();
        }

        private void RefreshDatas()
        {
            textures = Resources.LoadAll<Texture2D>("Brushes");
        }

        private void UpdateComponent()
        {
            if (brushSize.floatValue != brushSizeTemp)
            {
                component.BrushSize = brushSize.floatValue;
                brushSizeTemp = brushSize.floatValue;
            }

            if (opacity.floatValue != opacityTemp)
            {
                component.Opacity = opacity.floatValue;
                opacityTemp = opacity.floatValue;
            }

            if (brushColor.colorValue != brushColorTemp)
            {
                component.BrushColor = brushColor.colorValue;
                brushColorTemp = brushColor.colorValue;
            }

            if (drawOnTransparency.boolValue != drawOnTransparencyTemp)
            {
                component.DrawOnTransparency = drawOnTransparency.boolValue;
                drawOnTransparencyTemp = drawOnTransparency.boolValue;
            }
        }

        private void Initialize()
        {
            component = (DrawTextureUI)target;
            brushSize = serializedObject.FindProperty("brushSize");
            opacity = serializedObject.FindProperty("opacity");
            brushColor = serializedObject.FindProperty("brushColor");
            gradientColor = serializedObject.FindProperty("gradientColor");
            type = serializedObject.FindProperty("type");
            drawOnTransparency = serializedObject.FindProperty("drawOnTransparency");
            resolution = serializedObject.FindProperty("resolution");

            RefreshDatas();
        }

        [MenuItem("GameObject/UI/Draw Texture")]
        static void CreateDrawTextureUI(MenuCommand menuCommand)
        {
            GameObject drawGO = new GameObject("Daw Texture");
            drawGO.AddComponent<RawImage>();
            drawGO.AddComponent<DrawTextureUI>();

            GameObjectUtility.SetParentAndAlign(drawGO, menuCommand.context as GameObject);

            Undo.RegisterCreatedObjectUndo(drawGO, "Create " + drawGO.name);
            Selection.activeObject = drawGO;
        }
    }
}
