using System;
using System.Collections.Generic;

namespace KanabanBack.Models;

public partial class List
{
    public int ListId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int Order { get; set; }

    public int BoardId { get; set; }

    public virtual Board Board { get; set; } = null!;

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
}
