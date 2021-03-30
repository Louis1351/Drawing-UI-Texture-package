# Drawing UI Texture package
 Tool to add drawable RawImage (using only GPU to improve performance).
 
# How to install package
You can add the package with the new Package Manager from the window menu. (Window -> Package Manager)

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto.png)

Or add the line below in the dependencies block inside the manifest.json

```json
{
   "dependencies": 
   {
       "com.quokkaindiestudio.drawinguitexture":"https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package.git"
   }
}
```

# How it works
## Add a Drawing UI Texture 
First create a new canvas in World Space or in Screen Space - Camera.<br>
Doesn't work with canvas in Screen Space - Overlay render mode.<br>

Add the Draw texture from hiearchy window.

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto1.png)

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto2.PNG)

On the script object<br>
* **Brushes** - To select the current object.
* **Size** - To select the current object.
* **Opacity** - To select the current object.
* **Color** - To select the current object.
* **Type** - To select the current object.
* **Draw on tranparency** - To select the current object.
* **Resolution** - To select the current object.

## Add a new brush

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto3.PNG)

## Save texture

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto4.PNG)

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
    myDrawTexture.Size = 0.5f;
    
    //To change brush Opacity
    myDrawTexture.Opacity = 0.01f;
    
    //To change brush mode
    myDrawTexture.Type = DrawTextureUI.BrushType.none;
    
    //To change brush color
    myDrawTexture.Color = new Color(1.0f,1.0f,1.0f,1.0f);
    
    //To allow brush application on alpha texture
    myDrawTexture.DrawOnTransparency = true;
}
```
