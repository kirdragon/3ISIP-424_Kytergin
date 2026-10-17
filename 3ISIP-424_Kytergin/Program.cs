using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }
    }
    class Weapon
    {
        public string Name { get; set; }
        public string Damage { get; set; }
        public Weapon(string name, string damage)
        {
            Name = name;
            Damage = damage;
        }
    }
    class Armor
    {
        public string Name { get; set; }
        public string Protection { get; set; }
        public Armor(string name, string protection)
        {
            Name = name;
            Protection = protection;
        }
    }
}
