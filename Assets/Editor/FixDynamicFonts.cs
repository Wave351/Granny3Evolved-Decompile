using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

// Restoration fix: AssetRipper exported the dynamic TextMeshPro fonts (Oswald Bold, Anton, Bangers, ...) with their
// atlas texture missing, so any text using them rendered nothing (e.g. the shop's ">" page arrows) and the build
// logged "m_AtlasTextures of TMP_FontAsset doesn't exist anymore". This gives each such font a fresh atlas texture
// stored inside the font asset, points its materials at it, and clears the stale glyph tables so TMP repopulates them
// from the source font at runtime. Safe to run repeatedly: fonts with a valid atlas are left alone.
public static class FixDynamicFonts
{
	[MenuItem("Tools/Evolved Restoration/Repair Dynamic Fonts")]
	public static void Run()
	{
		int fixedCount = 0;
		foreach (var guid in AssetDatabase.FindAssets("t:TMP_FontAsset"))
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			var font = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
			if (!font || font.atlasPopulationMode != AtlasPopulationMode.Dynamic || !font.sourceFontFile) continue;
			var atlases = font.atlasTextures;
			if (atlases != null && atlases.Length > 0 && atlases.All(t => t)) continue;

			int w = font.atlasWidth > 0 ? font.atlasWidth : 1024, h = font.atlasHeight > 0 ? font.atlasHeight : 1024;
			var tex = new Texture2D(w, h, TextureFormat.Alpha8, false) { name = font.name + " Atlas" };
			AssetDatabase.AddObjectToAsset(tex, font);
			font.atlasTextures = new[] { tex };

			// The font's own material plus the preset materials next to it (Outline, Drop Shadow, ...).
			string folder = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
			string prefix = font.name;
			var mats = AssetDatabase.FindAssets("t:Material", new[] { folder })
				.Select(g => AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(g)))
				.Where(m => m && m.name.StartsWith(prefix) && m.HasProperty(ShaderUtilities.ID_MainTex) && !m.GetTexture(ShaderUtilities.ID_MainTex))
				.ToList();
			if (font.material && !mats.Contains(font.material)) mats.Add(font.material);
			foreach (var m in mats)
			{
				m.SetTexture(ShaderUtilities.ID_MainTex, tex);
				if (m.HasProperty(ShaderUtilities.ID_TextureWidth)) m.SetFloat(ShaderUtilities.ID_TextureWidth, w);
				if (m.HasProperty(ShaderUtilities.ID_TextureHeight)) m.SetFloat(ShaderUtilities.ID_TextureHeight, h);
				EditorUtility.SetDirty(m);
			}

			font.ClearFontAssetData(false);                                   // drop glyphs that pointed into the lost atlas
			EditorUtility.SetDirty(font);
			fixedCount++;
			Debug.Log($"[FixDynamicFonts] rebuilt atlas for {font.name} ({w}x{h}), {mats.Count} material(s)");
		}
		AssetDatabase.SaveAssets();
		Debug.Log($"[FixDynamicFonts] fixed {fixedCount} font(s)");
	}
}
