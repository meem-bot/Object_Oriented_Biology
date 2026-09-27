using SplashKitSDK;
using System;

/// <summary>
/// A simple water molecule.
/// </summary>
public class WaterMolecule : IMolecule
{
    /// <summary>
    /// Draws the water molecule on the screen.
    /// </summary>
    public void Draw(double cx, double cy, uint time)
    {
        SplashKit.FillCircle(Color.Red, cx, cy + 4, 6);
        SplashKit.DrawCircle(Color.DarkGray, cx, cy + 4, 6);
        SplashKit.FillCircle(Color.White, cx - 8, cy - 4, 4);
        SplashKit.DrawCircle(Color.DarkGray, cx - 8, cy - 4, 4);
        SplashKit.FillCircle(Color.White, cx + 8, cy - 4, 4);
        SplashKit.DrawCircle(Color.DarkGray, cx + 8, cy - 4, 4);
        SplashKit.DrawLine(Color.DarkGray, cx, cy + 4, cx - 8, cy - 4);
        SplashKit.DrawLine(Color.DarkGray, cx, cy + 4, cx + 8, cy - 4);
    }
}

/// <summary>
/// Base class for protein structures, handling their size and colour properties.
/// </summary>
public abstract class ProteinBase : IMolecule
{
    protected readonly int Count;
    protected readonly double Scale;
    protected readonly int ColorOffset;

    /// <summary>
    /// Sets up the base protein properties.
    /// </summary>
    /// <param name="count">The number of amino acids.</param>
    /// <param name="scale">The size multiplier.</param>
    /// <param name="colorOffset">The starting index for the colour palette.</param>
    protected ProteinBase(int count, double scale, int colorOffset)
    {
        Count = count;
        Scale = scale;
        ColorOffset = colorOffset;
    }

    /// <summary>
    /// Returns the colour palette used for drawing amino acids.
    /// </summary>
    /// <returns>An array of SplashKit colours.</returns>
    protected Color[] GetAminoColors()
    {
        return new Color[] {
            Color.Red, Color.Blue, Color.Yellow, Color.Orange,
            SplashKit.RGBAColor(200, 50, 255, 255), Color.Cyan,
            SplashKit.RGBAColor(255, 100, 100, 255), SplashKit.RGBAColor(100, 255, 100, 255),
            SplashKit.RGBAColor(100, 100, 255, 255), Color.Magenta,
            SplashKit.RGBAColor(255, 165, 0, 255), Color.LimeGreen
        };
    }

    /// <summary>
    /// Draws the protein on the screen.
    /// </summary>
    public abstract void Draw(double cx, double cy, uint time);
}

/// <summary>
/// A standard, moving protein chain.
/// </summary>
public class CanonicalProtein : ProteinBase
{
    /// <summary>
    /// Creates a new canonical protein.
    /// </summary>
    public CanonicalProtein(int count = 45, double scale = 1.0, int colorOffset = 0)
        : base(count, scale, colorOffset) { }

    /// <summary>
    /// Draws the canonical protein on the screen.
    /// </summary>
    public override void Draw(double cx, double cy, uint time)
    {
        Point2D[] chain = new Point2D[Count];
        Color[] colors = GetAminoColors();

        for (int i = 0; i < Count; i++)
        {
            double t1 = i * 0.35;
            double t2 = i * 0.22;
            chain[i] = new Point2D()
            {
                X = cx + (Math.Sin(t1) * 45 + Math.Sin(t2) * 20 + Math.Sin(time / 500.0 + i) * 2) * Scale,
                Y = cy + (Math.Cos(t1) * 45 + Math.Cos(t2) * 20 + Math.Cos(time / 400.0 + i) * 2) * Scale
            };
        }

        for (int i = 0; i < Count - 1; i++)
        {
            SplashKit.DrawLine(Color.LightGray, chain[i].X, chain[i].Y, chain[i + 1].X, chain[i + 1].Y);
            if (Scale >= 1.0)
            {
                SplashKit.DrawLine(Color.LightGray, chain[i].X + 1, chain[i].Y, chain[i + 1].X + 1, chain[i + 1].Y);
                SplashKit.DrawLine(Color.LightGray, chain[i].X, chain[i].Y + 1, chain[i + 1].X, chain[i + 1].Y + 1);
            }
        }

        for (int i = 0; i < Count; i++)
        {
            double radius = Scale < 0.5 ? 2 : 12 * Scale;
            SplashKit.FillCircle(colors[(i + ColorOffset) % colors.Length], chain[i].X, chain[i].Y, radius);
            SplashKit.DrawCircle(Color.DarkGray, chain[i].X, chain[i].Y, radius);
        }
    }
}

/// <summary>
/// An alternative protein variant with a different folding shape.
/// </summary>
public class VariantProtein : ProteinBase
{
    /// <summary>
    /// Creates a new variant protein.
    /// </summary>
    public VariantProtein(int count = 35, double scale = 1.0, int colorOffset = 4)
        : base(count, scale, colorOffset) { }

    /// <summary>
    /// Draws the variant protein on the screen.
    /// </summary>
    public override void Draw(double cx, double cy, uint time)
    {
        Point2D[] chain = new Point2D[Count];
        Color[] colors = GetAminoColors();

        for (int i = 0; i < Count; i++)
        {
            double t1 = i * 0.5;
            double t2 = i * 0.3;
            chain[i] = new Point2D()
            {
                X = cx + (Math.Cos(t1) * 45 + Math.Sin(t2) * 20 + Math.Sin(time / 500.0 + i) * 2) * Scale,
                Y = cy + (Math.Sin(t1) * 40 + Math.Cos(t2) * 15 + Math.Cos(time / 400.0 + i) * 2) * Scale
            };
        }

        for (int i = 0; i < Count - 1; i++)
        {
            SplashKit.DrawLine(Color.LightGray, chain[i].X, chain[i].Y, chain[i + 1].X, chain[i + 1].Y);
        }

        for (int i = 0; i < Count; i++)
        {
            double radius = Scale < 0.5 ? 2 : 11 * Scale;
            SplashKit.FillCircle(colors[(i + ColorOffset) % colors.Length], chain[i].X, chain[i].Y, radius);
            SplashKit.DrawCircle(Color.DarkGray, chain[i].X, chain[i].Y, radius);
        }
    }
}

/// <summary>
/// A plain, grey peptide chain used to represent an inactive template.
/// </summary>
public class StaticPeptide : IMolecule
{
    private readonly int _count;
    private readonly double _scale;

    /// <summary>
    /// Creates a new static peptide.
    /// </summary>
    public StaticPeptide(int count = 30, double scale = 1.0)
    {
        _count = count;
        _scale = scale;
    }

    /// <summary>
    /// Draws the static peptide on the screen.
    /// </summary>
    public void Draw(double cx, double cy, uint time)
    {
        Point2D[] chain = new Point2D[_count];
        for (int i = 0; i < _count; i++)
        {
            double t1 = i * 0.45;
            double t2 = i * 0.25;
            chain[i] = new Point2D()
            {
                X = cx + (Math.Sin(t1) * 40 + Math.Cos(t2) * 12) * _scale,
                Y = cy + (Math.Cos(t1) * 40 + Math.Sin(t2) * 12) * _scale
            };
        }

        for (int i = 0; i < _count - 1; i++)
        {
            SplashKit.DrawLine(Color.DarkGray, chain[i].X, chain[i].Y, chain[i + 1].X, chain[i + 1].Y);
        }

        for (int i = 0; i < _count; i++)
        {
            byte shade = (byte)(90 + (i * 4) % 110);
            Color grey = SplashKit.RGBAColor(shade, shade, shade, 255);
            double radius = Math.Max(3, 9 * _scale);
            SplashKit.FillCircle(grey, chain[i].X, chain[i].Y, radius);
            SplashKit.DrawCircle(Color.DarkGray, chain[i].X, chain[i].Y, radius);
        }
    }
}