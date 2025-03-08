using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
namespace CrabUI
{
  public enum CUISpriteDrawMode
  {
    Resize,
    Wrap,
    Static,
    StaticDeep,
  }

  /// <summary>
  /// Wrapper, containing link to texture, source rect, path, draw mode
  /// </summary>
  public class CUISprite : ICloneable
  {
    public static Texture2D BackupTexture => GUI.WhiteTexture;
    public static CUISprite Default => new CUISprite();

    public string Path = "";
    public SpriteEffects Effects;
    public CUISpriteDrawMode DrawMode;
    public Rectangle SourceRect;
    private Texture2D texture = BackupTexture;
    public Texture2D Texture
    {
      get => texture;
      set
      {
        texture = value;
        SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
      }
    }



    public float Rotation;
    public float RotationAngle
    {
      get => (float)(Rotation * 180 / Math.PI);
      set => Rotation = (float)(value * Math.PI / 180);
    }
    public Vector2 Origin;
    public Vector2 RelativeOrigin
    {
      set
      {
        if (Texture == null) return;
        Origin = new Vector2(value.X * Texture.Width, value.Y * Texture.Height);
      }
    }
    private Vector2 offset;
    public Vector2 Offset
    {
      get => offset;
      set
      {
        offset = value;
        RelativeOrigin = value;
      }
    }

    public override bool Equals(object obj)
    {
      if (obj is not CUISprite b) return false;


      return Texture == b.Texture &&
        SourceRect == b.SourceRect &&
        DrawMode == b.DrawMode &&
        Effects == b.Effects &&
        Rotation == b.Rotation &&
        Origin == b.Origin &&
        Offset == b.Offset;
    }


    public static CUISprite FromVanilla(Sprite sprite)
    {
      if (sprite == null) return Default;

      return new CUISprite(sprite.Texture, sprite.SourceRect)
      {
        Path = sprite.FullPath,
      };
    }

    public static CUISprite FromName(string name) => FromId(new Identifier(name));
    public static CUISprite FromId(Identifier id)
    {
      GUIComponentStyle? style = GUIStyle.ComponentStyles[id];
      if (style == null) return Default;

      return FromComponentStyle(style);
    }

    public static CUISprite FromComponentStyle(GUIComponentStyle style, GUIComponent.ComponentState state = GUIComponent.ComponentState.None)
    {
      return FromVanilla(style.Sprites[state].FirstOrDefault()?.Sprite);
    }


    public CUISprite()
    {
      Texture = BackupTexture;
      SourceRect = new Rectangle(0, 0, Texture.Width, Texture.Height);
    }
    public CUISprite(string path, Rectangle? sourceRect = null)
    {
      Path = path;
      Texture = CUI.TextureManager.GetTexture(path);
      if (sourceRect.HasValue) SourceRect = sourceRect.Value;
    }
    public CUISprite(Texture2D texture, Rectangle? sourceRect = null)
    {
      Texture = texture ?? BackupTexture;
      if (sourceRect.HasValue) SourceRect = sourceRect.Value;
    }

    public object Clone()
    {
      CUISprite sprite = new CUISprite(Texture, SourceRect)
      {
        Path = this.Path,
        Rotation = this.Rotation,
        Offset = this.Offset,
        Origin = this.Origin,
        Effects = this.Effects,
        DrawMode = this.DrawMode,
      };

      return sprite;
    }

    //TODO serialize offset, rotation
    public override string ToString()
    {
      string mode = DrawMode != CUISpriteDrawMode.Resize ? $", Mode: {DrawMode}" : "";
      string rect = SourceRect != Texture.Bounds ? $", SourceRect: {CUIExtensions.RectangleToString(SourceRect)}" : "";
      string effect = Effects != SpriteEffects.None ? $", Effects: {CUIExtensions.SpriteEffectsToString(Effects)}" : "";

      return $"{{ Path: {Path}{mode}{rect}{effect} }}";
    }
    public static CUISprite Parse(string raw)
    {
      Dictionary<string, string> props = CUIExtensions.ParseKVPairs(raw);

      if (!props.ContainsKey("path")) return new CUISprite();

      CUISprite sprite = CUI.TextureManager.GetSprite(props["path"]);
      if (props.ContainsKey("mode"))
      {
        sprite.DrawMode = Enum.Parse<CUISpriteDrawMode>(props["mode"]);
      }
      if (props.ContainsKey("sourcerect"))
      {
        sprite.SourceRect = CUIExtensions.ParseRectangle(props["sourcerect"]);
      }
      else
      {
        sprite.SourceRect = new Rectangle(0, 0, sprite.Texture.Width, sprite.Texture.Height);
      }
      if (props.ContainsKey("effects"))
      {
        sprite.Effects = CUIExtensions.ParseSpriteEffects(props["effects"]);
      }

      return sprite;
    }

    //TODO find less hacky solution
    public static CUISprite ParseWithContext(string raw, string baseFolder = null)
    {
      Dictionary<string, string> props = CUIExtensions.ParseKVPairs(raw);

      if (!props.ContainsKey("path")) return new CUISprite();

      if (!System.IO.Path.IsPathRooted(props["path"]) && baseFolder != null)
      {
        string localPath = System.IO.Path.Combine(baseFolder, props["path"]);

        if (File.Exists(localPath)) props["path"] = localPath;
      }

      CUISprite sprite = CUI.TextureManager.GetSprite(props["path"]);
      if (props.ContainsKey("mode"))
      {
        sprite.DrawMode = Enum.Parse<CUISpriteDrawMode>(props["mode"]);
      }
      if (props.ContainsKey("sourcerect"))
      {
        sprite.SourceRect = CUIExtensions.ParseRectangle(props["sourcerect"]);
      }
      else
      {
        sprite.SourceRect = new Rectangle(0, 0, sprite.Texture.Width, sprite.Texture.Height);
      }
      if (props.ContainsKey("effects"))
      {
        sprite.Effects = CUIExtensions.ParseSpriteEffects(props["effects"]);
      }

      return sprite;
    }
  }
}