

public class Special
{
    public enum Element
    {
        Water,
        Fire,
        Earth,
        Air
    }

    public string title;
    public int level = 1;
    public Element element;

    public Special(string p_Title, Element p_Element)
    {
        title = p_Title;
        element = p_Element;
    }
}
