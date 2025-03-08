using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace CrabUI
{
  public class CUIAnimation
  {
    internal static void InitStatic()
    {
      CUI.OnDispose += () => ActiveAnimations.Clear();
    }

    public static HashSet<CUIAnimation> ActiveAnimations = new();
    public static void UpdateAllAnimations(double time)
    {
      foreach (CUIAnimation animation in ActiveAnimations)
      {
        animation.Step(time);
      }
    }

    public bool Debug { get; set; }
    public static float StartLambda = 0.0f;
    public static float EndLambda = 1.0f;


    private object target;
    public object Target
    {
      get => target;
      set
      {
        target = value;
        UpdateSetter();
      }
    }
    private bool active;
    public bool Active
    {
      get => active;
      set
      {
        if (Blocked || active == value) return;
        active = value;

        if (active) ActiveAnimations.Add(this);
        else ActiveAnimations.Remove(this);
        ApplyValue();
      }
    }

    public double Duration
    {
      get => 1.0 / Speed * Timing.Step;
      set
      {
        double steps = value / Timing.Step;
        Speed = 1.0 / steps;
      }
    }

    public double ReverseDuration
    {
      get => 1.0 / (BackSpeed ?? 0) * Timing.Step;
      set
      {
        double steps = value / Timing.Step;
        BackSpeed = 1.0 / steps;
      }
    }

    public bool Blocked { get; set; }
    public double Lambda { get; set; }
    public double Speed { get; set; } = 0.01;
    public double? BackSpeed { get; set; }
    public bool Bounce { get; set; }
    public CUIDirection Direction { get; set; }

    public object StartValue { get; set; }
    public object EndValue { get; set; }

    private string property;
    private Action<object> setter;
    private Type propertyType;
    public string Property
    {
      get => property;
      set
      {
        property = value;
        UpdateSetter();
      }
    }

    public event Action<CUIDirection> OnStop;
    public Func<float, object> Interpolate;

    //...
    public Action<object> Convert<T>(Action<T> myActionT)
    {
      if (myActionT == null) return null;
      else return new Action<object>(o => myActionT((T)o));
    }


    private void UpdateSetter()
    {
      if (Target != null && Property != null)
      {
        PropertyInfo pi = Target.GetType().GetProperty(Property);
        if (pi == null)
        {
          CUI.Warning($"CUIAnimation couldn't find {Property} in {Target}");
          return;
        }

        propertyType = pi.PropertyType;

        Interpolate = (l) => CUIInterpolate.Interpolate[propertyType].Invoke(StartValue, EndValue, l);


        // https://coub.com/view/1mast0
        if (propertyType == typeof(float))
        {
          setter = Convert<float>(pi.GetSetMethod()?.CreateDelegate<Action<float>>(Target));
        }

        if (propertyType == typeof(Color))
        {
          setter = Convert<Color>(pi.GetSetMethod()?.CreateDelegate<Action<Color>>(Target));
        }
      }
    }



    public void Start() => Active = true;
    public void Stop()
    {
      Active = false;
      OnStop?.Invoke(Direction);
    }
    public void Forward()
    {
      Direction = CUIDirection.Straight;
      Active = true;
    }
    public void Back()
    {
      Direction = CUIDirection.Reverse;
      Active = true;
    }

    public void SetToStart() => Lambda = StartLambda;
    public void SetToEnd() => Lambda = EndLambda;


    public void UpdateState()
    {
      if (Direction == CUIDirection.Straight && Lambda >= EndLambda)
      {
        Lambda = EndLambda;
        if (Bounce) Direction = CUIDirection.Reverse;
        else Stop();
      }

      if (Direction == CUIDirection.Reverse && Lambda <= StartLambda)
      {
        Lambda = StartLambda;
        if (Bounce) Direction = CUIDirection.Straight;
        else Stop();
      }
    }

    public void ApplyValue()
    {
      if (Interpolate == null) return;
      object value = Interpolate.Invoke((float)Lambda);
      setter?.Invoke(value);
    }

    public void Step(double time)
    {
      UpdateState();
      ApplyValue();
      Lambda += Direction == CUIDirection.Straight ? Speed : -(BackSpeed ?? Speed);
      if (Debug) LogStatus();
    }

    public void LogStatus() => CUI.Log($"Active:{Active} Direction:{Direction} Lambda:{Lambda}");

  }
}