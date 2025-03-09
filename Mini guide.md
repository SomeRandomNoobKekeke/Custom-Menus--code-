CUIMenu is a CUI Component, it contains CUIMenuOptions as children, they are normal CUIComponents and has all the props CUIComponent has  
Check [CUI Docs](https://somerandomnoobkekeke.github.io/CrabUI/index.html),

CUIMenuOption has Value and will send it to CUIMenu when clicked

Mod loads CUIMenus from "Barotrauma\ModSettings\Custom Menus" and "Menus" in mod folder and executes all incoming Values as **semicolon** separated console commands

### How to create a menu

copy paste some example and change xml, or edit lua script

you can also set BaseColor and HoverColor of CUIMenuOption, change FadeInDuration of CUIMenu if you don't like transparency

set sprites for options, you can add ",SourceRect:[x,y,w,h]" there, clicks registered only on non transparent pixels and if sprites overlap click will be triggered on both

I highly recomend you to use same texture size for CUIMenu and all the options

CUIMenu takes its size from sprite thx to "ResizeToSprite="true"", if you don't like it remove "ResizeToSprite="true"" and set size with Relative/Absolute

#### most importantly
Set Values for CUIMenuOptions and Name for CUIMenu or it's not gonna work

### How to use it

bind "togglemenu menuname" commmand to some button




