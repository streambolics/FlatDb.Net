using System;
using System.Threading.Tasks;
using FlatDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlatDatabase.Controllers
{
    public class FlatDbController : Controller
    {
        FlatDbContext _context;

        public FlatDbController(FlatDbContext aContext) => _context = aContext;

        private async Task<Database> GetReadableDatabase(string token) => (await Database.Access(_context, token)).AssertReadable();

        private async Task<Database> GetWritableDatabase(string token) => (await Database.Access(_context, token)).AssertWritable();

        public async Task<ItemModel> Get(string token, string id) => await (await GetReadableDatabase(token)).Get(id);

        public async Task<ItemModel> Set(string token, string id, string TypeId, string SubTypeId,
            string StatusId,
            string MasterId,
            string DetailId,
            long? Sequence,
            DateTime? StartTime,
            DateTime? EndTime,
            string Text,
            string Data0,
            string Data1,
            string Data2,
            string Data3,
            string Data4,
            string Data5,
            string Data6,
            string Data7,
            long? Amount)
        {
            return await (await GetWritableDatabase(token)).Set(id, TypeId, SubTypeId, StatusId, MasterId, DetailId, Sequence,StartTime, EndTime,
            Text, Data0, Data1, Data2, Data3, Data4, Data5, Data6, Data7, Amount);
        }
    }
}