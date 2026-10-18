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

            List<Enemy> bosses = new List<Enemy>()
            {
                new GoblinBoss(),
                new Kovalski(),
                new MagBoss(),
                new Pestov()
            };

            List<Enemy> enemies = new List<Enemy>()
            {
                new Goblin(),
                new Skeleton(),
                new Mag()
            };

            int choice;
            int turn_count = 0;

            int random_enemy;
            int random_boss;
            Enemy current_enemy;

            int game_event;

            Player player = new Player(rustySword, leatherArmor);
            while (true)
            {
                Console.WriteLine("1. Начать игру");
                Console.WriteLine("2. Выйти");
                choice = GetInt("Выбор: ", 1, 2);
                switch (choice)
                {
                    case 1:
                        turn_count++;
                        if (turn_count % 10 == 0)
                        {
                            game_event = 2;
                        }
                        else
                        {
                            game_event = random.Next(1, 3);
                        }
                        if (game_event == 1)
                        {
                            Console.WriteLine("Ты получаешь сундук!");
                            int drop_chance = random.Next(1, 101);
                            if (drop_chance <=20)
                            {
                                Console.WriteLine("Из него тебе выпадает лечебное зелье! Теперь у тебя 100 HP");
                                player.Health = 100;
                            }
                            else if (drop_chance >20 && drop_chance <=70)
                            {
                                int weapon_chance = random.Next(0, 3);
                                Console.WriteLine($"Тебе досталось оружие {weapons[weapon_chance].Name}");

                                Console.WriteLine($"1. Заменить текущее оружие ({player.weapon.Name})");
                                Console.WriteLine("2. Отказаться");
                                int weapon_choice = GetInt("Выбор: ", 1, 2);
                                if (weapon_choice == 1)
                                {                                    
                                    player.weapon = weapons[weapon_chance];
                                    Console.WriteLine($"Ты экипировал {player.weapon.Name}");
                                 }
                                else
                                {
                                    Console.WriteLine($"Ты остался со старым оружием - {player.weapon.Name}");
                                 }                               
                            }
                            else
                            {
                                int armor_chance = random.Next(0, 3);
                                Console.WriteLine($"Тебе досталась броня {armors[armor_chance].Name}");

                                Console.WriteLine($"1. Заменить текущую броню ({player.armor.Name})");
                                Console.WriteLine("2. Отказаться");
                                int armor_choice = GetInt("Выбор: ", 1, 2);
                                if (armor_choice == 1)
                                {                                    
                                    player.armor = armors[armor_chance];
                                    Console.WriteLine($"Ты экипировал {player.armor.Name}");
                                }
                                else
                                {
                                    Console.WriteLine($"Ты остался со старой броней - {player.armor.Name}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("На тебя напал враг!");
                            if (turn_count % 10 == 0)
                            {
                                random_boss = random.Next(0, bosses.Count);
                                current_enemy = bosses[random_boss];
                                Console.WriteLine($"Это оказался {current_enemy.Name}");
                            }
                            else
                            {
                                random_enemy = random.Next(0, 3);
                                current_enemy = enemies[random_enemy];
                                Console.WriteLine($"Это оказался {current_enemy.Name}");
                            }
                            while (player.Health > 0 && current_enemy.Health > 0)
                            {
                                int player_choice = GetInt($"Ты имеешь {player.Health} Hp\nТвое оружие наносит {player.weapon.Damage} урона\nТвоя броня предоставляет как минимум {player.armor.Protection * 0.7} защиты\n\nТвой враг - {current_enemy.Name}\nОн имеет {current_enemy.Health} Hp\n и наносит {current_enemy.Attack} урона\n1. Атаковать\n2. Попытаться уклониться\nВыбор: ", 1, 2);
                                if (player_choice == 1)
                                {
                                    current_enemy.Health -= player.weapon.Damage+ player.Attack;
                                    Console.WriteLine($"Ты атаковал {current_enemy.Name} с помощью {player.weapon.Name} и у него осталось {current_enemy.Health}");
                                    if (current_enemy.Health <= 0)
                                    {
                                        Console.WriteLine($"Ты победил {current_enemy.Name}!");
                                        break;
                                    }
                                    EnemyAttack(player, current_enemy, player_choice);
                                }
                                else
                                {
                                    int chance = random.Next(1, 101);
                                    if (chance <= 40)
                                    {
                                        Console.WriteLine($"Ты уклонился от атаки {current_enemy.Name}!");
                                    }
                                    else
                                    {
                                        int weakness = random.Next(70, 101);
                                        int absorbed_damage = player.armor.Protection * weakness / 100;
                                        int weakened_attack = Math.Max(0, current_enemy.Attack - absorbed_damage);
                                        if (weakened_attack == 0)
                                        {
                                            Console.WriteLine($"{current_enemy.Name} атаковал тебя, но твоя броня полностью поглотила урон!");
                                        }
                                        else
                                        {
                                            player.Health -= weakened_attack;
                                            if (player.Health <= 0)
                                            {
                                                Console.WriteLine("Ты погиб! Игра окончена.");
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Тебе не удалось уклониться от атаки {current_enemy.Name}, но благодаря твоей броне ты получил пониженный урон - {weakened_attack}!");
                                            }  
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case 2:
                    Console.WriteLine("Выход из игры...");
                        return;
                }
            }
        }
        static public void EnemyAttack(Player player, Enemy current_enemy, int choice)
        {
            if (current_enemy.Attack - player.armor.Protection > 0)
            {
                player.Health -= current_enemy.Attack - player.armor.Protection;
                Console.WriteLine($"{current_enemy.Name} атаковал тебя и у тебя осталось {player.Health}");
            }
            else
            {
                Console.WriteLine($"{current_enemy.Name} атаковал тебя, но твоя броня все впитала!");
            }
            if (player.Health <= 0)
            {
                Console.WriteLine($"Ты умер {current_enemy.Name}! Игра окончена.");
                return;
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