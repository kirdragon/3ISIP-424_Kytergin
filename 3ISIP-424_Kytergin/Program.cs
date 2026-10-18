
using System;
using System.Collections.Generic;

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

            List<Weapon> weapons = new List<Weapon>() { rustySword, steelSword, legendarySword };
            List<Armor> armors = new List<Armor>() { leatherArmor, steelArmor, legendaryArmor };

            int turn_count = 0;
            Player player = new Player(rustySword, leatherArmor);

            while (true)
            {
                Console.WriteLine("1. Начать игру");
                Console.WriteLine("2. Выйти");
                int choice = GetInt("Выбор: ", 1, 2);

                switch (choice)
                {
                    case 1:
                        turn_count++;

                        int game_event = turn_count % 10 == 0 ? 2 : random.Next(1, 3);

                        if (game_event == 1)
                        {
                            Console.WriteLine("Ты получаешь сундук!");
                            int drop_chance = random.Next(1, 101);

                            if (drop_chance <= 20)
                            {
                                player.Health = 100;
                                Console.WriteLine("Из него тебе выпадает лечебное зелье! Теперь у тебя 100 HP");
                            }
                            else if (drop_chance <= 70)
                            {
                                Weapon weapon = weapons[random.Next(weapons.Count)];
                                Console.WriteLine($"Тебе досталось оружие {weapon.Name}");
                                Console.WriteLine($"1. Заменить текущее оружие ({player.weapon.Name})");
                                Console.WriteLine("2. Отказаться");

                                if (GetInt("Выбор: ", 1, 2) == 1)
                                {
                                    player.weapon = weapon;
                                    Console.WriteLine($"Ты экипировал {player.weapon.Name}");
                                }
                                else
                                {
                                    Console.WriteLine($"Ты остался со старым оружием - {player.weapon.Name}");
                                }
                            }
                            else
                            {
                                Armor armor = armors[random.Next(armors.Count)];
                                Console.WriteLine($"Тебе досталась броня {armor.Name}");
                                Console.WriteLine($"1. Заменить текущую броню ({player.armor.Name})");
                                Console.WriteLine("2. Отказаться");

                                if (GetInt("Выбор: ", 1, 2) == 1)
                                {
                                    player.armor = armor;
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
                            Enemy current_enemy;

                            if (turn_count % 10 == 0)
                            {
                                int random_boss = random.Next(0, 4);
                                switch (random_boss)
                                {
                                    case 0:
                                        current_enemy = new GoblinBoss();
                                        break;
                                    case 1:
                                        current_enemy = new Kovalski();
                                        break;
                                    case 2:
                                        current_enemy = new MagBoss();
                                        break;
                                    default:
                                        current_enemy = new Pestov();
                                        break;
                                }
                            }
                            else
                            {
                                int random_enemy = random.Next(0, 3);
                                switch (random_enemy)
                                {
                                    case 0:
                                        current_enemy = new Goblin();
                                        break;
                                    case 1:
                                        current_enemy = new Skeleton();
                                        break;
                                    default:
                                        current_enemy = new Mag();
                                        break;
                                }
                            }

                            Console.WriteLine($"На тебя напал {current_enemy.Name}!");
                            bool is_frozen = false;

                            while (player.Health > 0 && current_enemy.Health > 0)
                            {
                                if (is_frozen)
                                {
                                    Console.WriteLine("Ты заморожен и пропускаешь ход!");
                                    is_frozen = false;
                                    var result = EnemyAttack(player, current_enemy);
                                    is_frozen = result.frozen;
                                    continue;
                                }

                                int player_choice = GetInt(
                                    $"У тебя {player.Health} HP\n" +
                                    $"Оружие: {player.weapon.Name}, урон {player.Attack + player.weapon.Damage}\n" +
                                    $"Броня: {player.armor.Name}, защита {player.Defense + player.armor.Protection}\n" +
                                    $"Враг: {current_enemy.Name}, HP {current_enemy.Health}, атака {current_enemy.Attack}\n" +
                                    "1. Атаковать\n2. Попытаться уклониться\nВыбор: ", 1, 2);

                                if (player_choice == 1)
                                {
                                    int damage = Math.Max(1, player.Attack + player.weapon.Damage - current_enemy.Defense);
                                    current_enemy.Health = Math.Max(0, current_enemy.Health - damage);
                                    Console.WriteLine($"Ты атаковал {current_enemy.Name} и нанёс {damage} урона. У него осталось {current_enemy.Health} HP");

                                    if (current_enemy.Health <= 0)
                                    {
                                        Console.WriteLine($"Ты победил {current_enemy.Name}!");
                                        break;
                                    }

                                    var result = EnemyAttack(player, current_enemy);
                                    is_frozen = result.frozen;
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
                                        Console.WriteLine($"Тебе не удалось уклониться от атаки {current_enemy.Name}!");
                                        var result = EnemyAttack(player, current_enemy, true);
                                        is_frozen = result.frozen;
                                    }
                                }
                            }

                            if (player.Health <= 0)
                            {
                                Console.WriteLine("Игра окончена.");
                                return;
                            }
                        }
                        break;

                    case 2:
                        Console.WriteLine("Выход из игры...");
                        return;
                }
            }
        }

        static public (int health, bool frozen) EnemyAttack(Player player, Enemy current_enemy, bool failedDodge = false)
        {
            int enemy_attack = current_enemy.Attack;
            bool is_frozen = false;

            if (current_enemy is Goblin goblin)
            {
                if (random.Next(1, 101) <= goblin.CriticalChance)
                {
                    enemy_attack *= 2;
                    Console.WriteLine($"{current_enemy.Name} нанёс критический удар!");
                }
            }

            if (current_enemy is Mag mag)
            {
                is_frozen = random.Next(1, 101) <= mag.FreezChance;
            }
            else if (current_enemy is Pestov pestov)
            {
                is_frozen = random.Next(1, 101) <= pestov.FreezeChance;
            }

            int damage;

            if (current_enemy is Skeleton)
            {
                damage = enemy_attack;
            }
            else if (failedDodge)
            {
                int absorption = player.armor.Protection * random.Next(70, 101) / 100;
                damage = Math.Max(0, enemy_attack - player.Defense - absorption);
            }
            else
            {
                damage = Math.Max(0, enemy_attack - player.Defense - player.armor.Protection);
            }

            player.Health = Math.Max(0, player.Health - damage);

            if (damage > 0)
            {
                Console.WriteLine($"{current_enemy.Name} нанёс тебе {damage} урона. У тебя осталось {player.Health} HP");
            }
            else
            {
                Console.WriteLine($"{current_enemy.Name} атаковал тебя, но ты не получил урона!");
            }

            if (is_frozen && player.Health > 0)
            {
                Console.WriteLine($"{current_enemy.Name} заморозил тебя! Ты пропустишь следующий ход!");
            }

            return (player.Health, is_frozen && player.Health > 0);
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
        public int FreezeChance { get; set; } = 30;

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

        public Armor(string name, int protection) : base(name)
        {
            Protection = protection;
        }
    }
}
