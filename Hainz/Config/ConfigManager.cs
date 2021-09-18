using Hainz.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Hainz.Config
{
    public static class ConfigManager
    {
        private const string CONFIG_FILE = "config.json.encrypted";
        private const string KEY_FILE = "encryption.key";

        private static string _encryptionPassword;
        private static string _baseDir;
        private static string _configPath;
        private static string _keyPath;

        public static BotConfig GetOrCreate()
        {
            InitPaths();
            
            if (HasConfigFile())
            {
                bool useConfig = ConsoleUtil.ReadBoolYesNo("Found a config file! Do you want to load it? (y/n)");
                ConsoleUtil.ClearAndEmptyMessageList();

                if (useConfig)
                    return LoadFromExistingConfig();
            }

            return CreateNewConfig();
        }

        public static bool Save(BotConfig config)
        {
            try
            {
                byte[] key;

                if (HasKeyFile())
                    key = File.ReadAllBytes(_keyPath);
                else
                {
                    key = SecretBox.GenerateKey();
                    File.WriteAllBytes(_keyPath, key);
                }

                config.Checksum = GetChecksum(config);
                string json = JsonConvert.SerializeObject(config);
                byte[] data = Encoding.UTF8.GetBytes(json);
                byte[] salt = Encoding.UTF8.GetBytes(_encryptionPassword);

                data = SecretBox.Encrypt(data, key, salt);

                File.WriteAllBytes(_configPath, data);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static BotConfig LoadFromExistingConfig()
        {
            if (!HasKeyFile())
            {
                bool createNew = ConsoleUtil.ReadBoolYesNo("Config file found but key file is missing! Do you want to create a new config file? (y/n)");
                ConsoleUtil.ClearAndEmptyMessageList();
                if (!createNew)
                {
                    ConsoleUtil.Write("Application is shutdown. Cannot read encrypted config file because of missing key.");
                    return null;
                }
                else
                    return CreateNewConfig();
            }

            byte[] configData = null;
            byte[] keyData = null;
            try
            {
                configData = File.ReadAllBytes(_configPath);
                keyData = File.ReadAllBytes(_keyPath);
            }
            catch (Exception ex)
            {
                ConsoleUtil.Write($"Error trying to read config or key file. Internal message: {ex.Message}");
                return null;
            }

            BotConfig config = null;
            while (config == null)
            {
                string password = ConsoleUtil.ReadSecretNonEmpty("Enter your encryption password:");
                byte[] salt = Encoding.UTF8.GetBytes(password);
                ConsoleUtil.ClearAndEmptyMessageList();

                bool wasModified = false;
                try
                {
                    configData = SecretBox.Decrypt(configData, keyData, salt);

                    string json = Encoding.UTF8.GetString(configData);
                    config = JsonConvert.DeserializeObject<BotConfig>(json);

                    if (config == null)
                        throw new Exception();

                    string checksum = GetChecksum(config);
                    if (checksum != config?.Checksum)
                    {
                        wasModified = true;
                        throw new Exception();
                    }
                }
                catch
                {
                    if (wasModified)
                    {
                        ConsoleUtil.Write("Internal checksum violation! The config file has been modified. Please restart and create a new config file");
                        return null;
                    }
                    bool retry = ConsoleUtil.ReadBoolYesNo("Wrong password or config file modification! Would you like to try again? (y/n)");
                    if (!retry)
                    {
                        ConsoleUtil.Write("Config file couldn't be read!");
                        return null;
                    }
                    ConsoleUtil.ClearAndEmptyMessageList();
                }

                _encryptionPassword = password;
            }

            return config;
        }

        private static BotConfig CreateNewConfig()
        {
            ConsoleUtil.Write("Creating a new config file.");

            string password = null;
            string confirm = null;

            do
            {
                if (!string.IsNullOrEmpty(password))
                    ConsoleUtil.Write("Passwords are not the same.");

                password = ConsoleUtil.ReadSecretNonEmpty("Enter a password that will be used for config encryption:");
                confirm = ConsoleUtil.ReadSecretNonEmpty("Confirm encryption password:");
                ConsoleUtil.ClearAndEmptyMessageList();
            } while (password != confirm);

            _encryptionPassword = password;

            var botConfig = new BotConfig();
            botConfig.Token = ConsoleUtil.ReadSecretNonEmpty("Enter bot token:");
            ConsoleUtil.ClearAndEmptyMessageList();
            botConfig.YoutubeAPIKey = ConsoleUtil.ReadSecretNonEmpty("Enter youtube API key:");
            ConsoleUtil.ClearAndEmptyMessageList();

            if (!Save(botConfig))
                ConsoleUtil.Write("Saving config failed! Will need to be reentered next time");

            return botConfig;
        }

        private static void InitPaths()
        {
            _baseDir ??= AppContext.BaseDirectory;
            _configPath ??= Path.Combine(_baseDir, CONFIG_FILE);
            _keyPath ??= Path.Combine(_baseDir, KEY_FILE);
        }

        private static string GetChecksum(BotConfig botConfig)
        {
            StringBuilder checksumString = new StringBuilder();
            var properties = botConfig.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute(typeof(ChecksumIgnoreAttribute)) == null)
                {
                    checksumString.Append(property.Name);
                    checksumString.Append(property.GetValue(botConfig)?.ToString());
                }
            }

            byte[] content = Encoding.UTF8.GetBytes(checksumString.ToString());
            return SecretBox.CalculateChecksum(content);
        }

        private static bool HasConfigFile() => File.Exists(_configPath);
        private static bool HasKeyFile() => File.Exists(_keyPath);
    }
}
