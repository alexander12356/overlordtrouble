

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

    public Special(Element p_Element)
    {
        element = p_Element;
    }
}
