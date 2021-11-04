using Flogging.Core;
using FloggingConsole.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flogging.Data.CustomAdo;
using Flogging.Data.CustomDapper;

namespace FloggingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var fd = GetFlogDetail("starting application", null);
            Flogger.WriteDiagnostic(fd);

            var tracker = new PerfTracker("FloggerConsole_Exception", "", fd.UserName,
                fd.Location, fd.Product, fd.Layer);

            var connstr = @"Data Source=WSB-070-74\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;Database=LoggingCourse";
            using(var db = new SqlConnection(connstr))
            {
                db.Open();
                try
                {
                    var sp = new Sproc(db, "CreateNewCustomer");
                    sp.SetParam("@Name", "waytoolongforitsgood");
                    sp.SetParam("@TotalPurchases", 12000);
                    sp.SetParam("@TotalReturns", 100.50M);
                    sp.ExecNonQuery();
                }
                catch(Exception ex)
                {
                    var efd = GetFlogDetail("", ex);
                    Flogger.WriteError(efd);
                }

                try
                {
                    //DAPPER
                    db.DapperProcNonQuery("CreateNewCustomer", new
                    {
                        Name = "dappernametoollongtowork",
                        TotalPurchases = 12000,
                        TotalReturns = 100.50M
                    });
                }
                catch (Exception ex)
                {
                    var efd = GetFlogDetail("", ex);
                    Flogger.WriteError(efd);
                }
            }

            var ctx = new CustomerDbContext();
            try
            {
                //EF
                var name = new SqlParameter("@Name", "waytoolongforafield");
                var totalPurchases = new SqlParameter("@TotalPurchases", 12000);
                var totalReturns = new SqlParameter("@TotalReturns", 100.50M);
                ctx.Database.ExecuteSqlCommand("EXEC dbo.CreateNewCustomer @Name, @TotalPurchases, @TotalReturns",
                    name, totalPurchases, totalReturns);
            }
            catch(Exception ex)
            {
                var efd = GetFlogDetail("", ex);
                Flogger.WriteError(efd);
            }

            fd = GetFlogDetail("used flogging console", null);
            Flogger.WriteUsage(fd);

            fd = GetFlogDetail("stopping app", null);
            Flogger.WriteDiagnostic(fd);

            tracker.Stop();
        }

        private static FlogDetail GetFlogDetail(string message,
            Exception ex)
        {
            return new FlogDetail
            {
                Product = "Flogger",
                Location = "FloggerConsole",
                Layer = "Job",
                UserName = Environment.UserName,
                Hostname = Environment.MachineName,
                Message = message,
                Exception = ex
            };
        }
    }
}
