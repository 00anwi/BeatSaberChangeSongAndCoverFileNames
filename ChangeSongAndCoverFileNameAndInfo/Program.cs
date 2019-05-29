using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeSongAndCoverFileNameAndInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.GetCurrentDirectory();
            var path = Path.Combine(Directory.GetCurrentDirectory(), args[0]);

            Console.WriteLine("Changing names of songs and covers");
            foreach (var subdir in Directory.GetDirectories(path))
            {
                string filepathNewInfo = $"{subdir}/info.dat";
                string filepathOldInfo = $"{subdir}/info.json";

                if (FileExist(filepathNewInfo))
                {
                    ParseNewInfoFile(filepathNewInfo, subdir);
                }
                else if (FileExist(filepathOldInfo))
                {
                    ParseOldInfoFile(filepathOldInfo, subdir);
                    
                }
            }

            return;
        }

        static void ParseNewInfoFile(string filepath, string subdir)
        {
            string result = string.Empty;

            using (StreamReader r = new StreamReader(filepath))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                foreach (var item in jobj.Properties())
                {
                    if (item.Name == "_songFilename" && item.Value.ToString() == "song.ogg")
                    {
                        var songName = jobj.Properties().Single(s => s.Name == "_songName").Value.ToString() + ".ogg";
                        var isValid = !string.IsNullOrEmpty(songName) &&
                            songName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                            !File.Exists(Path.Combine(subdir, songName));

                        if (!isValid)
                        {
                            songName = string.Join("", songName.Split(Path.GetInvalidFileNameChars()));
                        }

                        item.Value = songName;

                        if (FileExist(subdir + "/song.ogg"))
                        {
                            File.Move(subdir + "/song.ogg", subdir + "/" + songName);
                        }

                        Console.WriteLine("Changed name off song.ogg to" + songName);
                    }

                    if (item.Name == "_coverImageFilename" && item.Value.ToString() == "cover.jpg" || item.Value.ToString() == "cover.png")
                    {
                        var extension = Path.GetExtension(item.Value.ToString()).ToLowerInvariant();
                        var songName = jobj.Properties().Single(s => s.Name == "_songName").Value.ToString() + extension;
                        var isValid = !string.IsNullOrEmpty(songName) &&
                            songName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                            !File.Exists(Path.Combine(subdir, songName));

                        if (!isValid)
                        {
                            songName = string.Join("", songName.Split(Path.GetInvalidFileNameChars()));
                        }

                        item.Value = songName;

                        if (FileExist(subdir + "/cover" + extension))
                        {
                            File.Move(subdir + "/cover" + extension, subdir + "/" + songName);
                        }

                        Console.WriteLine("Changed name off cover" + extension + " to" + songName);
                    }
                }
                result = jobj.ToString();
            }
            File.WriteAllText(filepath, result);
        }

        static void ParseOldInfoFile(string filepath, string subdir)
        {
            string result = string.Empty;

            using (StreamReader r = new StreamReader(filepath))
            {

                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                foreach (var item in jobj.Properties())
                {
                    if (item.Name == "audioPath" && item.Value.ToString() == "song.ogg")
                    {
                        var songName = jobj.Properties().Single(s => s.Name == "songName").Value.ToString() + ".ogg";
                        var isValid = !string.IsNullOrEmpty(songName) &&
                            songName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                            !File.Exists(Path.Combine(subdir, songName));

                        if (!isValid)
                        {
                            songName = string.Join("", songName.Split(Path.GetInvalidFileNameChars()));
                        }

                        item.Value = songName;

                        if (FileExist(subdir + "/song.ogg"))
                        {
                            File.Move(subdir + "/song.ogg", subdir + "/" + songName);
                        }

                        Console.WriteLine("Changed name off song.ogg to" + songName);
                    }

                    if (item.Name == "coverImagePath" && item.Value.ToString() == "cover.jpg" || item.Value.ToString() == "cover.png")
                    {
                        var extension = Path.GetExtension(item.Value.ToString()).ToLowerInvariant();
                        var songName = jobj.Properties().Single(s => s.Name == "songName").Value.ToString() + extension;
                        var isValid = !string.IsNullOrEmpty(songName) &&
                            songName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
                            !File.Exists(Path.Combine(subdir, songName));

                        if (!isValid)
                        {
                            songName = string.Join("", songName.Split(Path.GetInvalidFileNameChars()));
                        }

                        item.Value = songName;

                        if (FileExist(subdir + "/cover" + extension))
                        {
                            File.Move(subdir + "/cover" + extension, subdir + "/" + songName);
                        }

                        Console.WriteLine("Changed name off cover" + extension + " to" + songName);
                    }

                }
                result = jobj.ToString();
            }
            File.WriteAllText(filepath, result);

        }

        static bool FileExist(string FilePath)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                return false;
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
