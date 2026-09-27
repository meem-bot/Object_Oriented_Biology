using SplashKitSDK;
using System.Collections.Generic;

/// <summary>
/// Controls the main presentation loop, handling phases, timers, and UI buttons.
/// </summary>
public class Director
{
    private SplashKitSDK.Timer _animTimer;
    private Font _textFont;
    private Cell _cell;
    private List<Phase> _phases;
    private int _currentIndex = 0;

    private Button _btnPrev;
    private Button _btnNext;
    private Button _btnBegin;

    /// <summary>
    /// Sets up the director with all presentation phases and navigation buttons.
    /// </summary>
    public Director()
    {
        _animTimer = SplashKit.CreateTimer("AnimationTimer");
        _textFont = SplashKit.LoadFont("Arial", "arial.ttf");
        _cell = new Cell(600, 340);

        _phases = new List<Phase>
        {
            new IntroPhase(),
            new IntroductionPhase(),
            new ClassesPhase(),
            new ClassInheritancePhase(),
            new ObjectsPhase(),
            new FieldsPhase(),
            new PropertiesPhase(),
            new InterfaceInheritancePhase(),
            new MethodsPhase(),
            new ControlFlowPhase(),
            new AbstractionPhase(),
            new EncapsulationPhase(),
            new PolymorphismPhase(),
            new DelegatesPhase(),
            new OutroPhase()
        };

        _btnPrev = new Button("Previous", 30, 730, 120, 40, _textFont, () =>
        {
            if (_currentIndex > 0) _currentIndex--;
        });

        _btnNext = new Button("Next", 1050, 730, 120, 40, _textFont, () =>
        {
            if (_currentIndex < _phases.Count - 1) _currentIndex++;
        });

        _btnBegin = new Button("Begin", 1050, 730, 120, 40, _textFont, () =>
        {
            _currentIndex = 1;
        });

        SplashKit.StartTimer(_animTimer);
    }

    /// <summary>
    /// Updates the current phase and checks button clicks.
    /// </summary>
    public void Update()
    {
        uint time = SplashKit.TimerTicks(_animTimer);

        if (_currentIndex == 0 && time > 2000)
        {
            _btnBegin.Update();
        }
        else if (_currentIndex > 0 && _currentIndex < _phases.Count - 1)
        {
            _btnPrev.Update();
            _btnNext.Update();
        }
        else if (_currentIndex == _phases.Count - 1)
        {
            _btnPrev.Update();
        }

        _phases[_currentIndex].Update(time, _cell);
    }

    /// <summary>
    /// Draws the current phase, side panel, and navigation buttons.
    /// </summary>
    public void Draw()
    {
        uint time = SplashKit.TimerTicks(_animTimer);

        _phases[_currentIndex].Draw(time, _cell);

        if (_currentIndex > 0 && _currentIndex < _phases.Count - 1)
        {
            DrawSidePanel();
            _phases[_currentIndex].DrawLegend();
            _phases[_currentIndex].DrawSubtitles();
        }

        if (_currentIndex == 0 && time > 2000)
        {
            _btnBegin.Draw();
        }
        else if (_currentIndex > 0 && _currentIndex < _phases.Count - 1)
        {
            _btnPrev.Draw();
            _btnNext.Draw();
        }
        else if (_currentIndex == _phases.Count - 1)
        {
            _btnPrev.Draw();
        }
    }

    /// <summary>
    /// Draws the sidebar menu showing the list of chapters.
    /// </summary>
    private void DrawSidePanel()
    {
        SplashKit.FillRectangle(SplashKit.RGBAColor(20, 25, 35, 230), 0, 0, 280, 800);
        SplashKit.DrawText("CHAPTERS", Color.DarkGray, _textFont, 20, 20, 30);
        SplashKit.DrawLine(Color.DarkGray, 20, 60, 260, 60);

        for (int i = 1; i < _phases.Count - 1; i++)
        {
            Color c = (i == _currentIndex) ? Color.Yellow : Color.Gray;
            string prefix = (i == _currentIndex) ? "> " : "  ";
            SplashKit.DrawText($"{prefix}{i}. {_phases[i].ChapterName}", c, _textFont, 16, 20, 90 + ((i - 1) * 45));
        }
    }
}