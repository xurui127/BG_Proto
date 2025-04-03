public abstract class CardBehaviour
{
    protected GameManager GM => GameManager.Instance;

    internal abstract void OnGameStart(int amount);

    internal abstract void OnExecute();
}
