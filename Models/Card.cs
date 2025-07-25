using System;
using System.Collections.Generic;

namespace KanabanBack.Models;

public partial class Card
{
    public int CardId { get; set; }

    public string Title { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Order { get; set; }

    public int ListId { get; set; }

    public int? UsuarioId { get; set; }

    public int? TagId { get; set; }

    public virtual List List { get; set; } = null!;

    public virtual Tag? Tag { get; set; }

    public virtual User? Usuario { get; set; }
}
