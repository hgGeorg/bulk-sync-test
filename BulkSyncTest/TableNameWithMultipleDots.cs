using System;
using System.Linq;

namespace BulkSyncTest
{
    public static class TableNameWithMultipleDots
    {
        public static void Demo(MyDbContext context, MyDbContext syncContext)
        {
            if (!context.DottedTableName.Any())
            {
                context.DottedTableName.Add(new EntityWithMultipleDotsInTableName());
                context.SaveChanges();
            }
            try
            {
                syncContext.BulkSynchronize(context.DottedTableName, options =>
                {
                    options.SynchronizeKeepidentity = true;
                });
            }
            catch (Exception exc)
            {
                // relevant for this bug report
                Console.WriteLine(exc);
            }
        }
    }
}
