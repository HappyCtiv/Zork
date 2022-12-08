namespace Zork.Common
{
    public enum Commands
    {
        Quit,
        Q = Quit,
        Exit = Quit,
        Bye = Quit,
        Look,
        L = Look,
        North,
        N = North,
        South,
        S = South,
        East,
        E = East,
        West,
        W = West,
        Take,
        T = Take,
        Pick = Take,
        Drop,
        D = Drop,
        Inventory,
        I = Inventory,
        Reward,
        Score,
        Health,
        DamagePlayer, //Debug Purposes.
        HealPlayer, //DEBUG PURPOSES
        Use,
        Drink = Use,
        Consume = Use,
        Attack,
        A = Attack,
        Hit = Attack,
        Unknown
    }
}