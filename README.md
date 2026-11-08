# Granny 3 Evolved — Decompile

A Unity project of **Granny 3 Evolved** by OMGI, rebuilt from the shipped game.

The released game was compiled with IL2CPP, so its C# scripts weren't in the files. They were rebuilt by hand from Ghidra pseudocode of `GameAssembly.dll` and the Il2CppDumper metadata, using the assets AssetRipper exported. Evolved shares a lot of code with Granny: Legacy (same author), so classes that match were carried over from the [Granny: Legacy decompile](https://github.com/Wave351/GrannyLegacy-Decompile), with Evolved's changes put back in. The project compiles with 0 errors and builds as a playable Windows game.

> This is an unofficial fan project for study and preservation. It isn't affiliated with OMGI or the Granny developers. All game content belongs to its owners.

## Just want to play it?

Download `Granny3EvolvedDecompile-v1.0.0-Windows.zip` from the [latest release](../../releases/latest), unzip it, and run `Granny 3 Evolved Decompile.exe`.

## What's in this repo

| Path | Contents |
| --- | --- |
| `Assets/1_Scenes` | `Menu` and `Scene` (the house) |
| `Assets/Scripts/Assembly-CSharp` | Game scripts. `Foo.cs` keeps the original serialized fields and GUID; `Foo.Ported.cs` holds the restored logic as a `partial` class |
| `Assets/Editor` | Build script, font repair and other helpers, all under *Tools → Evolved Restoration* |
| `ProjectSettings`, `Packages` | Unity settings and package list |

Everything is in git, including textures, meshes, audio, cubemaps and sprites, so a clone opens as a complete project.

## Setup

1. Install **Unity 6000.5.5f1** exactly. Use Unity Hub → Installs → Install Editor → Archive if it isn't listed. (The original game used Unity 2022.3; this project was upgraded.)
2. Clone this repo, or download it as a zip (about 1.4 GB).
3. Open the folder in Unity 6000.5.5f1. The first import takes a while.
4. Open `Assets/1_Scenes/Menu.unity` and press Play.

Don't copy in your own AssetRipper export. A fresh export gives the files different GUIDs, so scenes and materials can't find them: the map turns invisible and everything looks white or grey.

### Troubleshooting

- **Hundreds of `CS0121: The call is ambiguous ... TMPro.TMP_ExtensionMethods` errors in `Library/PackageCache/com.unity.ugui...`:** the project was opened in a different Unity version. Close Unity, delete the `Library` folder, and reopen the project in **6000.5.5f1**.
- **Text shows as white boxes:** run *Tools → Evolved Restoration → Repair Dynamic Fonts*, then reopen the scene.
- **White or grey UI, missing map or textures:** the clone is incomplete (for example, an old copy from before the assets were added to git). Pull the latest version or download the zip again. You should end up with `Assets/Texture2D`, `Assets/Mesh` and `Assets/AudioClip` folders full of files.

## Building

You need **Unity 6000.5.5f1**.

- **In the editor:** *Tools → Evolved Restoration → Build Windows Game*.
- **From the command line:**
  ```
  Unity.exe -batchmode -quit -projectPath <this folder> -executeMethod BuildGame.BuildWindows -logFile build.log
  ```

The game is built to `../../Build/Granny3EvolvedDecompile/Granny 3 Evolved Decompile.exe`, using the Mono backend and the original game's icon.

## Differences from the original

- The Unity splash logo is removed, and the exe uses the original game's icon.
- AssetRipper can't recover compiled shaders. The TextMesh Pro and post-processing shaders were swapped for Unity's own source. The stained glass, water, eye and ground fog shaders were rewritten by hand, so they look a little different from the original.
- Third-party plugins such as Steamworks and Game Jolt only came through as empty stubs, so Steam and Game Jolt features (trophies, cloud saves) don't do anything.
- Fixed: the camera stayed zoomed in after Grandpa caught you.
- **Slientlo Never Clear** is the only difficulty: Granny and Grandpa are much faster, Grandpa shoots faster, Slendrina drains you faster, there are lasers and extra creaking floors, every escape has extra locks and puzzles, and the atmosphere is pitch dark.
- **Training** option (General section): enemies can't kill you, and G freezes or unfreezes every enemy.
- An editor-only cheat, *Tools → Evolved Restoration → Cheats → Unlock All Skins + 200 Coins*, for testing.

## Status

Every game script has been ported back to C#. Large parts of the game haven't been played through end to end yet, so expect bugs. Bug reports in the Issues tab are welcome.
