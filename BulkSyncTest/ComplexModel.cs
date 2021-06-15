using System;
using System.Linq;

namespace BulkSyncTest
{
    public static class ComplexModel
    {
        public static void Demo(MyDbContext context, MyDbContext syncContext)
        {
            if (!context.ComplexTypes.Any())
            {
                context.ComplexTypes.Add(new Complex
                {
                    OwnedA = new OwnedA
                    {
                        Referenced = new Referenced()
                    },
                    OwnedB = new OwnedB()
                });
                context.SaveChanges();
            }
            try
            {
                syncContext.BulkSynchronize(context.ComplexTypes, options =>
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
