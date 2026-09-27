/// <summary>
/// Base interface for all drawable molecules.
/// </summary>
public interface IMolecule
{
    /// <summary>
    /// Draws the molecule on the screen.
    /// </summary>
    /// <param name="cx">The center X coordinate.</param>
    /// <param name="cy">The center Y coordinate.</param>
    /// <param name="time">The current animation time.</param>
    void Draw(double cx, double cy, uint time);
}