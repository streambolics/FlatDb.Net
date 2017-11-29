using System;
using System.Threading.Tasks;
using FlatDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlatDatabase.Controllers
{
    public class AdminController : Controller
    {
        FlatDbContext _context;

        public AdminController(FlatDbContext aContext) => _context = aContext;

        public async Task<bool> CheckDatabaseExistsAsync(long database)
        {
            ItemModel itm = await _context.Item.SingleOrDefaultAsync(i => i.Database == database);
            return itm != null;
        }

        public string Shutdown(string admintoken)
        {
            if (!Program.CheckAdminSecret(admintoken))
            {
                return "Sorry...";
            }
            
            Program.Shutdown();
            return "Bye...";
        }

        public async Task<string> Grant(long database, string token, string writable, string admintoken)
        {
            if (!Program.CheckAdminSecret(admintoken))
            {
                return "Sorry...";
            }

            ItemModel n = new ItemModel();

            // TODO : If token is empty, generate a random one.

            if (String.IsNullOrEmpty(token))
            {
                byte[] b = new byte[15];
                Random r = new Random();
                r.NextBytes(b);
                token = System.Convert.ToBase64String(b);
            }

            n.ItemId = Database.TokenToId(token);

            if (String.IsNullOrEmpty(writable))
            {
                n.StatusId = Database.TokenReadStatusId;
            }
            else
            {
                n.StatusId = Database.TokenWriteStatusId;
            }

            n.Database = Database.TokenDatabase;

            if (database < 5)
            {
                do
                {
                    byte[] b = new byte[7];
                    Random r = new Random();
                    r.NextBytes(b);
                    database = 1;
                    foreach (byte bb in b)
                    {
                        database = (database << 8) + bb;
                    }
                }
                while (await CheckDatabaseExistsAsync(database));
            }

            n.MasterId = database.ToString();
            _context.Add(n);
            await _context.SaveChangesAsync();
            return token;

        }
    }
}
