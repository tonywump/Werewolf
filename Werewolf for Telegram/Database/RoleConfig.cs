using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Database
{
    [Flags]
    public enum RoleConfig : long
    {
        //Group settings are now as simple as adding an enum to this list.  No more database schema changes.
        None = 0,
        [Editable(true), Role("villager", "👱"), DefaultValue(true)]
        Villager = 1,
        [Editable(true), Role("drunk", "🍻"), DefaultValue(true)]
        Drunk = 2,
        [Editable(true), Role("seer", "👳"), DefaultValue(true)]
        Seer = 4,
        [Editable(true), Role("cursed", "😾"), DefaultValue(true)]
        Cursed = 8,
        [Editable(true), Role("harlot", "💋"), DefaultValue(true)] 
        Harlot = 16,
        [Editable(true), Role("beholder", "👁"), DefaultValue(true)]
        Beholder = 32,
        [Editable(true), Role("gunner", "🔫"), DefaultValue(true)]
        Gunner = 64,
        [Editable(true), Role("traitor", "🖕"), DefaultValue(true)]
        Traitor = 128,
        [Editable(true), Role("guardianangel", "👼"), DefaultValue(true)]
        GuardianAngel = 256,
        [Editable(true), Role("detective", "🕵️"), DefaultValue(true)]
        Detective = 512,
        [Editable(true), Role("appseer", "🙇"), DefaultValue(true)]
        ApprenticeSeer = 1024,
        [Editable(true), Role("wildchild", "👶"), DefaultValue(true)]
        WildChild = 2048,
        [Editable(true), Role("fool", "🃏"), DefaultValue(true)]
        Fool = 4096,
        [Editable(true), Role("mason", "👷"), DefaultValue(true)]
        Mason = 8192,
        [Editable(true), Role("doppelganger", " 🎭"), DefaultValue(true)]
        Doppelgänger = 16384,
        [Editable(true), Role("cupid", "🏹"), DefaultValue(true)]
        Cupid = 32768,
        [Editable(true), Role("hunter", "🎯"), DefaultValue(true)]
        Hunter = 65536,
        [Editable(true), Role("tanner", "👺"), DefaultValue(true)]
        Tanner = 131072,
        [Editable(true), Role("mayor", "👑"), DefaultValue(true)]
        Mayor = 262144,
        [Editable(true), Role("prince", "👑"), DefaultValue(true)]
        Prince = 524288,
        [Editable(true), Role("clumsy", "🤕"), DefaultValue(true)]
        ClumsyGuy = 1048576,
        [Editable(true), Role("blacksmith", "⚒"), DefaultValue(true)]
        Blacksmith = 2097152,
        [Editable(true), Role("sandman", "💤"), DefaultValue(true)]
        Sandman = 4194304,
        [Editable(true), Role("oracle", "🌀"), DefaultValue(true)]
        Oracle = 8388608,
        [Editable(true), Role("wolfman", "👱🌚"), DefaultValue(true)]
        WolfMan = 16777216,
        [Editable(true), Role("pacifist", "☮️"), DefaultValue(true)]
        Pacifist = 33554432,
        [Editable(true), Role("wiseelder", "📚"), DefaultValue(true)]
        WiseElder = 67108864,
        [Editable(true), Role("thief", "😈"), DefaultValue(true)]
        Thief = 134217728,
        [Editable(true), Role("wolf", "🐺"), DefaultValue(true)]
        Wolf = 268435456,
        [Editable(true), Role("alpha", "⚡️"), DefaultValue(true)]
        AlphaWolf = 536870912,
        [Editable(true), Role("cub", "🐶"), DefaultValue(true)]
        WolfCub = 1073741824,
        [Editable(true), Role("lycan", "🐺🌝"), DefaultValue(true)]
        Lycan = 2147483648,
        [Editable(true), Role("sorcerer", "🔮"), DefaultValue(true)]
        Sorcerer = 4294967296,
        [Editable(true), Role("cultist", "👤"), DefaultValue(true)]
        Cultist = 8589934592,
        [Editable(true), Role("sk", "🔪"), DefaultValue(true)]
        SerialKiller = 17179869184,




        //this is a flag that will be set on ALL groups indicating we need to update the settings
        Update = 4611686018427387904
    }

    public static class RoleDefaults
    {
        public static RoleConfig LoadDefaults()
        {
            RoleConfig result = RoleConfig.Update;
            foreach (RoleConfig flag in Enum.GetValues(typeof(RoleConfig)))
            {
                if (flag.GetDefaultValue())
                    result |= flag;
            }
            return result;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class RoleAttribute : Attribute
    {
        public string ShortName { get; set; }
        public string Emoji { get; set; }
        public RoleAttribute(string shortName, string emoji)
        {
            ShortName = shortName;
            Emoji = emoji;
        }
    }

    public static class RoleExtensions
    {
        public static bool IsEditable(this RoleConfig value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var dA = fi.GetCustomAttribute(typeof(EditableAttribute)) as EditableAttribute;
            return dA?.AllowEdit ?? false;
        }
        public static RoleAttribute GetInfo(this RoleConfig value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var qA = fieldInfo.GetCustomAttributes(
                typeof(RoleAttribute), false) as RoleAttribute[];

            if (qA == null) return null;
            return (qA.Length > 0) ? qA[0] : null;
        }

        public static bool GetDefaultValue(this RoleConfig value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var dA = fi.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute;
            if (dA?.Value == null) return false;
            return ((bool)dA.Value);
        }



        public static IEnumerable<RoleConfig> GetUniqueSettings(this Enum flags)
        {
            ulong flag = 1;
            foreach (var value in Enum.GetValues(flags.GetType()).Cast<RoleConfig>())
            {
                ulong bits = Convert.ToUInt64(value);
                while (flag < bits)
                {
                    flag <<= 1;
                }

                if (flag == bits && flags.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        public static RoleConfig Toggle(this RoleConfig f, RoleConfig flag)
        {
            return f ^= flag;
        }
        
    }
}
