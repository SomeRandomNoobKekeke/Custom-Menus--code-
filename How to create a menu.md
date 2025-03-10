### How to create a menu
Edit xml and pngs of some example, or write a lua script that would generate xml for you  
Then move it to "Barotrauma\ModSettings\Custom Menus"  
Then you can bind command "togglemenu YourMenuName" to any key and use it

### Theory:
Those menus are CUIComponents  
They are serializable to xml, like all CUIComponents, and have all the props normal CUIComponent has  
For more info about what CUIComponent are read [CUI Docs](https://somerandomnoobkekeke.github.io/CrabUI/index.html)

Mod loads all xml files in "ModFolder/Menus" and "Barotrauma\ModSettings\Custom Menus" as CUIMenu and makes them execute selected Value as semicolon separated console commands  
Also mod adds "togglemenu" and "openmenu" commands that can open menu by name

### Let's look at the xml:
Root element is loaded CUIMenu, and CUIMenuOptions are child components of CUIMenu  
All the attributes are properties of those components

### Important properties:

#### CUIMenu:
- Name - Without name you won't be able to open it
- FadeInDuration - Duration of initial fadein animation in seconds

#### CUIMenuOption:
- Value - The value which is send to CUIMenu on click, console command in this case
- BaseColor - Normal sprite color
- HoverColor - Color on mouse hover

#### Common:
- BackgroundSprite - Sets background sprite, Path can be relative to xml folder or barotrauma folder, also CUIMenuOption detects clicks only on non transparent pixels of its sprite
- BackgroundColor - Color of background sprite, useless on CUIMenuOption because it takes color from animation, use BaseColor, HoverColor instead
- Absolute - Rectangle, absolute position and size in pixels
- Relative - Rectangle, relative to the parent position and size, CUIMenuOption has Relative="[0,0,1,1]" by default
- CrossRelative - Same as Relative but to the opposite dimension
- ResizeToSprite - If true will set Absolute to BackgroundSprite size
- Anchor - Position will be calculated for this point from that parent point, e.g. Absolute="[0,0,,]" Anchor="[0.5,0.5]" will attach child center to parent center

### Notes:
I highly recommend you to use same texture size for menu and all the options because that way it's much easier to make them

CUIMenu is just a normal CUIComponent, it means you can add any CUIComponent as its child  
E.g. you can add CUITextBoxes to add reusable texts or add some static overlay 
