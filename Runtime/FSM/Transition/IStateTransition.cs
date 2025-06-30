public interface IStateTransition<TFactory, TInput, TTrigger>
{
    TTrigger Trigger { get; }
    IStateDefinition<TFactory, TInput> FromStateDefinition { get; }
    IStateDefinition<TFactory, TInput> NextStateDefinition { get; }
}