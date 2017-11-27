using System;
using System.Threading.Tasks;
using FlatDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatDatabase.Controllers
{
    public class AdminController : Controller
    {
        FlatDbContext _context;

        public AdminController(FlatDbContext aContext) => _context = aContext;

        public string Grant (long DbId, string Token)
        {
            // TODO : This here is a huge security issue as anyone can grant 
            //        access to any database.
            //        We need a secondary check with an administrative token.

            ItemModel n = new ItemModel ();

            // TODO : If token is empty, generate a random one.

            n.ItemId = Database.TokenToId(Token);

            // TODO : Check whether we want a read-only or write token

            n.StatusId = Database.TokenWriteStatusId;
            n.Database = Database.TokenDatabase;

            // TODO : If DbId is zero, generate a new DbId. Check that it does
            //        not correspond to an existing database.
            
            n.MasterId = DbId.ToString();
            _context.Add(n);
            _context.SaveChanges();
            return Token;
        }
    }
}
