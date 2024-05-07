using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(0, "tsv")]
public class StatsImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var file = File.OpenText(assetPath);

        // Check if this is a Unit
        bool isUnit = !assetPath.Contains("Buildings");

        // The first line is the header, ignore
        file.ReadLine();

        var main = new TextAsset();
        ctx.AddObjectToAsset("Main", main);
        ctx.SetMainObject(main);

        while (!file.EndOfStream)
        {
            // Read the line and divide at the tabs
            var line = file.ReadLine();
            var lineElements = line.Split('\t');

            var name = lineElements[0].ToLower();
            if (isUnit)
            {
                // Fill all the values
                var entry = ScriptableObject.CreateInstance<UnitData>();
                entry.name = name;
                entry._life = int.Parse(lineElements[1]);
                entry._damage = int.Parse(lineElements[2]);
                entry._defense = int.Parse(lineElements[3]);
                entry._attackRatio = float.Parse(lineElements[4]);
                entry._attackRange = float.Parse(lineElements[5]);
                entry._scanRange = float.Parse(lineElements[6]);
                entry._speed = float.Parse(lineElements[7]);

                ctx.AddObjectToAsset(name, entry);
            }
            else
            {
                // Fill all the values
                var entry = ScriptableObject.CreateInstance<UnitData>();
                entry.name = name;
                entry._life = int.Parse(lineElements[1]);

                ctx.AddObjectToAsset(name, entry);
            }
        }

        // Close the file
        file.Close();
    }
}