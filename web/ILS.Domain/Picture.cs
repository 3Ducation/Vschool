﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILS.Domain
{
    public class Picture : EntityBase
    {
        public int OrderNumber { get; set; }
        public string Path { get; set; }
        [ForeignKey("Paragraph")] public Guid Paragraph_Id { get; set; }

        public virtual Paragraph Paragraph { get; set; }
    }
}
