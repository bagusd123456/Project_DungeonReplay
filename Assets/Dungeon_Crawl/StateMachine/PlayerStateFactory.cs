using System.Collections.Generic;

enum PlayerStates
{
    grounded,
    idle,
    run,
    attack,
    dash,
    switchweapon
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
        _states[PlayerStates.attack] = new PlayerAttackState(_context, this);
        _states[PlayerStates.dash] = new PlayerDashState(_context, this);
        _states[PlayerStates.switchweapon] = new PlayerSwitchWeaponSubState (_context, this);
    }

    public PlayerBaseState Grounded()
    {
        return _states[PlayerStates.grounded];
    }

    public PlayerBaseState Idle()
    {
        return _states[PlayerStates.idle];
    }
    public PlayerBaseState Run()
    {
        return _states[PlayerStates.run];
    }

    public PlayerBaseState Attack()
    {
        return _states[PlayerStates.attack];
    }

    public PlayerBaseState Dash()
    {
        return _states[PlayerStates.dash];
    }

    public PlayerBaseState SwitchWeapon()
    {
        return _states[PlayerStates.switchweapon];
    }

}
