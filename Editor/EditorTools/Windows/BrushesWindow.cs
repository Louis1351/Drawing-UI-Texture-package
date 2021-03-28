using UnityEditor;
using UnityEngine;

namespace LS.DrawTexture.EditorScript
{
    public class BrushesWindow : EditorWindow
    {
        private Vector2 scrollView = Vector2.zero;
        private DrawTextureUIEditor componentEditor = null;
        private BrushesWindow window = null;
        private Vector2 sizeBrushImage = new Vector2(100.0f, 100.0f);
        public void Init(DrawTextureUIEditor _target)
        {
            window = (BrushesWindow)EditorWindow.GetWindow(typeof(BrushesWindow));
            window.titleContent.text = "Brushes Selection";
            componentEditor = _target;
            window.Show();
        }

        void OnGUI()
        {
            if (!componentEditor.Component)
                Close();

            float w = 0.0f;
            float maxW = 0.0f;
            GUILayout.Label("Brushes", EditorStyles.boldLabel);
            scrollView = EditorGUILayout.BeginScrollView(scrollView);

            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < componentEditor.Textures.Length; ++i)
            {
                if (w + sizeBrushImage.x > position.width)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    maxW = w;
                    w = 0.0f;
                }

                EditorGUILayout.BeginHorizontal(GUILayout.Width(sizeBrushImage.x));
                EditorGUILayout.BeginVertical();

                GUIStyle style = new GUIStyle();

                if (componentEditor.Component.IdTexture == i)
                {
                    style.normal.textColor = Color.green;
                    GUI.backgroundColor = Color.green;
                }
                else
                {
                    style.normal.textColor = Color.white;
                    GUI.backgroundColor = Color.grey;
                }

                if (GUILayout.Button(componentEditor.Textures[i],
                GUILayout.Width(sizeBrushImage.x),
                GUILayout.Height(sizeBrushImage.y)))
                {
                    componentEditor.Component.ChangeBrush(i, componentEditor.Textures[i]);
                    EditorUtility.SetDirty(componentEditor.Component);
                    Close();
                }


                style.alignment = TextAnchor.MiddleCenter;


                EditorGUILayout.LabelField(componentEditor.Textures[i].name, style, GUILayout.Width(sizeBrushImage.x), GUILayout.ExpandWidth(true));
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                w += sizeBrushImage.x;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }
}
