using System;

namespace FlatDatabase.Models
{
    public class ItemModel
    {
        public long Id { get; set; }
        public long Database { get; set; }

        public string ItemId { get; set; }
        public string TypeId { get; set; }
        public string SubTypeId { get; set; }
        public string StatusId { get; set; }
        public string MasterId { get; set; }
        public string DetailId { get; set; }
        public long Sequence { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Text { get; set; }
        public string Data0 { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public long Amount { get; set; }
    }
}