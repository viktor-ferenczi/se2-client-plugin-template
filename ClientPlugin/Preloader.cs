#if DISABLED_PRELOADER_EXAMPLE

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming

using ClientPlugin.Patches;
using Mono.Cecil;

// IMPORTANT: MUST NOT USE A NAMESPACE, otherwise Pulsar won't find the Preloader class! 
//namespace ClientPlugin;

// ReSharper disable once UnusedType.Global
public static class Preloader
{
    // ReSharper disable once UnusedMember.Global
    public static IEnumerable<string> TargetDLLs { get; } =
    [
        // Game DLLs
        // TODO: List the game assemblies to patch in the preloader
        "Game2.Client.dll",
    ];

    // This method is called for all the assemblies listed in TargetDLLs
    // ReSharper disable once UnusedMember.Global
    public static void Patch(AssemblyDefinition asmDef)
    {
        /* !!! WARNING !!!
        Currently, the preloader patches do not work with SE2. That's because the Mono.Cecil library used
        is not compatible with the "ReadyToRun" (aka R2R) mode .NET assemblies the game is built with.
        We plan to fix this by replacing Mono.Cecil with a different library.
        */

        // TODO: Call you preloader patches 
        ExamplePrepatch.Prepatch(asmDef);
    }

    // ReSharper disable once UnusedMember.Global
    public static void Finish()
    {
        // Called after applying all the preloader patches and before Space Engineers 2 starts.
        // The plugin is loaded only as part of the game's initializations, so that comes later.
    }
}

#endif
