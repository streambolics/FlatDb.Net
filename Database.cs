using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FlatDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace FlatDatabase
{

    public class Database
    {
        private const long NullDatabase = 0;
        private const long TokenDatabase = 2;

        private const string TokenTypeId = "3EL4OPEQH4MSUSYLBS7MUBRQ2YLW6RML";
        private const string TokenReadStatusId = "UGYVWAP7FSQTWPZXSODKVUIL5RYB4UJ7";
        private const string TokenWriteStatusId = "MVWJ6NQ2HOA66AINNMST2OVSR3ZK4XE5";
        private const string EmptyMarker = "AN6E2653AQD5DYWGJGAYKWWYNAOQ3BWR";

        private long _DatabaseId;
        private bool _CanRead, _CanWrite;

        private FlatDbContext _Context;

        public static async Task<Database> Access(FlatDbContext Context, string Token)
        {
            return await new Database().Connect(Context, Token);
        }

        public async Task<Database> Connect(FlatDbContext Context, string Token)
        {
            _Context = Context;

            using (var encryptor = SHA256.Create())
            {
                var hash = encryptor.ComputeHash(Encoding.ASCII.GetBytes(Token));
                var key = System.Convert.ToBase64String(hash);
                var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == TokenDatabase && m.ItemId == key);

                if (item != null)
                {
                    _DatabaseId = Int64.Parse(item.MasterId, NullDatabase);
                    _CanWrite = _DatabaseId != NullDatabase && item.StatusId == TokenWriteStatusId;
                    _CanRead = _CanWrite || (_DatabaseId != NullDatabase && item.StatusId == TokenReadStatusId);
                }
                else
                {
                    _DatabaseId = NullDatabase;
                    _CanRead = false;
                    _CanWrite = false;
                }
            }

            return this;
        }
        public bool IsReadable => _CanRead;
        public bool IsWritable => _CanWrite;

        public Database AssertReadable()
        {
            if (IsReadable)
            {
                return this;
            }
            else
            {
                throw new Exception("Token does not correspond to a readable database");
            }
        }

        public Database AssertWritable()
        {
            if (IsWritable)
            {
                return this;
            }
            else
            {
                throw new Exception("Token does not correspond to a writable database");
            }
        }
        public async Task<Item> Get(string Id)
        {
            var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == _DatabaseId && m.ItemId == Id);
            if (item == null)
            {
                item = new Item();
                item.Database = _DatabaseId;
                item.ItemId = Id;
            }
            return item;
        }

        public async Task<Item> Set(string Id, string TypeId, string SubTypeId)
        {
            bool changed = false;
            var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == _DatabaseId && m.ItemId == Id);
            if (item == null)
            {
                item = new Item();
                item.Database = _DatabaseId;
                item.ItemId = Id;
                changed = true;
            }


            if (TypeId != null) { item.TypeId = TypeId; changed = true; }
            if (SubTypeId != null) { item.SubTypeId = SubTypeId; changed = true; }

            if (changed)
            {
                _Context.Update(item);
            }
            return item;
        }
    }
}