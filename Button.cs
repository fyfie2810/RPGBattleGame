using SplashKitSDK;
using System;

public class Button
{

  private Font _MainFont;

  public Button()
  {
  _MainFont = new Font("MainFont", "pixels.ttf");
  }

  public int X { get; set; }
  public int Y { get; set; }
  public int Width { get; set; }
  public int Height { get; set; }

  public string Caption { get; set; }

  public void Draw()
  {
    SplashKit.FillRectangle(Color.DarkBlue, X, Y, Width, Height);
    SplashKit.DrawRectangle(Color.White, X, Y, Width, Height);
    SplashKit.DrawText(Caption, Color.White, _MainFont, 30, X + 5, Y + 5);
  }

  public Rectangle Rectangle
  {
    get
    {
      return new Rectangle() { X = X, Y = Y, Width = Width, Height = Height };
    }
  }

  public bool IsMouseHover
  {
    get
    {
      return SplashKit.PointInRectangle(SplashKit.MousePosition(), Rectangle);
    }
  }

  public event ClickAction OnClick;

  public void HandleInput()
  {
    if (SplashKit.MouseClicked(MouseButton.LeftButton) && IsMouseHover)
    {
      if (OnClick != null)
        OnClick(this);
    }
  }

  public void ChangeInput(string Message)
  {
    Caption = Message;
  }

}