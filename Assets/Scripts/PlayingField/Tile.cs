using UnityEngine;

public enum Type
{
    Circle   = 0,
    Diamond  = 1,
    Square   = 2,
    Star     = 3,
    Triangle = 4,
}

// TODO - подумать - нужен ли такой класс вообще

public class Tile : MonoBehaviour
{
    private Type type;
    private Color32 colorOfBadge;
    private Color32 backgroundColor;

    public Type Type { get => type; set => type = value; }
    public Color32 ColorOfBadge { get => colorOfBadge; set => colorOfBadge = value; }
    public Color32 BackgroundColor { get => backgroundColor; set => backgroundColor = value; }

    private void InstallBadge()
    {

    }
}

