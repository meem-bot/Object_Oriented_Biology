using SplashKitSDK;
using System;

/// <summary>
/// A clickable UI button.
/// </summary>
public class Button
{
    private double _x, _y, _width, _height;
    private string _text;
    private Font _font;
    private Action _onClick;

    /// <summary>
    /// Creates a new button with the specified text, position, dimensions, and click action.
    /// </summary>
    /// <param name="text">The text to show on the button.</param>
    /// <param name="x">The button's X coordinate.</param>
    /// <param name="y">The button's Y coordinate.</param>
    /// <param name="width">The width of the button.</param>
    /// <param name="height">The height of the button.</param>
    /// <param name="font">The font used for the text.</param>
    /// <param name="onClick">The method to call when the button is clicked.</param>
    public Button(string text, double x, double y, double width, double height, Font font, Action onClick)
    {
        _text = text;
        _x = x;
        _y = y;
        _width = width;
        _height = height;
        _font = font;
        _onClick = onClick;
    }

    /// <summary>
    /// Checks if the mouse is currently over the button.
    /// </summary>
    /// <returns>True if hovered, false otherwise.</returns>
    private bool IsHovered()
    {
        double mx = SplashKit.MouseX();
        double my = SplashKit.MouseY();
        return mx >= _x && mx <= _x + _width && my >= _y && my <= _y + _height;
    }

    /// <summary>
    /// Checks for mouse clicks and runs the click action if the button is clicked.
    /// </summary>
    public void Update()
    {
        if (IsHovered() && SplashKit.MouseClicked(MouseButton.LeftButton))
        {
            _onClick?.Invoke();
        }
    }

    /// <summary>
    /// Draws the button and its text on the screen.
    /// </summary>
    public void Draw()
    {
        bool hovered = IsHovered();
        SplashKit.FillRectangle(hovered ? Color.Gray : Color.DarkGray, _x, _y, _width, _height);
        SplashKit.DrawRectangle(Color.Black, _x, _y, _width, _height);

        if (_font != null)
        {
            int textX = (int)(_x + (_width - SplashKit.TextWidth(_text, _font, 18)) / 2);
            SplashKit.DrawText(_text, Color.Black, _font, 18, textX, _y + 10);
        }
    }
}