    
public interface IHitControl
{
    void Hit(int damage, int direction);
    void HitReaction(int direction);
    bool IsHitPossible();
}