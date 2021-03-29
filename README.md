# Drawing UI Texture package
 Tool to add drawable RawImage (using only GPU to improve performance).
 
# How to install package
![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto.png)

Or add the line below in the dependencies bloc inside the manifest.json

```json
{
   "dependencies": 
   {
       "com.quokkaindiestudio.drawinguitexture":"https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package.git"
   }
}
```

# How it works
```C#
using LS.DrawTexture.Runtime;

private void MyFunction()
{
    DrawTextureUI myDrawTexture = GameObject.FindObjectOfType<DrawTextureUI>();
    
    //To get current texture
    Texture2D myTexture = myDrawTexture.GetTexture2D();
    
    //To reset texture
    myDrawTexture.Clear();
    
    //To change brush texture
    myDrawTexture.IdTexture = 0;
    
    //To change brush size
    myDrawTexture.BrushSize = 0.5f;
    
    //To change brush diffusion
    myDrawTexture.Diffusion = 0.01f;
    
    //To change brush Opacity
    myDrawTexture.Opacity = 0.01f;
    
    //To change brush mode
    myDrawTexture.Type = DrawTextureUI.BrushType.none;
    
    //To change brush color
    myDrawTexture.BrushColor = new Color(1.0f,1.0f,1.0f,1.0f);
    
    //To allow brush application on alpha texture
    myDrawTexture.DrawOnTransparency = true;
}
```
