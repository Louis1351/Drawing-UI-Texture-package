using LS.DrawTexture.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace LS.DrawTexture.Runtime
{

    [RequireComponent(typeof(RawImage))]
    public class DrawTextureUI : MonoBehaviour
    {

        #region Public Fields
        public enum BrushType
        {
            none,
            eraser,
            negative,
            additive,
            multiply,
            subtractAlpha,
            addAlpha
        }

        public enum ResolutionTexture
        {
            res32x32,
            res64x64,
            res128x128,
            res256x256,
            res512x512,
            res1024x1024,
            res2048x2048,
            res4096x4096,
        }


        #endregion


        #region Private Fields

        [SerializeField]
        [Tooltip("Change the brush size")]
        [Range(0.0f, 1.0f)]
        private float brushSize = 0.5f;
        [Tooltip("Change the brush diffusion")]
        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float diffusion = 0.01f;
        [Tooltip("Change the brush opacity")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float opacity = 0.01f;
        [Tooltip("Change the brush mode")]
        [SerializeField]
        private BrushType type = BrushType.none;
        [Tooltip("Change the brush color")]
        [SerializeField]
        private Color brushColor = Color.white;
        [Tooltip("If the brush can be apply on the transparency or not (ie when Alpha Texture is inferior to 1)")]
        [SerializeField]
        private bool drawOnTransparency = true;
        [Tooltip("Change the image resolution")]
        [SerializeField]
        private ResolutionTexture resolution = ResolutionTexture.res512x512;

        [SerializeField]
        private Texture2D brushTexture = null;
        [SerializeField]
        private int idTexture = 0;

        private Vector2Int res;
        private Texture mainTexture = null;
        private RenderTexture drawTexture = null;
        private RenderTexture tempTexture = null;
        private RectTransform rectTransform;
        private Material material = null;
        private RawImage rawImage = null;

        private Vector2 lastMousePos;

        private Vector2 divRect;

        //Identifiers for the shaders
        private int initTexId = Shader.PropertyToID("_InitTex");
        private int pos0Id = Shader.PropertyToID("_Position0");
        private int pos1Id = Shader.PropertyToID("_Position1");
        private int brushSizeId = Shader.PropertyToID("_brushSize");
        private int diffusionId = Shader.PropertyToID("_diffusion");
        private int opacityId = Shader.PropertyToID("_opacity");
        private int brushColorId = Shader.PropertyToID("_brushColor");
        private int drawOnTransparencyId = Shader.PropertyToID("_drawOnTransparency");
        private int brushTexId = Shader.PropertyToID("_BrushTex");
        #endregion


        #region Accessors
        public int IdTexture
        {
            get => idTexture;
            set
            {
                idTexture = value;
                ChangeBrush();
            }
        }
        public float BrushSize
        {
            get => brushSize;
            set
            {
                brushSize = value;
                ChangeSize();
            }
        }
        public float Diffusion
        {
            get => diffusion;
            set
            {
                diffusion = value;
                ChangeDiffusion();
            }
        }
        public float Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                ChangeOpacity();
            }
        }
        public BrushType Type
        {
            get => type;
            set
            {
                type = value;
            }
        }
        public Color BrushColor
        {
            get => brushColor;
            set
            {
                brushColor = value;
                ChangeColor();
            }
        }
        public bool DrawOnTransparency
        {
            get => drawOnTransparency;
            set
            {
                drawOnTransparency = value;
                ChangeTransparency();
            }
        }

        #endregion


        #region MonoBehaviour Methods

        private void Start()
        {
            rawImage = GetComponent<RawImage>();
            rectTransform = transform.GetComponent<RectTransform>();

            divRect = Vector2.one / rectTransform.rect.size;

            material = new Material(Resources.Load<Shader>("Shaders/Draw_Texture"));

            ChangeResolution();

            //Create a new texture if the current texture is null
            if (!rawImage.texture)
            {
                mainTexture = TextureUtils.CreateTexture(res.x, res.y, rawImage.color);
                rawImage.texture = mainTexture;
            }
            else
            {
                mainTexture = rawImage.texture;
            }

            InitTexture(mainTexture);
            SetShader();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition;
                BlitTexture();
            }
            else if (Input.GetMouseButton(0))
            {
                if (lastMousePos.x != Input.mousePosition.x ||
                lastMousePos.y != Input.mousePosition.y)
                {
                    BlitTexture();
                }
                lastMousePos = Input.mousePosition;
            }
        }


        #endregion


        #region Private Methods
        /// <summary>
        /// Draw the brush on the drawTexture.
        /// </summary>
        private void BlitTexture()
        {
            if (GetPixel(out Vector2 pixelCoord0, lastMousePos) &&
                GetPixel(out Vector2 pixelCoord1, Input.mousePosition))
            {
                material.SetVector(pos0Id, pixelCoord0);
                material.SetVector(pos1Id, pixelCoord1);

                Graphics.Blit(drawTexture, tempTexture, material, (int)type);
                Graphics.Blit(tempTexture, drawTexture);
            }
        }

        /// <summary>
        /// Get the pixel coordinates from the current image.
        /// </summary>
        /// <param name="_pixelCoord">Get the pixel coordinates</param>
        /// <param name="_pos">screen position</param>
        /// <returns>True or false if the image was found</returns>
        private bool GetPixel(out Vector2 _pixelCoord, Vector3 _pos)
        {
            Vector2 pixel;

            bool findImage = RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, _pos, Camera.main, out Vector2 localPoint);

            pixel = (localPoint - rectTransform.rect.position) * divRect;

            _pixelCoord = pixel;

            return findImage;
        }
        /// <summary>
        /// Change image resolution.
        /// </summary>
        private void ChangeResolution()
        {
            switch (resolution)
            {
                case ResolutionTexture.res32x32:
                    res = new Vector2Int(32, 32);
                    break;
                case ResolutionTexture.res64x64:
                    res = new Vector2Int(64, 64);
                    break;
                case ResolutionTexture.res128x128:
                    res = new Vector2Int(128, 128);
                    break;
                case ResolutionTexture.res256x256:
                    res = new Vector2Int(256, 256);
                    break;
                case ResolutionTexture.res512x512:
                    res = new Vector2Int(512, 512);
                    break;
                case ResolutionTexture.res1024x1024:
                    res = new Vector2Int(1024, 1024);
                    break;
                case ResolutionTexture.res2048x2048:
                    res = new Vector2Int(2048, 2048);
                    break;
                case ResolutionTexture.res4096x4096:
                    res = new Vector2Int(4096, 4096);
                    break;
            }
        }
        /// <summary>
        /// Initialize shader with the properties.
        /// </summary>
        private void SetShader()
        {
            material.SetTexture(initTexId, mainTexture);

            ChangeColor();
            ChangeSize();
            ChangeDiffusion();
            ChangeOpacity();
            ChangeBrush();
            ChangeTransparency();
        }

        /// <summary>
        /// Create each RenderTexture uses for the dawing.
        /// </summary>
        /// <param name="_texture">Texture to copy on the RenderTextures</param>
        private void InitTexture(Texture _texture)
        {
            drawTexture = TextureUtils.CreateTexture(_texture, res.x, res.y);
            tempTexture = TextureUtils.CreateTexture(res.x, res.y, Color.white);

            rawImage.texture = drawTexture;
        }
        #endregion
        /// <summary>
        /// Set property shader for the brush size.
        /// </summary>
        private void ChangeSize()
        {
            if (material)
            {
                material.SetFloat(brushSizeId, (1 - brushSize) * 100);
            }
        }
        /// <summary>
        /// Set property shader for the brush diffusion.
        /// </summary>
        private void ChangeDiffusion()
        {
            if (material)
                material.SetFloat(diffusionId, diffusion);
        }
        /// <summary>
        /// Set property shader for the brush opacity.
        /// </summary>
        private void ChangeOpacity()
        {
            if (material)
                material.SetFloat(opacityId, opacity);
        }
        /// <summary>
        /// Set property shader for the brush color.
        /// </summary>
        private void ChangeColor()
        {
            if (material)
                material.SetColor(brushColorId, brushColor);
        }
        /// <summary>
        /// Set property shader for the brush application.
        /// </summary>
        private void ChangeTransparency()
        {
            if (material)
                material.SetInt(drawOnTransparencyId, (drawOnTransparency) ? 0 : 1);
        }
        /// <summary>
        /// Set property shader for the brush texture.
        /// </summary>
        private void ChangeBrush()
        {
            Texture2D[] textures = Resources.LoadAll<Texture2D>("Brushes");

            if (idTexture >= 0 && idTexture < textures.Length)
            {
                brushTexture = textures[idTexture];

                if (material && brushTexture)
                    material.SetTexture(brushTexId, brushTexture);
            }
        }
        #region Public Methods



#if UNITY_EDITOR
        /// <summary>
        /// Set property shader for the brush texture.
        /// </summary>
        public void ChangeBrush(int _idTexture, Texture2D _brushTexture)
        {
            brushTexture = _brushTexture;
            idTexture = _idTexture;

            if (material && brushTexture)
                material.SetTexture(brushTexId, brushTexture);
        }
#endif
        /// <summary>
        /// Get the current image texture as Texture2D.
        /// </summary>
        /// <returns>the Texture2D of the current image texture</returns>
        public Texture2D GetTexture2D()
        {
            Texture2D texture = TextureUtils.ToTexture2D(drawTexture);
            return texture;
        }
        /// <summary>
        /// Reset the current image texture.
        /// </summary>
        public void Clear()
        {
            Graphics.Blit(mainTexture, drawTexture);
        }
        #endregion


    }
}
