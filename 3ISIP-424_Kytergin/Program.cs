using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3ISIP_424_Kytergin
{
    internal class Program
    {
        static Random random = new Random();
        static void Main(string[] args)
        {
            Weapon rustySword = new Weapon("Ржавый меч", 5);
            Weapon steelSword = new Weapon("Стальной меч", 15);
            Weapon legendarySword = new Weapon("Легендарный меч", 30);

            Armor leatherArmor = new Armor("Кожаная броня", 5);
            Armor steelArmor = new Armor("Стальные доспехи", 10);
            Armor legendaryArmor = new Armor("Легендарные доспехи", 20);

            List <Weapon> weapons = new List<Weapon>()
            {
                rustySword,
                steelSword,
                legendarySword
            };
            List<Armor> armors = new List<Armor>()
            {
                leatherArmor,
                steelArmor,
                legendaryArmor
            };
            List <Item> inventory = new List<Item>();
            int choice;
            Player player = new Player(rustySword, leatherArmor);
            while (true)
            {
                Console.WriteLine("1. Начать игру");
                Console.WriteLine("2. Выйти");
                choice = GetInt("Выбор: ", 1, 3);
                switch (choice)
                {
                    case 1:
                        int game_event = random.Next(1, 2);
                        if (game_event == 1)
                        {
                            Console.WriteLine("Ты получаешь сундук!");
                            int drop_chance = random.Next(1, 101);
                            if (drop_chance <=20)
                            {
                                Console.WriteLine("Из него тебе выпадает лечебное зелье!");
                                player.Health = 100;
                            }
                            else if (drop_chance >20 && drop_chance <=70)
                            {
                                int weapon_chance = random.Next(0, 3);
                                Console.WriteLine($"Тебе досталось оружие {weapons[weapon_chance].Name}");

                                Console.WriteLine($"1. Заменить текущее оружие ({inventory.First(item => item is Weapon).Name})");
                                Console.WriteLine("2. Отказаться");
                                int weapon_choice = GetInt("Выбор: ", 1, 2);
                                if (weapon_choice == 1)
                                {
                                    inventory.Remove(inventory.First(item => item is Weapon));
                                    inventory.Add(weapons[weapon_chance]);
                                    Console.WriteLine($"Ты экипировал {weapons[weapon_chance]}");
                                 }
                                else
                                {
                                    Console.WriteLine($"Ты остался со старым оружием - {inventory.First(item => item is Weapon).Name}");
                                 }                               
                            }
                            else
                            {
                                int armor_chance = random.Next(0, 3);
                                Console.WriteLine($"Тебе досталась броня {armors[armor_chance].Name}");

                                Console.WriteLine($"1. Заменить текущую броню ({inventory.First(item => item is Armor).Name})");
                                Console.WriteLine("2. Отказаться");
                                int armor_choice = GetInt("Выбор: ", 1, 2);
                                if (armor_choice == 1)
                                {
                                    inventory.Remove(inventory.First(item => item is Armor));
                                    inventory.Add(armors[armor_chance]);
                                    Console.WriteLine($"Ты экипировал {armors[armor_chance]}");
                                }
                                else
                                {
                                    Console.WriteLine($"Ты остался со старой броней - {inventory.First(item => item is Armor).Name}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("На тебя напал враг!");
                        }
                        break;

                    case 2:
                        break;

                    default:
                        Console.WriteLine("Ошибка! Вводи только 1 или 2.");
                        break;
                }
            }
        }
        static public int GetInt(string message, int min, int max)
        {
            Console.Write(message);
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int number) && number >= min && number <= max)
                {
                    return number;
                }
                Console.Write($"Вводи только целые числа от {min} до {max}\nВвод: ");
            }
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
        public Player(Weapon weapon, Armor armor) : base("Игрок", 100, 10, 5)
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
        public Goblin() : base("Гоблин", 40, 12, 3)
        {
            CriticalChance = 20;
        }

    }
    class Skeleton : Enemy
    {
        public Skeleton() : base("Скелет", 50, 10, 5)
        {
        }
    }
    class Mag : Enemy
    {
        public int FreezChance { get; set; }
        public Mag() : base("Маг", 30, 15, 2)
        {
            FreezChance = 15;
        }
    }
    class GoblinBoss : Goblin
    {
        public GoblinBoss() : base()
        {
            Name = "Гоблин-Босс";
            Health = (int)(Health * 2.0);
            Attack = (int)(Attack * 1.5);
            Defense = (int)Math.Round(Defense * 1.2);
            CriticalChance += 10;
        }
    }
    class Kovalski : Skeleton
    {
        public Kovalski() : base()
        {
            Name = "Босс Ковальски";
            Health = (int)(Health * 2.5);
            Attack = (int)(Attack * 1.3);
            Defense = (int)(Defense * 1.4);
        }
    }
    class MagBoss : Mag
    {
        public MagBoss() : base()
        {
            Name = "Маг-Босс";
            Health = (int)(Health * 1.8);
            Attack = (int)(Attack * 1.6);
            Defense = (int)(Defense * 1.1);
            FreezChance += 10;
        }
    }
    class Pestov : Skeleton
    {
        public int FreesChanceForPestov { get; set; } = 30;
        public Pestov() : base()
        {
            Name = "Пестов";
            Health = (int)(Health * 1.3);
            Attack = (int)(Attack * 1.8);
            Defense = (int)(Defense * 0.6);
        }
    }
    class Item
    {
        public string Name { get; set; }
        public Item(string name)
        {
            Name = name;
        }
    }
    class Weapon : Item
    {
        public int Damage { get; set; }
        public Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }
    }
    class Armor : Item
    {
        public int Protection { get; set; }
        public Armor(string name, int protection) : base (name)
        {
            Protection = protection;
        }
    }
}