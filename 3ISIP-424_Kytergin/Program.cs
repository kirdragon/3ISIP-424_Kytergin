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
            Weapon rustySword = new Weapon("Ржавый меч", 5);
            Weapon steelSword = new Weapon("Стальной меч", 15);
            Weapon legendarySword = new Weapon("Легендарный меч", 30);

            Armor leatherArmor = new Armor("Кожаная броня", 5);
            Armor steelArmor = new Armor("Стальные доспехи", 10);
            Armor legendaryArmor = new Armor("Легендарные доспехи", 20);
        }
    }
    class Entity
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public Entity(string name, int health, int attack, int defense)
        {
            Name = name;
            Health = health;
            Attack = attack;
            Defense = defense;
        }
    }
    class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public Weapon(string name, int damage)
        {
            Name = name;
            Damage = damage;
        }
    }
    class Armor
    {
        public string Name { get; set; }
        public int Protection { get; set; }
        public Armor(string name, int protection)
        {
            Name = name;
            Protection = protection;
        }
    }
}
