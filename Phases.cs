using SplashKitSDK;
using System;
using System.Collections.Generic;

/// <summary>
/// Base class for all presentation slides.
/// </summary>
public abstract class Phase
{
    protected Font _textFont;
    protected Font _boldFont;

    public string ChapterName { get; protected set; } = string.Empty;
    public string BioText { get; protected set; } = string.Empty;
    public string OopText { get; protected set; } = string.Empty;

    /// <summary>
    /// Loads the fonts used in the phase.
    /// </summary>
    public Phase()
    {
        _textFont = SplashKit.LoadFont("Arial", "arial.ttf");
        _boldFont = SplashKit.LoadFont("ArialBold", "arialbd.ttf");
    }

    /// <summary>
    /// Updates the slide's state.
    /// </summary>
    /// <param name="time">The animation timer in milliseconds.</param>
    /// <param name="cell">The global cell instance.</param>
    public abstract void Update(uint time, Cell cell);

    /// <summary>
    /// Draws the slide.
    /// </summary>
    /// <param name="time">The animation timer.</param>
    /// <param name="cell">The global cell instance.</param>
    public abstract void Draw(uint time, Cell cell);

    /// <summary>
    /// Draws the legend on the right side of the screen.
    /// </summary>
    public virtual void DrawLegend()
    {
        if (_textFont == null) return;

        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;

        SplashKit.DrawCircle(SplashKit.RGBAColor(255, 20, 147, 255), 960, y, 10);
        SplashKit.DrawText("Cell Membrane", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), 960, y, 11);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), 960, y, 3);
        SplashKit.DrawText("Nucleus", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        SplashKit.FillCircle(Color.Gray, 960, y, 6);
        SplashKit.FillCircle(Color.DarkGray, 960, y - 4, 4);
        SplashKit.DrawText("Ribosomes", Color.White, _textFont, 16, 985, y - 7);
    }

    /// <summary>
    /// Draws wrapped text at the bottom of the screen.
    /// </summary>
    public void DrawSubtitles()
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(35, 40, 50, 245), 0, 660, 1200, 140);

        if (_boldFont != null)
        {
            const double safeLeft = 160;
            const double safeRight = 1040;
            const double maxWidth = safeRight - safeLeft;

            var bioLines = WrapText(BioText, _boldFont, 18, maxWidth);
            var oopLines = WrapText(OopText, _boldFont, 18, maxWidth);

            int lineHeight = 24;
            int totalLines = bioLines.Count + oopLines.Count;
            double totalHeight = totalLines * lineHeight + (bioLines.Count > 0 && oopLines.Count > 0 ? 6 : 0);
            double currentY = 730 - (totalHeight / 2.0);

            foreach (var line in bioLines)
            {
                double lineWidth = SplashKit.TextWidth(line, _boldFont, 18);
                double lineX = safeLeft + (maxWidth - lineWidth) / 2.0;
                SplashKit.DrawText(line, Color.LightGreen, _boldFont, 18, lineX, currentY);
                currentY += lineHeight;
            }

            if (bioLines.Count > 0 && oopLines.Count > 0)
            {
                currentY += 4;
            }

            foreach (var line in oopLines)
            {
                double lineWidth = SplashKit.TextWidth(line, _boldFont, 18);
                double lineX = safeLeft + (maxWidth - lineWidth) / 2.0;
                SplashKit.DrawText(line, Color.Pink, _boldFont, 18, lineX, currentY);
                currentY += lineHeight;
            }
        }
    }

    /// <summary>
    /// Splits text into multiple lines to fit a specific width.
    /// </summary>
    private List<string> WrapText(string text, Font font, int size, double maxWidth)
    {
        List<string> lines = new List<string>();
        if (string.IsNullOrEmpty(text)) return lines;

        string[] words = text.Split(' ');
        string currentLine = "";

        foreach (string word in words)
        {
            string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
            if (SplashKit.TextWidth(testLine, font, size) > maxWidth)
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    lines.Add(currentLine);
                }
                currentLine = word;
            }
            else
            {
                currentLine = testLine;
            }
        }
        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine);
        }
        return lines;
    }
}

/// <summary>
/// Shows the title screen.
/// </summary>
public class IntroPhase : Phase
{
    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the title text.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        string t1 = "OBJECT-ORIENTED BIOLOGY";
        string t2 = "An Animated Guide to Key Concepts in Object-Oriented Development";

        if (_textFont != null)
        {
            SplashKit.DrawText(t1, Color.White, _textFont, 36, (1200 - SplashKit.TextWidth(t1, _textFont, 36)) / 2, 350);
            SplashKit.DrawText(t2, Color.LightGray, _textFont, 20, (1200 - SplashKit.TextWidth(t2, _textFont, 20)) / 2, 410);
        }
    }

    /// <summary>
    /// Empty legend for the intro screen.
    /// </summary>
    public override void DrawLegend() { }
}

/// <summary>
/// Shows the complete cell and acts as a map for the user.
/// </summary>
public class IntroductionPhase : Phase
{
    /// <summary>
    /// Sets up the introduction text.
    /// </summary>
    public IntroductionPhase()
    {
        ChapterName = "Introduction";
        BioText = "Here is a simplified 2D cross section of a typical animal cell.";
        OopText = "Much like a cell, a computer program is a structured ecosystem composed of collaborating components.";
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { cell.Update(time); }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell) { cell.Draw(time, true); }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;

        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;

        SplashKit.DrawCircle(SplashKit.RGBAColor(255, 20, 147, 255), 960, y, 10);
        SplashKit.DrawText("Cell Membrane", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 215, 255), 960, y, 10);
        SplashKit.DrawText("Cytoplasm Fluid", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), 960, y, 11);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), 960, y, 3);
        SplashKit.DrawText("Nucleus", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillCircle(Color.Gray, 960, y, 6);
        SplashKit.FillCircle(Color.DarkGray, 960, y - 4, 4);
        SplashKit.DrawText("Ribosomes", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillEllipse(SplashKit.RGBAColor(40, 120, 200, 220), 950, y - 5, 20, 10);
        SplashKit.DrawText("Mitochondria", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillCircle(SplashKit.RGBAColor(190, 150, 230, 190), 960, y, 8);
        SplashKit.DrawText("Vacuoles", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        double erScale = 0.35;
        double erStartX = 960 - (15 * erScale);
        double erStartY = y - (10 * erScale);
        for (int r = 0; r < 5; r++)
        {
            SplashKit.FillRectangle(SplashKit.RGBAColor(230, 90, 110, 220), erStartX + (r * 8 * erScale), erStartY + (r * 9 * erScale), 30 * erScale, 6 * erScale);
        }
        SplashKit.DrawText("Endoplasmic Reticulum", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        Color golgiColor = SplashKit.RGBAColor(240, 150, 70, 255);
        Color golgiInner = SplashKit.RGBAColor(255, 200, 120, 255);
        for (int i = 0; i < 5; i++)
        {
            double yOffset = (i - 2) * 3;
            double curveWidth = (15 - Math.Abs(i - 2));
            SplashKit.FillEllipse(golgiColor, 960 - curveWidth, y + yOffset - 2, curveWidth * 2, 4);
            SplashKit.DrawEllipse(golgiInner, 960 - curveWidth, y + yOffset - 2, curveWidth * 2, 4);
        }
        SplashKit.DrawText("Golgi Apparatus", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillCircle(SplashKit.RGBAColor(170, 40, 90, 210), 960, y, 8);
        SplashKit.DrawText("Lysosomes", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.FillRectangle(SplashKit.RGBAColor(60, 60, 90, 240), 952, y - 4, 16, 8);
        SplashKit.DrawText("Centrosome", Color.White, _textFont, 14, 985, y - 7);
        y += 32;

        SplashKit.DrawLine(SplashKit.RGBAColor(210, 210, 160, 200), 950, y, 970, y);
        SplashKit.DrawText("Cytoskeleton", Color.White, _textFont, 14, 985, y - 7);
    }
}

/// <summary>
/// Explains how DNA acts like a programming class.
/// </summary>
public class ClassesPhase : Phase
{
    /// <summary>
    /// Sets up the classes phase text.
    /// </summary>
    public ClassesPhase()
    {
        ChapterName = "Classes";
        BioText = "In the nucleus, DNA acts as the blueprint containing instructions to synthesise a specific molecule.";
        OopText = "In programming, a class is the blueprint that holds instructions for building objects.";
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { cell.Update(time); }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        cell.Draw(time, true);
        double pathX = cell.CellNucleus.Position.X;
        double pathY = cell.CellNucleus.Position.Y;

        SplashKit.FillRectangle(SplashKit.RGBAColor(0, 0, 0, 180), 280, 0, 640, 660);

        for (int i = 0; i < 12; i++)
        {
            double xOffset = Math.Sin(time / 300.0 + i) * 20;
            double xOffsetOpposite = Math.Sin(time / 300.0 + i + Math.PI) * 20;
            double curY = pathY - 40 + (i * 7);

            SplashKit.FillCircle(Color.Cyan, pathX + xOffset, curY, 3);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), pathX + xOffsetOpposite, curY, 3);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), pathX + xOffset, curY, pathX + xOffsetOpposite, curY);
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), 960, y, 11);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), 960, y, 3);
        SplashKit.DrawText("Nucleus", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        for (int i = 0; i < 5; i++)
        {
            double xOff = Math.Sin(i * 1.2) * 5;
            SplashKit.FillCircle(Color.Cyan, 960 + xOff, y - 8 + (i * 4), 2);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), 960 - xOff, y - 8 + (i * 4), 2);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), 960 + xOff, y - 8 + (i * 4), 960 - xOff, y - 8 + (i * 4));
        }
        SplashKit.DrawText("DNA", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains class inheritance using germline DNA.
/// </summary>
public class ClassInheritancePhase : Phase
{
    private readonly IMolecule _staticPeptide;

    /// <summary>
    /// Sets up the class inheritance text and peptide model.
    /// </summary>
    public ClassInheritancePhase()
    {
        ChapterName = "Class Inheritance";
        BioText = "The germline DNA template acts as an abstract blueprint for constructing proteins, with foundational traits that will be shared by all descendant proteins.";
        OopText = "The efficient programmer creates an abstract parent class that defines core, shared features that subclasses will inherit automatically.";
        _staticPeptide = new StaticPeptide(30, 1.0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { cell.Update(time); }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        cell.Draw(time, true);
        double pathX = cell.CellNucleus.Position.X;
        double pathY = cell.CellNucleus.Position.Y;

        SplashKit.FillRectangle(SplashKit.RGBAColor(0, 0, 0, 180), 280, 0, 640, 660);

        for (int i = 0; i < 12; i++)
        {
            double xOffset = Math.Sin(time / 300.0 + i) * 20;
            double xOffsetOpposite = Math.Sin(time / 300.0 + i + Math.PI) * 20;
            double curY = pathY - 40 + (i * 7);

            SplashKit.FillCircle(Color.Cyan, pathX + xOffset, curY, 3);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), pathX + xOffsetOpposite, curY, 3);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), pathX + xOffset, curY, pathX + xOffsetOpposite, curY);
        }

        double boxX = 300;
        double boxY = 40;
        double boxW = 180;
        double boxH = 170;
        double boxCenterX = boxX + boxW / 2;
        double boxCenterY = boxY + boxH / 2;
        double angle11 = -5.0 * Math.PI / 6.0;
        double nucEdgeX = pathX + Math.Cos(angle11) * 65;
        double nucEdgeY = pathY + Math.Sin(angle11) * 65;

        Color neonGreen = SplashKit.RGBAColor(57, 255, 20, 255);

        for (int i = -1; i <= 1; i++)
        {
            SplashKit.DrawLine(neonGreen, nucEdgeX, nucEdgeY + i, boxX + boxW, boxY + boxH + i);
        }

        SplashKit.FillRectangle(SplashKit.RGBAColor(40, 50, 70, 255), boxX, boxY, boxW, boxH);
        SplashKit.DrawRectangle(neonGreen, boxX, boxY, boxW, boxH);

        _staticPeptide.Draw(boxCenterX, boxCenterY + 10, time);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), 960, y, 11);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), 960, y, 3);
        SplashKit.DrawText("Nucleus", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        for (int i = 0; i < 5; i++)
        {
            double xOff = Math.Sin(i * 1.2) * 5;
            SplashKit.FillCircle(Color.Cyan, 960 + xOff, y - 8 + (i * 4), 2);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), 960 - xOff, y - 8 + (i * 4), 2);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), 960 + xOff, y - 8 + (i * 4), 960 - xOff, y - 8 + (i * 4));
        }
        SplashKit.DrawText("DNA", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        IMolecule miniPeptide = new StaticPeptide(30, 0.3);
        miniPeptide.Draw(960, y, 0);

        SplashKit.DrawText("Germline Template", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains objects using protein creation.
/// </summary>
public class ObjectsPhase : Phase
{
    private readonly IMolecule _staticPeptide;
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the objects phase text and models.
    /// </summary>
    public ObjectsPhase()
    {
        ChapterName = "Objects";
        BioText = "The mRNA transmits the blueprint to ribosome to construct a new, live protein in the cell.";
        OopText = "Similarly, objects are the real, active instances constructed from the blueprint and placed in memory.";
        _staticPeptide = new StaticPeptide(30, 1.0);
        _canonicalProtein = new CanonicalProtein(35, 0.25, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { cell.Update(time); }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        cell.Draw(time, true);
        double pathX = cell.CellNucleus.Position.X;
        double pathY = cell.CellNucleus.Position.Y;

        SplashKit.FillRectangle(SplashKit.RGBAColor(0, 0, 0, 180), 280, 0, 640, 660);

        for (int i = 0; i < 12; i++)
        {
            double xOffset = Math.Sin(time / 300.0 + i) * 20;
            double xOffsetOpposite = Math.Sin(time / 300.0 + i + Math.PI) * 20;
            double curY = pathY - 40 + (i * 7);

            SplashKit.FillCircle(Color.Cyan, pathX + xOffset, curY, 3);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), pathX + xOffsetOpposite, curY, 3);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), pathX + xOffset, curY, pathX + xOffsetOpposite, curY);
        }

        double boxX = 300;
        double boxY = 40;
        double boxW = 180;
        double boxH = 170;
        double boxCenterX = boxX + boxW / 2;
        double boxCenterY = boxY + boxH / 2;

        double angle11 = -5.0 * Math.PI / 6.0;
        double nucEdgeX11 = pathX + Math.Cos(angle11) * 65;
        double nucEdgeY11 = pathY + Math.Sin(angle11) * 65;

        Color neonGreen = SplashKit.RGBAColor(57, 255, 20, 255);

        for (int i = -1; i <= 1; i++)
        {
            SplashKit.DrawLine(neonGreen, nucEdgeX11, nucEdgeY11 + i, boxX + boxW, boxY + boxH + i);
        }

        SplashKit.FillRectangle(SplashKit.RGBAColor(40, 50, 70, 255), boxX, boxY, boxW, boxH);
        SplashKit.DrawRectangle(neonGreen, boxX, boxY, boxW, boxH);
        _staticPeptide.Draw(boxCenterX, boxCenterY + 10, time);

        double dx = (boxX + boxW) - nucEdgeX11;
        double dy = (boxY + boxH) - nucEdgeY11;
        double targetLineLength = Math.Sqrt(dx * dx + dy * dy);
        double angle5 = Math.PI / 3.0;
        double nucEdgeX5 = pathX + Math.Cos(angle5) * 65;
        double nucEdgeY5 = pathY + Math.Sin(angle5) * 65;
        double endX = nucEdgeX5 + Math.Cos(angle5) * targetLineLength;
        double endY = nucEdgeY5 + Math.Sin(angle5) * targetLineLength;

        Color blueColor = SplashKit.RGBAColor(0, 150, 255, 255);

        for (int i = -1; i <= 1; i++)
        {
            SplashKit.DrawLine(blueColor, nucEdgeX5, nucEdgeY5 + i, endX, endY + i);
        }

        double ribX = endX + Math.Cos(angle5) * 22;
        double ribY = endY + Math.Sin(angle5) * 22;
        double cycle = (time % 3000) / 3000.0;
        double driftDist = 80;
        double angle9 = Math.PI;
        double edgeX = ribX + Math.Cos(angle9) * 20;
        double edgeY = ribY + Math.Sin(angle9) * 20;
        double currentX = edgeX + Math.Cos(angle9) * (cycle * driftDist);
        double currentY = edgeY + Math.Sin(angle9) * (cycle * driftDist);

        byte alpha = cycle > 0.75 ? (byte)(255 * (1.0 - (cycle - 0.75) / 0.25)) : (byte)255;

        _canonicalProtein.Draw(currentX, currentY, time);

        SplashKit.FillCircle(Color.DarkGray, ribX, ribY, 22.5);
        SplashKit.FillCircle(Color.Gray, ribX, ribY - 14, 15);

        double arrowSize = 12;
        double a1 = angle5 - Math.PI / 6.0;
        double a2 = angle5 + Math.PI / 6.0;
        SplashKit.FillTriangle(blueColor, endX, endY,
            endX - Math.Cos(a1) * arrowSize, endY - Math.Sin(a1) * arrowSize,
            endX - Math.Cos(a2) * arrowSize, endY - Math.Sin(a2) * arrowSize);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        SplashKit.FillCircle(SplashKit.RGBAColor(130, 50, 200, 255), 960, y, 11);
        SplashKit.FillCircle(SplashKit.RGBAColor(180, 110, 240, 255), 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 180, 0, 255), 960, y, 3);
        SplashKit.DrawText("Nucleus", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        for (int i = 0; i < 5; i++)
        {
            double xOff = Math.Sin(i * 1.2) * 5;
            SplashKit.FillCircle(Color.Cyan, 960 + xOff, y - 8 + (i * 4), 2);
            SplashKit.FillCircle(SplashKit.RGBAColor(255, 80, 80, 255), 960 - xOff, y - 8 + (i * 4), 2);
            SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 100), 960 + xOff, y - 8 + (i * 4), 960 - xOff, y - 8 + (i * 4));
        }
        SplashKit.DrawText("DNA", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        IMolecule miniPeptide = new StaticPeptide(30, 0.3);
        miniPeptide.Draw(960, y, 0);
        SplashKit.DrawText("Germline Template", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        Color blueColor = SplashKit.RGBAColor(0, 150, 255, 255);
        for (int i = -1; i <= 1; i++)
        {
            SplashKit.DrawLine(blueColor, 950, y + i, 970, y + i);
        }
        SplashKit.DrawText("mRNA", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        SplashKit.FillCircle(Color.DarkGray, 960, y, 6);
        SplashKit.FillCircle(Color.Gray, 960, y - 4, 4);
        SplashKit.DrawText("Ribosome", Color.White, _textFont, 16, 985, y - 7);
        y += 45;

        IMolecule miniProtein = new CanonicalProtein(35, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains private fields using amino acids.
/// </summary>
public class FieldsPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the fields phase text and model.
    /// </summary>
    public FieldsPhase()
    {
        ChapterName = "Fields";
        BioText = "A specific sequence of amino acids forms the internal structure of the protein.";
        OopText = "In programming, fields are variables that store internal data and are hidden inside an object.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cx = 600;
        double cy = 340;
        _canonicalProtein.Draw(cx, cy, time);

        int targetIndex = 8;
        double t1 = targetIndex * 0.35;
        double t2 = targetIndex * 0.22;
        double beadX = cx + Math.Sin(t1) * 45 + Math.Sin(t2) * 20 + Math.Sin(time / 500.0 + targetIndex) * 2;
        double beadY = cy + Math.Cos(t1) * 45 + Math.Cos(t2) * 20 + Math.Cos(time / 400.0 + targetIndex) * 2;

        double textX = cx + 110;
        double textY = cy - 90;

        double angle = Math.Atan2(beadY - (textY + 20), beadX - (textX + 30));
        double surfaceX = beadX - Math.Cos(angle) * 15;
        double surfaceY = beadY - Math.Sin(angle) * 15;

        SplashKit.DrawLine(Color.White, textX + 30, textY + 20, surfaceX, surfaceY);

        double arrowSize = 10;
        double a1 = angle - Math.PI / 6.0;
        double a2 = angle + Math.PI / 6.0;
        SplashKit.FillTriangle(Color.White, surfaceX, surfaceY,
            surfaceX - Math.Cos(a1) * arrowSize, surfaceY - Math.Sin(a1) * arrowSize,
            surfaceX - Math.Cos(a2) * arrowSize, surfaceY - Math.Sin(a2) * arrowSize);

        if (_boldFont != null)
        {
            SplashKit.DrawText("Amino acid", Color.White, _boldFont, 16, textX, textY);
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains properties using protein receptor sites.
/// </summary>
public class PropertiesPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the properties phase text and model.
    /// </summary>
    public PropertiesPhase()
    {
        ChapterName = "Properties";
        BioText = "Exposed receptor sites on the protein's surface provide controlled, strictly validated access to its internal chemical state.";
        OopText = "In programming, properties provide controlled getter and setter logic to safely inspect or modify private fields.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cx = 600;
        double cy = 340;

        _canonicalProtein.Draw(cx, cy, time);

        double pocketX = 0;
        double pocketY = 0;

        for (int i = 4; i <= 6; i++)
        {
            double t1 = i * 0.35;
            double t2 = i * 0.22;
            pocketX += cx + Math.Sin(t1) * 45 + Math.Sin(t2) * 20 + Math.Sin(time / 500.0 + i) * 2;
            pocketY += cy + Math.Cos(t1) * 45 + Math.Cos(t2) * 20 + Math.Cos(time / 400.0 + i) * 2;
        }

        pocketX /= 3.0;
        pocketY /= 3.0;

        double outAngle = Math.Atan2(pocketY - cy, pocketX - cx);
        pocketX += Math.Cos(outAngle) * 8;
        pocketY += Math.Sin(outAngle) * 8;

        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 0, 80), pocketX, pocketY, 12);
        SplashKit.DrawCircle(Color.Yellow, pocketX, pocketY, 12);

        double cycle = (time % 3000) / 3000.0;
        double progress = cycle < 0.5 ? cycle * 2 : 1.0;

        double startX = pocketX + 160;
        double startY = pocketY + 80;

        double approachAngle = Math.Atan2(startY - pocketY, startX - pocketX);
        double dockX = pocketX + Math.Cos(approachAngle) * 2;
        double dockY = pocketY + Math.Sin(approachAngle) * 2;

        double ligandX = startX - (progress * (startX - dockX));
        double ligandY = startY - (progress * (startY - dockY));

        Color neutralPink = SplashKit.RGBAColor(220, 160, 180, 255);
        SplashKit.FillCircle(neutralPink, ligandX, ligandY, 10);
        SplashKit.DrawCircle(Color.White, ligandX, ligandY, 10);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        Color neutralPink = SplashKit.RGBAColor(220, 160, 180, 255);
        SplashKit.FillCircle(neutralPink, 960, y, 8);
        SplashKit.DrawCircle(Color.White, 960, y, 8);
        SplashKit.DrawText("Ligand", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 0, 80), 960, y, 6);
        SplashKit.DrawCircle(Color.Yellow, 960, y, 6);
        SplashKit.DrawText("Receptor Site", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains interface inheritance using protein evolution.
/// </summary>
public class InterfaceInheritancePhase : Phase
{
    private readonly IMolecule _proteinA;
    private readonly IMolecule _proteinB;

    /// <summary>
    /// Sets up the interface inheritance phase text and models.
    /// </summary>
    public InterfaceInheritancePhase()
    {
        ChapterName = "Interface Inheritance";
        BioText = "Unrelated types of proteins can evolve identical features, guaranteeing they both fulfill a specific function.";
        OopText = "Likewise, inherited interfaces force completely different classes to share the same abilities.";
        _proteinA = new CanonicalProtein(40, 1.0, 0);
        _proteinB = new VariantProtein(35, 1.0, 4);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cy = 340;
        double cx1 = 450;
        double cx2 = 750;

        _proteinA.Draw(cx1, cy, time);
        _proteinB.Draw(cx2, cy, time);

        double p1X = cx1;
        double p1Y = cy - 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 0, 80), p1X, p1Y, 12);
        SplashKit.DrawCircle(Color.Yellow, p1X, p1Y, 12);

        double p2X = cx2;
        double p2Y = cy - 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 0, 80), p2X, p2Y, 12);
        SplashKit.DrawCircle(Color.Yellow, p2X, p2Y, 12);

        double cycle = (time % 3000) / 3000.0;
        double progress = cycle < 0.5 ? cycle * 2 : 1.0;

        double startY = cy - 140;
        double ligandY1 = startY + (progress * (p1Y - startY));
        double ligandY2 = startY + (progress * (p2Y - startY));

        Color neutralPink = SplashKit.RGBAColor(220, 160, 180, 255);
        SplashKit.FillCircle(neutralPink, p1X, ligandY1, 10);
        SplashKit.DrawCircle(Color.White, p1X, ligandY1, 10);

        SplashKit.FillCircle(neutralPink, p2X, ligandY2, 10);
        SplashKit.DrawCircle(Color.White, p2X, ligandY2, 10);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProteinA = new CanonicalProtein(40, 0.2, 0);
        miniProteinA.Draw(960, y, 0);
        SplashKit.DrawText("Protein A", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        IMolecule miniProteinB = new VariantProtein(35, 0.2, 4);
        miniProteinB.Draw(960, y, 0);
        SplashKit.DrawText("Protein B", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        Color neutralPink = SplashKit.RGBAColor(220, 160, 180, 255);
        SplashKit.FillCircle(neutralPink, 960, y, 8);
        SplashKit.DrawCircle(Color.White, 960, y, 8);
        SplashKit.DrawText("Ligand", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(255, 255, 0, 80), 960, y, 6);
        SplashKit.DrawCircle(Color.Yellow, 960, y, 6);
        SplashKit.DrawText("Binding Pocket", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains methods using energy and protein movement.
/// </summary>
public class MethodsPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the methods phase text and model.
    /// </summary>
    public MethodsPhase()
    {
        ChapterName = "Methods";
        BioText = "A protein is built with certain abilities, including movement, which is triggered by ingesting adenosine triphosphate.";
        OopText = "Programmed methods are the action commands that trigger an object to perform a task, such as moving.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cycle = (time % 4000) / 4000.0;
        double proteinX = 500;
        double cy = 340;
        bool movingRight = cycle < 0.5;

        double localCycle = movingRight ? cycle * 2.0 : (cycle - 0.5) * 2.0;

        if (movingRight)
        {
            if (localCycle > 0.4)
            {
                double moveProgress = (localCycle - 0.4) / 0.6;
                moveProgress = moveProgress < 0.5 ? 2 * moveProgress * moveProgress : 1 - Math.Pow(-2 * moveProgress + 2, 2) / 2;
                proteinX = 500 + (moveProgress * 200);
            }
            else proteinX = 500;
        }
        else
        {
            if (localCycle > 0.4)
            {
                double moveProgress = (localCycle - 0.4) / 0.6;
                moveProgress = moveProgress < 0.5 ? 2 * moveProgress * moveProgress : 1 - Math.Pow(-2 * moveProgress + 2, 2) / 2;
                proteinX = 700 - (moveProgress * 200);
            }
            else proteinX = 700;
        }

        if (localCycle > 0.4 && localCycle < 0.95)
        {
            double startLine = movingRight ? 500 : 700;
            double lineEndX = proteinX + (movingRight ? 95 : -95);

            for (int i = -2; i <= 2; i++)
            {
                SplashKit.DrawLine(SplashKit.RGBAColor(255, 255, 255, 180), startLine, cy + i, lineEndX, cy + i);
            }

            double arrowTipX = proteinX + (movingRight ? 110 : -110);
            double arrowBaseX = proteinX + (movingRight ? 95 : -95);
            SplashKit.FillTriangle(Color.White, arrowTipX, cy, arrowBaseX, cy - 10, arrowBaseX, cy + 10);
        }

        if (localCycle <= 0.3)
        {
            double atpProgress = localCycle / 0.3;
            double atpX = movingRight ? 500 : 700;
            double atpY = 100 + (atpProgress * 160);

            SplashKit.FillCircle(Color.Cyan, atpX, atpY, 10);
            SplashKit.FillCircle(Color.Yellow, atpX + 12, atpY, 5);
            SplashKit.FillCircle(Color.Yellow, atpX + 24, atpY, 5);
            SplashKit.FillCircle(Color.Yellow, atpX + 36, atpY, 5);
        }

        if (localCycle > 0.3 && localCycle <= 0.4)
        {
            double flashIntensity = 1.0 - ((localCycle - 0.3) / 0.1);
            double flashX = movingRight ? 500 : 700;
            SplashKit.FillCircle(SplashKit.RGBAColor(0, 255, 255, (byte)(255 * flashIntensity)), flashX, 260, 40 * (1.5 - flashIntensity));
        }

        _canonicalProtein.Draw(proteinX, cy, time);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Cyan, 946, y, 6);
        SplashKit.FillCircle(Color.Yellow, 954, y, 3);
        SplashKit.FillCircle(Color.Yellow, 962, y, 3);
        SplashKit.FillCircle(Color.Yellow, 970, y, 3);
        SplashKit.DrawText("ATP (Energy)", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        for (int i = -2; i <= 2; i++)
        {
            SplashKit.DrawLine(Color.White, 945, y + i, 965, y + i);
        }
        SplashKit.FillTriangle(Color.White, 975, y, 965, y - 6, 965, y + 6);
        SplashKit.DrawText("Movement Vector", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains control flow using biochemical pathways.
/// </summary>
public class ControlFlowPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the control flow phase text and model.
    /// </summary>
    public ControlFlowPhase()
    {
        ChapterName = "Control Flow";
        BioText = "Biochemical pathways act as conditional logic, routing molecules down different reaction chains based on specific chemical conditions.";
        OopText = "In a program, control flow uses 'if/else' rules to decide which path the code will take.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cx = 600;
        double cy = 340;

        bool isIfBranch = (time % 5000) < 2500;
        double phaseTime = (time % 2500) / 2500.0;

        double inProgress = phaseTime < 0.5 ? phaseTime * 2 : 1.0;
        double outProgress = phaseTime >= 0.5 ? (phaseTime - 0.5) * 2 : 0.0;

        double inputX = cx - 180 + (inProgress * 180);
        if (phaseTime < 0.5)
        {
            SplashKit.DrawLine(Color.DarkGray, inputX, cy - 15, inputX - 15, cy + 10);
            SplashKit.DrawLine(Color.DarkGray, inputX - 15, cy + 10, inputX + 10, cy + 15);
            SplashKit.DrawLine(Color.DarkGray, inputX, cy - 15, inputX + 10, cy + 15);

            SplashKit.FillCircle(Color.LightBlue, inputX, cy - 15, 8);
            SplashKit.FillCircle(Color.Cyan, inputX - 15, cy + 10, 10);
            SplashKit.FillCircle(Color.White, inputX + 10, cy + 15, 6);
        }

        _canonicalProtein.Draw(cx, cy, time);

        Color fluoroPink = SplashKit.RGBAColor(255, 20, 147, 255);
        double startX = cx;
        double startY = cy;
        double endTopX = cx + 180;
        double endTopY = cy - 120;
        double endBotX = cx + 180;
        double endBotY = cy + 120;

        for (int i = -5; i <= 5; i++)
        {
            SplashKit.DrawLine(fluoroPink, startX, startY + i, endTopX, endTopY + i);
            SplashKit.DrawLine(fluoroPink, startX, startY + i, endBotX, endBotY + i);
        }

        SplashKit.FillTriangle(fluoroPink, endTopX + 15, endTopY - 10, endTopX - 5, endTopY - 15, endTopX, endTopY + 10);
        SplashKit.FillTriangle(fluoroPink, endBotX + 15, endBotY + 10, endBotX - 5, endBotY + 15, endBotX, endBotY - 10);

        if (isIfBranch)
        {
            if (phaseTime >= 0.5)
            {
                double outX = cx + (outProgress * 180);
                double outY = cy - (outProgress * 120);
                SplashKit.FillCircle(Color.Gold, outX, outY, 18);
                SplashKit.FillCircle(Color.Orange, outX, outY, 10);
                SplashKit.DrawCircle(Color.Yellow, outX, outY, 18);
            }
        }
        else
        {
            if (phaseTime >= 0.5)
            {
                double outX = cx + (outProgress * 180);
                double outY = cy + (outProgress * 120);

                double spread = outProgress * 60;
                SplashKit.FillCircle(Color.LightBlue, outX - spread, outY - spread * 0.5, 4);
                SplashKit.FillCircle(Color.Cyan, outX + spread * 0.8, outY + spread * 0.3, 5);
                SplashKit.FillCircle(Color.White, outX - spread * 0.2, outY + spread, 4);
                SplashKit.FillCircle(Color.LightBlue, outX + spread * 0.5, outY - spread * 0.8, 3);
            }
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.DrawLine(Color.DarkGray, 960, y - 8, 948, y + 4);
        SplashKit.DrawLine(Color.DarkGray, 948, y + 4, 970, y + 6);
        SplashKit.DrawLine(Color.DarkGray, 960, y - 8, 970, y + 6);
        SplashKit.FillCircle(Color.LightBlue, 960, y - 8, 4);
        SplashKit.DrawCircle(Color.DarkGray, 960, y - 8, 4);
        SplashKit.FillCircle(Color.Cyan, 948, y + 4, 5);
        SplashKit.DrawCircle(Color.DarkGray, 948, y + 4, 5);
        SplashKit.FillCircle(Color.White, 970, y + 6, 3);
        SplashKit.DrawCircle(Color.DarkGray, 970, y + 6, 3);
        SplashKit.DrawText("Substrate", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        Color fluoroPink = SplashKit.RGBAColor(255, 20, 147, 255);
        SplashKit.DrawLine(fluoroPink, 945, y, 975, y);
        SplashKit.DrawText("Pathway Route", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Gold, 954, y, 7);
        SplashKit.DrawText("Product", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.LightBlue, 952, y - 3, 3);
        SplashKit.DrawCircle(Color.DarkGray, 952, y - 3, 3);
        SplashKit.FillCircle(Color.Cyan, 968, y + 2, 4);
        SplashKit.DrawCircle(Color.DarkGray, 968, y + 2, 4);
        SplashKit.FillCircle(Color.White, 958, y + 6, 3);
        SplashKit.DrawCircle(Color.DarkGray, 958, y + 6, 3);
        SplashKit.FillCircle(Color.LightBlue, 964, y - 5, 2);
        SplashKit.DrawCircle(Color.DarkGray, 964, y - 5, 2);
        SplashKit.DrawText("Degraded Components", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains abstraction by hiding internal protein workings.
/// </summary>
public class AbstractionPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the abstraction phase text and model.
    /// </summary>
    public AbstractionPhase()
    {
        ChapterName = "Abstraction";
        BioText = "A protein performs complex metabolic work internally while only exposing a simple result externally.";
        OopText = "Similarly, abstraction hides the messy, complicated background code and only shows the simple result.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cx = 600;
        double cy = 340;

        bool isIfBranch = (time % 5000) < 2500;
        double phaseTime = (time % 2500) / 2500.0;

        double inProgress = phaseTime < 0.5 ? phaseTime * 2 : 1.0;
        double outProgress = phaseTime >= 0.5 ? (phaseTime - 0.5) * 2 : 0.0;

        double inputX = cx - 180 + (inProgress * 180);
        if (phaseTime < 0.5)
        {
            SplashKit.DrawLine(Color.DarkGray, inputX, cy - 15, inputX - 15, cy + 10);
            SplashKit.DrawLine(Color.DarkGray, inputX - 15, cy + 10, inputX + 10, cy + 15);
            SplashKit.DrawLine(Color.DarkGray, inputX, cy - 15, inputX + 10, cy + 15);

            SplashKit.FillCircle(Color.LightBlue, inputX, cy - 15, 8);
            SplashKit.FillCircle(Color.Cyan, inputX - 15, cy + 10, 10);
            SplashKit.FillCircle(Color.White, inputX + 10, cy + 15, 6);
        }

        _canonicalProtein.Draw(cx, cy, time);

        if (phaseTime >= 0.4 && phaseTime <= 0.6)
        {
            double pulse = Math.Sin((phaseTime - 0.4) * 5 * Math.PI);
            SplashKit.FillCircle(SplashKit.RGBAColor(0, 255, 255, (byte)(80 * pulse)), cx, cy, 60 + (pulse * 20));
        }

        if (phaseTime >= 0.5)
        {
            if (isIfBranch)
            {
                double outX = cx + (outProgress * 180);
                double outY = cy - (outProgress * 120);
                SplashKit.FillCircle(Color.Gold, outX, outY, 18);
                SplashKit.FillCircle(Color.Orange, outX, outY, 10);
                SplashKit.DrawCircle(Color.Yellow, outX, outY, 18);
            }
            else
            {
                double outX = cx + (outProgress * 180);
                double outY = cy + (outProgress * 120);

                double spread = outProgress * 60;
                SplashKit.FillCircle(Color.LightBlue, outX - spread, outY - spread * 0.5, 4);
                SplashKit.FillCircle(Color.Cyan, outX + spread * 0.8, outY + spread * 0.3, 5);
                SplashKit.FillCircle(Color.White, outX - spread * 0.2, outY + spread, 4);
                SplashKit.FillCircle(Color.LightBlue, outX + spread * 0.5, outY - spread * 0.8, 3);
            }
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.DrawLine(Color.DarkGray, 960, y - 8, 948, y + 4);
        SplashKit.DrawLine(Color.DarkGray, 948, y + 4, 970, y + 6);
        SplashKit.DrawLine(Color.DarkGray, 960, y - 8, 970, y + 6);
        SplashKit.FillCircle(Color.LightBlue, 960, y - 8, 4);
        SplashKit.DrawCircle(Color.DarkGray, 960, y - 8, 4);
        SplashKit.FillCircle(Color.Cyan, 948, y + 4, 5);
        SplashKit.DrawCircle(Color.DarkGray, 948, y + 4, 5);
        SplashKit.FillCircle(Color.White, 970, y + 6, 3);
        SplashKit.DrawCircle(Color.DarkGray, 970, y + 6, 3);
        SplashKit.DrawText("Substrate", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Gold, 954, y, 7);
        SplashKit.DrawText("Output", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.LightBlue, 952, y - 3, 3);
        SplashKit.DrawCircle(Color.DarkGray, 952, y - 3, 3);
        SplashKit.FillCircle(Color.Cyan, 968, y + 2, 4);
        SplashKit.DrawCircle(Color.DarkGray, 968, y + 2, 4);
        SplashKit.FillCircle(Color.White, 958, y + 6, 3);
        SplashKit.DrawCircle(Color.DarkGray, 958, y + 6, 3);
        SplashKit.FillCircle(Color.LightBlue, 964, y - 5, 2);
        SplashKit.DrawCircle(Color.DarkGray, 964, y - 5, 2);
        SplashKit.DrawText("Degraded Components", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains encapsulation using a protein's protective shell.
/// </summary>
public class EncapsulationPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the encapsulation phase text and model.
    /// </summary>
    public EncapsulationPhase()
    {
        ChapterName = "Encapsulation";
        BioText = "The protein physically folds to lock its reactive, vulnerable amino acids deep inside a hydration shell, safe from external interference.";
        OopText = "Encapsulation protects an object's internal state (private fields) by heavily restricting direct external access.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double shellPulse = Math.Sin(time / 800.0) * 3;
        double shellRadius = 85 + shellPulse;
        SplashKit.FillCircle(SplashKit.RGBAColor(0, 150, 255, 25), 600, 340, shellRadius);
        SplashKit.DrawCircle(SplashKit.RGBAColor(0, 150, 255, 60), 600, 340, shellRadius);

        _canonicalProtein.Draw(600, 340, time);

        double[] hazardAngles = { Math.PI / 4.0, 3 * Math.PI / 4.0, 5 * Math.PI / 4.0, 7 * Math.PI / 4.0 };
        for (int i = 0; i < hazardAngles.Length; i++)
        {
            double angle = hazardAngles[i];
            double pulse = Math.Abs(Math.Sin((time / 400.0) + i * 2.0));
            double dist = 88 + (pulse * 70);

            double hx = 600 + Math.Cos(angle) * dist;
            double hy = 340 + Math.Sin(angle) * dist;

            SplashKit.FillCircle(Color.Red, hx, hy, 8);
            SplashKit.FillCircle(SplashKit.RGBAColor(120, 0, 0, 255), hx, hy, 4);

            for (int s = 0; s < 8; s++)
            {
                double spikeAngle = s * (Math.PI / 4.0) + (time / 200.0);
                SplashKit.DrawLine(Color.DarkRed, hx, hy, hx + Math.Cos(spikeAngle) * 11, hy + Math.Sin(spikeAngle) * 11);
            }
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(0, 150, 255, 25), 960, y, 10);
        SplashKit.DrawCircle(SplashKit.RGBAColor(0, 150, 255, 60), 960, y, 10);
        SplashKit.DrawText("Hydration Shell", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Red, 960, y, 8);
        SplashKit.FillCircle(SplashKit.RGBAColor(120, 0, 0, 255), 960, y, 4);
        for (int s = 0; s < 8; s++)
        {
            double spikeAngle = s * (Math.PI / 4.0);
            SplashKit.DrawLine(Color.DarkRed, 960, y, 960 + Math.Cos(spikeAngle) * 11, y + Math.Sin(spikeAngle) * 11);
        }
        SplashKit.DrawText("Hazard", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains polymorphism using homologous receptors.
/// </summary>
public class PolymorphismPhase : Phase
{
    private readonly IMolecule _proteinA;
    private readonly IMolecule _proteinB;
    private readonly IMolecule _waterMolecule;

    /// <summary>
    /// Sets up the polymorphism phase text and models.
    /// </summary>
    public PolymorphismPhase()
    {
        ChapterName = "Polymorphism";
        BioText = "A single canonical ligand binds to homologous receptors on different protein isoforms, triggering an identical catalytic action that outputs different results.";
        OopText = "In programming, polymorphism allows a single method call to trigger different, overridden behaviours depending on the specific subclass executing it.";
        _proteinA = new CanonicalProtein(45, 1.0, 0);
        _proteinB = new VariantProtein(35, 1.0, 4);
        _waterMolecule = new WaterMolecule();
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double cy = 340;
        double cx1 = 450;
        double cx2 = 750;

        _proteinA.Draw(cx1, cy, time);
        _proteinB.Draw(cx2, cy, time);

        double p1X = cx1;
        double p1Y = cy - 60;
        double p2X = cx2;
        double p2Y = cy - 60;

        DrawPinkStar(p1X, p1Y, 18);
        DrawPinkStar(p2X, p2Y, 18);

        double cycle = (time % 4000) / 4000.0;
        double fallProgress = cycle < 0.4 ? cycle * 2.5 : 1.0;
        double reactionProgress = cycle >= 0.4 ? (cycle - 0.4) * 1.66 : 0.0;

        double startY = cy - 160;
        double h2oY1 = startY + (fallProgress * (p1Y - startY));
        double h2oY2 = startY + (fallProgress * (p2Y - startY));

        if (cycle < 0.4)
        {
            _waterMolecule.Draw(p1X, h2oY1, time);
            _waterMolecule.Draw(p2X, h2oY2, time);
        }

        if (cycle >= 0.4)
        {
            double outX1 = p1X;
            double outY1 = p1Y - (reactionProgress * 80);
            SplashKit.FillCircle(Color.Gold, outX1, outY1, 12 + (reactionProgress * 8));
            SplashKit.FillCircle(Color.Orange, outX1, outY1, 6 + (reactionProgress * 4));
            SplashKit.DrawCircle(Color.Yellow, outX1, outY1, 12 + (reactionProgress * 8));

            double spread = reactionProgress * 50;
            double outX2 = p2X;
            double outY2 = p2Y - (reactionProgress * 80);
            SplashKit.FillCircle(Color.LightBlue, outX2 - spread, outY2 - spread * 0.5, 5);
            SplashKit.FillCircle(Color.Cyan, outX2 + spread * 0.8, outY2 - spread * 0.3, 6);
            SplashKit.FillCircle(Color.White, outX2 - spread * 0.2, outY2 - spread, 4);
        }
    }

    /// <summary>
    /// Draws a decorative star shape representing catalytic binding sites.
    /// </summary>
    private void DrawPinkStar(double cx, double cy, double size)
    {
        Color pink = SplashKit.RGBAColor(255, 20, 147, 255);
        for (int i = 0; i < 5; i++)
        {
            double a1 = i * (2 * Math.PI / 5) - Math.PI / 2;
            double a2 = (i + 1) * (2 * Math.PI / 5) - Math.PI / 2;
            double aMid = a1 + (2 * Math.PI / 10);

            double tipX = cx + Math.Cos(aMid) * size;
            double tipY = cy + Math.Sin(aMid) * size;
            double base1X = cx + Math.Cos(a1) * (size * 0.4);
            double base1Y = cy + Math.Sin(a1) * (size * 0.4);
            double base2X = cx + Math.Cos(a2) * (size * 0.4);
            double base2Y = cy + Math.Sin(a2) * (size * 0.4);

            SplashKit.FillTriangle(pink, cx, cy, tipX, tipY, base1X, base1Y);
            SplashKit.FillTriangle(pink, cx, cy, tipX, tipY, base2X, base2Y);
        }
        SplashKit.FillCircle(Color.White, cx, cy, size * 0.25);
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        DrawPinkStar(960, y, 10);
        SplashKit.DrawText("Catalytic Action", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        IMolecule miniProteinA = new CanonicalProtein(45, 0.2, 0);
        miniProteinA.Draw(960, y, 0);
        SplashKit.DrawText("Anabolic Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        IMolecule miniProteinB = new VariantProtein(35, 0.2, 4);
        miniProteinB.Draw(960, y, 0);
        SplashKit.DrawText("Catabolic Protein", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        IMolecule water = new WaterMolecule();
        water.Draw(960, y, 0);
        SplashKit.DrawText("Substrate", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Gold, 954, y, 7);
        SplashKit.FillCircle(Color.Orange, 954, y, 4);
        SplashKit.DrawCircle(Color.Yellow, 954, y, 7);
        SplashKit.DrawText("Output A", Color.White, _textFont, 16, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.Cyan, 948, y - 3, 3);
        SplashKit.FillCircle(Color.LightBlue, 956, y + 4, 3);
        SplashKit.FillCircle(Color.White, 962, y - 2, 2);
        SplashKit.DrawText("Output B", Color.White, _textFont, 16, 985, y - 7);
    }
}

/// <summary>
/// Explains delegates and lambdas using transport vesicles.
/// </summary>
public class DelegatesPhase : Phase
{
    private readonly IMolecule _canonicalProtein;

    /// <summary>
    /// Sets up the delegates phase text and model.
    /// </summary>
    public DelegatesPhase()
    {
        ChapterName = "Delegates & Lambdas";
        BioText = "The Golgi apparatus packages nascent components into transport vesicles, creating portable functional payloads delivered on demand.";
        OopText = "Delegates wrap actions into portable containers, while lambda statements define inline instructions packaged within them.";
        _canonicalProtein = new CanonicalProtein(45, 1.0, 0);
    }

    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the phase.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(15, 15, 20, 255), 280, 0, 640, 660);

        double golgiX = 420;
        double golgiY = 340;
        double targetX = 750;
        double targetY = 340;

        DrawGolgiStack(golgiX, golgiY, time, 1.0);
        _canonicalProtein.Draw(targetX, targetY, time);

        double cycle = (time % 3500) / 3500.0;

        double vesicleX, vesicleY;
        if (cycle < 0.3)
        {
            double p = cycle / 0.3;
            double startX = golgiX - 80;
            double startY = golgiY + 40;
            vesicleX = startX + (p * 50);
            vesicleY = startY - (p * 20);

            SplashKit.FillCircle(Color.LightBlue, vesicleX, vesicleY, 6);
            SplashKit.DrawCircle(Color.White, vesicleX, vesicleY, 6);
        }
        else
        {
            double p = (cycle - 0.3) / 0.7;
            p = Math.Sin(p * Math.PI / 2);
            vesicleX = golgiX + 40 + (p * (targetX - golgiX - 60));
            vesicleY = golgiY - 20 - (p * 30);

            SplashKit.FillCircle(SplashKit.RGBAColor(240, 160, 80, 220), vesicleX, vesicleY, 14);
            SplashKit.DrawCircle(Color.Yellow, vesicleX, vesicleY, 14);

            SplashKit.FillCircle(Color.LightBlue, vesicleX, vesicleY, 5);
            SplashKit.DrawCircle(Color.White, vesicleX, vesicleY, 5);
        }
    }

    /// <summary>
    /// Draws the stacked structure of the Golgi Apparatus.
    /// </summary>
    private void DrawGolgiStack(double cx, double cy, uint time, double scale)
    {
        Color golgiColor = SplashKit.RGBAColor(240, 150, 70, 255);
        Color golgiInner = SplashKit.RGBAColor(255, 200, 120, 255);

        for (int i = 0; i < 5; i++)
        {
            double yOffset = (i - 2) * 18 * scale;
            double curveWidth = (90 - Math.Abs(i - 2) * 6) * scale;

            double leftTipX = cx - curveWidth;
            double leftTipY = cy + yOffset;
            double rightTipX = cx + curveWidth;
            double rightTipY = cy + yOffset;

            SplashKit.FillEllipse(golgiColor, cx - curveWidth, cy + yOffset - (6 * scale), curveWidth * 2, 12 * scale);
            SplashKit.DrawEllipse(golgiInner, cx - curveWidth, cy + yOffset - (6 * scale), curveWidth * 2, 12 * scale);

            SplashKit.FillCircle(golgiInner, leftTipX, leftTipY, 7 * scale);
            SplashKit.FillCircle(golgiInner, rightTipX, rightTipY, 7 * scale);
        }
    }

    /// <summary>
    /// Draws the legend.
    /// </summary>
    public override void DrawLegend()
    {
        if (_textFont == null) return;
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 920, 0, 280, 800);
        SplashKit.DrawText("LEGEND", Color.DarkGray, _textFont, 20, 940, 30);
        SplashKit.DrawLine(Color.DarkGray, 940, 60, 1180, 60);

        int y = 93;
        Color golgiColor = SplashKit.RGBAColor(240, 150, 70, 255);
        Color golgiInner = SplashKit.RGBAColor(255, 200, 120, 255);
        for (int i = 0; i < 5; i++)
        {
            double yOffset = (i - 2) * 3;
            double curveWidth = (15 - Math.Abs(i - 2));
            SplashKit.FillEllipse(golgiColor, 960 - curveWidth, y + yOffset - 2, curveWidth * 2, 4);
            SplashKit.DrawEllipse(golgiInner, 960 - curveWidth, y + yOffset - 2, curveWidth * 2, 4);
        }
        SplashKit.DrawText("Golgi Apparatus", Color.White, _textFont, 15, 985, y - 7);

        y += 45;
        IMolecule miniProtein = new CanonicalProtein(45, 0.2, 0);
        miniProtein.Draw(960, y, 0);
        SplashKit.DrawText("Protein", Color.White, _textFont, 15, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(SplashKit.RGBAColor(240, 160, 80, 220), 960, y, 8);
        SplashKit.DrawCircle(Color.Yellow, 960, y, 8);
        SplashKit.DrawText("Vesicle", Color.White, _textFont, 15, 985, y - 7);

        y += 45;
        SplashKit.FillCircle(Color.LightBlue, 960, y, 5);
        SplashKit.DrawCircle(Color.White, 960, y, 5);
        SplashKit.DrawText("Nascent Component", Color.White, _textFont, 15, 985, y - 7);
    }
}

/// <summary>
/// Shows the final end screen.
/// </summary>
public class OutroPhase : Phase
{
    /// <summary>
    /// Updates the phase.
    /// </summary>
    public override void Update(uint time, Cell cell) { }

    /// <summary>
    /// Draws the ending text.
    /// </summary>
    public override void Draw(uint time, Cell cell)
    {
        string t1 = "THE END. THANKS FOR VIEWING!";
        if (_textFont != null) SplashKit.DrawText(t1, Color.White, _textFont, 36, (1200 - SplashKit.TextWidth(t1, _textFont, 36)) / 2, 350);
    }

    /// <summary>
    /// Empty legend for the outro screen.
    /// </summary>
    public override void DrawLegend() { }
}