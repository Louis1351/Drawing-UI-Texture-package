# Drawing UI Texture package
 Tool to add drawable RawImage (using only GPU to improve performance).
 
 ```C#
using LS.DrawTexture;

private void MyFunction()
{
    DrawTextureUI myDrawTexture = GameObject.FindObjectOfType<DrawTextureUI>();
    Texture2D myTexture = myDrawTexture.GetTexture2D();
    myDrawTexture.Clear();
    
    myDrawTexture.IdTexture = 0;
    myDrawTexture.BrushSize = 0.5f;
    myDrawTexture.Diffusion = 0.01f;
    myDrawTexture.Opacity = 0.01f;
    myDrawTexture.BrushType = DrawTextureUI.BrushType.none;
    myDrawTexture.BrushColor = new Color(1.0f,1.0f,1.0f,1.0f);
    myDrawTexture.DrawOnTransparency = true;
}
```
