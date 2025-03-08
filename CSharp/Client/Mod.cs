using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CrabUI;


namespace RadialMenus
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string Name = "Radial Menus";

    public static Mod Instance { get; set; }
    public ModPaths Paths { get; set; }

    public Dictionary<string, CUIMenu> Menus = new();

    public void Initialize()
    {
      Instance = this;
      AddCommands();

      Paths = new ModPaths(Name);


      CUI.ModDir = Paths.ModDir;
      CUI.AssetsPath = Paths.AssetsFolder;
      CUI.Initialize();

      Menus["1"] = new CUIMenu()
      {
        ResizeToSprite = true,
        BackgroundColor = new Color(0, 0, 0, 0),
        BackgroundSprite = new CUISprite("Menu/Menu.png"),
        Name = "Example",
        // Relative = new CUINullRect(h: 0.8f),
        // CrossRelative = new CUINullRect(w: 0.8f),
      };

      Menus["1"]["lol"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("Menu/1.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "spawnitem \"Riot Shotgun\" inventory",
      };

      Menus["1"]["kek"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("Menu/2.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "spawnitem \"Shotgun Shell\" inventory 32",
      };



      Menus["1"].Open();

      Menus["1"].SaveToFile(Path.Combine(Paths.ModDir, "test.xml"));
      Menus["1"] = CUIComponent.LoadFromFile<CUIMenu>(Path.Combine(Paths.ModDir, "test.xml"));
      Menus["1"].OnSelect += (s) => DebugConsole.ExecuteCommand(s);
    }



    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
    public void Dispose()
    {
      CUI.Dispose();
      RemoveCommands();
    }

    public static void Log(object msg, Color? color = null)
    {
      color ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", color * 0.8f, color);
    }
  }
}