namespace InputGlyphs.Display
{
    public interface IGlyphDisplayManager
    {
        void Register(IGlyphDisplay display);
        void Unregister(IGlyphDisplay display);
    }
}