# Takipsilim — Game Documentation (Issue #6)

## Game Engine: Unity
After comparing **Godot** and **Unity**, our team decided to use **Unity** as our main engine for *Takipsilim*, a 3D top-down MMORPG inspired by the legend of Bakunawa.

### Why We Chose Unity
- **Better support for 3D and hybrid 2.5D projects.**
  Unity allows smooth integration between 2D and 3D elements, which fits our early 2.5D prototype.
- **Extensive library of free and paid assets.**
  There are many stylized 3D models, environments, and animations available, making it easier for us (students with no 3D experience) to build content faster.
- **Powerful animation tools and lighting systems.**
  Unity’s built-in Animator and URP lighting give more control over stylized visuals and atmosphere.
- **Stronger community and documentation.**
  Tutorials, forums, and guides are easier to find for Unity, especially for MMORPG and RPG genres.

### Godot Comparison
While **Godot** performed well with pure 2D projects and simple sprite animations, it struggled when blending 2D sprites into 3D environments. Even after disabling filtering, sprites appeared blurry, which affected the overall quality.  
Unity handled the 2.5D setup better and offered more stable lighting, rendering, and camera control.

### Unity:
![Unity](https://i.imgur.com/hzfT6jH.png)

### Godot:
![Godot](https://i.imgur.com/c8L6sJp.png)
---

## Art Style

### Visual Direction
We aim for a **stylized semi–low poly 3D art style** — similar to games like *Darksburg* and *Death’s Door*.
This look emphasizes clean shapes, soft lighting, and warm, atmospheric tones that match the mysterious and folkloric theme of *Takipsilim*.

![Reference1](https://i.pinimg.com/1200x/4a/93/54/4a935496eef7b7fc881fe0d9d7467e4d.jpg)
![Reference2](https://i.pinimg.com/1200x/f5/51/12/f55112727e42dab4488e789bd00e8339.jpg)

### Reason for Choosing Stylized 3D
- Easier to maintain consistency even with different asset sources.  
- Lighting and shadows play a huge part in storytelling, which works better in 3D.  
- Supports dynamic environments and day–night cycles.  
- Many ready-made stylized packs are available online for faster prototyping.  

---

## Tools & Resources

| Purpose                          | Tool                                       | Notes                                        |
| -------------------------------- | ------------------------------------------ | -------------------------------------------- |
| 3D Animation                     | [Mixamo](https://www.mixamo.com/)          | Free character animations and rigging.       |
| Assets / Skybox / Textures       | [FreeStylized](https://freestylized.com/)  | Free stylized materials and skyboxes.        |
| General Assets (Props, UI, etc.) | [Kenney.nl](https://kenney.nl/)            | Free stylized 3D and 2D game assets.         |
| Modeling                         | Blender                                    | For editing or creating simple custom props. |

