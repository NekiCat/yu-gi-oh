using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TigeR.YuGiOh.Core.Cards;

namespace TigeR.YuGiOh.Core.Data
{
    /*

        Structure:
        File (ZIP archive)
         | - mimetype (Always contains "application/x-yugioh-card+zip")
         | - META-INF/manifest.xml (Contains a list of all files and how they should be treated)
         | - META-INF/metadata.xml (Contains meta information such as name, author, level, type of the card...)
         | - META-INF/thumbnail.png (Contains a thumbnail for display in Explorer - easier and faster than loading up WPF in the shell and generating one)
         | - scripts/<effect>.lua (Contains the effects of the cards) (mime type either "text/plain" or "text/x-lua" or "application/x-lua")
         | - images/cover.png (Can be different name and extension; cover image)
         |

    */

    public class CardLoader
    {
        public Image GetThumbnail(Stream stream)
        {
            using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, false))
            {
                var entry = zipArchive.GetEntry("META-INF/thumbnail.png");
                if (entry == null) return null;
                using (var thumb = entry.Open())
                {
                    return Image.FromStream(thumb);
                }
            }
        }

        #region Loading

        public Card LoadFromStream(Stream stream)
        {
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Read, false))
            {
                return Load(zipFile);
            }
        }

        public Card LoadFromFile(string filename)
        {
            using (var zipFile = ZipFile.Open(filename, ZipArchiveMode.Read))
            {
                return Load(zipFile);
            }
        }

        private Card Load(ZipArchive zipArchive)
        {
            var card = new Card();

            // Check mime type validity
            using (var mimeStream = zipArchive.GetEntry("mimetype").Open())
            using (var mimeReader = new StreamReader(mimeStream))
            {
                if (mimeReader.ReadToEnd() != "application/x-yugioh-card+zip")
                {
                    throw new InvalidDataException("File has wrong mime type");
                }
            }

            // Read the manifest
            using (var manifestStream = zipArchive.GetEntry("META-INF/manifest.xml").Open())
            {
                var manifest = XDocument.Load(manifestStream);
                foreach (var entry in manifest.Descendants("file-entry"))
                {
                    var zipEntry = zipArchive.GetEntry(entry.Attribute("full-path").Value);
                    using (var stream = zipEntry.Open())
                    {
                        var resource = new Resource(
                            entry.Attribute("full-path").Value,
                            new MemoryStream((int)zipEntry.Length),
                            entry.Attribute("media-type").Value
                        );
                        resource.Stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(resource.Stream);
                        resource.Stream.Seek(0, SeekOrigin.Begin);

                        card.Resources.Add(resource.Filename, resource);
                    }
                }
            }

            // Read out card metadata
            string renderInfoPath = null;
            using (var metaStream = zipArchive.GetEntry("META-INF/metadata.xml").Open())
            {
                var meta = XDocument.Load(metaStream);
                foreach (var entry in meta.Descendants("meta-entry"))
                {
                    var key = entry.Attribute("name")?.Value ?? String.Empty;
                    switch (key)
                    {
                        case "Author":
                            card.Author = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "CardType":
                            card.CardType = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "CardSubType":
                            card.CardSubType = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Rarity":
                            card.Rarity = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Name":
                            card.Name = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Attribute":
                            card.Attribute = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Level":
                            card.Level = Convert.ToInt32(entry.Attribute("value")?.Value);
                            break;
                        case "LevelReversed":
                            // todo: is this renderinfo? is this based on another card property?
                            bool levelReversed = false;
                            Boolean.TryParse(entry.Attribute("value")?.Value, out levelReversed);
                            card.LevelReversed = levelReversed;
                            break;
                        case "Cover":
                            card.Cover = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Edition":
                            card.Edition = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Set":
                            card.Set = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Type":
                            card.Type = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Description":
                            card.Description = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Attack":
                            card.Attack = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Defense":
                            card.Defense = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Number":
                            card.Number = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "Copyright":
                            card.Copyright = entry.Attribute("value")?.Value ?? String.Empty;
                            break;
                        case "RenderInfo":
                            renderInfoPath = entry.Attribute("value")?.Value;
                            break;
                    }
                }
            }

            if (renderInfoPath != null)
            {
                using (var renderInfoStream = zipArchive.GetEntry(renderInfoPath).Open())
                {
                    var renderInfo = XDocument.Load(renderInfoStream);
                    foreach (var entry in renderInfo.Descendants("element"))
                    {
                        var key = entry.Attribute("name")?.Value ?? String.Empty;
                        switch (key)
                        {
                            
                        }
                    }
                }
            }

            return card;
        }

        #endregion

        #region Saving

        public void SaveToStream(Stream stream, Card card, Stream thumbnail = null)
        {
            using (var zipFile = new ZipArchive(stream, ZipArchiveMode.Create, false))
            {
                Save(zipFile, card, thumbnail);
            }
        }

        public void SaveToFile(string filename, Card card, Stream thumbnail = null, bool overwrite = true)
        {
            if (overwrite && File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var zipFile = ZipFile.Open(filename, ZipArchiveMode.Create))
            {
                Save(zipFile, card, thumbnail);
            }
        }

        private void Save(ZipArchive zipArchive, Card card, Stream thumbnail = null)
        {
            // Put correct mime type
            using (var mimeStream = zipArchive.CreateEntry("mimetype").Open())
            using (var mimeWriter = new StreamWriter(mimeStream))
            {
                mimeWriter.Write("application/x-yugioh-card+zip");
            }

            // Write the manifest
            var manifest = new XElement("manifest");
            foreach (var entry in card.Resources.Values)
            {
                using (var stream = zipArchive.CreateEntry(entry.Filename).Open())
                {
                    entry.Stream.Seek(0, SeekOrigin.Begin);
                    entry.Stream.CopyTo(stream);
                }

                var xentry = new XElement("file-entry");
                xentry.SetAttributeValue("media-type", entry.Mimetype);
                xentry.SetAttributeValue("full-path", entry.Filename);

                manifest.Add(xentry);
            }

            using (var manifestStream = zipArchive.CreateEntry("META-INF/manifest.xml").Open())
            {
                new XDocument(manifest).Save(manifestStream);
            }

            // Write card metadata
            var meta = new XElement("metadata");

            AddMetaEntry(meta, "Author", card.Author);
            AddMetaEntry(meta, "CardType", card.CardType);
            AddMetaEntry(meta, "CardSubType", card.CardSubType);
            AddMetaEntry(meta, "Rarity", card.Rarity);
            AddMetaEntry(meta, "Name", card.Name);
            AddMetaEntry(meta, "Attribute", card.Attribute);
            AddMetaEntry(meta, "Level", card.Level.ToString());
            AddMetaEntry(meta, "LevelReversed", card.LevelReversed.ToString().ToLower());
            AddMetaEntry(meta, "Cover", card.Cover);
            AddMetaEntry(meta, "Edition", card.Edition);
            AddMetaEntry(meta, "Set", card.Set);
            AddMetaEntry(meta, "Type", card.Type);
            AddMetaEntry(meta, "Description", card.Description);
            AddMetaEntry(meta, "Attack", card.Attack);
            AddMetaEntry(meta, "Defense", card.Defense);
            AddMetaEntry(meta, "Number", card.Number);
            AddMetaEntry(meta, "Copyright", card.Copyright);

            using (var metaStream = zipArchive.CreateEntry("META-INF/metadata.xml").Open())
            {
                new XDocument(meta).Save(metaStream);
            }

            // Write the thumbnail
            using (var thumbStream = zipArchive.CreateEntry("META-INF/thumbnail.png").Open())
            {
                thumbnail.Seek(0, SeekOrigin.Begin);
                thumbnail.CopyTo(thumbStream);
            }
        }

        private void AddMetaEntry(XElement root, string name, string value)
        {
            var element = new XElement("meta-entry");
            element.SetAttributeValue("name", name);
            element.SetAttributeValue("value", value);
            root.Add(element);
        }

        #endregion
    }
}
