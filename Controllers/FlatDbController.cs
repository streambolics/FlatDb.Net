using System;
using System.Threading.Tasks;
using FlatDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatDatabase.Controllers
{
    public class FlatDbController : Controller
    {
        FlatDbContext _context;

        private async Task<Database> GetReadableDatabase(string token) => (await Database.Access(_context, token)).AssertReadable();

        private async Task<Database> GetWritableDatabase(string token) => (await Database.Access(_context, token)).AssertWritable();

        public async Task<Item> Get(string token, string id) => await (await GetReadableDatabase(token)).Get(id);

        public async Task<Item> Set(string token, string id, string TypeId, string SubTypeId)
        {
            return await (await GetWritableDatabase(token)).Set(id, TypeId, SubTypeId);
        }
    }
}