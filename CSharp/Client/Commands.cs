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
using CrabUI;

namespace CustomMenus
{
  public partial class Mod
  {
    public List<DebugConsole.Command> AddedCommands = new List<DebugConsole.Command>();

    public DebugConsole.Command VanillaBindKey;

    public void AddCommands()
    {
      VanillaBindKey = DebugConsole.Commands.Find(c => c.Names.Contains("bindkey"));

      AddedCommands.Add(new DebugConsole.Command("speak", "", (string[] args) =>
      {
        string msg = args.ElementAtOrDefault(0);
        if (msg == null) return;

        Character.Controlled.Speak(msg);
        DebugConsole.ExecuteCommand($"say \"{msg}\"");
      }));

      AddedCommands.Add(new DebugConsole.Command("togglemenu", "", (string[] args) =>
      {
        if (args.Length < 1) return;
        if (CUIMenu.Menus.ContainsKey(args[0]))
        {
          CUIMenu.Menus[args[0]].Toggle();
        }
      }, () => new string[][] { CUIMenu.Menus.Keys.ToArray() }));

      AddedCommands.Add(new DebugConsole.Command("openmenu", "", (string[] args) =>
      {
        if (args.Length < 1) return;
        if (CUIMenu.Menus.ContainsKey(args[0]))
        {
          CUIMenu.Menus[args[0]].Open();
        }
      }, () => new string[][] { CUIMenu.Menus.Keys.ToArray() }));

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
          Mod.Log($"{keyOrMouse} - {value}");
        }
      }));

      DebugConsole.Commands.InsertRange(0, AddedCommands);
    }

    public void RemoveCommands()
    {
      AddedCommands.ForEach(c => DebugConsole.Commands.Remove(c));
      AddedCommands.Clear();
    }

    public static void PermitCommands(Identifier command, ref bool __result)
    {
      if (Instance.AddedCommands.Any(c => c.Names.Contains(command.Value))) __result = true;
    }
  }
}