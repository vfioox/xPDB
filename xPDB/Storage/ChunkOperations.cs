using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xPDB.Models.Storage;
using xPDB.Utility;

namespace xPDB.Storage
{
    public class ChunkOperations
    {
        public static string getChunkPath(string key, ConfigManager cm)
        {
            return cm.cfg.ChunkPath + string.Format("{0}.epdbc", key);
        }
        public static string newChunk(ref ConfigManager cm)
        {
            var chunk = new Chunk
            {
                ChunkId = RandomData.RandomString(14),
                FileCodes = new Dictionary<string, int>(),
                FileOffsets = new Dictionary<string, int>(),
                FileLenghts = new Dictionary<string, int>(),
                Vacant = true
            };
            var intact = FileOperations.writeViaStream(getChunkPath(chunk.ChunkId, cm), "");
            if (intact)
            {
                cm.cfg.Chunks.Add(chunk.ChunkId, chunk);
                cm.saveConfig();
                return chunk.ChunkId;
            }
            else return null;
        }
        public static bool writeToChunkEnd(string chunk_key, string file_key, byte[] data, ref ConfigManager cm)
        {
            var dce = cm.doesChunkExist(chunk_key);
            if (dce)
            {
                var chunk = cm.getChunk(chunk_key);
                string b64 = Convert.ToBase64String(data);

                b64 = cm.Encrypt(b64);
                string chunk_path = getChunkPath(chunk_key, cm);
                var intact = FileOperations.writeAppend(chunk_path, b64 + "\n");
                if (!intact)
                {
                    chunk.PossiblyCorrupted = true;
                    return false;
                }
                else
                {
                    int c = FileOperations.countFileLines(chunk_path);
                    cm.cfg.Chunks[chunk_key].FileCodes.Add(file_key, c);
                    //cm.cfg.Chunks[chunk_key].FileOffsets[file_key] = totalOffset;
                    cm.cfg.Chunks[chunk_key].FileLenghts[file_key] = b64.Length;
                    //totalOffset += b64.Length + 1;
                    if (cm.cfg.Chunks[chunk_key].FileCodes.Count > cm.cfg.ChunkSize)
                    {
                        cm.cfg.Chunks[chunk_key].Vacant = false;
                    }
                }
                return true;
            }
            else return false;
        }
        public static bool emptyChunkPosition(string file_key, string chunk_key, ref ConfigManager cm)
        {
            var file = cm.cfg.FileDeclarators[file_key];
            var chunk = cm.cfg.Chunks[chunk_key];

            var line = chunk.FileCodes[file_key];

            chunk.FileCodes.Remove(file_key);
            chunk.FileCodes.Add("deleted_" + file_key, line);
            FileOperations.writeToLine(getChunkPath(chunk_key, cm), "removed", line);

            chunk.FileOffsets = new Dictionary<string, int>();
            chunk.FileLenghts = new Dictionary<string, int>();


            cm.cfg.Chunks[chunk_key] = chunk;
            cm.cfg.FileDeclarators.Remove(file_key);
            cm.saveConfig();
            return true;
        }
        public static void emptyAndCascadeChunkPosition()
        {
            

        }
        public static byte[] readChunkPosition(string chunk_key, string file_key, ref ConfigManager cm)
        {
            //string b64 = FileOperations.readStreamLine(ChunkOperations.getChunkPath(chunk_key, cm), position);
            //return Convert.FromBase64String(Encryption.Encryptor.Decrypt(b64, cm.getPwd()));
            var chunk = cm.getChunk(chunk_key);
            var firstOffset = 0;
            var secondOffset = 0;
            if(!chunk.FileOffsets.ContainsKey(file_key)) return null;
            firstOffset = chunk.FileOffsets[file_key];
            secondOffset = chunk.FileLenghts[file_key];
            string b64 = Encoding.UTF8.GetString(FileOperations.readBinaryPosition(ChunkOperations.getChunkPath(chunk_key, cm), firstOffset, secondOffset));
            if (b64 == "removed") return Encoding.UTF8.GetBytes("removed");
            var dec = Encryption.Encryptor.Decrypt(b64, cm.getPwd());
            if (dec == null) throw new Exception("Encryption error, chunk possibly corrupted");
            return Convert.FromBase64String(dec);
        }
        public static byte[] readChunkPositionLegacy(string chunk_key, int position, ref ConfigManager cm)
        {
            string b64 = FileOperations.readStreamLine(ChunkOperations.getChunkPath(chunk_key, cm), position);
            return Convert.FromBase64String(Encryption.Encryptor.Decrypt(b64, cm.getPwd()));
        }
        public static bool examineChunkPosition(string chunk_key, int position, ref ConfigManager cm)
        {
            var fde = new FileDeclarator();
            var chunk = cm.getChunk(chunk_key);
            var firstOffset = 0;
            var secondOffset = 0;
            foreach (var fd in chunk.FileCodes)
            {
                if (fd.Value == position)
                {
                    fde = cm.cfg.FileDeclarators[fd.Key];
                    firstOffset = chunk.FileOffsets[fd.Key];
                    secondOffset = chunk.FileLenghts[fd.Key];
                }
            }
            string b64 = Encoding.UTF8.GetString(FileOperations.readBinaryPosition(ChunkOperations.getChunkPath(chunk_key, cm), firstOffset, secondOffset));
            string b642 = FileOperations.readStreamLine(ChunkOperations.getChunkPath(chunk_key, cm), position);
            Debug.WriteLine(b64);
            Debug.WriteLine(b642);
            return b64 == b642;
        }
    }
}
