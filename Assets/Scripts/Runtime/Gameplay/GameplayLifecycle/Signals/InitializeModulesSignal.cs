namespace ProjectTemplate.Runtime.Gameplay.GameplayLifecycle.Signals
{
	/// <summary>
	/// Fired from <see cref="GameplayInitializer"/> <br/>
	/// Listened by <see cref="PlayerAbilityRequestFillerInitializer"/> , <see cref="CombatGridInitializer"/> , <see cref="PlayerCharactersSpawner"/> and <see cref="TurnController"/>
	/// </summary>
	public readonly struct InitializeModulesSignal
	{
	}
}