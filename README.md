# Drawing UI Texture package
 Tool to add drawable RawImage for Unity (using only GPU to improve performance).
 
![Header image](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/header.gif)
 
# How to install package
You can add the package with the new Package Manager from the window menu using the repository address https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package.git. (Window -> Package Manager)

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

* **Brushes** - To change brush texture.
* **Size** - To change brush size.
* **Opacity** - To change brush opacity.
* **Color** - To change brush color.
* **Type** - To change brush mode.
* **Draw on tranparency** - To allow brush application on alpha texture.
* **Resolution** - To change image resolution in the Start function.

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto6.PNG)

The different types of brush are pretty straight forward.

* **None** - To apply color 
* **Eraser** - To erase the drawing
* **Negative** - To change image color in negative color.
* **Additive** - To apply color additively.
* **Multiply** - To multiply color.
* **Subtract Alpha** - To reduce alpha.
* **Add Alpha** - To add alpha.

## In code

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

## Add a new brush

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto3.PNG)

To add a new brush, you have to do a Resources folder and inside it, add the Brushes folder.

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto5.PNG)

The texture must have a white border and the width and height texture have to be the same (for instance 512x512).<br>
**WARNING do not generate mip maps.**

## Other parameters for the texture (Display only at Runtime)

![Image of Tutorial git](https://github.com/Quokka-Indie-Studio/Drawing-UI-Texture-package/blob/main/Images/gitTuto4.PNG)

* **Clear** - To reset texture.
* **Save** - To save current texture.

Save the current texture in the Textures Saved folder (Assets/Textures Saved/)<br>
After that you have to refresh files to see the folder.<br>

**WARNING do not refresh files while the scene is playing.**

# TO DO

* Add more modes

