using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace Construct.Configs
{
    public class SystemConfig : ModConfig
    {
        public static SystemConfig instance => ModContent.GetInstance<SystemConfig>();

        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("Classes")]//"Contraption ModSystem class list")]
        [DefaultValue(false)]
        public bool classWhitelist;
        public List<string> classList = new();

        [Header("Methods")]//"Contraption ModSystem method list")]
        [DefaultValue(true)]
        public bool methodWhitelist;
        public List<string> methodList = new();

        [Header("Pairs")]//"Contraption ModSystem pair lists")]
        public List<string> pairWhitelist = new();
        public List<string> pairBlacklist = new();

        [Header("Dump")]
        public bool includeWhitelisted;
        public bool includeBlacklisted;
        public bool DumpClasses
        {
            get => false;
            set
            {
                StringBuilder output = new StringBuilder();
                foreach (var system in SystemLoader.Systems)
                {
                    string name = system.FullName;
                    bool allowed = classList.Contains(name) == classWhitelist;
                    if (includeWhitelisted && allowed || includeBlacklisted && !allowed)
                        output.AppendLine(name);
                }
                Console.WriteLine("\nConstruct ModSystem Dump:");
                Console.WriteLine(output.ToString());
                _dumpOutput = output.ToString();
            }
        }
        public bool DumpMethods
        {
            get => false;
            set
            {
                StringBuilder output = new StringBuilder();
                foreach (var hook in SystemLoader.hooks)
                {
                    string name = hook.Method.Name;
                    bool allowed = methodList.Contains(name) == methodWhitelist;
                    if (includeWhitelisted && allowed || includeBlacklisted && !allowed)
                        output.AppendLine(name);
                }
                Console.WriteLine("\nConstruct ModSystem Dump:");
                Console.WriteLine(output.ToString());
                _dumpOutput = output.ToString();
            }
        }
        public bool DumpPairs
        {
            get => false;
            set
            {
                StringBuilder output = new StringBuilder();
                foreach (var hook in SystemLoader.hooks)
                {
                    string methodName = hook.Method.Name;
                    bool methodAllowed = methodList.Contains(methodName) == methodWhitelist;
                    foreach (var system in hook.Enumerate())
                    {
                        string systemName = system.Name;
                        string pairName = $"{systemName}:{methodName}";

                        bool systemAllowed = classList.Contains(systemName) == classWhitelist;

                        bool pairAllowed = methodAllowed && systemAllowed;
                        pairAllowed &= !pairBlacklist.Contains(systemName);
                        pairAllowed |= pairWhitelist.Contains(systemName);

                        if (includeWhitelisted && pairAllowed || includeBlacklisted && !pairAllowed)
                            output.AppendLine(pairName);
                    }
                }
                Console.WriteLine("\nConstruct ModSystem Dump:");
                Console.WriteLine(output.ToString());
                _dumpOutput = output.ToString();
            }
        }

        private string _dumpOutput = "";
        public string DumpOutput { get => _dumpOutput; }
    }
}
