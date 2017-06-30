using SplashKitSDK;
using System;

public abstract class Move
{
    protected Character _Caster;
    protected Character _Target;

    public Move()
    {

    }

    public abstract void Execute();

    public abstract string Message();

}