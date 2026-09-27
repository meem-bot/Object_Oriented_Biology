using SplashKitSDK;
using System;

/// <summary>
/// Base class for all organelles floating within the cell.
/// </summary>
public abstract class Organelle
{
    protected double OffsetX { get; }
    protected double OffsetY { get; }
    protected double TimeOffset { get; }

    /// <summary>
    /// Sets the starting position offsets for the organelle.
    /// </summary>
    protected Organelle(double offsetX, double offsetY, double timeOffset = 0)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
        TimeOffset = timeOffset;
    }

    /// <summary>
    /// Calculates the drifting movement of the organelle based on the elapsed time.
    /// </summary>
    protected void CalculateDrift(uint time, double cellX, double cellY, double speedX, double speedY, double scale, out double x, out double y)
    {
        x = cellX + OffsetX + Math.Sin(time / speedX + TimeOffset) * scale;
        y = cellY + OffsetY + Math.Cos(time / speedY + TimeOffset) * scale;
    }

    /// <summary>
    /// Draws the organelle on the screen.
    /// </summary>
    public abstract void Draw(uint time, double cellX, double cellY);
}

/// <summary>
/// Represents the cell's nucleus.
/// </summary>
public class Nucleus : Organelle
{
    public Point2D Position { get; private set; }

    /// <summary>
    /// Creates a new nucleus at the given offsets.
    /// </summary>
    public Nucleus(double offsetX, double offsetY) : base(offsetX, offsetY)
    {
        Position = new Point2D() { X = offsetX, Y = offsetY };
    }

    /// <summary>
    /// Updates the exact coordinate position of the nucleus.
    /// </summary>
    public void MoveTo(double x, double y)
    {
        Position = new Point2D() { X = x, Y = y };
    }

    /// <summary>
    /// Draws the nucleus on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), Position.X, Position.Y, 65);
        SplashKit.DrawCircle(SplashKit.RGBAColor(90, 20, 150, 255), Position.X, Position.Y, 65);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), Position.X, Position.Y, 55);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), Position.X, Position.Y, 24);
    }
}

/// <summary>
/// Represents a mitochondrion.
/// </summary>
public class Mitochondria : Organelle
{
    /// <summary>
    /// Creates a new mitochondrion.
    /// </summary>
    public Mitochondria(double offsetX, double offsetY, double timeOffset) : base(offsetX, offsetY, timeOffset) { }

    /// <summary>
    /// Draws the mitochondrion on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 1400.0, 1200.0, 5, out double x, out double y);
        SplashKit.FillEllipse(SplashKit.RGBAColor(40, 120, 200, 220), x - 20, y - 9, 40, 18);
    }
}

/// <summary>
/// Represents a vacuole.
/// </summary>
public class Vacuole : Organelle
{
    private readonly double _radius;

    /// <summary>
    /// Creates a new vacuole with a specific radius.
    /// </summary>
    public Vacuole(double offsetX, double offsetY, double timeOffset, double radius) : base(offsetX, offsetY, timeOffset)
    {
        _radius = radius;
    }

    /// <summary>
    /// Draws the vacuole on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 1800.0, 1600.0, 5, out double x, out double y);
        SplashKit.FillCircle(SplashKit.RGBAColor(190, 150, 230, 190), x, y, _radius);
    }
}

/// <summary>
/// Represents a lysosome.
/// </summary>
public class Lysosome : Organelle
{
    private readonly double _radius;

    /// <summary>
    /// Creates a new lysosome with a specific radius.
    /// </summary>
    public Lysosome(double offsetX, double offsetY, double timeOffset, double radius) : base(offsetX, offsetY, timeOffset)
    {
        _radius = radius;
    }

    /// <summary>
    /// Draws the lysosome on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 1350.0, 1250.0, 4, out double x, out double y);
        SplashKit.FillCircle(SplashKit.RGBAColor(170, 40, 90, 210), x, y, _radius);
    }
}

/// <summary>
/// Represents the endoplasmic reticulum.
/// </summary>
public class EndoplasmicReticulum : Organelle
{
    /// <summary>
    /// Creates a new endoplasmic reticulum.
    /// </summary>
    public EndoplasmicReticulum(double offsetX, double offsetY) : base(offsetX, offsetY) { }

    /// <summary>
    /// Draws the endoplasmic reticulum on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 2000.0, 1800.0, 4, out double erX, out double erY);

        double startX = erX - 31;
        double startY = erY - 21;

        for (int r = 0; r < 5; r++)
        {
            SplashKit.FillRectangle(SplashKit.RGBAColor(230, 90, 110, 220), startX + (r * 8), startY + (r * 9), 30, 6);
        }
    }
}

/// <summary>
/// Represents the Golgi apparatus.
/// </summary>
public class GolgiApparatus : Organelle
{
    /// <summary>
    /// Creates a new Golgi apparatus.
    /// </summary>
    public GolgiApparatus(double offsetX, double offsetY) : base(offsetX, offsetY) { }

    /// <summary>
    /// Draws the Golgi apparatus on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 2100.0, 1900.0, 4, out double gX, out double gY);

        Color golgiColor = SplashKit.RGBAColor(240, 150, 70, 255);
        Color golgiInner = SplashKit.RGBAColor(255, 200, 120, 255);

        for (int i = 0; i < 5; i++)
        {
            double yOffset = (i - 2) * 4;
            double curveWidth = 18 - Math.Abs(i - 2) * 2;

            double leftTipX = gX - curveWidth;
            double leftTipY = gY + yOffset;
            double rightTipX = gX + curveWidth;
            double rightTipY = gY + yOffset;

            SplashKit.FillEllipse(golgiColor, gX - curveWidth, gY + yOffset - 3, curveWidth * 2, 6);
            SplashKit.DrawEllipse(golgiInner, gX - curveWidth, gY + yOffset - 3, curveWidth * 2, 6);

            SplashKit.FillCircle(golgiInner, leftTipX, leftTipY, 3);
            SplashKit.FillCircle(golgiInner, rightTipX, rightTipY, 3);
        }
    }
}

/// <summary>
/// Represents a centrosome.
/// </summary>
public class Centrosome : Organelle
{
    /// <summary>
    /// Creates a new centrosome.
    /// </summary>
    public Centrosome(double offsetX, double offsetY) : base(offsetX, offsetY) { }

    /// <summary>
    /// Draws the centrosome on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 3000.0, 2800.0, 2, out double centX, out double centY);

        SplashKit.FillRectangle(SplashKit.RGBAColor(60, 60, 90, 240), centX - 7, centY - 2.5, 14, 5);
        SplashKit.FillRectangle(SplashKit.RGBAColor(60, 60, 90, 240), centX - 2.5, centY - 7, 5, 14);
    }
}

/// <summary>
/// Represents a ribosome.
/// </summary>
public class Ribosome : Organelle
{
    /// <summary>
    /// Creates a new ribosome.
    /// </summary>
    public Ribosome(double offsetX, double offsetY, double timeOffset) : base(offsetX, offsetY, timeOffset) { }

    /// <summary>
    /// Draws the ribosome on the screen.
    /// </summary>
    public override void Draw(uint time, double cellX, double cellY)
    {
        CalculateDrift(time, cellX, cellY, 1500.0, 1300.0, 4, out double x, out double y);

        SplashKit.FillCircle(Color.Gray, x, y, 6);
        SplashKit.FillCircle(Color.DarkGray, x, y - 4, 4);
    }
}