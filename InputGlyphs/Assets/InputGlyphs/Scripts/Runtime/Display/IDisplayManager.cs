namespace InputGlyphs.Display
{
    public interface IDisplayManager
    {
        void Register(IGlyphDisplay display);
        void Unregister(IGlyphDisplay display);
    }
}