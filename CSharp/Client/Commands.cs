using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using HarmonyLib;
using Barotrauma.ClientSource.Settings;

namespace RadialMenus
{
  public partial class Mod
  {
    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();

    public DebugConsole.Command VanillaBindKey;

    public void AddCommands()
    {
      VanillaBindKey = DebugConsole.Commands.Find(c => c.Names.Contains("bindkey"));

      AddedCommands.Add(new DebugConsole.Command("togglemenu", "", (string[] args) =>
      {
        if (args.Length < 1) return;
        if (Menus.ContainsKey(args[0]))
        {
          Menus[args[0]].Toggle();
        }
      }));

      AddedCommands.Add(new DebugConsole.Command("openmenu", "", (string[] args) =>
      {
        if (args.Length < 1) return;
        if (Menus.ContainsKey(args[0]))
        {
          Menus[args[0]].Open();
        }
      }));

      AddedCommands.Add(new DebugConsole.Command("bindkey", "", (string[] args) =>
      {
        if (args.Length == 0)
        {
          foreach (var (keyOrMouse, value) in DebugConsoleMapping.Instance.Bindings)
          {
            Mod.Log($"{keyOrMouse} - {value}");
          }
        }
        else
        {
          VanillaBindKey.ClientExecute(args);
        }
      }));

      AddedCommands.Add(new DebugConsole.Command("keybindings", "", (string[] args) =>
      {
        foreach (var (keyOrMouse, value) in DebugConsoleMapping.Instance.Bindings)
        {
          Mod.Log($"{keyOrMouse} -> {value}");
        }
      }));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }
  }
}