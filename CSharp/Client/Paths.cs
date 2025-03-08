using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.IO;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

namespace CustomMenus
{
  public class ModPaths
  {
    private string modName; public string ModName
    {
      get => modName;
      set { modName = value; FindModDir(); }
    }
    private string modDir = "";
    public string ModDir
    {
      get => modDir;
      set
      {
        modDir = value;
        AssetsFolder = Path.Combine(ModDir, "Assets");
        MenusFolder = Path.Combine(ModDir, "Menus");
        SettingsFolder = $"ModSettings/{ModName}";
        IsInLocalMods = modDir.Contains("LocalMods");
      }
    }
    public string AssetsFolder { get; set; }
    public string MenusFolder { get; set; }
    public string SettingsFolder { get; set; }
    public bool IsInLocalMods { get; set; }

    public void FindModDir()
    {
      ContentPackage package = ContentPackageManager.EnabledPackages.All.ToList().Find(
        p => p.Name.Contains(ModName)
      );

      if (package != null) ModDir = Path.GetFullPath(package.Dir);
      else Mod.Log($"Couldn't find mod folder for {ModName}, are you sure it matches name in the filelist?", Color.Orange);
    }


    public void CreateModSettings()
    {
      if (!Directory.Exists(SettingsFolder)) Directory.CreateDirectory(SettingsFolder);
    }

    public ModPaths(string modName)
    {
      ModName = modName;
      CreateModSettings();
    }

  }

}