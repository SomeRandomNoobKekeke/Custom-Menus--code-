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


      //CreateMenu();

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

        menu.OnSelect += (value) =>
        {
          if (value == null || value == "") return;
          string[] calls = value.Split(';');
          foreach (string call in calls)
          {
            string command = call.Split(" ").ElementAtOrDefault(0) ?? "";
            command = command.Trim();
            if (command == "") continue;
            if (!DebugConsole.Commands.Any(c => c.Names.Contains(command))) continue;
            DebugConsole.ExecuteCommand(call);
          }
        };
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
      CUISprite.BaseFolder = Path.Combine(Paths.MenusFolder, "ReportEnemies");
      CUIMenu menu = new CUIMenu()
      {
        ResizeToSprite = true,
        BackgroundSprite = new CUISprite("Background.png"),
        Name = "ReportEnemies",
      };

      menu["crawler"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("crawler.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak crawler!",
      };

      menu["mudraptor"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("mudraptor.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak mudraptor!",
      };
      menu["thresher"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("thresher.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak thresher!",
      };
      menu["spineling"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("spineling.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak spineling!",
      };

      menu["moloch"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("moloch.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak moloch!",
      };
      menu["hammerhead"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("hammerhead.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak hammerhead!",
      };
      menu["husk"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("husk.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak husk!",
      };
      menu["watcher"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("watcher.png"),
        HoverColor = new Color(0, 255, 255, 255),
        Value = "speak watcher!",
      };
      menu["thalamus"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("thalamus.png"),
        HoverColor = new Color(255, 0, 0, 255),
        Value = "speak thalamus!",
      };
      menu["charybdis"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("charybdis.png"),
        HoverColor = new Color(255, 0, 0, 255),
        Value = "speak charybdis!",
      };
      menu["latcher"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("latcher.png"),
        HoverColor = new Color(255, 0, 0, 255),
        Value = "speak latcher!",
      };
      menu["endworm"] = new CUIMenuOption()
      {
        BackgroundSprite = new CUISprite("endworm.png"),
        HoverColor = new Color(255, 0, 0, 255),
        Value = "speak endworm!",
      };


      CUISprite.BaseFolder = null;

      menu.SaveToFile(Path.Combine(Paths.MenusFolder, "ReportEnemies/ReportEnemies.xml"));
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