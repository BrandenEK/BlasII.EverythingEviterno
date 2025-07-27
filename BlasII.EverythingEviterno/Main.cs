using MelonLoader;

namespace BlasII.EverythingEviterno;

internal class Main : MelonMod
{
    public static EverythingEviterno EverythingEviterno { get; private set; }

    public override void OnLateInitializeMelon()
    {
        EverythingEviterno = new EverythingEviterno();
    }
}