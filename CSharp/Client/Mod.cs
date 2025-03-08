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


namespace CustomMenus
{
  public partial class Mod : IAssemblyPlugin
  {
    public static string Name = "Custom Menus";

    public static Mod Instance { get; set; }
    public ModPaths Paths { get; set; }
    public Harmony harmony = new Harmony("CustomMenus");

    public void Initialize()
    {
      Instance = this;
      AddCommands();
      PatchAll();

      Paths = new ModPaths(Name);

      CUI.ModDir = Paths.ModDir;
      CUI.AssetsPath = Paths.AssetsFolder;
      CUI.UpdateHookIdentifier = Name;
      CUI.Initialize();

      foreach (string file in Directory.GetFiles(Paths.MenusFolder, "*.xml", SearchOption.AllDirectories))
      {
        CUIMenu.Load(file);
      }

      foreach (string file in Directory.GetFiles(Paths.SettingsFolder, "*.xml", SearchOption.AllDirectories))
      {
        CUIMenu.Load(file);
      }

      foreach (CUIMenu menu in CUIMenu.Menus.Values)
      {
        menu.OnSelect += (s) => DebugConsole.ExecuteCommand(s);
      }
    }

    public void PatchAll()
    {
      harmony.Patch(
        original: typeof(LuaGame).GetMethod("IsCustomCommandPermitted"),
        postfix: new HarmonyMethod(typeof(Mod).GetMethod("PermitCommands"))
      );
    }

    public void CreateMenu()
    {
      // Menus["1"] = new CUIMenu()
      // {
      //   ResizeToSprite = true,
      //   BackgroundColor = new Color(0, 0, 0, 0),
      //   BackgroundSprite = new CUISprite("Menu/Menu.png"),
      //   Name = "Example",
      //   // Relative = new CUINullRect(h: 0.8f),
      //   // CrossRelative = new CUINullRect(w: 0.8f),
      // };

      // Menus["1"]["lol"] = new CUIMenuOption()
      // {
      //   BackgroundSprite = new CUISprite("Menu/1.png"),
      //   HoverColor = new Color(0, 255, 255, 255),
      //   Value = "spawnitem \"Riot Shotgun\" inventory",
      // };

      // Menus["1"]["kek"] = new CUIMenuOption()
      // {
      //   BackgroundSprite = new CUISprite("Menu/2.png"),
      //   HoverColor = new Color(0, 255, 255, 255),
      //   Value = "spawnitem \"Shotgun Shell\" inventory 32",
      // };

      //Menus["1"].SaveToFile(Path.Combine(Paths.ModDir, "test.xml"));
      //Menus["1"] = CUIComponent.LoadFromFile<CUIMenu>(Path.Combine(Paths.ModDir, "test.xml"));
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