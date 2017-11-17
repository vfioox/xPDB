using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPDB.Models.Storage;
using xPDB.Models.Systematization;

namespace xPDB.Storage
{
    public partial class ConfigManager
    {
        public bool doesTagExist(string key)
        {
            return cfg.Tags.ContainsKey(key);
        }
        public TagDeclarator getTagDeclarator(string key)
        {
            return cfg.Tags[key];
        }

        public bool doesFamilyExist(string key)
        {
            if (key == null) return false;
            return cfg.Families.ContainsKey(key);
        }
        public FamilyDeclarator getFamilyDeclarator(string key)
        {
            return cfg.Families[key];
        }

        public bool doesSuperfamilyExist(string key)
        {
            return cfg.Superfamilies.ContainsKey(key);
        }
        public SuperfamilyDeclarator getSuperfamilyDeclarator(string key)
        {
            return cfg.Superfamilies[key];
        }

        public bool doesFileTypeExist(string key)
        {
            return cfg.FileTypes.ContainsKey(key);
        }
        public FileType getFileType(string key)
        {
           
            return cfg.FileTypes[key];
        }

        public FileType determineFileType(string extension)
        {
            foreach (KeyValuePair<string, FileType> ft in cfg.FileTypes)
            {
                foreach (string ext in ft.Value.Extensions)
                {
                    if (ext == extension)
                    {
                        return ft.Value;
                    }
                }
            }
            return null;
        }

        public bool doesChunkExist(string key)
        {
            return cfg.Chunks.ContainsKey(key);
        }
        public Chunk getChunk(string key)
        {
            return cfg.Chunks[key];
        }
    }
}
