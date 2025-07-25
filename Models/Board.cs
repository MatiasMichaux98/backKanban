using System;
using System.Collections.Generic;

namespace KanabanBack.Models;

public partial class Board
{
    public int BoardId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<List> Lists { get; set; } = new List<List>();

    public virtual User? Usuario { get; set; }
}
