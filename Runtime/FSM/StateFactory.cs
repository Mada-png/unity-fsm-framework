
/// <summary>
/// 
/// </summary>
/// <typeparam name="TStateMachine"></typeparam>
public class StateFactory<TStateMachine>
{
    public TStateMachine StateMachine { get; set; }

    public StateFactory(TStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
}