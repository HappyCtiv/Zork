using UnityEngine;
using Zork.Common;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    [SerializeField]    
    private Transform ContentTransform;

    [SerializeField]
    [Range(0, 1000)]
    private uint MaxEntries = 15;


    public void Write(object obj) => ParseAndWriteLine(obj.ToString());

    public void Write(string message) => ParseAndWriteLine(message);

    public void WriteLine(object obj) => ParseAndWriteLine(obj.ToString());

    public void WriteLine(string message) => ParseAndWriteLine(message);

    private void ParseAndWriteLine(string message)
    {
        var textLine = Instantiate(TextLinePrefab, ContentTransform);
        textLine.text = message;
        _entries.Add (textLine.gameObject);

        string lineSeparator = "/n";
        string[] lineTokens = message.Split(lineSeparator);

        if (_entries.Count >= MaxEntries)
        {
            GameObject _oldEntry = _entries[0];
            _entries.Remove(_oldEntry);
            Destroy(_oldEntry);
        }

        if (lineTokens.Length == 2 )
        {
            for (int i = 1; i < lineTokens.Length; i++)
            {
                ParseAndWriteLine(lineTokens[i]);
            }
        }

        textLine.ForceMeshUpdate();

        if(textLine.isTextOverflowing)
        {
            string overflowing = textLine.text.Substring(textLine.firstOverflowCharacterIndex);
            textLine.text = textLine.text.Remove(textLine.firstOverflowCharacterIndex);
            ParseAndWriteLine(overflowing);
            textLine.ForceMeshUpdate();
        }


    }

    private List<GameObject> _entries = new List<GameObject>();
}
