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
            Player player = new Player("Игрок", 100, 10, 5, rustySword, leatherArmor);
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
    class Player : Entity
    {
        public Weapon weapon;
        public Armor armor;
        public Player (string name, int health, int attack, int defense, Weapon weapon, Armor armor) : base(name, health, attack, defense)
        {
            this.weapon = weapon;
            this.armor = armor;
        }
    }
    class Enemy : Entity
    {
        public Enemy(string name, int health, int attack, int defense) : base(name, health, attack, defense)
        {
        }
    }
    class Goblin : Enemy
    {
        public int CriticalChance { get; set; }
        public Goblin(string name, int health, int attack, int defense, int criticalChance) : base(name, health, attack, defense)
        {
            CriticalChance = criticalChance;
        }

    }
    class Skeleton : Enemy
    {
        public Skeleton(string name, int health, int attack, int defense) : base(name, health, attack, defense)
        {
        }
    }
    class Mag : Enemy
    {
        public int FreezChance { get; set; }
        public Mag(string name, int health, int attack, int defense, int freezeChance) : base(name, health, attack, defense)
        {
            FreezChance = freezeChance;
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
