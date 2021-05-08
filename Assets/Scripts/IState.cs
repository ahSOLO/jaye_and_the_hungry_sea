public interface IState
{
    void FixedTick();
    void Tick();
    void OnEnter();
    void OnExit();
}
