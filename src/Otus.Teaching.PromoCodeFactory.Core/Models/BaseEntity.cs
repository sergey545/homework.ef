using System;

namespace Otus.Teaching.PromoCodeFactory.Core
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.Now;
        }

        public string SysMessage { get; set; } // для диагностических нужд
        public string ShortTimeStamp { get { return $"{CreatedDateTime.ToShortDateString()} {CreatedDateTime.ToShortTimeString()}"; } }

        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDatTime { get; set; }

    }
}