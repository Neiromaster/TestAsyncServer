using System.Data.Entity;

namespace Model.DataBaseModel
{
    public class MyContextInitializer : CreateDatabaseIfNotExists<UserContext>
    {
        protected override void Seed(UserContext db)
        {
            var p1 = new People() { Name = "asd", Password = "asd"};
            var p2 = new People() { Name = "1", Password = "1"};
            var t1 = new Goods() { Name = "Товар 1", Price = (decimal) 10.0, Description = "Первый"};
            var t2 = new Goods() { Name = "Товар 2", Price = (decimal) 100.0, Description = "Второй"};
            var t3 = new Goods() { Name = "Товар 3", Price = (decimal) 50.0, Description = "Третий"};


            db.Peoples.Add(p1);
            db.Peoples.Add(p2);
            db.Goods.Add(t1);
            db.Goods.Add(t2);
            db.Goods.Add(t3);
            db.SaveChanges();
        }
    }
    public class UserContext: DbContext
    {
        public UserContext(): base("DbConnection")
        {
            Database.SetInitializer<UserContext>(new MyContextInitializer());
        }

        static UserContext()
        {
            Database.SetInitializer<UserContext>(new MyContextInitializer());
        }

        public DbSet<People> Peoples { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Goods> Goods { get; set; }


    }

}