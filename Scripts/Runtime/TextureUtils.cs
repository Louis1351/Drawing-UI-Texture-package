using UnityEngine;

namespace LS.DrawTexture.Utils
{
    public class TextureUtils
    {
        /// <summary>
        /// Copy a Texture to a new RenderTexture.
        /// </summary>
        /// <param name="_texture">texture to copy</param>
        /// <param name="_width">RenderTexture width</param>
        /// <param name="_height">RenderTexture height</param>
        /// <returns>The RenderTexture created</returns>
        public static RenderTexture CreateTexture(Texture _texture, int _width, int _height)
        {
            RenderTexture newRenderTexture = new RenderTexture(_width, _height, 0);
            Graphics.Blit(_texture, newRenderTexture);
            return newRenderTexture;
        }

        /// <summary>
        /// Create RenderTexture fills with color.
        /// </summary>
        /// <param name="_width">RenderTexture width</param>
        /// <param name="_height">RenderTexture height</param>
        /// <param name="_color">RenderTexture color</param>
        /// <returns>The RenderTexture created</returns>
        public static RenderTexture CreateTexture(int _width, int _height, Color _color)
        {
            RenderTexture newRenderTexture = new RenderTexture(_width, _height, 0);
            RenderTexture.active = newRenderTexture;
            GL.Clear(true, true, _color);
            RenderTexture.active = null;
            return newRenderTexture;
        }

        /// <summary>
        /// Convert a RenderTexture to a Texture2D.
        /// </summary>
        /// <param name="_renderTexture">RenderTexture to convert</param>
        /// <returns>The Texture2D created</returns>
        public static Texture2D ToTexture2D(RenderTexture _renderTexture)
        {
            if (!_renderTexture)
                return null;
                
            Texture2D newTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, true);
            RenderTexture.active = _renderTexture;
            newTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            newTexture.Apply();
            return newTexture;
        }
    }
}
