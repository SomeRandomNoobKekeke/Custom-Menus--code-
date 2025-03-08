require "CUITypes"

local modFolder = ...
local menuFolder = modFolder .. '/Menus/ReportEnemies'

CUISprite.BaseFolder = menuFolder

local  menu =  CUIMenu()
menu.ResizeToSprite = true
menu.Anchor = Vector2(1, 0.5)
-- menu.Absolute = CUINullRect(nil, nil, 600, 100)
-- menu.Relative = CUINullRect(nil, nil, nil, 0.2)
-- menu.CrossRelative = CUINullRect(nil, nil, 0.2, nil)
menu.BackgroundSprite = CUISprite("Background.png")
menu.Name = "ReportEnemies"
      
menu["crawler"] = CUIMenuOption()
menu["crawler"].BackgroundSprite = CUISprite("crawler.png")
menu["crawler"].HoverColor = Color(0, 255, 255, 255)
menu["crawler"].Value = "speak crawler!"

menu["mudraptor"] =  CUIMenuOption()
menu["mudraptor"].BackgroundSprite = CUISprite("mudraptor.png")
menu["mudraptor"].HoverColor =  Color(0, 255, 255, 255)
menu["mudraptor"].Value = "speak mudraptor!"

menu["thresher"] =  CUIMenuOption()
menu["thresher"].BackgroundSprite = CUISprite("thresher.png")
menu["thresher"].HoverColor =  Color(0, 255, 255, 255)
menu["thresher"].Value = "speak thresher!"

menu["spineling"] =  CUIMenuOption()
menu["spineling"].BackgroundSprite = CUISprite("spineling.png")
menu["spineling"].HoverColor =  Color(0, 255, 255, 255)
menu["spineling"].Value = "speak spineling!"

menu["moloch"] =  CUIMenuOption()
menu["moloch"].BackgroundSprite = CUISprite("moloch.png")
menu["moloch"].HoverColor =  Color(0, 255, 255, 255)
menu["moloch"].Value = "speak moloch!"

menu["hammerhead"] =  CUIMenuOption()
menu["hammerhead"].BackgroundSprite = CUISprite("hammerhead.png")
menu["hammerhead"].HoverColor =  Color(0, 255, 255, 255)
menu["hammerhead"].Value = "speak hammerhead!"

menu["husk"] =  CUIMenuOption()
menu["husk"].BackgroundSprite = CUISprite("husk.png")
menu["husk"].HoverColor =  Color(0, 255, 255, 255)
menu["husk"].Value = "speak husk!"

menu["watcher"] =  CUIMenuOption()
menu["watcher"].BackgroundSprite = CUISprite("watcher.png")
menu["watcher"].HoverColor =  Color(0, 255, 255, 255)
menu["watcher"].Value = "speak watcher!"

menu["thalamus"] =  CUIMenuOption()
menu["thalamus"].BackgroundSprite = CUISprite("thalamus.png")
menu["thalamus"].HoverColor =  Color(255, 0, 0, 255)
menu["thalamus"].Value = "speak thalamus!"

menu["charybdis"] =  CUIMenuOption()
menu["charybdis"].BackgroundSprite = CUISprite("charybdis.png")
menu["charybdis"].HoverColor =  Color(255, 0, 0, 255)
menu["charybdis"].Value = "speak charybdis!"

menu["latcher"] =  CUIMenuOption()
menu["latcher"].BackgroundSprite = CUISprite("latcher.png")
menu["latcher"].HoverColor =  Color(255, 0, 0, 255)
menu["latcher"].Value = "speak latcher!"

menu["endworm"] =  CUIMenuOption()
menu["endworm"].BackgroundSprite = CUISprite("endworm.png")
menu["endworm"].HoverColor =  Color(255, 0, 0, 255)
menu["endworm"].Value = "speak endworm!"
      
menu.SaveToFile(menuFolder .. '/ReportEnemies.xml')