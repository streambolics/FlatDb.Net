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
        public const long NullDatabase = 0;
        public const long TokenDatabase = 2;

        public const string TokenTypeId = "3EL4OPEQH4MSUSYLBS7MUBRQ2YLW6RML";
        public const string TokenReadStatusId = "UGYVWAP7FSQTWPZXSODKVUIL5RYB4UJ7";
        public const string TokenWriteStatusId = "MVWJ6NQ2HOA66AINNMST2OVSR3ZK4XE5";
        public const string EmptyMarker = "AN6E2653AQD5DYWGJGAYKWWYNAOQ3BWR";

        private long _DatabaseId;
        private bool _CanRead, _CanWrite;

        private FlatDbContext _Context;

        public static async Task<Database> Access(FlatDbContext Context, string Token)
        {
            return await new Database().Connect(Context, Token);
        }

        public static string TokenToId(string Token)
        {
            using (var encryptor = SHA256.Create())
            {
                var hash = encryptor.ComputeHash(Encoding.ASCII.GetBytes(Token));
                Console.WriteLine("TokenToID " + System.Convert.ToBase64String(hash));
                return System.Convert.ToBase64String(hash);
            }
        }

        public async Task<Database> Connect(FlatDbContext Context, string Token)
        {
            _Context = Context;

            var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == TokenDatabase && m.ItemId == TokenToId(Token));

            if (item != null)
            {
                Console.WriteLine("Database item found");
                _DatabaseId = Int64.Parse(item.MasterId, NullDatabase);
                _CanWrite = _DatabaseId != NullDatabase && item.StatusId == TokenWriteStatusId;
                _CanRead = _CanWrite || (_DatabaseId != NullDatabase && item.StatusId == TokenReadStatusId);
            }
            else
            {
                Console.WriteLine("No data found");
                _DatabaseId = NullDatabase;
                _CanRead = false;
                _CanWrite = false;
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
        public async Task<ItemModel> Get(string Id)
        {
            var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == _DatabaseId && m.ItemId == Id);
            if (item == null)
            {
                item = new ItemModel();
                item.Database = _DatabaseId;
                item.ItemId = Id;
            }
            return item;
        }

        private void Let(string s, Action<string> Setter)
        {
            if (String.IsNullOrEmpty(s))
            { }
            else if (s == EmptyMarker)
            {
                Setter("");
            }
            else
            { Setter(s); }
        }

        private void Let(long? i, Action<long> Setter)
        {
            if (i.HasValue) { Setter(i.Value); }
        }

        private void Let(DateTime? i, Action<DateTime> Setter)
        {
            if (i.HasValue) { Setter(i.Value); }
        }

        public async Task<ItemModel> Set(
            string Id,
            string TypeId,
            string SubTypeId,
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
            long? Amount
            )
        {
            bool changed = false;
            bool added = false;

            var item = await _Context.Item.SingleOrDefaultAsync(m => m.Database == _DatabaseId && m.ItemId == Id);
            if (item == null)
            {
                item = new ItemModel();
                item.Database = _DatabaseId;
                item.ItemId = Id;
                added = true;
            }

            Let(TypeId, s => { item.TypeId = s; changed = true; });
            Let(SubTypeId, s => { item.SubTypeId = s; changed = true; });
            Let(StatusId, s => { item.StatusId = s; changed = true; });
            Let(MasterId, s => { item.MasterId = s; changed = true; });
            Let(DetailId, s => { item.DetailId = s; changed = true; });
            Let(Sequence, s => { item.Sequence = s; changed = true; });
            Let(StartTime, s => { item.StartTime = s; changed = true; });
            Let(EndTime, s => { item.EndTime = s; changed = true; });
            Let(Text, s => { item.Text = s; changed = true; });
            Let(Data0, s => { item.Data0 = s; changed = true; });
            Let(Data1, s => { item.Data1 = s; changed = true; });
            Let(Data2, s => { item.Data2 = s; changed = true; });
            Let(Data3, s => { item.Data3 = s; changed = true; });
            Let(Data4, s => { item.Data4 = s; changed = true; });
            Let(Data5, s => { item.Data5 = s; changed = true; });
            Let(Data6, s => { item.Data6 = s; changed = true; });
            Let(Data7, s => { item.Data7 = s; changed = true; });
            Let(Amount, s => { item.Amount = s; changed = true; });

            if (added)
            {
                _Context.Add(item);
                await _Context.SaveChangesAsync();
            }
            else if (changed)
            {
                _Context.Update(item);
                await _Context.SaveChangesAsync();
            }
            return item;
        }
    }
}