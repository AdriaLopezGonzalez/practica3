using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using System.Diagnostics;

namespace TCEngine
{
    public class Resources
    {
        private const string DATA_DIRECTORY_NAME = "Data";

        private Dictionary<string, Texture> m_Textures;
        private Dictionary<string, Font> m_Fonts;
        private Dictionary<string, SoundBuffer> m_SoundBuffers;

        public Texture GetTexture(string _name)
        {
            return m_Textures[_name];
        }

        public Font GetFont(string _name)
        {
            return m_Fonts[_name];
        }

        public SoundBuffer GetSound(string _name)
        {
            return m_SoundBuffers[_name];
        }

        public void LoadResources()
        {
            m_Textures = new Dictionary<string, Texture>();
            m_Fonts = new Dictionary<string, Font>();
            m_SoundBuffers = new Dictionary<string, SoundBuffer>();

            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dataDirectoryInfo = Directory.CreateDirectory(workingDirectory + DATA_DIRECTORY_NAME);
            Debug.Assert(dataDirectoryInfo.Exists);

            LoadResources(dataDirectoryInfo);
        }

        private void LoadResources(DirectoryInfo _directoryInfo, string _path = "")
        {
            LoadTextures(_directoryInfo, _path);
            LoadFonts(_directoryInfo, _path);
            LoadSounds(_directoryInfo, _path);

            var subDirectories = _directoryInfo.GetDirectories();
            foreach (DirectoryInfo subDirectoryInfo in subDirectories)
            {
                // Load resourcecs in the subdirectories as well
                LoadResources(subDirectoryInfo, _path + subDirectoryInfo.Name + "/");
            }
        }

        private void LoadTextures(DirectoryInfo _directoryInfo, string _path)
        {
            string[] extensions = { "png", "jpg" };
            LoadResources(m_Textures, _path, _directoryInfo, extensions);
        }

        private void LoadFonts(DirectoryInfo _directoryInfo, string _path)
        {
            string[] extensions = { "ttf" };
            LoadResources(m_Fonts, _path, _directoryInfo, extensions);
        }
        private void LoadSounds(DirectoryInfo _directoryInfo, string _path)
        {
            string[] extensions = { "wav" };
            LoadResources(m_SoundBuffers, _path, _directoryInfo, extensions);
        }

        private void LoadResources<T>(Dictionary<string, T> _resourceDictionary, string _path, DirectoryInfo _directoryInfo, string[] _extensions)
        {
            foreach (string extension in _extensions )
            {
                FileInfo[] fileNames = _directoryInfo.GetFiles("*." + extension);
                foreach (var fileInfo in fileNames)
                {
                    string name = fileInfo.Name.Remove(fileInfo.Name.IndexOf("." + extension));
                    string key = _path + name;
                    T resource = (T)Activator.CreateInstance(typeof(T), DATA_DIRECTORY_NAME + "/" + key + "." + extension);
                    _resourceDictionary.Add(key, resource);
                }
            }
        }


    }
}
