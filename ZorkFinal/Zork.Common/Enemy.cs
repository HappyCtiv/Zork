namespace Zork.Common
{
    public class Enemy
    {
        public string Name { get; }
        public string EnemyDescription { get; }
        public string DefeatDescription { get; }
        public string AtackDescription { get; }
        public int Health { get; }
        public int Damage { get; }

        public Enemy(string name, string enemyDescription, string defeatDescription, string atackDescription, int health, int damage)
        {
            Name = name;
            EnemyDescription = enemyDescription;
            DefeatDescription = defeatDescription;
            AtackDescription = atackDescription;
            Health = health;
            Damage = damage;
        }

        public override string ToString() => Name;
    }
}
