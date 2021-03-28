using UnityEngine;
using UnityEditor;
using System.IO;

namespace LS.DrawTexture.EditorScript
{
    public class SaveTextureWindow : EditorWindow
    {
        string path = "";
        private string textureName = "";
        private DrawTextureUIEditor componentEditor = null;
        public string TextureName { get => textureName; set => textureName = value; }

        private int TexCount()
        {
            return Directory.GetFiles(path, "*.png").Length + 1;
        }

        private FileInfo[] GetFiles()
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] info = dir.GetFiles();

            return info;
        }

        private bool OverrideImage()
        {
            FileInfo[] fileInfos = GetFiles();
            string currentTextureName = textureName + ".png";
            for (int i = 0; i < fileInfos.Length; ++i)
            {
                if (currentTextureName == fileInfos[i].Name)
                {
                    return true;
                }
            }
            return false;
        }

        public void Init(DrawTextureUIEditor _target)
        {
            SaveTextureWindow window = (SaveTextureWindow)EditorWindow.GetWindow(typeof(SaveTextureWindow));
            window.position = new Rect((Screen.currentResolution.width - 250) / 2, (Screen.currentResolution.height - 110) / 2, 250, 110);
            window.Show();

            componentEditor = _target;
            path = Application.dataPath + "/Textures Saved/";
            textureName = "Image " + TexCount();
        }

        void OnGUI()
        {

            EditorGUILayout.LabelField("The texture will be save with the name :", EditorStyles.wordWrappedLabel);
            textureName = EditorGUILayout.TextField(textureName);
            GUILayout.Space(5);
            if (GUILayout.Button("Ok"))
            {
                if (OverrideImage())
                {
                    if (EditorUtility.DisplayDialog("Override Texture", "Do you want to override the texture ?", "ok", "cancel"))
                    {
                        SaveTexture();
                    }
                }
                else
                {
                    SaveTexture();
                }

                Close();
            }

            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
        }

        private void SaveTexture()
        {
            byte[] bytes = componentEditor.Component.GetTexture2D().EncodeToPNG();


            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(path + textureName + ".png", bytes);
        }
    }
}