using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TranslationExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: TranslationExtractor <prefix> <srcPath> <outPath>");
                return;
            }

            var prefix = args[0];
            var srcPath = args[1];
            var outputPath = args[2];

            var translationJson = JObject.Parse(File.ReadAllText(outputPath));

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(srcPath);

            var fileKey = $"{prefix}_{Path.GetFileNameWithoutExtension(srcPath).ToUpper().Replace(' ', '_')}";

            if (xmlDoc.FirstChild.Name == "DialogueTree")
            {
                var dialogueDictionary = (translationJson["DialogueDictionary"] as JObject) ?? (translationJson["DialogueDictionary"] = new JObject());

                var nameField = xmlDoc.FirstChild.SelectSingleNode("NameField");
                if (nameField != null)
                {
                    var key = $"{fileKey}_{nameField.InnerText.ToUpper().Replace(' ', '_')}";
                    dialogueDictionary[key] = nameField.InnerText;
                    nameField.InnerText = key;
                }

                var dialogueNodes = xmlDoc.FirstChild.SelectNodes("DialogueNode");
                foreach (XmlNode dialogueNode in dialogueNodes)
                {
                    var name = dialogueNode.SelectSingleNode("Name").InnerText;
                    var nodeKey = $"{fileKey}_{name}";
                    var pages = dialogueNode.SelectSingleNode("Dialogue").SelectNodes("Page");
                    for (int i = 0; i < pages.Count; i++)
                    {
                        var pageKey = $"{nodeKey}_{i + 1}";
                        dialogueDictionary[pageKey] = pages[i].InnerText;
                        pages[i].InnerText = pageKey;
                    }

                    var dialogueOptionList = dialogueNode.SelectSingleNode("DialogueOptionsList");
                    if (dialogueOptionList != null)
                    {
                        var options = dialogueOptionList.SelectNodes("DialogueOption");
                        for (int i = 0; i < options.Count; i++)
                        {
                            var optionKey = $"{nodeKey}_OPTION_{i + 1}";
                            dialogueDictionary[optionKey] = options[i].SelectSingleNode("Text").InnerText;
                            options[i].InnerText = optionKey;
                        }
                    }
                }
            }
            else if (xmlDoc.FirstChild.Name == "NomaiObject")
            {
                var dialogueDictionary = (translationJson["DialogueDictionary"] as JObject) ?? (translationJson["DialogueDictionary"] = new JObject());

                var textBlocks = xmlDoc.FirstChild.SelectNodes("TextBlock");
                foreach (XmlNode textBlock in textBlocks)
                {
                    var id = textBlock.SelectSingleNode("ID").InnerText;
                    var key = $"{fileKey}_{id}";
                    dialogueDictionary[key] = textBlock.SelectSingleNode("Text").InnerText;
                    textBlock.SelectSingleNode("Text").InnerText = key;
                }
            }
            else if (xmlDoc.FirstChild.Name == "AstroObjectEntry")
            {
                var shipLogDictionary = (translationJson["ShipLogDictionary"] as JObject) ?? (translationJson["ShipLogDictionary"] = new JObject());

                void AddEntry(XmlNode entry)
                {
                    var id = entry.SelectSingleNode("ID").InnerText;
                    var entryKey = id;
                    shipLogDictionary[entryKey] = entry.SelectSingleNode("Name").InnerText;
                    entry.SelectSingleNode("Name").InnerText = entryKey;

                    var exploreFacts = entry.SelectNodes("ExploreFact");
                    foreach (XmlNode exploreFact in exploreFacts)
                    {
                        var factId = exploreFact.SelectSingleNode("ID").InnerText;
                        var factKey = factId;
                        shipLogDictionary[factKey] = exploreFact.SelectSingleNode("Text").InnerText;
                        exploreFact.SelectSingleNode("Text").InnerText = factKey;
                    }

                    var rumorFacts = entry.SelectNodes("RumorFact");
                    foreach (XmlNode rumorFact in rumorFacts)
                    {
                        var factId = rumorFact.SelectSingleNode("ID").InnerText;
                        var factKey = factId;
                        shipLogDictionary[factKey] = rumorFact.SelectSingleNode("Text").InnerText;
                        rumorFact.SelectSingleNode("Text").InnerText = factKey;
                    }

                    var subEntries = entry.SelectNodes("Entry");
                    foreach (XmlNode subEntry in subEntries)
                    {
                        AddEntry(subEntry);
                    }
                }

                var entries = xmlDoc.FirstChild.SelectNodes("Entry");

                foreach (XmlNode entry in entries)
                {
                    AddEntry(entry);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unsupported XML format. Expected 'DialogueTree' or 'NomaiObject'.");
            }

            xmlDoc.Save(srcPath);
            File.WriteAllText(outputPath, translationJson.ToString());
        }
    }
}
