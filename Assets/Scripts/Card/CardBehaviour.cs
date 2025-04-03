public abstract class CardBehaviour
{
    protected GameManager GM => GameManager.Instance;
    internal abstract void OnExecute();
}
