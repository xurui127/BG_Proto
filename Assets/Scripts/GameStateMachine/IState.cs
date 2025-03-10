public interface IState
{
    public void OnEnter();

    public void OnUpdate();

    public void OnExit();
}

public abstract class AbstractState : IState
{
    protected GameManager GM;

    protected FSMController stateMachine;

    protected float waitingTime;

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}