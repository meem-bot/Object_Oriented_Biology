using SplashKitSDK;
using System;
using System.Collections.Generic;

/// <summary>
/// Manages the biological cell environment and coordinates the position of its organelles.
/// </summary>
public class Cell
{
    private readonly List<Organelle> _organelles;
    private readonly List<(double X, double Y, double Radius)> _occupiedSpaces;

    /// <summary>
    /// Gets the collection of organelles inside the cell.
    /// </summary>
    public IReadOnlyList<Organelle> Organelles => _organelles.AsReadOnly();

    /// <summary>
    /// Gets the cell's nucleus.
    /// </summary>
    public Nucleus CellNucleus { get; }

    /// <summary>
    /// Gets the absolute X coordinate of the cell.
    /// </summary>
    public double X { get; private set; }

    /// <summary>
    /// Gets the absolute Y coordinate of the cell.
    /// </summary>
    public double Y { get; private set; }

    /// <summary>
    /// Creates a new cell at the specified coordinates.
    /// </summary>
    /// <param name="x">The starting X position.</param>
    /// <param name="y">The starting Y position.</param>
    public Cell(double x, double y)
    {
        X = x;
        Y = y;
        CellNucleus = new Nucleus(0, 0);
        _organelles = new List<Organelle>();
        _occupiedSpaces = new List<(double X, double Y, double Radius)>();

        InitialiseFixedOrganelles();
        PopulateDynamicOrganelles();
    }

    /// <summary>
    /// Sets up fixed organelles like the Endoplasmic Reticulum and Golgi Apparatus at specific relative positions.
    /// </summary>
    private void InitialiseFixedOrganelles()
    {
        RegisterOrganelle(new EndoplasmicReticulum(100, -90), 100, -90, 40);
        RegisterOrganelle(new EndoplasmicReticulum(-110, 80), -110, 80, 40);
        RegisterOrganelle(new GolgiApparatus(-120, -50), -120, -50, 30);
        RegisterOrganelle(new Centrosome(120, 90), 120, 90, 20);
    }

    /// <summary>
    /// Spawns mobile organelles and ensures they do not overlap.
    /// </summary>
    private void PopulateDynamicOrganelles()
    {
        SpawnWithClearance(5, 110, 210, 22, (px, py) => new Mitochondria(px, py, SplashKit.Rnd() * 10));

        SpawnWithClearance(7, 100, 220, 10, (px, py) =>
            new Vacuole(px, py, SplashKit.Rnd() * 10, SplashKit.Rnd() * 4 + 6));

        SpawnWithClearance(5, 100, 220, 8, (px, py) =>
            new Lysosome(px, py, SplashKit.Rnd() * 10, SplashKit.Rnd() * 3 + 5));

        SpawnWithClearance(25, 90, 230, 6, (px, py) =>
            new Ribosome(px, py, SplashKit.Rnd() * 10));
    }

    /// <summary>
    /// Adds an organelle to the cell and reserves its space to prevent overlaps.
    /// </summary>
    /// <param name="organelle">The organelle to add.</param>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <param name="radius">The radius of the space to reserve.</param>
    private void RegisterOrganelle(Organelle organelle, double x, double y, double radius)
    {
        _organelles.Add(organelle);
        _occupiedSpaces.Add((x, y, radius));
    }

    /// <summary>
    /// Spawns a set number of organelles within a specific distance range, checking for empty space.
    /// </summary>
    /// <param name="count">The number of organelles to spawn.</param>
    /// <param name="minD">The minimum distance from the nucleus.</param>
    /// <param name="maxD">The maximum distance from the nucleus.</param>
    /// <param name="clearance">The empty space required around each organelle.</param>
    /// <param name="organelleFactory">A method to create the organelle.</param>
    private void SpawnWithClearance(int count, double minD, double maxD, double clearance, Func<double, double, Organelle> organelleFactory)
    {
        for (int i = 0; i < count; i++)
        {
            for (int attempt = 0; attempt < 200; attempt++)
            {
                double angle = SplashKit.Rnd() * Math.PI * 2;
                double dist = SplashKit.Rnd() * (maxD - minD) + minD;
                double px = Math.Cos(angle) * dist;
                double py = Math.Sin(angle) * dist;

                if (IsSpaceFree(px, py, clearance))
                {
                    RegisterOrganelle(organelleFactory(px, py), px, py, clearance);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Checks if a given position and radius overlap with any existing organelles.
    /// </summary>
    /// <param name="checkX">The X coordinate to check.</param>
    /// <param name="checkY">The Y coordinate to check.</param>
    /// <param name="checkRadius">The required radius.</param>
    /// <returns>True if the space is free, false if it is blocked.</returns>
    private bool IsSpaceFree(double checkX, double checkY, double checkRadius)
    {
        foreach (var space in _occupiedSpaces)
        {
            double dx = space.X - checkX;
            double dy = space.Y - checkY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < (space.Radius + checkRadius + 8)) return false;
        }
        return true;
    }

    /// <summary>
    /// Updates the position of the cell and its nucleus to simulate drifting.
    /// </summary>
    /// <param name="time">The current animation time.</param>
    public void Update(uint time)
    {
        X = 600 + Math.Sin(time / 1500.0) * 2;
        Y = 340 + Math.Cos(time / 1300.0) * 2;

        double pathX = 600 + Math.Sin(time / 3500.0) * 8;
        double pathY = 340 + Math.Cos(time / 4000.0) * 6;

        CellNucleus.MoveTo(pathX, pathY);
    }

    /// <summary>
    /// Draws the cell membrane, cytoplasm, and optionally its internal parts.
    /// </summary>
    /// <param name="time">The current animation time.</param>
    /// <param name="drawCytoskeleton">True to draw internal organelles and the cytoskeleton.</param>
    public void Draw(uint time, bool drawCytoskeleton)
    {
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 20, 147, 255), X, Y, 260);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 215, 255), X, Y, 252);

        if (drawCytoskeleton)
        {
            SplashKit.DrawLine(SplashKit.RGBAColor(210, 210, 160, 60), X - 180, Y - 100, X + 160, Y + 140);
            SplashKit.DrawLine(SplashKit.RGBAColor(210, 210, 160, 60), X - 160, Y + 120, X + 170, Y - 90);
            SplashKit.DrawLine(SplashKit.RGBAColor(210, 210, 160, 60), X - 40, Y - 190, X + 30, Y + 180);

            foreach (var organelle in _organelles)
            {
                organelle.Draw(time, X, Y);
            }

            CellNucleus.Draw(time, X, Y);
        }
    }
}