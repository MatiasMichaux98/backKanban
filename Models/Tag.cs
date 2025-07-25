using System;
using System.Collections.Generic;

namespace KanabanBack.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
