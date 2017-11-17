using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xPDB.Models.Storage;
using xPDB.Utility;

namespace xPDB.Storage
{
    public enum ConfigLoadStatus
    {
        CreatedNewConfig,
        FileLocked,
        UnexpectedEncryption,
        ParsingFailed,
        Success
    }
    public enum ConfigWriteStatus
    {
        SerializationError,
        EncryptionError,
        WriteError,
        Success
    }
    public enum ReadMethods
    {
        ReadAllText,
        ReadViaStream
    }
    public enum WriteMethods
    {
        Stream
    }
    public partial class ConfigManager
    {
        private string password;

        public const string defaultInstanceName = "main";
        public const string defaultExtension = ".epdb";

        public Config cfg;

        public ConfigManager(string password)
        {
            this.password = password;
        }
        public static Dictionary<string, FileType> getDefaultFileTypes()
        {
            Dictionary<string, FileType> data = new Dictionary<string,FileType>();

            FileType t = new FileType();
            t._FileType = FileTypes.Image;
            t.UserCustom = false;
            List<string> ext = new List<string>();
            ext.Add(".jpg");
            ext.Add(".png");
            ext.Add(".jpeg");
            ext.Add(".bmp");
            ext.Add(".wmf");
            t.Extensions = ext;
            t.TypeName = "Default image type";
            t.TypeKey = "def_image";
            data.Add(t.TypeKey, t);

            t = new FileType();
            t._FileType = FileTypes.AnimatedImage;
            t.UserCustom = false;
            ext = new List<string>();
            ext.Add(".gif");
            t.Extensions = ext;
            t.TypeKey = "def_animated_image";
            t.TypeName = "Default animated image type";
            data.Add(t.TypeKey, t);

            t = new FileType();
            t._FileType = FileTypes.TextFile;
            t.UserCustom = false;
            ext = new List<string>();
            ext.Add(".txt");
            t.Extensions = ext;
            t.TypeKey = "def_text";
            t.TypeName = "Default text type";
            data.Add(t.TypeKey, t);

            t = new FileType();
            t._FileType = FileTypes.Unknown;
            t.UserCustom = false;
            ext = new List<string>();
            ext.Add("*");
            t.Extensions = ext;
            t.TypeKey = "def_unkn";
            t.TypeName = "Default unknown type";
            data.Add(t.TypeKey, t);

            return data;
        }
        public static Config initializeNewConfig()
        {
            Config cfg = new Config();
            cfg.ChunkPath = "chunks/";
            cfg.ChunkSize = 35;
            cfg.MRS = false;
            cfg.Instance = "main";
            cfg.NoOfLaunches = 1;
            cfg.LogPassword = RandomData.RandomString(16);
            cfg.InstanceChunkDir = RandomData.RandomString(10);
            cfg.Preferences = new RememberPreferences();
            cfg.Preferences.ReadMethod = ReadMethods.ReadViaStream;
            cfg.Preferences.WriteMethod = WriteMethods.Stream;
            cfg.UseEncryption = true;

            cfg.Chunks = new Dictionary<string, Chunk>();
            cfg.FileDeclarators = new Dictionary<string, FileDeclarator>();

            cfg.OStorage = new Dictionary<string, object>();

            cfg.FileTypes = getDefaultFileTypes();
            cfg.Families = new Dictionary<string, Models.Systematization.FamilyDeclarator>();
            cfg.Superfamilies = new Dictionary<string, Models.Systematization.SuperfamilyDeclarator>();
            cfg.Tags = new Dictionary<string, Models.Systematization.TagDeclarator>();
            

            return cfg;
        }
        public void setThisAsNewConfig()
        {
            cfg = initializeNewConfig();
        }

        public static string[] findInstances()
        {
            DirectoryInfo d = new DirectoryInfo(".");
            FileInfo[] f = d.GetFiles("*" + defaultExtension);
            List<string> instances = new List<string>();
            foreach(FileInfo fi in f)
            {
                instances.Add(fi.Name.Remove(fi.Name.IndexOf("."), fi.Name.Length - fi.Name.IndexOf(".")));
            }
            return instances.ToArray();
        }

        public string getPwd()
        {
            return password;
        }
        public ConfigWriteStatus saveConfig()
        {
            string data = "";
            try
            {
                data = JsonConvert.SerializeObject(cfg);
            }
            catch(Exception ex)
            {
                GlobalErrorHandler.logError(ex, cfg.LogPassword);
                return ConfigWriteStatus.SerializationError;
            }
            try
            {
                if (cfg.UseEncryption == true)
                {
                    data = Encrypt(data);
                }
            }
            catch(Exception ex)
            {
                GlobalErrorHandler.logError(ex, cfg.LogPassword);
                return ConfigWriteStatus.EncryptionError;
            }
            try
            {
                bool s = FileOperations.writeViaStream(defaultInstanceName + defaultExtension, data);
                if (s) return ConfigWriteStatus.Success;
                else return ConfigWriteStatus.WriteError;
            }
            catch(Exception ex)
            {
                GlobalErrorHandler.logError(ex, cfg.LogPassword);
                return ConfigWriteStatus.WriteError;
            }
        }
        public ConfigLoadStatus loadConfig(string instance = null)
        {
            string path = defaultInstanceName + defaultExtension;
            if (instance != null) path = instance + defaultExtension;
            string data = "";
            if (File.Exists(path))
            {
                if (IsFileLocked(new FileInfo(path)))
                {
                    return ConfigLoadStatus.FileLocked;
                }
                else
                {
                    if(cfg != null)
                    {
                        switch (cfg.Preferences.ReadMethod)
                        {
                            case ReadMethods.ReadAllText:
                                data = FileOperations.readAllText(path);
                                break;
                            case ReadMethods.ReadViaStream:
                                data = FileOperations.readViaStreamReader(path);
                                break;
                        }
                    }
                    else
                    {
                        data = FileOperations.readViaStreamReader(path);
                    }
                    if (IsConfigEncrypted(data))
                    {
                        data = Decrypt(data);
                    }
                    try
                    {
                        Config cfg = JsonConvert.DeserializeObject<Config>(data);
                        this.cfg = cfg;
                    }
                    catch (Exception ex)
                    {
                        if(cfg != null) GlobalErrorHandler.logError(ex, cfg.LogPassword);
                        return ConfigLoadStatus.ParsingFailed;
                    }
                    return ConfigLoadStatus.Success;
                }
            }
            else
            {
                setThisAsNewConfig();
                saveConfig();
                return ConfigLoadStatus.CreatedNewConfig;
            }
        }
        public string Decrypt(string data)
        {
            return Encryption.Encryptor.Decrypt(data, password);
        }
        public string Encrypt(string data)
        {
            return Encryption.Encryptor.Encrypt(data, password);
        }
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
        protected virtual bool IsConfigEncrypted(string data)
        {
            if (data[0] == '{') return false;
            else return true;
        }
    }
}
