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

    public void Initialize()
    {
      Instance = this;


      Paths = new ModPaths(Name);


      CUI.ModDir = Paths.ModDir;
      CUI.AssetsPath = Paths.AssetsFolder;
      CUI.Initialize();


      CUIRadialMenu menu = new CUIRadialMenu()
      {
        OptionTemplate = new CUIRadialMenu.Option()
        {
          BackgroundSprite = new CUISprite("RadialMenuOption.png"),
        }
      };

      menu.OptionTemplate.Animations["hover"].Interpolate = (d) => ToolBox.GradientLerp(d,
        new Color(0, 0, 32, 255),
        new Color(0, 0, 255, 255),
        new Color(0, 32, 255, 255)
      );



      menu.AddOption("shotgun", () => DebugConsole.ExecuteCommand("spawnitem \"Riot Shotgun\" inventory"));
      menu.AddOption("ammo", () => DebugConsole.ExecuteCommand("spawnitem \"Shotgun Shell\" inventory 32"));
      menu.AddOption("3", () => CUI.Log("3"));
      menu.AddOption("4", () => CUI.Log("4"));
      menu.AddOption("5", () => CUI.Log("5"));
      menu.AddOption("6", () => CUI.Log("6"));
      menu.AddOption("crawler", () => DebugConsole.ExecuteCommand("spawncharacter crawler cursor"));
      menu.AddOption("mudraptor", () => DebugConsole.ExecuteCommand("spawncharacter mudraptor cursor"));


      CUI.Main.Global.OnKeyDown += (e) =>
      {
        if (e.PressedKeys.Contains(Keys.G))
        {
          if (!menu.IsOpened) menu.Open();
          else menu.Close();
        }

        if (e.PressedKeys.Contains(Keys.Escape))
        {
          menu.Close();
        }
      };


    }



    public void OnLoadCompleted() { }
    public void PreInitPatching() { }
    public void Dispose()
    {
      CUI.Dispose();
    }

    public void Log(object msg, Color? color = null)
    {
      color ??= Color.Cyan;
      LuaCsLogger.LogMessage($"{msg ?? "null"}", color * 0.8f, color);
    }
  }
}