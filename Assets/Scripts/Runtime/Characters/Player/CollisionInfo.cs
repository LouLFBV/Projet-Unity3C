public struct CollisionInfo 
{
    public bool _below;
    public bool _above;

    public bool _left;
    public bool _right;

    public void Reset()
    {
        _below = false;
        _above = false;
        _left = false;
        _right = false;
    }
}