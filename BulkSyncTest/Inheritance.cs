using System;
using System.Linq;

namespace BulkSyncTest
{
    public static class Inheritance
    {
        public static void Demo(MyDbContext context, MyDbContext syncContext)
        {
            if (!context.Inheritance.Any())
            {
                // add items to table if empty
                for (int i = 0; i < 100; i++)
                {
                    var _ = (i % 3) switch
                    {
                        2 => context.Inheritance.Add(new InheritEmpty()),
                        1 => context.Inheritance.Add(new InheritA()),
                        0 => context.Inheritance.Add(new InheritB()),
                        _ => throw new NotImplementedException()
                    };
                }
                context.SaveChanges();
            }
            try
            {
                syncContext.BulkSynchronize(context.Inheritance, options =>
                {
                    options.SynchronizeKeepidentity = true;
                });
            }
            catch (InvalidCastException exc)
            {
                // relevant for this bug report
                Console.WriteLine(exc);
            }
        }
    }
}
